using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using Grids;

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
                before.Before.Add(after.Val);
                after.After.Add(before.Val);
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
                PrintLn($"Checking {rule.ToString()}\n");
                var isAfterNums = nums.Take(i-1);
                var isBeforeNums = nums.Skip(i);
                foreach(var nn in isAfterNums){
                    if(rule.Before.Contains(nn)){
                        PrintLn($"  Failed on {nn} - in After");
                        return false;
                    }
                }

                foreach(var nn in isBeforeNums){
                    if(rule.After.Contains(nn)){
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
            public HashSet<int> Before;
            public HashSet<int> After;
            public Rule(int number){
                Val = number;
                Before = new HashSet<int>();
                After = new HashSet<int>();
            }

            public override string ToString()
            {
                return $"{Val} \n Before: {string.Join(",", Before.ToArray())}\n After: {string.Join(",", After.ToArray())}";
            }
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
                before.Before.Add(after.Val);
                after.After.Add(before.Val);
            }

            PrintLn($"Rules: {rules.Count}");
            foreach(var r in rules) {
                PrintLn(r.Value.ToString());
            }

            foreach (var (line,l) in updates.ForEachIndex()) {
                PrintLn($"\n===\nEval: {line}");
                var n = Utilties.StringToNums<int>(line, ",");
                if(!checkUpdate(rules, n)){
                    var m = middlePage(n);
                    total += m;
                    PrintLn($"update {line} valid; adding {m}; total {total}");
                }
            }
            return $"{total}";
        }

    }
}