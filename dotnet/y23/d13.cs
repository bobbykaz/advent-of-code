using Grids;

namespace y23 {
    public class D13 : AoCDay
    {
        public D13(): base(23, 13) {
            _DebugPrinting = false;
        }

        public override string P1()
        {
            var lines = InputAsLines();
            var blocks = Utilties.GroupInputAsBlocks(lines);
            var total = 0L;
            foreach(var block in blocks) {
                var grid = Utilties.RectangularCharGridFromLines(block);
                int hMir = findMirror(grid.Rows()) + 1;
                int vMir = findMirror(grid.Cols()) + 1;
                total += vMir;
                total += hMir*100;
            }

            
            return $"{total}";
        }

        public int findMirror(List<List<char>> block) {
            for(int i = 0; i < block.Count - 1; i++) {
                if(CompareFan(block, i))
                    return i;
            }
            return -1;
        }

        public bool CompareFan(List<List<char>> block, int div) {
            int top = div;
            int bot = div+1;
            bool keepGoing = true;
            while(keepGoing && top >= 0 && bot < block.Count) {
                keepGoing = Compare(block[top], block[bot]);
                top--;
                bot++;
            }

            return keepGoing;
        }
        public bool Compare(List<char>r1, List<char>r2) {
            return r1.SequenceEqual(r2);
        }

        public override string P2()
        {
            var lines = InputAsLines();
            var blocks = Utilties.GroupInputAsBlocks(lines);
            var total = 0L;
            foreach(var block in blocks) {
                var grid = Utilties.RectangularCharGridFromLines(block);
                int hMir = findMirror(grid.Rows());
                int vMir = findMirror(grid.Cols());
                total += BFblock(grid, hMir, vMir);
            }

            
            return $"{total}";
        }

        public int findMirrorSkipOld(List<List<char>> block, int old) {
            for(int i = 0; i < block.Count - 1; i++) {
                if(i != old && CompareFan(block, i))
                    return i;
            }
            return -1;
        }

        public int BFblock(Grid<char> grid, int origH, int origV) {
            var rslt = -1;
            grid.ForEachRowCol((row, col, v) => {
                var orig = v;
                var opposite = v switch {
                    '.' => '#',
                    '#' => '.',
                    _ => throw new Exception("bad grid char")   
                };
                grid.G[row][col] = opposite;

                int hMir = findMirrorSkipOld(grid.Rows(),origH) + 1;
                int vMir = findMirrorSkipOld(grid.Cols(),origV) + 1;
                grid.G[row][col] = orig;
                var sum = vMir + hMir*100;
                if(sum > 0) {
                    rslt = sum;
                }
            });
            return rslt;
        }
    }
}