using System.ComponentModel;
using Grids;

namespace y24 {
    public class D11: AoCDay {

        public D11(): base(24, 11) {
            _DebugPrinting = true;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var nums = Utilties.StringToNums<long>(lines[0], " ");
            for(int i = 0; i < 25; i++) {
                var newNums = new List<long>();
                for(int n = 0; n < nums.Count; n++) {
                    var pts = eval(nums[n]);
                    newNums.AddRange(pts);
                }
                nums = newNums;
            }
            var total = nums.Count;
           
            return $"{total}";
        }

        private List<long> eval(long num) {
            var result = new List<long>();
            var nStr = $"{num}";
            if (num == 0) {
                result.Add(1);
            } else if( nStr.Length % 2 == 0) {
                var str = $"{num}";
                var len = str.Length;
                var p1 = int.Parse(str.Substring(0,len/2));
                var p2 = int.Parse(str.Substring(len/2));
                result.Add(p1);
                result.Add(p2);
            } else {
                result.Add(num*2024);
            }

            return result;
        }

        public override string P2()
        {
            var lines = InputAsLines();
            var nums = Utilties.StringToNums<long>(lines[0], " ");
            var initInput = new List<long>(nums);
            Dictionary<long, List<long>> evalMap = [];
            var currentSet = new HashSet<long>();
            foreach(var l in nums) {currentSet.Add(l);}
            // populate all items in the set - 75 rounds
            for(int i = 0; i < 75; i++) {
                var newSet = new HashSet<long>();
                foreach(var n in currentSet) {
                    if(!evalMap.ContainsKey(n)) {
                        var nextNums = eval(n);
                        evalMap[n] = nextNums;
                        foreach(var nn in nextNums) {newSet.Add(nn);};
                    }
                }
                currentSet = newSet;
            }
            PrintLn($"Done prefilling values: {evalMap.Keys.Count} keys");

            //memoize (val, round) stone-count = memos[round-1][evalMap[val]]
            var memos = new Dictionary<string, long>();
            for(int round = 0; round <= 75; round++) {
                foreach(var val in evalMap.Keys) {
                    if(round == 0) {
                        memos[StoneKey(val, round)] = 1;
                    } else {
                        var evs = evalMap[val];
                        long subtotal = 0;
                        foreach(var ev in evs) {
                            subtotal += memos[StoneKey(ev, round-1)];
                        }
                        memos[StoneKey(val, round)] = subtotal;
                    }
                }
            }
           
            long total = 0;
            foreach(var n in initInput) {
                total += memos[StoneKey(n, 75)];
            }
            return $"{total}";
        }
        
        private string StoneKey(long val, int round) {
            return $"{val}-{round}";
        }

    }
}