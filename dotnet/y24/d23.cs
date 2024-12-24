namespace y24 {
    public class D23: AoCDay {

        public D23(): base(24, 23) {
            _DebugPrinting = true;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            Dictionary<string, HashSet<string>> connections = [];
            foreach(var l in lines){
                var pts = l.Split("-");
                process(connections, pts[0], pts[1]);
                process(connections, pts[1], pts[0]);
            }
            HashSet<Result> rslts = [];
            foreach(var p1 in connections.Keys) {
                if(p1.StartsWith('t')){
                    foreach(var p2 in connections[p1]){
                        foreach(var p3 in connections[p2]){
                            if(connections[p3].Contains(p1)){
                                var ordered = new List<string>(){p1,p2,p3};
                                ordered.Sort();
                                rslts.Add(new Result(ordered[0], ordered[1], ordered[2]));
                            }
                        }
                    }
                }
            }

            var total = rslts.Count;
           
            return $"{total}";
        }

        private record struct Result(string one, string two, string three);

        private void process(Dictionary<string, HashSet<string>> connections, string from, string to) {
            if(!connections.ContainsKey(from))
                connections[from] = new HashSet<string>();
            connections[from].Add(to);
        }


        private HashSet<string> BronKerbosch(Dictionary<string, HashSet<string>> connections){
            return BronKerboschHelper(connections, [], connections.Keys.ToHashSet(), []);
        }

        private HashSet<string> BronKerboschHelper(Dictionary<string, HashSet<string>> connections, HashSet<string> R, HashSet<string> P, HashSet<string> X) {
            if(P.Count == 0 && X.Count == 0) {
                PrintLn($"Clique {string.Join(",",R)} found");
                return R;
            }
            var nR = new HashSet<string>(R);
            var nP = new HashSet<string>(P);
            var nX = new HashSet<string>(X);
            HashSet<string> best = [];
            foreach(var v in P) {

                var tR = new HashSet<string>(nR);
                var tP = new HashSet<string>(nP);
                var tX = new HashSet<string>(nX);
                tR.Add(v);
                tP.IntersectWith(connections[v]);
                tX.IntersectWith(connections[v]);
                var subResult = BronKerboschHelper(connections, 
                                                    tR, tP, tX
                                                    );
                if(subResult.Count > best.Count) {
                    best = subResult;
                }
                nP.Remove(v);
                nX.UnionWith(new List<string>(){v});
            }

            return best;
        }

        public override string P2()
        {
            var lines = InputAsLines();
            Dictionary<string, HashSet<string>> connections = [];
            foreach(var l in lines){
                var pts = l.Split("-");
                process(connections, pts[0], pts[1]);
                process(connections, pts[1], pts[0]);
            }
            var result = BronKerbosch(connections);
            var ordered = result.ToList().OrderBy(s => s).ToList();
            var answer = string.Join(",", ordered);
           
            return $"{answer}";
        }

    }
}