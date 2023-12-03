public class GridUtils {
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

        public List<Cell> CardinalNeighbors(int r, int c) 
        {
            var results = new List<Cell>();
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

        public struct Cell {
            public int R {get; set;}
            public int C {get; set;}
            public T V {get; set;}
        }

        public void Print() {
            foreach(var r in G) {
                r.ForEach(c => Console.Write(c));
                Console.WriteLine("");
            }
        }
    }
    public static Grid<char> RectangularCharGridFromLines(List<String> lines) {
        var h = lines.Count;
        var w = lines[0].Length;
        var g = new Grid<char>(w,h,'.');
        for(int r = 0; r < lines.Count; r++) {
            var chars = lines[r].ToCharArray();
            for(int c = 0; c < chars.Length; c++){
                g.G[r][c] = chars[c];
            }
        }

        return g;
    }

    public static Grid<int> NGrid(int w, int h, int def) {
        var g = new Grid<int>(w,h,def);

        return g;
    }
}