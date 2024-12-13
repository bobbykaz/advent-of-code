using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace Grids {
    public enum Dir {
        N, S, E, W, NE, NW, SE, SW
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
        /// <param name="dir">'U' 'D' 'L' 'R' or 'N' 'S' 'E' 'W' for cardinal dirs. UL/UR/DL/DR, NW/NE/SW/SE for diagonals.</param>
        /// <returns></returns>
        public Cell<T>? GetNeighbor(int r, int c, string dirStr) {
            var dir = GridUtilities.DirFromStr(dirStr);
            return GetNeighbor(r,c,dir);
        }

        public Cell<T>? GetNeighbor(int r, int c, Dir dir) {
            var (nr,nc) = dir switch {
                Dir.N => (r-1,c),
                Dir.S => (r+1,c),
                Dir.E => (r,c+1),
                Dir.W => (r,c-1),

                Dir.NE => (r-1,c+1),
                Dir.SW => (r+1,c-1),
                Dir.SE => (r+1,c+1),
                Dir.NW => (r-1,c-1),
                _ => throw new Exception("invalid dir enum")
            };

            return GetCellIfValid(nr,nc);

        }

        public Cell<T>? GetCellIfValid(int r, int c) {
            if (r < 0 || c < 0)
                return null;
            
            if (r > LastRowIndex || c > LastColIndex)
                return null;
            
            return new Cell<T>(r, c, G[r][c]);
        }

        public List<Cell<T>> CardinalNeighbors(int r, int c) 
        {
            var results = CardinalNeighborsWithDir(r,c).Select(p => p.Item1).ToList();
            return results;
        }

        public List<(Cell<T>, Dir)> CardinalNeighborsWithDir(int r, int c) 
        {
            var results = new List<(Cell<T>,Dir)>();
            //L
            if (c > 0) {
                var cell = new Cell<T>(r, c-1, this.G[r][c-1]);
                results.Add((cell, Dir.W));
            }
            //U
            if (r > 0) {
                var cell = new Cell<T>(r-1, c, this.G[r-1][c]);
                results.Add((cell, Dir.N));
            }
            //R
            if (c < LastColIndex) {
                var cell = new Cell<T>(r, c+1, this.G[r][c+1]);
                results.Add((cell, Dir.E));
            }
            //D
            if (r < LastRowIndex) {
                var cell = new Cell<T>(r+1, c, this.G[r+1][c]);
                results.Add((cell,Dir.S));
            }

            return results;
        }

        // returns the cell, the direction of the cell relative to current r,c, and whether it wrapped around the grid
        public List<(Cell<T>,Dir,bool)> CardinalNeighborsWrapped(int r, int c) 
        {
            var results = new List<(Cell<T>, Dir, bool)>();
            //L
            if (c > 0) {
                var cell = new Cell<T>(r, c-1, this.G[r][c-1]);
                results.Add((cell, Dir.W, false));
            } else {
                var cell = new Cell<T>(r, LastColIndex, this.G[r][LastColIndex]);
                results.Add((cell, Dir.W, true));
            }
            //U
            if (r > 0) {
                var cell = new Cell<T>(r-1, c, this.G[r-1][c]);
                results.Add((cell, Dir.N, false));
            } else {
                var cell = new Cell<T>(LastRowIndex, c, this.G[LastRowIndex][c]);
                results.Add((cell, Dir.N, true));
            }
            //R
            if (c < LastColIndex) {
                var cell = new Cell<T>(r, c+1, this.G[r][c+1]);
                results.Add((cell, Dir.E, false));
            } else {
                var cell = new Cell<T>(r, 0, this.G[r][0]);
                results.Add((cell, Dir.E, true));
            }
            //D
            if (r < LastRowIndex) {
                var cell = new Cell<T>(r+1, c, this.G[r+1][c]);
                results.Add((cell, Dir.S, false));
            } else {
                var cell = new Cell<T>(0, c, this.G[0][c]);
                results.Add((cell, Dir.S, true));
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

        public string Key{get {return $"{R}-{C}";}}
        public override string ToString()
        {
            return $"[({R}, {C}): {V}]";
        }
    }

    public struct Pos {
        public int R {get; set;}
        public int C {get; set;}

        public Pos(int r, int c) {
            R = r;
            C = c;
        }

        public string Key{get {return $"{R}-{C}";}}
        public override string ToString()
        {
            return $"({R}, {C})";
        }
        public override bool Equals(object obj)
        {            
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            var objAsPos = (Pos) obj;
            return this.R == objAsPos.R && this.C == objAsPos.C;
            
        }
        
        public override int GetHashCode()
        {
            return this.Key.GetHashCode();
        }
    }

    public class VisitedMap {
        private Dictionary<string, Pos> SeenMap = new Dictionary<string, Pos>();

        private static string Key(int r, int c) { return $"{r}-{c}"; }

        public void Visit(int r, int c) { 
            var k = Key(r,c);
            if(!SeenMap.ContainsKey(k)) {
                SeenMap[k] = new Pos(r,c);
            } 
        }

        public bool WasVisited(int r, int c) {return SeenMap.ContainsKey(Key(r,c));}
        public void Reset() { SeenMap = new Dictionary<string, Pos>(); }

        public int VisitedCount() { return SeenMap.Keys.Count;} 
        
        public List<Pos> VisitedPositions() {
            return SeenMap.Values.ToList();
        }

        public VisitedMap Copy() {
            var rslt = new VisitedMap();
            foreach(var (k,v) in SeenMap) {
                rslt.SeenMap[k] = new Pos(v.R, v.C);
            }
            return rslt;
        }
    }

    public struct PosWithDir {
        public int R {get; set;}
        public int C {get; set;}
        public Dir Dir {get; set;}

        public PosWithDir(int r, int c, Dir d) {
            R = r;
            C = c;
            Dir = d;
        }

        public string Key{get {return $"{R}-{C}-{Dir}";}}
        public override string ToString()
        {
            return $"({R}, {C}): {Dir}";
        }
    }
    
    public class VisitedMapWithDir {
        private Dictionary<string, PosWithDir> SeenMap = new Dictionary<string, PosWithDir>();

        private static string Key(int r, int c, Dir d) { return $"{r}-{c}-{d}"; }

        public void Visit(int r, int c, Dir d) { 
            var k = Key(r,c,d);
            if(!SeenMap.ContainsKey(k)) {
                SeenMap[k] = new PosWithDir(r,c,d);
            } 
        }

        public bool WasVisited(int r, int c, Dir d) {return SeenMap.ContainsKey(Key(r,c,d));}
        public void Reset() { SeenMap = new Dictionary<string, PosWithDir>(); }

        public int VisitedCount() { return SeenMap.Keys.Count;} 
        
        public List<PosWithDir> VisitedPositions() {
            return SeenMap.Values.ToList();
        }

        public VisitedMapWithDir Copy() {
            var rslt = new VisitedMapWithDir();
            foreach(var (k,v) in SeenMap) {
                rslt.SeenMap[k] = new PosWithDir(v.R, v.C, v.Dir);
            }
            return rslt;
        }
    }

    public class GridUtilities {
        public static Dir CardinalDirFromChar(char dir) {
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

        public static Dir DirFromStr(string dir) {
            return dir.ToLowerInvariant() switch {
                "u" => Dir.N,
                "d" => Dir.S,
                "l" => Dir.W,
                "r" => Dir.E,

                "n" => Dir.N,
                "s" => Dir.S,
                "e" => Dir.E,
                "w" => Dir.W,

                "ul" => Dir.NW,
                "dl" => Dir.SW,
                "ur" => Dir.NE,
                "dr" => Dir.SE,

                "nw" => Dir.NW,
                "ne" => Dir.NE,
                "sw" => Dir.SW,
                "se" => Dir.SE,
                _ => throw new ArgumentException($"char dir must be a cardinal dir, not {dir}")
            };
        }

        public static Dir OppositeDir(Dir d) {
            return d switch {
                Dir.N => Dir.S,
                Dir.S => Dir.N,
                Dir.W => Dir.E,
                Dir.E => Dir.W,
                Dir.NE => Dir.SW,
                Dir.SE => Dir.NW,
                Dir.NW => Dir.SE,
                Dir.SW => Dir.NE,
                _ => throw new Exception("bad dir")
            };
        }
    }
}