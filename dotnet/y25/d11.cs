using System.Security.Cryptography;

namespace y25 {
    public class D11: AoCDay {

        public D11(): base(25, 11) {
            _DebugPrinting = false;
            _UseSample = false;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var total = 0L;
            foreach (var line in lines)
            {
                var n = parse(line);
                _Lookup[n.Name] = n;
                PrintLn($"{n.Name} : {string.Join(",",n.Children)}");
            }
            if(!_Lookup.ContainsKey("out"))
                _Lookup["out"] = new Node("out");
            
            return "Skipping p1";
            
            var start = _Lookup["you"];
            var queue = new Queue<(string, string)>();
            queue.Enqueue((start.Name, ""));
            var seen = new HashSet<string>();
            while (queue.Any())
            {
                var next = queue.Dequeue();
                var n = next.Item1;
                var history = next.Item2;
                var newHistory = history + "," + n;
                if (!seen.Contains(newHistory))
                {
                    seen.Add(newHistory);
                    var node = _Lookup[n];
                    foreach (var cn in node.Children)
                    {
                        if (cn == "out")
                        {
                            total++;
                        }
                        else
                        {
                            queue.Enqueue((cn, newHistory));
                        }
                    }
                }
            }
            
            return $"total {total}";
        }

        private Dictionary<string, Node> _Lookup = [];

        private Node parse(string line)
        {
            var pts = line.Split(": ");
            var children = pts[1].Split(" ").ToHashSet();
            var n = new Node(pts[0]);
            n.Children = children;
            return n;
        }
        
        private record Node(string Name) {
            public HashSet<string> Children = [];
            public HashSet<string> Parents = [];
        }

        private void BuildParents()
        {
            foreach (var node in _Lookup.Values)
            {
                foreach (var childName in node.Children)
                {
                    var childNode = _Lookup[childName];
                    childNode.Parents.Add(node.Name);
                }
            }
        }

        private long FindPathsUniq(string from, string to, HashSet<string>? notContaining = null)
        {
            PrintLn($"Finding unique paths from {from} to {to}");
            var total = 0L;
            var start = _Lookup[from];
            var queue = new Queue<(string, string)>();
            queue.Enqueue((start.Name, ""));
            var seen = new HashSet<string>();
            while (queue.Any())
            {
                var next = queue.Dequeue();
                var n = next.Item1;
                var history = next.Item2;
                var newHistory = history + "," + n;
                if (!seen.Contains(newHistory))
                {
                    seen.Add(newHistory);
                    var node = _Lookup[n];
                    foreach (var cn in node.Children)
                    {
                        if (cn == to)
                        {
                            PrintLn($"  {newHistory},{to}");
                            total++;
                        }
                        else
                        {
                            if(notContaining == null)
                                queue.Enqueue((cn, newHistory));
                            else
                            {
                                if (!notContaining.Contains(cn))
                                    queue.Enqueue((cn, newHistory));
                            }
                        }
                    }
                }
            }
            
            PrintLn($"  = Unique paths {from} to {to}: {total}");

            return total;
        }

        private long Recurse(string next, bool passedDac, bool passedFft, Dictionary<string, long> memos)
        {
            if (next == "out" && passedDac && passedFft)
                return 1;

            var key = $"{next}-{passedDac}-{passedFft}";
            if (memos.ContainsKey(key))
            {
                return memos[key];
            }
            else
            {
                var node = _Lookup[next];
                var total = 0L;
                foreach (var child in node.Children)
                {
                    var subDac = passedDac || child == "dac";
                    var subFft = passedFft || child == "fft";
                    total += Recurse(child, subDac, subFft, memos);
                }

                memos[key] = total;
                return total;
            }
        }
        
        public override string P2()
        {
            _DebugPrinting = true;
            var memos = new Dictionary<string, long>();
            var total = Recurse("svr", false, false, memos);

            return $"total {total}";
        }
    }
}