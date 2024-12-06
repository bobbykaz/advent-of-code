using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Headers;
using System.Numerics;
using Grids;

namespace y24 {
    public class D6: AoCDay {

        public D6(): base(24, 6) {
            _DebugPrinting = true;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var g = Utilties.RectangularCharGridFromLines(lines);
            var (rp,cp) = (-1,-1);
            g.ForEachRowCol((r,c,v) => {
                if(v == '^'){
                   (rp,cp) = (r,c);
                }
            });
            var seenMap = new VisitedMap();
            seenMap.Visit(rp, cp);

            var dir = Dir.N;

            var next = g.GetNeighbor(rp, cp, dir);
            PrintLn($"Start: {rp},{cp} => {next}");
            while(next.HasValue) {
                if(next.Value.V == '#') {
                    dir = turn(dir);
                } else {
                    rp =  next.Value.R;
                    cp =  next.Value.C;
                    seenMap.Visit(rp, cp);
                }
                next = g.GetNeighbor(rp, cp, dir);
            }

            var total = seenMap.VisitedCount();
            return $"{total}";
        }

        private Dir turn(Dir dir) {
            return dir switch {
                Dir.N => Dir.E,
                Dir.E => Dir.S,
                Dir.S => Dir.W,
                Dir.W => Dir.N,
                _ => throw new Exception()
            };
        }

    
        private string key(int r, int c, Dir d) {
            return $"{r}-{c}-{d}";
        }

        private bool Loops(Grid<char> g, int rp, int cp) {
            var seenMap = new Dictionary<string, bool>();
            var dir = Dir.N;
            
            seenMap[key(rp, cp, dir)] = true;

            var next = g.GetNeighbor(rp, cp, dir);
            while(next.HasValue) {
                if (seenMap.ContainsKey(key(next.Value.R, next.Value.C, dir))) {
                    return true;
                }

                if(next.Value.V == '#') {
                    dir = turn(dir);
                } else {
                    rp =  next.Value.R;
                    cp =  next.Value.C;
                    seenMap[key(rp, cp, dir)] = true;
                }
                next = g.GetNeighbor(rp, cp, dir);
            }
            return false;
        }

        public override string P2()
        {
            var lines = InputAsLines();
            var g = Utilties.RectangularCharGridFromLines(lines);
            var (rp,cp) = (-1,-1);
            g.ForEachRowCol((r,c,v) => {
                if(v == '^'){
                    (rp,cp) = (r,c);
                }
            });
            var total = 0;
            PrintLn($"Dim: {g.Width} x {g.Height}");
            var count = 0;
            // should just try all the possibilities of the original visited path, but this is easier
            g.ForEachRowCol((r,c,v) => {
                if(v == '.'){
                    var ng = Utilties.RectangularCharGridFromLines(lines);
                    ng.G[r][c] = '#';
                    if (Loops(ng,rp, cp)) {
                        total++;
                    }
                }
                count++;
                if(count % 500 == 0){
                    PrintLn($"{count}: {total}");
                }
            }); 

            return $"{total}";
        }

    }
}