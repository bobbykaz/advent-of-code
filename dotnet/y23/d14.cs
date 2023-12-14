using System.Xml.XPath;

namespace y23 {
    public class D14 : AoCDay
    {
        public D14(): base(23, 14) {
            _DebugPrinting = true;
        }

        public override string P1()
        {
            var lines = InputAsLines();
            var grid = GridUtils.RectangularCharGridFromLines(lines);
            while(TiltGridOnceN(grid)) {

            }
            var total = evalLoad(grid);

            
            return $"{total}";
        }

        public bool TiltGridOnceN(GridUtils.Grid<char> grid) {
            bool anyChange = false;

            grid.ForEachRowCol((r,c,v) => {
                if(r != 0) {
                    if (v == 'O') {
                        if(grid.G[r-1][c] == '.') {
                            grid.G[r-1][c] = 'O';
                            grid.G[r][c] = '.';
                            anyChange = true;
                        }
                    }
                }
            });

            return anyChange;
        }

        public bool TiltGridOnceS(GridUtils.Grid<char> grid) {
            bool anyChange = false;

            grid.ForEachRowColBottomUp((r,c,v) => {
                if(r != grid.Height-1) {
                    if (v == 'O') {
                        if(grid.G[r+1][c] == '.') {
                            grid.G[r+1][c] = 'O';
                            grid.G[r][c] = '.';
                            anyChange = true;
                        }
                    }
                }
            });

            return anyChange;
        }

        public bool TiltGridOnceW(GridUtils.Grid<char> grid) {
            bool anyChange = false;

            grid.ForEachColRow((r,c,v) => {
                if(c != 0) {
                    if (v == 'O') {
                        if(grid.G[r][c-1] == '.') {
                            grid.G[r][c-1] = 'O';
                            grid.G[r][c] = '.';
                            anyChange = true;
                        }
                    }
                }
            });

            return anyChange;
        }

        public bool TiltGridOnceE(GridUtils.Grid<char> grid) {
            bool anyChange = false;

            grid.ForEachColRowRightBack((r,c,v) => {
                if(c != grid.Width-1) {
                    if (v == 'O') {
                        if(grid.G[r][c+1] == '.') {
                            grid.G[r][c+1] = 'O';
                            grid.G[r][c] = '.';
                            anyChange = true;
                        }
                    }
                }
            });

            return anyChange;
        }

        public int evalLoad(GridUtils.Grid<char> grid) {
            var total = 0;
            grid.ForEachRowCol((r,c,v) => {
                if (v == 'O') {
                    total += grid.Height - r;
                }
            });
            return total;
        }

        public void spinCycle(GridUtils.Grid<char> grid) {
            while(TiltGridOnceN(grid)){};
            while(TiltGridOnceW(grid)){};
            while(TiltGridOnceS(grid)){};
            while(TiltGridOnceE(grid)){};
        }

        public override string P2()
        {
            var lines = InputAsLines();
            var grid = GridUtils.RectangularCharGridFromLines(lines);
            var count = 0L;
            var seenGrids = new Dictionary<string, long>();
            seenGrids[grid.ToString()] = 0;
            bool seenCycle = false;
            while(count < 1000000000) {
                spinCycle(grid);
                //PrintLn($"After cycle {count}");
                //grid.Print();
                count++;
                if(!seenCycle && seenGrids.ContainsKey(grid.ToString())) {
                    seenCycle = true;
                    var lastCycle = seenGrids[grid.ToString()];
                    PrintLn($"Cycle found from {lastCycle} to {count}");
                    PrintLn("Accelerating to end");
                    var diff = count - lastCycle;
                    while(count + diff < 1000000000) {
                        count += diff;
                    }
                    PrintLn($"resuming from {count}");
                }
                seenGrids[grid.ToString()] = count;
                if (count % 1000000 == 0) {
                    PrintLn($"{count}");
                }
            }
            grid.Print();
            var total = evalLoad(grid);

            
            return $"{total}";
        }
    }
}