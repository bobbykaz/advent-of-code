

using System.Collections.Specialized;
using System.Dynamic;
using Grids;
using Microsoft.VisualBasic;

namespace y23 {
    public class D21 : AoCDay
    {
        public D21(): base(23, 21) {
            _DebugPrinting = true;
        }
        
        public override string P1()
        {
            var lines = InputAsLines();
            var grid = Utilties.RectangularCharGridFromLines(lines);
            var(rs,cs) = (0,0);
            grid.ForEachRowCol((r,c,v) =>{
                if(v == 'S') {
                    rs = r;
                    cs = c;
                }
            });
            var stepMap = new Dictionary<int, List<Cell<char>>>();
            stepMap[0] = new List<Cell<char>>
            {
                new Cell<char>(rs, cs, 'S')
            };
            grid.G[rs][cs] = '.';
            //var vm = new VisitedMap();
            //vm.Visit(rs,cs);
            int NumSteps = 64;
            for(int i = 1; i <= NumSteps; i++) {
                stepMap[i] = new List<Cell<char>>();
                var vm = new VisitedMap();
                foreach(var cell in stepMap[i-1]) {
                    var nn = grid.CardinalNeighbors(cell.R, cell.C).Where(c => c.V == '.' && !vm.WasVisited(c.R,c.C)).ToList();
                    foreach(var nc in nn) {
                        vm.Visit(nc.R,nc.C);
                        stepMap[i].Add(nc);
                    }
                }
                PrintLn($"{i}: {stepMap[i].Count}");
            }
            foreach(var c in stepMap[NumSteps]) {
                grid.G[c.R][c.C] = 'O';
            }
            //grid.Print();
            return $"{stepMap[NumSteps].Count}";
        }
        public class MvCell  {
            public HashSet<(int,int)> planes = new HashSet<(int, int)>();
            public Cell<char> Cell;
            public MvCell(Cell<char> c) {
                Cell = c;
            }

            public MvCell(Cell<char> c, HashSet<(int, int)> hs) {
                Cell = c;
                planes = hs;
            }
        }
        public override string P2()
        {
            var lines = InputAsLines();
            var grid = Utilties.RectangularCharGridFromLines(lines);
            var(rs,cs) = (0,0);
            grid.ForEachRowCol((r,c,v) =>{
                if(v == 'S') {
                    rs = r;
                    cs = c;
                }
            });

            var prevCells = new List<MvCell>
            {
                new MvCell(new Cell<char>(rs, cs, 'S'))
            };
            prevCells[0].planes.Add((0,0));
            grid.G[rs][cs] = '.';
            int NumSteps = 26501365;
            for(int i = 1; i <= NumSteps; i++) {
                var nextCells = new Dictionary<string, MvCell>();
                foreach(var mvc in prevCells) {
                    var nn = grid.CardinalNeighborsWrapped(mvc.Cell.R, mvc.Cell.C).Where(c => c.Item1.V == '.').ToList();
                    foreach(var (nc, d, wrapped) in nn) {
                        string key = $"{nc.R}-{nc.C}";
                        if(!wrapped){
                            if(nextCells.ContainsKey(key))
                                nextCells[key].planes.UnionWith(mvc.planes);
                            else 
                                nextCells[key] = new MvCell(nc, mvc.planes);
                        } else {
                            var curPlanes = new HashSet<(int,int)>(mvc.planes);
                            var nextPlanes = curPlanes.Select(p => d switch {
                                Dir.N => (p.Item1-1,p.Item2),
                                Dir.S => (p.Item1+1,p.Item2),
                                Dir.E => (p.Item1,p.Item2+1),
                                Dir.W => (p.Item1,p.Item2-1),
                                _ => throw new Exception()
                            }).ToHashSet();
                            curPlanes.UnionWith(nextPlanes);
                            if(nextCells.ContainsKey(key))
                                nextCells[key].planes.UnionWith(curPlanes);
                            else 
                                nextCells[key] = new MvCell(nc, curPlanes);
                        }
                    }
                }
                prevCells = nextCells.Values.ToList();
                if( i % 100 == 0) {
                    PrintLn($"{i}: {_ElapsedMillis}");
                }
            }
            var total = prevCells.Sum( c => (long)c.planes.Count);
            return $"{total}";

        }

    }
}
