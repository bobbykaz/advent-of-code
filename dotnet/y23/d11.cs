using GridUtilities;

namespace y23 {
    public class D11 : AoCDay
    {
        public D11(): base(23, 11) {
            _DebugPrinting = false;
        }

        public override string P1()
        {
            var lines = InputAsLines();
            var g = Utilties.RectangularCharGridFromLines(lines);
            PrintLn($"Pre expand: {g.Height},{g.Width}");
            for(var r = 0; r < g.Height; r++) {
                var row = g.G[r];
                if(row.All(c => c == '.')){
                    g.AddRow('.', r);
                    r += 1;
                }
            }
            for(var c = 0; c < g.Width; c++) {
                var col = g.G.Select(r => r[c]).ToList();
                if(col.All(ch => ch == '.')){
                    g.AddCol('.', c);
                    c += 1;
                }
            }
            PrintLn($"Post expand: {g.Height},{g.Width}");

            var gals = new List<Galaxy>();
            int gid = 0;
            g.ForEachRowCol((r,c,v) => {
                if(v == '#') {
                    gals.Add(new Galaxy(r,c,gid));
                    PrintLn($"{r},{c},{gid}");
                    gid++;
                }
            });

            var dists = new List<long>();
            var ids = new List<int>();
            foreach(var gal in gals) {
                foreach(var otherGal in gals.Where(o=>o.ID != gal.ID)) {
                    dists.Add((long) gal.Dist(otherGal.R, otherGal.C));
                }
            }

            //list is double counted
            return $"{dists.Sum()/2}";
        }

        public class Galaxy {
            public int R;
            public int C;
            public int ID;
            public Galaxy(int r, int c, int id){
                R = r;
                C = c;
                ID = id;
            }

            public int Dist(int r, int c) {
                return Math.Abs(R-r) + Math.Abs(C-c);
            }
        }

       
        public override string P2()
        {
            var lines = InputAsLines();
            var grid = Utilties.RectangularCharGridFromLines(lines);
            //build galaxies first, just inc their coords instead;
            const int EXPAND = (100 - 1);

            var gals = new List<Galaxy>();
            int gid = 0;
            grid.ForEachRowCol((r,c,v) => {
                if(v == '#') {
                    gals.Add(new Galaxy(r,c,gid));
                    PrintLn($"{r},{c},{gid}");
                    gid++;
                }
            });

            var rMod = 0;
            for(var r = 0; r < grid.Height; r++) {
                var row = grid.G[r];
                if(row.All(c => c == '.')){
                    foreach(var g in gals) {
                        if(g.R > rMod + r) {
                            g.R += EXPAND;
                        }
                    }
                    rMod += EXPAND;
                }
            }

            var cMod = 0;
            for(var c = 0; c < grid.Width; c++) {
                var col = grid.G.Select(r => r[c]).ToList();
                if(col.All(ch => ch == '.')){
                    foreach(var g in gals) {
                        if(g.C > cMod + c) {
                            g.C += EXPAND;
                        }
                    }
                    cMod += EXPAND;
                }
            }

            var dists = new List<long>();
            var ids = new List<int>();
            foreach(var gal in gals) {
                foreach(var otherGal in gals.Where(o=>o.ID != gal.ID)) {
                    dists.Add((long) gal.Dist(otherGal.R, otherGal.C));
                }
            }

            //list is double counted
            return $"{dists.Sum()/2}";
        }
    }
}