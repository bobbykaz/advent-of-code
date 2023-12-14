namespace GridUtilities {
    public class Grid<T> {
        public int Width {get; set;}
        public int Height {get; set;}
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

        public List<Cell> CardinalNeighbors(int r, int c) 
        {
            var results = new List<Cell>();
            //L
            if (c > 0) {
                var cell = new Cell {R = r, C = c-1, V = this.G[r][c-1]};
                results.Add(cell);
            }
            //U
            if (r > 0) {
                var cell = new Cell {R = r-1, C = c, V = this.G[r-1][c]};
                results.Add(cell);
            }
            //R
            if (c < (this.Width - 1)) {
                var cell = new Cell {R = r, C = c+1, V = this.G[r][c+1]};
                results.Add(cell);
            }
            //D
            if (r < (this.Height - 1)) {
                var cell = new Cell {R = r+1, C = c, V = this.G[r+1][c]};
                results.Add(cell);
            }

            return results;
        }

        public List<Cell> DiagNeighbors(int r, int c) 
        {
            var results = new List<Cell>();
            //UL
            if (c > 0 && r > 0) {
                var cell = new Cell {R = r-1, C = c-1, V = this.G[r-1][c-1]};
                results.Add(cell);
            }
            //UR
            if (c < (this.Width - 1) && r > 0) {
                var cell = new Cell {R = r-1, C = c + 1, V = this.G[r-1][c+1]};
                results.Add(cell);
            }
            //DL
            if (c > 0 && r < (this.Height - 1)) {
                var cell = new Cell {R = r + 1, C = c-1, V = this.G[r+1][c-1]};
                results.Add(cell);
            }
            //DR
            if (c < (this.Width - 1) && r < (this.Height - 1)) {
                var cell = new Cell {R = r+1, C = c+1, V = this.G[r+1][c+1]};
                results.Add(cell);
            }

            return results;
        }

        public List<Cell> AllNeighbors(int r, int c) 
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

        public struct Cell {
            public int R {get; set;}
            public int C {get; set;}
            public T V {get; set;}
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

    public class VisitedMap {
        private Dictionary<string, bool> SeenMap = new Dictionary<string, bool>();

        private static string Key(int r, int c) { return $"{r}-{c}"; }

        public void Visit(int r, int c) { SeenMap[Key(r,c)] = true; }
        public bool WasVisited(int r, int c) {return SeenMap.ContainsKey(Key(r,c));}
        public void Reset() { SeenMap = new Dictionary<string, bool>(); }
    }
}