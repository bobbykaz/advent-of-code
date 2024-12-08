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


        private bool Loops(Grid<char> g, int rp, int cp) {
            var seenMap = new VisitedMapWithDir();
            var dir = Dir.N;
            
            seenMap.Visit(rp, cp, dir);

            var next = g.GetNeighbor(rp, cp, dir);
            while(next.HasValue) {
                if (seenMap.WasVisited(next.Value.R, next.Value.C, dir)) {
                    return true;
                }

                if(next.Value.V == '#') {
                    dir = turn(dir);
                } else {
                    rp =  next.Value.R;
                    cp =  next.Value.C;
                    seenMap.Visit(rp, cp, dir);
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
            var (ir,ic) = (rp,cp);
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

            var total = 0;

            foreach(var p in seenMap.VisitedPositions()) {
                if ((p.R, p.C) != (ir,ic)){
                    var ng = Utilties.RectangularCharGridFromLines(lines);
                    ng.G[p.R][p.C] = '#';
                    if (Loops(ng,ir, ic)) {
                        total++;
                    }
                }
            }

            return $"{total}";
        }

    }
}