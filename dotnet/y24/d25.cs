using System.Data;
using System.Numerics;
using Grids;

namespace y24 {
    public class D25: AoCDay {

        public D25(): base(24, 25) {
            _DebugPrinting = true;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var groups = Utilties.GroupInputAsBlocks(lines);
            var keys = new List<KeyOrLock>();
            var locks = new List<KeyOrLock>();
            foreach(var group in groups){
                var grid = Utilties.RectangularCharGridFromLines(group);
                var rslt = parseGrid(grid);
                if(rslt.isLock) {
                    locks.Add(rslt);
                    PrintLn($"Lock: {string.Join(",", rslt.Heights)}");
                } else {
                    keys.Add(rslt);
                    PrintLn($"Key: {string.Join(",", rslt.Heights)}");
                }
            }
            var total = 0L;
            foreach(var k in keys){
                foreach(var l in locks) {
                    if(Fit(k, l)){
                        total++;
                    }
                }
            }
            
           
            return $"{total}";
        }
        
        private bool Fit(KeyOrLock key, KeyOrLock theLock) {
            for(int i = 0; i < key.Heights.Count; i++) {
                if(key.Heights[i] + theLock.Heights[i] > 5)  {
                    return false;
                }
            }
            return true;
        }

        private record class KeyOrLock(bool isLock, List<int> Heights);
        private  KeyOrLock parseGrid(Grid<char> g) {
            bool isLock = false;
            if(g.G[0][0] == '#') {
                isLock = true;
            }

            List<int> heights = new List<int>();
            if(isLock) {
                for(int c = 0; c < g.Width; c++) {
                    int h = 0;
                    for(int r = 1; r < g.Height; r++) {
                        if(g.G[r][c] == '#') {
                            h++;
                        } else {
                            break;
                        }
                    }
                    heights.Add(h);
                }
            } else {
                for(int c = 0; c < g.Width; c++) {
                    int h = 0;
                    for(int r = g.Height -2; r >= 0; r--) {
                     if(g.G[r][c] == '#') {
                            h++;
                        } else {
                            break;
                        }
                    }
                    heights.Add(h);
                }
            }
            return new KeyOrLock(isLock, heights);
        }

        public override string P2()
        {
            var lines = InputAsLines();
            var total = 0L;
           
            return $"{total}";
        }

    }
}