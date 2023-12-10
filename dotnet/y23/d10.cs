using System.Data;
using System.Xml.XPath;

namespace y23 {
    public class D10 : AoCDay
    {
        public D10(): base(23, 10) {
            _DebugPrinting = false;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var g = GridUtils.RectangularCharGridFromLines(lines);
            
            var (sr,sc) = (0,0);
            bool found = false;
            for(int r = 0; r < g.G.Count; r++) {
                for(int c = 0; c < g.G[r].Count; c++) {
                    if(g.G[r][c] == 'S') {
                        (sr, sc) = (r,c);
                        found = true;
                        break;
                    }
                    if(found) {
                        break;
                    }
                }
            }

            //row, col, dist to tile
            var bfs = new Queue<(int, int, int)>();
            bfs.Enqueue((sr, sc, 0));
            Dictionary<string, bool> seen = new Dictionary<string, bool>();
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

            PrintLn( $"{maxDist}");

            var g2 = GridUtils.RectangularCharGridFromLines(lines);

            for(int r = 0; r < g2.G.Count; r++) {
                for(int c = 0; c < g2.G[r].Count; c++) {
                    if(!seen.ContainsKey($"{r}-{c}")) {
                        g2.G[r][c] = '.';
                    }
                }
            }

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
            for(int r = 0; r < g2.G.Count; r++) {
                for(int c = 0; c < g2.G[r].Count; c++) {
                   var (exR,exC) = (r*3 + 1, c*3 + 1); 
                   if(g2.G[r][c] == '.') {
                        if(!seen.ContainsKey($"{exR}-{exC}")){
                            enclosed++;
                        }
                   }
                }
            }

            return $"{enclosed}";
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
            var lines = InputAsLines();
            var total = 0L;
            
            return $"{total}";
        }
    }
}