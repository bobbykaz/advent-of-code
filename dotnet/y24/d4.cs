using System.Diagnostics;
using System.Numerics;
using Grids;

namespace y24 {
    public class D4: AoCDay {

        public D4(): base(24, 4) {
            _DebugPrinting = false;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            long total = 0;
            var grid = Utilties.RectangularCharGridFromLines(lines);
            if(_DebugPrinting) {
                grid.Print();
            }
            grid.ForEachRowCol((r,c,v) => {
                if(v == 'X') {
                    PrintLn($"{r} {c}: {v} (running total: {total})");
                    if ("MAS" == getNCharsInDir(grid, r,c, "U", 3)) {
                        total += 1;
                    }
                    if ("MAS" == getNCharsInDir(grid, r,c, "UR", 3)) {
                        total += 1;
                    }
                    if ("MAS" == getNCharsInDir(grid, r,c, "UL", 3)) {
                        total += 1;
                    }
                    if ("MAS" == getNCharsInDir(grid, r,c, "L", 3)) {
                        total += 1;
                    }
                    if ("MAS" == getNCharsInDir(grid, r,c, "R", 3)) {
                        total += 1;
                    }
                    if ("MAS" == getNCharsInDir(grid, r,c, "D", 3)) {
                        total += 1;
                    }
                    if ("MAS" == getNCharsInDir(grid, r,c, "DR", 3)) {
                        total += 1;
                    }
                    if ("MAS" == getNCharsInDir(grid, r,c, "DL", 3)) {
                        total += 1;
                    }
                }
            });
            return $"{total}";
        }

        private (int, int) next(int r, int c, string dir) {
            switch(dir) {
                case "D": return (r+1, c);
                case "U": return (r-1, c);
                case "L": return (r, c-1);
                case "R": return (r, c+1);
                case "DR": return (r+1, c+1);
                case "DL": return (r+1, c-1);
                case "UR": return (r-1, c+1);
                case "UL": return (r-1, c-1);
                default: throw new Exception("wrong dir");
            }
        }

        private string getNCharsInDir(Grid<char> grid, int r, int c, string dir, int n) {
            string result = "";
            var (nr, nc) = next(r,c, dir);
            for( int i = 0; i < n; i++) {
                var nextCell = grid.GetCellIfValid(nr, nc);
                if(!nextCell.HasValue) break;
                result += nextCell.Value.V;
                (nr, nc) = next(nr,nc, dir);
            }
            PrintLn($"   {dir}: {result}");
            return result;
        }

        public override string P2()
        {
            var lines = InputAsLines();
            long total = 0;
            var grid = Utilties.RectangularCharGridFromLines(lines);
            if(_DebugPrinting) {
                grid.Print();
            }
            grid.ForEachRowCol((r,c,v) => {
                if(v == 'A') {
                    PrintLn($"{r} {c}: {v} (running total: {total})");
                    var ur = getNCharsInDir(grid, r,c, "UR", 1);
                    var ul = getNCharsInDir(grid, r,c, "UL", 1);
                    var dr = getNCharsInDir(grid, r,c, "DR", 1);
                    var dl = getNCharsInDir(grid, r,c, "DL", 1);
                    
                    List<string> d1 = [ur, dl];
                    List<string> d2 = [ul, dr];
                    d1.Sort();
                    d2.Sort();
                    if (d1[0] == "M" && d1[1] == "S" && d2[0] == "M" && d2[1] == "S")
                        total += 1;
                }
            });
            return $"{total}";
        }

    }
}