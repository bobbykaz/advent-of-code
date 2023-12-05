namespace y23 {
    public class D4 : AoCDay
    {
        public D4(): base(23, 4) {
            _DebugPrinting = false;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var total = 0;
            foreach(var line in lines) {
                var (win, num) = parseCard(line);
                win.ForEach(n => Print($"{n} "));
                PrintLn(".");
                num.ForEach(n => Print($"{n} "));
                PrintLn("..");
                var mw = win.Intersect(num).ToList();
                mw.ForEach(n => Print($"{n} "));
                PrintLn("...");
                var points = (int) Math.Pow(2, mw.Count - 1);
                PrintLn($"{points} points");
                total += points;
            }
            
            return $" {total}";
        }

        private (List<int>, List<int>) parseCard(string card) {
            var pts = card.Split(": ");
            var cards = pts[1].Split(" | ");
            var win = cards[0].Split(" ").Where(s => s.Length > 0).Select(int.Parse).ToList();
            var num = cards[1].Split(" ").Where(s => s.Length > 0).Select(int.Parse).ToList();
            win.Sort();
            num.Sort();
            return (win, num);
        }

        public override string P2()
        {
            var lines = InputAsLines();
            var cardCopies = new List<int>();
            foreach (var line in lines){
                cardCopies.Add(1);
            }
            int c = 0;
            foreach(var line in lines) {
                var (win, num) = parseCard(line);
                var mw = win.Intersect(num).ToList();
                mw.ForEach(n => Print($"{n} "));
                var points = mw.Count;
                PrintLn($"points {points}");
                for (var i = 1; i <= points; i++) {
                    cardCopies[c+i] += cardCopies[c];
                }
                c++;
            }
            var total = cardCopies.Sum();
            return $" {total}";
        }
    }
}