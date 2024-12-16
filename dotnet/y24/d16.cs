using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Grids;

namespace y24 {
    public class D16: AoCDay {

        public D16(): base(24, 16) {
            _DebugPrinting = false;
        }

        private long BestScore = 0L;
        private Dictionary<long, HashSet<Pos>> VisitedPathMap = [];
        public override string P1()
        {
            var lines = InputAsLines();
            var grid = Utilties.RectangularCharGridFromLines(lines);
            var start = new PosWithDir(0,0,Dir.E);
            var end = new Pos(0,0);
            grid.ForEachRowCol((r,c,v)=>{
                if(v == 'S') {
                    start = new PosWithDir(r,c, Dir.E);
                }

                if(v == 'E') {
                    end = new Pos(r,c);
                }
            });

            var branchs = new Queue<Branch>();
            var init = new Branch(0, start, new VisitedMap());
            init.VM.Visit(start.R,start.C);
            branchs.Enqueue(init);
            var cache = new Dictionary<String, long>();
            long minScore = long.MaxValue;
            PrintLn($"Start: {start}; end {end}");
            while(branchs.Any()) {
                var nb = branchs.Dequeue();
                if(nb.AtTarget(end)){
                    PrintLn($"Branch at end; Score {nb.Score}, current min {minScore}");
                    if(nb.Score < minScore) {
                        minScore = nb.Score;
                    }
                    CloseBranch(nb);
                } else {
                    nb.AdvanceToChoice(grid, end);
                    PrintLn($"Advanced to {nb.Current}, score {nb.Score}");
                    if(nb.AtTarget(end)){
                        PrintLn($"Branch at end; Score {nb.Score}, current min {minScore}");
                        if(nb.Score < minScore) {
                            minScore = nb.Score;
                        }
                        CloseBranch(nb);
                    } else {
                        var multiverse = nb.Split(grid);
                        PrintLn($"Splitting to {multiverse.Count()}; current min {minScore}; branches = {branchs.Count()}");
                        foreach(var n in multiverse) {
                            if(cache.ContainsKey(n.Current.Key)) {
                                var prevScore = cache[n.Current.Key];
                                if(n.Score <= prevScore) {
                                    cache[n.Current.Key] = n.Score;
                                    branchs.Enqueue(n);
                                }
                            } else {
                                cache[n.Current.Key] = n.Score;
                                branchs.Enqueue(n);
                            }
                        }
                        //prune
                        
                        var prunedBranches = branchs.ToList().OrderBy(b => b.Score).ToList();
                        branchs.Clear();
                        foreach(var b in prunedBranches) branchs.Enqueue(b);
                    }
                }
            }
            BestScore = minScore;
            return $"{minScore}";
        }

        private void CloseBranch(Branch b){
            if(!VisitedPathMap.ContainsKey(b.Score)){
                VisitedPathMap[b.Score] = new HashSet<Pos>();
            }
            foreach(var p in b.VM.VisitedPositions()) {
                VisitedPathMap[b.Score].Add(p);
            }
        }

        private class Branch {
            public long Score;
            public PosWithDir Current;
            public VisitedMap VM;

            public Branch(long s, PosWithDir c, VisitedMap vm) {
                Score = s;
                Current = c;
                VM = vm;
            }

            public bool AtTarget(Pos t) {
                return Current.C == t.C && Current.R == t.R;
            }
            public void AdvanceToChoice(Grid<char> g, Pos target) {
                var neighbors = GetNextChoices(g);
                
                while(neighbors.Count() == 1) {
                    var (nextCell, nextDir) = neighbors[0];
                    var rotScore = rotateTo(Current.Dir, nextDir);
                    Score += rotScore;
                    Current = new PosWithDir(nextCell.R, nextCell.C, nextDir);
                    VM.Visit(nextCell.R, nextCell.C);
                    Score += 1;
                    if(AtTarget(target)) {
                        break;
                    }
                    neighbors = GetNextChoices(g);
                }
            }

            public List<Branch> Split(Grid<char> g) {
                var results = new List<Branch>();
                var neighbors = GetNextChoices(g);
                if(neighbors.Count() == 0) return results;
                if(neighbors.Count() == 1) throw new Exception();

                foreach(var (next, nextDir) in neighbors){
                    var nextBranch = new Branch(Score, new PosWithDir(Current.R, Current.C, Current.Dir), VM.Copy());
                    var rotScore = rotateTo(nextBranch.Current.Dir, nextDir);
                    nextBranch.Score += rotScore;
                    nextBranch.Current = new PosWithDir(next.R, next.C, nextDir);
                    nextBranch.VM.Visit(next.R, next.C);
                    nextBranch.Score += 1;
                    results.Add(nextBranch);
                }
                return results;
            }

            private List<(Cell<char>,Dir)> GetNextChoices(Grid<char> g) {
                return g.CardinalNeighborsWithDir(Current.R, Current.C)
                .Where(c => 
                        c.Item1.V != '#' 
                        && !VM.WasVisited(c.Item1.R, c.Item1.C)
                    )
                .ToList();

            }
            private long rotateTo(Dir From, Dir To) {
            if(From == To) {
                return 0;
            }

            if(From == GridUtilities.OppositeDir(To)){
                return 2000;
            }

            return 1000;
        }
        }

        public override string P2()
        {
            // var lines = InputAsLines();
            // var grid = Utilties.RectangularCharGridFromLines(lines);
            // foreach(var p in VisitedPathMap[BestScore]) {
            //     grid.G[p.R][p.C] = 'O';
            // }

            // grid.Print();
            var target = VisitedPathMap[BestScore].Count();
           
            return $"{target}";
        }

    }
}