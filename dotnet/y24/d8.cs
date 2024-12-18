using System.Data;
using System.Reflection.PortableExecutable;
using Grids;

namespace y24 {
    public class D8: AoCDay {

        public D8(): base(24, 8) {
            _DebugPrinting = false;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var inputGrid = Utilties.RectangularCharGridFromLines(lines);

            var grid = new Grid<Node>(inputGrid.Width, inputGrid.Height, new Node());
            var antMap = new Dictionary<char, List<Pos>>();
            var antinodes = new List<Cell<char>>();
            var antinodePosSet = new VisitedMap();

            inputGrid.ForEachRowCol((r,c,v) => {
                grid.G[r][c] = new Node();
                if(v != '.') {
                    grid.G[r][c].Fill(v);
                    if(!antMap.ContainsKey(v))
                        antMap[v] = new List<Pos>();
                    antMap[v].Add(new Pos(r,c));
                }
            });

            foreach(var (antenna, locations) in antMap) {
                if(locations == null) throw new Exception();
                
                for(int i = 0; i < locations.Count; i++){
                    for(int j = 0; j < locations.Count; j++){
                        if(i != j) {
                            var a = locations[i];
                            var b = locations[j];
                            var rowDiff = b.R - a.R;
                            var colDiff = b.C - a.C;

                            var a1 = new Cell<char>(b.R + rowDiff, b.C + colDiff, antenna);
                            var a2 = new Cell<char>(a.R - rowDiff, a.C - colDiff, antenna);
                            if(grid.GetCellIfValid(a1.R, a1.C).HasValue) {
                                antinodes.Add(a1);
                                antinodePosSet.Visit(a1.R, a1.C);
                            }

                            if(grid.GetCellIfValid(a2.R, a2.C).HasValue) {
                                antinodes.Add(a2);
                                antinodePosSet.Visit(a2.R, a2.C);
                            }
                        }
                    }
                }
            }

            return $"{antinodePosSet.VisitedCount()}";
        }

        private class Node {
            public bool IsEmpty;
            public char Antenna;
            public List<Cell<char>> Antinodes = [];
            public Node() {
                IsEmpty = true;
                Antenna = '.';
            }

            public void Fill(char ant){
                IsEmpty = false;
                Antenna = ant;
            }
        }

        public override string P2()
        {
            var lines = InputAsLines();
            var inputGrid = Utilties.RectangularCharGridFromLines(lines);

            var grid = new Grid<Node>(inputGrid.Width, inputGrid.Height, new Node());
            var antMap = new Dictionary<char, List<Pos>>();
            var antinodePosSet = new VisitedMap();

            inputGrid.ForEachRowCol((r,c,v) => {
                grid.G[r][c] = new Node();
                if(v != '.') {
                    grid.G[r][c].Fill(v);
                    if(!antMap.ContainsKey(v))
                        antMap[v] = new List<Pos>();
                    antMap[v].Add(new Pos(r,c));
                }
            });

            PrintLn($"Found {antMap.Keys.Count} frequencies");

            foreach(var (antenna, locations) in antMap) {
                if(locations == null) throw new Exception();
                PrintLn($"Calculating {antenna}");
            
                for(int i = 0; i < locations.Count; i++){
                    for(int j = 0; j < locations.Count; j++){
                        if(i != j) {
                            var a = locations[i];
                            var b = locations[j];
                            var rowDiff = b.R - a.R;
                            var colDiff = b.C - a.C;

                            var bNext = new Pos(b.R + rowDiff, b.C + colDiff);
                            while(grid.GetCellIfValid(bNext.R, bNext.C).HasValue) {
                                antinodePosSet.Visit(bNext.R, bNext.C);
                                bNext = new Pos(bNext.R + rowDiff, bNext.C + colDiff);
                            }

                            var aNext = new Pos(a.R - rowDiff, a.C - colDiff);
                            while(grid.GetCellIfValid(aNext.R, aNext.C).HasValue) {
                                antinodePosSet.Visit(aNext.R, aNext.C);
                                aNext = new Pos(aNext.R - rowDiff, aNext.C - colDiff);
                            }
                        }
                    }

                    if(_DebugPrinting) {
                    foreach(var p in antinodePosSet.VisitedPositions()) {
                        if (inputGrid.G[p.R][p.C] == '.')
                            inputGrid.G[p.R][p.C] = '#';
                    }
                    inputGrid.Print();
                    PrintLn("========");
                }
                }

                if(locations.Count > 1) {
                    foreach(var l in locations){
                        antinodePosSet.Visit(l.R, l.C);
                    }
                }
                
            }



            return $"{antinodePosSet.VisitedCount()}";
        }

    }
}