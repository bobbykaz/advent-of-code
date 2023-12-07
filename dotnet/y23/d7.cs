using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Xml.XPath;
using Microsoft.VisualBasic;

namespace y23 {
    public class D7 : AoCDay
    {
        public D7(): base(23, 7) {
            _DebugPrinting = true;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var total = 0L;
            var hands = new List<Hand>();
            foreach(var line in lines) {
                var pts = line.Split(" ");
                var hand = new Hand(pts[0], long.Parse(pts[1]));
                hands.Add(hand);
            }

            hands.Sort();
            
            for(var i = 0; i < hands.Count(); i++) {
                var h = hands[i];
                PrintLn($"{h.Print()} - {h.HandType} - {h.Bid} - {(i+1)}");
                total += (i+1) * h.Bid;
            }

            return $"{total}";
        }

        public override string P2()
        {
            var lines = InputAsLines();
            var total = 0L;
            var hands = new List<JokerHand>();
            foreach(var line in lines) {
                var pts = line.Split(" ");
                var hand = new JokerHand(pts[0], long.Parse(pts[1]));
                hands.Add(hand);
            }

            hands.Sort();
            
            for(var i = 0; i < hands.Count(); i++) {
                var h = hands[i];
                PrintLn($"{h.Print()} ({h.RawCards}) - {h.HandType} - {h.Bid} - {(i+1)}");
                total += (i+1) * h.Bid;
            }

            return $"{total}";
        }

        public class CG {
            public char Card {get; set;}
            public int Num {get; set;}
        }

        public class Hand : IComparable<Hand>{
            public string RawCards;
            public List<char> Cards;
            public long Bid;

            public HandTypeEnum HandType;

            public List<CG> CardGroups;

            public Hand(string c, long b) {
                RawCards = c;
                Cards = c.ToCharArray().ToList().OrderByDescending(c => Rank(c)).ToList();
                Bid = b;
                CardGroups = Cards.GroupBy(c => c)
                        .Select(group => new CG{ 
                             Card = group.Key, 
                             Num = group.Count() 
                        })
                        .OrderByDescending(x => x.Num)
                        .ThenByDescending(x => Rank(x.Card) )
                        .ToList();
                HandType = parseHandType();
            }

            public string Print() {
                var chars = new List<char>();
                foreach(var cg in CardGroups) {
                    for(var i = 0; i < cg.Num; i++) {
                        chars.Add(cg.Card);
                    }
                }

                return new string(chars.ToArray());
            }

            public HandTypeEnum parseHandType() {
                if (CardGroups.Count() == 1)
                    return HandTypeEnum.FiveOAK;

                if (CardGroups.Count() == 2) {
                    if (CardGroups[0].Num == 3)
                        return HandTypeEnum.FullHouse;
                    else return HandTypeEnum.FourOAK;
                }

                if (CardGroups.Count() == 3) {
                    if (CardGroups[0].Num == 3)
                        return HandTypeEnum.ThreeOAK;
                    else return HandTypeEnum.TwoPair;
                }

                if (CardGroups.Count() == 4)
                    return HandTypeEnum.OnePair;

                return HandTypeEnum.HighCard;
            }

            public int CompareTo(Hand? other) {
                return CompareToBasic(other);
            }

            public int CompareToBasic(Hand? other) {
                if(other is null)
                    throw new ArgumentException("other hand is null");

                if(this.HandType != other.HandType)
                    return this.HandType - other.HandType;
                else {
                    var mChars = this.RawCards.ToCharArray();
                    var oChars = other.RawCards.ToCharArray();
                    for(var i = 0; i < mChars.Count(); i++) {
                        var comp = Rank(mChars[i]) - Rank(oChars[i]);
                        if(comp != 0) {
                            return comp;
                        }
                    }
                    return 0;
                }
            }

            public int CompareToFancy(Hand? other)
            {
                if(other is null)
                    throw new ArgumentException("other hand is null");

                if(this.HandType != other.HandType)
                    return this.HandType - other.HandType;
                else {
                    var setRank = Rank(this.CardGroups[0].Card) - Rank(other.CardGroups[0].Card);
                    var offRank = Rank(this.CardGroups[1].Card) - Rank(other.CardGroups[1].Card);
                    var nonSetChars = this.CardGroups.Where(cg => cg.Num == 1).Select(cg => cg.Card).OrderByDescending(c => Rank(c)).ToList();
                    var otherNonSetChars = other.CardGroups.Where(cg => cg.Num == 1).Select(cg => cg.Card).OrderByDescending(c => Rank(c)).ToList();
                    List<int> nonSetDiffs = new List<int>();
                    
                    switch(this.HandType) {
                        case HandTypeEnum.FiveOAK:
                            return setRank;
                        case HandTypeEnum.FourOAK:
                        case HandTypeEnum.FullHouse:
                            return setRank == 0 ? offRank : setRank;
                        case HandTypeEnum.ThreeOAK:
                        case HandTypeEnum.OnePair:
                            if (setRank != 0) 
                                return setRank;
                            else {
                                for(var i = 0; i < nonSetChars.Count(); i++) {
                                    var comp = Rank(nonSetChars[i]) - Rank(otherNonSetChars[i]);
                                    if(comp != 0) {
                                        return comp;
                                    }
                                }
                                return 0;
                            }
                        case HandTypeEnum.TwoPair:
                            if (setRank != 0) 
                                return setRank;
                            else if (offRank != 0) 
                                return offRank;
                            else {
                                for(var i = 0; i < nonSetChars.Count(); i++) {
                                    var comp = Rank(nonSetChars[i]) - Rank(otherNonSetChars[i]);
                                    if(comp != 0) {
                                        return comp;
                                    }
                                }
                                return 0;
                            }
                        case HandTypeEnum.HighCard:
                            for(var i = 0; i < nonSetChars.Count(); i++) {
                                    var comp = Rank(nonSetChars[i]) - Rank(otherNonSetChars[i]);
                                    if(comp != 0) {
                                        return comp;
                                    }
                                }
                                return 0;
                        default:
                            return 0;
                    }
                }
            }

            public const string CardRanks = "23456789TJQKA";
            public const string JokerRanks = "J23456789TQKA";

            public virtual int Rank(char c) {
                return CardRanks.IndexOf(c);
            }

        }

        public class JokerHand : Hand
        {
            public JokerHand(string c, long b) : base(c, b)
            {
                if(c.Contains('J') && HandType != HandTypeEnum.FiveOAK) {
                    var jokerGroup = CardGroups.Where(cg => cg.Card == 'J').First();
                    CardGroups = CardGroups.Where(cg => cg.Card != 'J').ToList();
                    CardGroups[0].Num += jokerGroup.Num;
                    HandType = parseHandType();
                }
            }

            public override int Rank(char c) {
                return JokerRanks.IndexOf(c);
            }
        }

        public enum HandTypeEnum {
            FiveOAK = 7,
            FourOAK = 6,
            FullHouse = 5,
            ThreeOAK = 4,
            TwoPair = 3,
            OnePair = 2,
            HighCard = 1
        }
    }
}