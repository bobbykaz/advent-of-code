namespace Grids {
    public enum Dir {
        N, S, E, W
    }

    public class Grid<T> {
        public int Width {get; set;}
        public int LastColIndex {get {return Width-1;}}
        public int Height {get; set;}
        public int LastRowIndex {get {return Height-1;}}
        public List<List<T>> G {get; set;}
        public Grid(int w, int h, T defVal) {
            Width = w;
            Height = h;
            G = new List<List<T>>();
            for(int r = 0; r < h; r++) {
                var row = new List<T>();
                for(int c = 0; c < w; c++) {
                    row.Add(defVal);
                }
                G.Add(row);
            }
        }

        public void AddRow(T defVal, int rowIndex) {
            var row = new List<T>();
            for(var i = 0; i < Width; i++) {
                row.Add(defVal);
            }
            G.Insert(rowIndex,row);
            Height += 1;
        }

        public void AddCol(T defVal, int colIndex) {
            for(var i = 0; i < Height; i++) {
                G[i].Insert(colIndex,defVal);
            }
            Width += 1;
        }

        /// <summary>
        /// Gets adjacent cell, or null if it would be O.O.B.
        /// </summary>
        /// <param name="dir">'U' 'D' 'L' 'R' or 'N' 'S' 'E' 'W'</param>
        /// <returns></returns>
        public Cell<T>? GetNeighbor(int r, int c, char dirCh) {
            var dir = GridUtilities.DirFromChar(dirCh);
            return GetNeighbor(r,c,dir);
        }

        public Cell<T>? GetNeighbor(int r, int c, Dir dir) {
            if (r == 0 && dir == Dir.N)             return null;
            if (r == LastRowIndex && dir == Dir.S)  return null;
            if (c == 0 && dir == Dir.W)             return null;
            if (c == LastColIndex && dir == Dir.E)  return null;

            var (nr,nc) = dir switch {
                Dir.N => (r-1,c),
                Dir.S => (r+1,c),
                Dir.E => (r,c+1),
                Dir.W => (r,c-1),
                _ => throw new Exception("invalid dir enum")
            };

            return new Cell<T>(nr,nc, this.G[nr][nc]);

        }

        public List<Cell<T>> CardinalNeighbors(int r, int c) 
        {
            var results = new List<Cell<T>>();
            //L
            if (c > 0) {
                var cell = new Cell<T>(r, c-1, this.G[r][c-1]);
                results.Add(cell);
            }
            //U
            if (r > 0) {
                var cell = new Cell<T>(r-1, c, this.G[r-1][c]);
                results.Add(cell);
            }
            //R
            if (c < LastColIndex) {
                var cell = new Cell<T>(r, c+1, this.G[r][c+1]);
                results.Add(cell);
            }
            //D
            if (r < LastRowIndex) {
                var cell = new Cell<T>(r+1, c, this.G[r+1][c]);
                results.Add(cell);
            }

            return results;
        }

        public List<Cell<T>> DiagNeighbors(int r, int c) 
        {
            var results = new List<Cell<T>>();
            //UL
            if (c > 0 && r > 0) {
                var cell = new Cell<T> {R = r-1, C = c-1, V = this.G[r-1][c-1]};
                results.Add(cell);
            }
            //UR
            if (c < (this.Width - 1) && r > 0) {
                var cell = new Cell<T> {R = r-1, C = c + 1, V = this.G[r-1][c+1]};
                results.Add(cell);
            }
            //DL
            if (c > 0 && r < (this.Height - 1)) {
                var cell = new Cell<T> {R = r + 1, C = c-1, V = this.G[r+1][c-1]};
                results.Add(cell);
            }
            //DR
            if (c < (this.Width - 1) && r < (this.Height - 1)) {
                var cell = new Cell<T> {R = r+1, C = c+1, V = this.G[r+1][c+1]};
                results.Add(cell);
            }

            return results;
        }

        public List<Cell<T>> AllNeighbors(int r, int c) 
        {
            return this.CardinalNeighbors(r,c).Concat(this.DiagNeighbors(r,c)).ToList();
        }

        /// <summary>
        /// Iterates Left to right, top to bottom
        /// </summary>
        public void ForEachRowCol(Action<int,int,T> cellFunc) {
            for(int r = 0; r < Height; r++){
                for( int c = 0; c < Width; c++) {
                    cellFunc.Invoke(r,c,G[r][c]);
                }
            }
        }

        /// <summary>
        /// Iterates Left to right, bottom to top
        /// </summary>
        public void ForEachRowColBottomUp(Action<int,int,T> cellFunc) {
            for(int r = Height - 1; r >= 0; r--){
                for( int c = 0; c < Width; c++) {
                    cellFunc.Invoke(r,c,G[r][c]);
                }
            }
        }

        /// <summary>
        /// Iterates Top to bottom, left to right
        /// </summary>
        public void ForEachColRow(Action<int,int,T> cellFunc) {
            for( int c = 0; c < Width; c++){
                for(int r = 0; r < Height; r++) {
                    cellFunc.Invoke(r,c,G[r][c]);
                }
            }
        }

        /// <summary>
        /// Iterates Top to bottom, right to left
        /// </summary>
        public void ForEachColRowRightBack(Action<int,int,T> cellFunc) {
            for( int c = Width - 1; c >=0; c--){
                for(int r = 0; r < Height; r++) {
                    cellFunc.Invoke(r,c,G[r][c]);
                }
            }
        }

        public List<List<T>> Rows() {
            return G.ToList();
        }

        public List<List<T>> Cols() {
            var rslt = new List<List<T>>();
            for(int c = 0; c < Width; c++) {
                var col = G.Select(r => r[c]).ToList();
                rslt.Add(col);
            }

            return rslt;
        }

        public override string ToString() {
            var str = "";
            ForEachRowCol((r,c,v) => {
                str += v == null ? "{null}": v.ToString();
            });

            return str;
        }
        public void Print() {
            foreach(var r in G) {
                r.ForEach(c => Console.Write(c));
                Console.WriteLine("");
            }
        }
    }

    public struct Cell<T> {
            public int R {get; set;}
            public int C {get; set;}
            public T V {get; set;}

            public Cell(int r, int c, T v) {
                R = r;
                C = c;
                V = v;
            }
        }

    public class VisitedMap {
        private Dictionary<string, bool> SeenMap = new Dictionary<string, bool>();

        private static string Key(int r, int c) { return $"{r}-{c}"; }

        public void Visit(int r, int c) { SeenMap[Key(r,c)] = true; }
        public bool WasVisited(int r, int c) {return SeenMap.ContainsKey(Key(r,c));}
        public void Reset() { SeenMap = new Dictionary<string, bool>(); }
    }

    public class GridUtilities {
        public static Dir DirFromChar(char dir) {
            return dir switch {
                'U' or 'u' => Dir.N,
                'D' or 'd' => Dir.S,
                'L' or 'l' => Dir.W,
                'R' or 'r' => Dir.E,
                'N' or 'n' => Dir.N,
                'S' or 's' => Dir.S,
                'E' or 'e' => Dir.E,
                'W' or 'w' => Dir.W,
                _ => throw new ArgumentException($"char dir must be a cardinal dir, not {dir}")
            };
        }

        public static Dir OppositeDir(Dir d) {
            return d switch {
                Dir.N => Dir.S,
                Dir.S => Dir.N,
                Dir.W => Dir.E,
                Dir.E => Dir.W,
                _ => throw new Exception("bad dir")
            };
        }
    }
}