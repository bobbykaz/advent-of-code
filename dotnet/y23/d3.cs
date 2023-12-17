using Grids;

namespace y23 {
    public class D3 {
        public void Run(){
            P1();
            P2();
        }
        public void P1() {
            var lines = Utilties.ReadFileToLines("../input/y23/d3.txt");
            var grid = Utilties.RectangularCharGridFromLines(lines);
            var parts = new List<int>();

            for(var r = 0; r < grid.Height; r++){
                var c = 0;
                while(c < grid.Width) {
                    if (char.IsDigit(grid.G[r][c])) {
                        var (part, endC) = ExtractNum(grid, r, c);
                        if (PartNextToSymbol(grid, r, c, endC)) {
                            parts.Add(part);
                        }
                        c = endC;
                    }
                    c++;
                }
            }
            Console.WriteLine(parts.Sum());
        }

        // Returns parsed number, endCol of last number
        private (int, int) ExtractNum(Grid<char> g, int r, int cStart) {
            string n = "" + g.G[r][cStart];
            for( var c = cStart + 1; c < g.Width; c++) {
                if (char.IsDigit(g.G[r][c])) {
                    n += g.G[r][c];
                } else {
                    return (int.Parse(n), c-1);
                }
            }

            return (int.Parse(n), g.Width-1);
        }

        private bool PartNextToSymbol(Grid<char> g, int r, int cs, int ce) {
            for(var c = cs; c <= ce; c++) {
                var n = g.AllNeighbors(r,c);
                if(n.Any(ch => !char.IsDigit(ch.V) && ch.V != '.')) {
                    return true;
                }
            }
            return false;
        }

        public void P2() {
            var lines = Utilties.ReadFileToLines("../input/y23/d3.txt");
            var grid = Utilties.RectangularCharGridFromLines(lines);
            var partMap = new Dictionary<int, int>();
            var partId = 1;
            var pGrid = Utilties.NGrid(grid.Width, grid.Height, -1);
            var ratios = new List<int>();
            for(var r = 0; r < grid.Height; r++){
                var c = 0;
                while(c < grid.Width) {
                    if (char.IsDigit(grid.G[r][c])) {
                        var (part, endC) = ExtractNum(grid, r, c);
                        if (PartNextToSymbol(grid, r, c, endC)) {
                            for(var pc = c; pc <= endC; pc++) {
                                pGrid.G[r][pc] = partId;
                                partMap[partId] = part;
                            }
                            Console.WriteLine($"new part {partId} - {part}");
                            partId++;
                        } 
                        c = endC;
                    }
                    c++;
                }
            }

            for(var r = 0; r < grid.Height; r++){
                 for(var c = 0; c < grid.Width; c++){
                    if (grid.G[r][c] == '*') {
                        Console.WriteLine($"checking {r}, {c}");
                        var N = pGrid.AllNeighbors(r,c);
                        N.ForEach(x => Console.Write($"{x.V},"));
                        
                        var uniqN = N.Select(cell => cell.V).Where(i => i != -1).Distinct().ToList();
                        Console.WriteLine("/ valid: ");
                        uniqN.ForEach(x => Console.Write($"{x},"));
                        Console.WriteLine("");
                        if(uniqN != null && uniqN.Count == 2) {
                            Console.WriteLine("... is gear!");
                            ratios.Add(partMap[uniqN[0]] * partMap[uniqN[1]]);
                        }
                    }
                }
            }
            Console.WriteLine("Ratios: ");
            Console.WriteLine(ratios.Sum());
        }
    }
}