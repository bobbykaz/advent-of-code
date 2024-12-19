namespace y24 {
    public class D19: AoCDay {

        public D19(): base(24, 19) {
            _DebugPrinting = true;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var groups = Utilties.GroupInputAsBlocks(lines);
            var towels = groups[0][0].Split(", ").ToList();
            Dictionary<char, List<string>> towelSet =  [];
            foreach(var towel in towels){
                var c = towel.First();
                if(!towelSet.ContainsKey(c))
                    towelSet[c] = new List<string>();
                towelSet[c].Add(towel);
                towelSet[c] = towelSet[c].OrderByDescending(s => s.Length).ToList();
            }
            var total = 0L;

            var layouts = groups[1];
            foreach(var l in layouts) {
                PrintLn($"{l}");
                Dictionary<string, bool> memos = [];
                if(isPossible(towelSet, l, memos)){
                    total++;
                    PrintLn($"... possible! {total}");
                }
            }
           
            return $"{total}";
        }

        private bool isPossible(Dictionary<char, List<string>> towelSet, string remaining, Dictionary<string, bool> memos) {
            if(remaining.Length == 0)
                return true;
            
            if(memos.ContainsKey(remaining))
                return false;

            //PrintLn($"  {remaining}");
            var firstChar = remaining.First();
            if (!towelSet.ContainsKey(firstChar))
                return false;

            var towels = towelSet[firstChar];
            foreach(var towel in towels) {
                if(remaining.IndexOf(towel) == 0){
                    var nextLevel = remaining.Substring(towel.Length);
                    if(isPossible(towelSet, nextLevel, memos)) {
                        return true;
                    } else {
                        memos[remaining] = false;
                    }
                }
            }
            return false;
        }

        private long countPossible(Dictionary<char, List<string>> towelSet, string remaining, Dictionary<string, long> memos) {
            if(remaining.Length == 0)
                return 1;
            
            if(memos.ContainsKey(remaining))
                return memos[remaining];
                
            //PrintLn($"  {remaining}");
            var firstChar = remaining.First();
            if (!towelSet.ContainsKey(firstChar))
                return 0;

            var towels = towelSet[firstChar];
            var found = 0L;
            foreach(var towel in towels) {
                if(remaining.IndexOf(towel) == 0){
                    var nextLevel = remaining.Substring(towel.Length);
                    var sub = countPossible(towelSet, nextLevel, memos);
                    memos[nextLevel] = sub;
                    found += sub;
                }
            }
            return found;
        }

        public override string P2()
        {
             var lines = InputAsLines();
            var groups = Utilties.GroupInputAsBlocks(lines);
            var towels = groups[0][0].Split(", ").ToList();
            Dictionary<char, List<string>> towelSet =  [];
            foreach(var towel in towels){
                var c = towel.First();
                if(!towelSet.ContainsKey(c))
                    towelSet[c] = new List<string>();
                towelSet[c].Add(towel);
                towelSet[c] = towelSet[c].OrderByDescending(s => s.Length).ToList();
            }
            var total = 0L;

            var layouts = groups[1];
            foreach(var l in layouts) {
                PrintLn($"{l}");
                Dictionary<string, long> memos = [];
                var count = countPossible(towelSet, l, memos);
                PrintLn($"   {count}");
                total += count;
            }
           
            return $"{total}";
        }

    }
}