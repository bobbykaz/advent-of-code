using Grids;

namespace y23 {
    public class D16 : AoCDay
    {
        public D16(): base(23, 16) {
            _DebugPrinting = false;
        }

        public override string P1()
        {
            var lines = InputAsLines();
            var grid = Utilties.RectangularCharGridFromLines(lines);
            
            return $"{energy(grid, 0, 0, Dir.E)}";
        }

        public int energy(Grid<char> grid, int r, int c, Dir startDir) {
            var egrid = Utilties.NGrid(grid.Width, grid.Height, 0);

            Dictionary<string, bool> seenMap = new Dictionary<string, bool>();

            Queue<(Cell<char>, Dir)> q = new Queue<(Cell<char>, Dir)>();
            q.Enqueue((new Cell<char>(r,c,grid.G[r][c]), startDir));

            while(q.Count > 0) {
                var (cell,dir) = q.Dequeue();
                seenMap[$"{cell.R}-{cell.C}-{dir}"] = true;
                egrid.G[cell.R][cell.C] += 1;
                var beams = Next(grid, cell.R, cell.C, dir);
                foreach(var item in beams) {
                    if(!seenMap.ContainsKey($"{item.Item1.R}-{item.Item1.C}-{item.Item2}"))
                        q.Enqueue(item);
                }
            }

            var total = 0;
            var combo = 0;
            egrid.ForEachColRow((r,c,v) => {
                if(v > 0) {
                    total++;
                    combo+= v;
                }
            });
            return total;
        }
        public Dir[] BeamTravel(char current, Dir dir) {
            return (current, dir) switch {
                ('.', _) => new Dir[]{ dir },
                ('/', Dir.S) => new Dir[]{ Dir.W },
                ('/', Dir.W) => new Dir[]{ Dir.S },
                ('/', Dir.N) => new Dir[]{ Dir.E },
                ('/', Dir.E) => new Dir[]{ Dir.N },
                ('\\', Dir.N) => new Dir[]{ Dir.W },
                ('\\', Dir.W) => new Dir[]{ Dir.N },
                ('\\', Dir.S) => new Dir[]{ Dir.E },
                ('\\', Dir.E) => new Dir[]{ Dir.S },
                ('|', Dir.E) => new Dir[]{ Dir.N, Dir.S },
                ('|', Dir.W) => new Dir[]{ Dir.N, Dir.S },
                ('|', _) => new Dir[]{ dir},
                ('-', Dir.S) => new Dir[]{ Dir.E },
                ('-', _) => new Dir[]{ dir },
                _ => throw new Exception($"unexpected input {current}, {dir}")
            };
        }

        public List<(Cell<char>, Dir)> Next(Grid<char> grid, int cr, int cc, Dir dir) {
            var rslt = new List<(Cell<char>, Dir)>();
            var newDirs = new List<Dir>();
            var current = grid.G[cr][cc];
            switch(current) {
                case '.':
                    newDirs.Add(dir);
                    break;
                case '/':
                    var flip = dir switch {
                        Dir.S => Dir.W,
                        Dir.W => Dir.S,
                        Dir.N => Dir.E,
                        Dir.E => Dir.N,
                        _ => throw new Exception("bad dir")
                    };
                    newDirs.Add(flip);
                    break;
                case '\\':
                    var flip2 = dir switch {
                        Dir.N => Dir.W,
                        Dir.W => Dir.N,
                        Dir.S => Dir.E,
                        Dir.E => Dir.S,
                        _ => throw new Exception("bad dir")
                    };
                    newDirs.Add(flip2);
                    break;
                case '|':
                    if (dir == Dir.S || dir == Dir.N){
                        newDirs.Add(dir);
                    } else {
                        newDirs.Add(Dir.N);
                        newDirs.Add(Dir.S);
                    }
                    break;
                case '-':
                    if (dir == Dir.E || dir == Dir.W){
                        newDirs.Add(dir);
                    } else {
                        newDirs.Add(Dir.E);
                        newDirs.Add(Dir.W);
                    }
                    break;
                default:
                    throw new Exception("bad tile");
            }

            foreach(var nd in newDirs) {
                var n = grid.GetNeighbor(cr,cc, nd);
                if(n != null) 
                    rslt.Add((n.Value, nd));
            }

            return rslt;
        }

    
        public override string P2()
        {
            var lines = InputAsLines();
            var grid = Utilties.RectangularCharGridFromLines(lines);
            var max = 0;
            for(int r = 0; r < grid.Height; r++) {
                var e = energy(grid, r, 0, Dir.E);
                PrintLn($"{r}: E - {e}");
                if(e > max) max = e;
            }

            for(int r = 0; r < grid.Height; r++) {
                var e = energy(grid, r, grid.LastColIndex, Dir.W);
                PrintLn($"{r}: W - {e}");
                if(e > max) max = e;
            }

            for(int c = 0; c < grid.Width; c++) {
                var e = energy(grid, 0, c, Dir.S);
                PrintLn($"{c}: S - {e}");
                if(e > max) max = e;
            }

            for(int c = 0; c < grid.Width; c++) {
                var e = energy(grid, grid.LastRowIndex, c, Dir.N);
                PrintLn($"{c}: N - {e}");
                if(e > max) max = e;
            }
            
            return $"{max}";
        }
    }
}