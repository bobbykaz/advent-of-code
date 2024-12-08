namespace y24 {
    public class D5: AoCDay {

        public D5(): base(24, 5) {
            _DebugPrinting = false;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var blocks = Utilties.GroupInputAsBlocks(lines, "");
            PrintLn($"Blocks: {blocks.Count}");
            var rulesRaw = blocks[0];
            var updates = blocks[1];
            var rules = new Dictionary<int, Rule>();
            long total = 0;
            foreach (var (line,l) in rulesRaw.ForEachIndex()) {
                var pts = line.Split("|").Select(int.Parse).ToList() ?? throw new Exception();

                if(!rules.ContainsKey(pts[0])) {
                    rules.Add(pts[0], new Rule(pts[0]));
                }

                if(!rules.ContainsKey(pts[1])) {
                    rules.Add(pts[1], new Rule(pts[1]));
                }

                var before = rules[pts[0]];
                var after = rules[pts[1]];
                before.MustBeBefore.Add(after.Val);
                after.MustBeAfter.Add(before.Val);
            }

            PrintLn($"Rules: {rules.Count}");
            foreach(var r in rules) {
                PrintLn(r.Value.ToString());
            }

            foreach (var (line,l) in updates.ForEachIndex()) {
                PrintLn($"\n===\nEval: {line}");
                var n = Utilties.StringToNums<int>(line, ",");
                if(checkUpdate(rules, n)){
                    var m = middlePage(n);
                    total += m;
                    PrintLn($"update {line} valid; adding {m}; total {total}");
                }
            }
            return $"{total}";
        }

        private bool checkUpdate(Dictionary<int, Rule> rules, List<int> nums) {
            foreach( var (n,i) in nums.ForEachIndex()) {
                var rule = rules[n] ?? throw new Exception();
                PrintLn($"Checking {rule}\n");
                var isAfterNums = nums.Take(i-1);
                var isBeforeNums = nums.Skip(i);
                foreach(var nn in isAfterNums){
                    if(rule.MustBeBefore.Contains(nn)){
                        PrintLn($"  Failed on {nn} - in After");
                        return false;
                    }
                }

                foreach(var nn in isBeforeNums){
                    if(rule.MustBeAfter.Contains(nn)){
                        PrintLn($"  Failed on {nn} - in Before");
                        return false;
                    }
                }   
            }

            return true;
        }

        private int middlePage(List<int> nums) {
            return nums[(nums.Count / 2)];
        }

        public class Rule {
            public int Val;
            public HashSet<int> MustBeBefore;
            public HashSet<int> MustBeAfter;
            public Rule(int number){
                Val = number;
                MustBeBefore = new HashSet<int>();
                MustBeAfter = new HashSet<int>();
            }

            public override string ToString()
            {
                return $"{Val} \n Before: {string.Join(",", MustBeBefore.ToArray())}\n After: {string.Join(",", MustBeAfter.ToArray())}";
            }
        }

        private class RulesComparer : IComparer<int>
        {
            private Dictionary<int, Rule> Rules;
            public RulesComparer(Dictionary<int, Rule> rules) {
                Rules = rules;
            }
            public int Compare(int x, int y)
            {
                var xr = Rules[x];
                if(xr.MustBeBefore.Contains(y)) {
                    return -1;
                }

                if (xr.MustBeAfter.Contains(y)) {
                    return 1;
                }

                return 0;
            }

        }

        private int sortNums(Dictionary<int, Rule> rules, List<int> nums) {
            var comp = new RulesComparer(rules);
            nums.Sort(comp);
            return middlePage(nums);
        }

        private int reorderNums(Dictionary<int, Rule> rules, List<int> nums) {
            var chosen = new List<int>();
            PrintLn($"Checking: {string.Join(",", nums)}");
            while(nums.Any()) {
                var anyFound = false;
                foreach(var (n,i) in nums.ForEachIndex()){
                    var remaining = new List<int>(nums).Where(s => s != n).ToList();
                    PrintLn($"  {n} - Remaining: {string.Join(",", remaining)}");
                    //n must be AFTER everything in newList and BEFORE everything in remaining
                    var rule = rules[n] ?? throw new Exception();

                    var anyRemainingShouldBeBefore = remaining.Any(x => rule.MustBeBefore.Contains(x));
                    var anyAlreadyChosenShouldBeAfter = chosen.Any(x => rule.MustBeAfter.Contains(x));
                    if(!anyAlreadyChosenShouldBeAfter && !anyRemainingShouldBeBefore) {
                        PrintLn($"  adding {n} to list");
                        nums.RemoveAt(i);
                        chosen.Add(n);
                        PrintLn($" now {string.Join(",", chosen)}");
                        anyFound = true;
                        break;
                    }
                }
                if(!anyFound){
                    PrintLn("  Didnt find anything");
                    throw new Exception();
                }
            }

            chosen.Reverse();

            PrintLn($"Checking final pass of {string.Join(",", chosen)}");
            _DebugPrinting = false;
            if(!checkUpdate(rules, chosen)) {
                _DebugPrinting = true;
                PrintLn($"Something didnt go right!");
            }

            return middlePage(chosen);
        }

        public override string P2()
        {
            var lines = InputAsLines();
            var blocks = Utilties.GroupInputAsBlocks(lines, "");
            PrintLn($"Blocks: {blocks.Count}");
            var rulesRaw = blocks[0];
            var updates = blocks[1];
            var rules = new Dictionary<int, Rule>();
            long total = 0;
            foreach (var (line,l) in rulesRaw.ForEachIndex()) {
                var pts = line.Split("|").Select(int.Parse).ToList() ?? throw new Exception();

                if(!rules.ContainsKey(pts[0])) {
                    rules.Add(pts[0], new Rule(pts[0]));
                }

                if(!rules.ContainsKey(pts[1])) {
                    rules.Add(pts[1], new Rule(pts[1]));
                }

                var before = rules[pts[0]];
                var after = rules[pts[1]];
                before.MustBeBefore.Add(after.Val);
                after.MustBeAfter.Add(before.Val);
            }

            PrintLn($"Rules: {rules.Count}");
            foreach(var r in rules) {
                PrintLn(r.Value.ToString());
            }

            foreach (var (line,l) in updates.ForEachIndex()) {
                PrintLn($"\n===\nEval: {line}");
                var n = Utilties.StringToNums<int>(line, ",");
                if(!checkUpdate(rules, n)){
                    _DebugPrinting = true;
                    var m = reorderNums(rules, n);
                    total += m;
                    PrintLn($"update {line} valid; adding {m}; total {total}");
                    _DebugPrinting = false;
                }
            }
            return $"{total}";
        }

    }
}