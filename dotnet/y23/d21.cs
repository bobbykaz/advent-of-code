

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
            var baseGrid = Utilties.RectangularCharGridFromLines(lines);
            var grid = new Grid<char>(baseGrid.Width*5, baseGrid.Height * 5, '.');
            for(int i = 0; i < 5; i++) {
                for(int j = 0; j < 5; j++) {
                    baseGrid.ForEachColRow((r,c,v) => {
                        char n = v == 'S' ? '.' : v;
                        grid.G[baseGrid.Width*i + r][baseGrid.Width*j + c] = n;
                    });
                }
            }
            var(rs,cs) = (baseGrid.Width*2 + 65,baseGrid.Width*2 + 65);

           var stepMap = new Dictionary<int, List<Cell<char>>>();
            stepMap[0] = new List<Cell<char>>
            {
                new Cell<char>(rs, cs, 'S')
            };
            grid.G[rs][cs] = '.';
            //var vm = new VisitedMap();
            //vm.Visit(rs,cs);
            int NumSteps = 350;
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
            return $"Full grid:{stepMap[65].Count}; Full grid plus 1 grid length: {stepMap[65+131].Count};full grid plus 2 grid length {stepMap[65+262].Count}";
            // fit answer to a quadratic formula
            // each time you move a grid length (131), you number of grids is squared by the side of the grid; 1, 4, 9, etc
            // 65 - number in the first grid
            // 65 + 131 - gets you to the center of any of the '2nd' grids, then explores that whole grid, etc
            // plug in the proper value for (26501365 - 65) / 131 = 202300
            // is this right?
        }

    }
}
