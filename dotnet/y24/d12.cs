using System.Diagnostics.Contracts;
using Grids;

namespace y24 {
    public class D12: AoCDay {

        public D12(): base(24, 12) {
            _DebugPrinting = false;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var chargrid = Utilties.RectangularCharGridFromLines(lines);
            var total = 0L;

            var plots = new List<Plot>();
            var vm = new VisitedMap();
            chargrid.ForEachRowCol((r,c,v) => {
                if(!vm.WasVisited(r,c)) {
                    var plotArea = getPlotFrom(chargrid, r,c);
                    plots.Add(new Plot(v, plotArea));
                    foreach(var p in plotArea) {
                        vm.Visit(p.R, p.C);
                    }
                }
            });

            foreach(var plot in plots) {
                total += plot.ScorePlot(chargrid);
            }
           
            return $"{total}";
        }

        private List<Pos> getPlotFrom(Grid<char> g, int r, int c) {
            var vm = new VisitedMap();

            var stack = new Stack<Cell<char>>();
            vm.Visit(r,c);
            var startingCell = g.GetCellIfValid(r,c) ?? throw new Exception();
            stack.Push(startingCell);
            var root = g.G[r][c];
            while(stack.Count > 0) {
                var next = stack.Pop();
                foreach(var n in g.CardinalNeighbors(next.R, next.C)) {
                    if(n.V == root && !vm.WasVisited(n.R, n.C)) {
                        vm.Visit(n.R,n.C);
                        stack.Push(n);
                    }
                }
            }

            return vm.VisitedPositions();
        }

        private class Plot{
            public char Plant;
            public long Perimiter;
            public List<Pos> Positions = [];
            public long Area {get {return Positions.Count;}}

            public Plot(char pl, List<Pos> pos) {
                Plant = pl;
                Positions = pos;
            }

            public long ScorePlot(Grid<char> g) {
                Perimiter = 0L;
                var perimEdge = 0L;
                foreach(var p in Positions) {
                    var neighbors = g.CardinalNeighbors(p.R, p.C);
                    var edgeFences = 4 - neighbors.Count;
                    var fences = neighbors.Where(c => c.V != Plant).ToList().Count;
                    perimEdge += edgeFences;
                    Perimiter += fences;
                }
                Perimiter += perimEdge;
                return Area * Perimiter;
            }

            public long ScoreDiscountedPlot(Grid<char> g) {
                List<Pos> posOnPerim = [];
                foreach(var p in Positions) {
                    var n = g.CardinalNeighbors(p.R,p.C);
                    var isInternal = n.Count(c => c.V == Plant) == 4;
                    if(!isInternal){
                        posOnPerim.Add(p);
                    }
                }

                HashSet<PosWithDir> edges = [];
                foreach(var p in posOnPerim) {
                    var u = g.GetNeighbor(p.R,p.C, Dir.N);
                    if(!u.HasValue || u.Value.V != Plant)
                        edges.Add(new PosWithDir(p.R-1, p.C, Dir.N));

                    var d = g.GetNeighbor(p.R,p.C, Dir.S);
                    if(!d.HasValue || d.Value.V != Plant)
                        edges.Add(new PosWithDir(p.R+1, p.C, Dir.S));

                    var l = g.GetNeighbor(p.R,p.C, Dir.W);
                    if(!l.HasValue || l.Value.V != Plant)
                        edges.Add(new PosWithDir(p.R, p.C-1, Dir.W));

                    var r = g.GetNeighbor(p.R,p.C, Dir.E);
                    if(!r.HasValue || r.Value.V != Plant)
                        edges.Add(new PosWithDir(p.R, p.C+1, Dir.E));
                }

                var sides = 0;
                sides += UniqNS(edges, Dir.N);
                sides += UniqNS(edges, Dir.S);
                sides += UniqEW(edges, Dir.E);
                sides += UniqEW(edges, Dir.W);
                return Area * (sides);
            }

            private int UniqNS(HashSet<PosWithDir> edges, Dir d) {
                var filteredEdges = edges.Where(e => e.Dir == d).ToList().OrderBy(e => e.R).ThenBy(e => e.C).ToList();
                if(filteredEdges.Count == 0)
                    return 0; 

                int sides = 0;
                bool newEdge = true;
                for( int i = 0; i < filteredEdges.Count - 1; i++) {
                    if(newEdge) {
                        sides++;
                        newEdge = false;
                    }
                    var cur = filteredEdges[i];
                    var next = filteredEdges[i+1];
                    if(cur.R == next.R && next.C == (cur.C + 1)) {
                        newEdge = false;
                    } else {
                        newEdge = true;
                    }
                }

                if(newEdge) { sides++; }
                return sides;
            }

            private int UniqEW(HashSet<PosWithDir> edges, Dir d) {
                var filteredEdges = edges.Where(e => e.Dir == d).ToList().OrderBy(e => e.C).ThenBy(e => e.R).ToList();
                if(filteredEdges.Count == 0)
                    return 0; 

                int sides = 0;
                bool newEdge = true;
                for( int i = 0; i < filteredEdges.Count - 1; i++) {
                    if(newEdge) {
                        sides++;
                        newEdge = false;
                    }
                    var cur = filteredEdges[i];
                    var next = filteredEdges[i+1];
                    if(cur.C == next.C && next.R == (cur.R + 1)) {
                        newEdge = false;
                    } else {
                        newEdge = true;
                    }
                }

                if(newEdge) { sides++; }
                return sides;
            }
            
        }

        public override string P2()
        {
            var lines = InputAsLines();
            var chargrid = Utilties.RectangularCharGridFromLines(lines);
            var total = 0L;

            var plots = new List<Plot>();
            var vm = new VisitedMap();
            chargrid.ForEachRowCol((r,c,v) => {
                if(!vm.WasVisited(r,c)) {
                    var plotArea = getPlotFrom(chargrid, r,c);
                    plots.Add(new Plot(v, plotArea));
                    foreach(var p in plotArea) {
                        vm.Visit(p.R, p.C);
                    }
                }
            });

            foreach(var plot in plots) {
                total += plot.ScoreDiscountedPlot(chargrid);
            }
           
            return $"{total}";
        }

    }
}