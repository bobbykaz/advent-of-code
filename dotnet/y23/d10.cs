namespace y23 {
    public class D10 : AoCDay
    {
        public D10(): base(23, 10) {
            _DebugPrinting = false;
            g = new GridUtils.Grid<char>(1,1,'.');
            seen = new Dictionary<string, bool>();
        }

        private GridUtils.Grid<char> g;
        private Dictionary<string, bool> seen;
        public override string P1()
        {
            var lines = InputAsLines();
            g = GridUtils.RectangularCharGridFromLines(lines);
            
            var (sr,sc) = (0,0);
            g.ForEachRowCol((r,c,v ) => {
                if(v == 'S') {
                    (sr, sc) = (r,c);
                }
            });

            //row, col, dist to tile
            var bfs = new Queue<(int, int, int)>();
            bfs.Enqueue((sr, sc, 0));
            
            seen[$"{sr}-{sc}"] = true;
            int maxDist = 0;
            while(bfs.Any()) {
                var (nr,nc,dist) = bfs.Dequeue();
                if(dist > maxDist) {
                    maxDist = dist;
                }
                var pipe = parse(g.G[nr][nc]);
                var ns = neighbors(pipe, nr, nc);
                foreach(var (nnr, nnc) in ns) {
                    if(! seen.ContainsKey($"{nnr}-{nnc}")) {
                        seen[$"{nnr}-{nnc}"] = true;
                        bfs.Enqueue((nnr,nnc, dist+1));
                    }
                }
            }

            return $"{maxDist}";
        }

        public GridUtils.Grid<char> ExpandGrid(GridUtils.Grid<char> g2) {
            var exG = new GridUtils.Grid<char>(g2.Width * 3, g2.Height * 3, ' ');
            for(var r = 0; r < g2.Height; r++) {
                for(var c = 0; c < g2.Width; c++) {
                    if(g2.G[r][c] != '.') {
                        var (exr,exc) = (r*3 + 1, c*3 + 1);
                        exG.G[exr][exc] = '.';
                        var p = parse(g2.G[r][c]);
                        foreach(var (nr,nc) in neighbors(p,exr,exc)) {
                            exG.G[nr][nc] = '.';
                        }
                    }
                }   
            }

            return exG;

        }

        public List<(int,int)> neighbors(PipeEnum e, int r, int c) {
            return e switch {
                PipeEnum.NS => new List<(int,int)>(){(r-1, c),(r+1, c)},
                PipeEnum.EW => new List<(int,int)>(){(r, c-1),(r, c+1)},
                PipeEnum.NE => new List<(int,int)>(){(r-1, c),(r, c+1)},
                PipeEnum.NW => new List<(int,int)>(){(r-1, c),(r, c-1)},
                PipeEnum.SE => new List<(int,int)>(){(r+1, c),(r, c+1)},
                PipeEnum.SW => new List<(int,int)>(){(r+1, c),(r, c-1)},
                _ => throw new Exception("bad pipe enum")
            };
        }


        /*
        | is a vertical pipe connecting north and south.
        - is a horizontal pipe connecting east and west.
        L is a 90-degree bend connecting north and east.
        J is a 90-degree bend connecting north and west.
        7 is a 90-degree bend connecting south and west.
        F is a 90-degree bend connecting south and east.
        . is ground; there is no pipe in this tile.
        S is the starting position of the animal; there is a pipe on this tile, but your sketch doesn't show what shape the pipe has.
        */

        public enum PipeEnum {
            NS,
            EW,
            NE,
            NW,
            SW,
            SE
        }

        public PipeEnum parse( char c) {
            return c switch {
                '|' => PipeEnum.NS,
                '-' => PipeEnum.EW,
                'L' => PipeEnum.NE,
                'J' => PipeEnum.NW,
                '7' => PipeEnum.SW,
                'F' => PipeEnum.SE,
                'S' => PipeEnum.SE, // for my input
                _ => throw new Exception($"parsed bad pipe char {c}")
            };
        }

        public override string P2()
        {
            var g2 = GridUtils.RectangularCharGridFromLines(InputAsLines());
            g2.ForEachRowCol((r,c,v ) => {
                if(!seen.ContainsKey($"{r}-{c}")) {
                        g2.G[r][c] = '.';
                }
            });

            //we have just the pipes of the main loop now;
            // expand the grid to 3x the size so the space between the pipes becomes a clear character;
            // floodfill bfs from the corner
            var exG = ExpandGrid(g2);
            seen = new Dictionary<string, bool>();
            var bfs2 = new Queue<(int,int)>();
            bfs2.Enqueue((0,0));
            seen[$"0-0"] = true;
            while(bfs2.Any()){
                var (r,c) = bfs2.Dequeue();
                var ns = exG.CardinalNeighbors(r,c);
                foreach(var cell in ns) {
                    if(cell.V != '.' && !seen.ContainsKey($"{cell.R}-{cell.C}")) {
                        seen[$"{cell.R}-{cell.C}"] = true;
                        bfs2.Enqueue((cell.R, cell.C));
                    }
                }
            }

            var enclosed = 0;
            // look at non-pipe, not seen corresponding spots of exG
            g2.ForEachRowCol((r,c,v) => {
                var (exR,exC) = (r*3 + 1, c*3 + 1); 
                if(g2.G[r][c] == '.') {
                    if(!seen.ContainsKey($"{exR}-{exC}")){
                        enclosed++;
                    }
                }
            });
            return $"{enclosed}";
        }
    }
}