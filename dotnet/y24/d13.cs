using System.Numerics;

namespace y24 {
    public class D13: AoCDay {

        public D13(): base(24, 13) {
            _DebugPrinting = true;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var blocks = Utilties.GroupInputAsBlocks(lines);
            var games = new List<Game>();
            var total = 0L;
            foreach(var block in blocks) {
                var ba = new Coord(block[0], false);
                var bb = new Coord(block[1], false);
                var t = new Coord(block[2], true);
                var g = new Game(ba,bb,t);
                PrintLn($"{ba}, {bb}, : {t}");
                
                var sol = g.Solve();
                if(sol.HasValue)
                    total += sol.Value;
            }

            
           
            return $"{total}";
        }

        private struct Coord {
            public long x;
            public long y;
            public Coord(string line, bool isTarget){
                var pts = line.Split(": ");
                var coords = pts[1].Split(", ");
                if(isTarget) {
                    x = long.Parse(coords[0].Split("=")[1]);
                    y = long.Parse(coords[1].Split("=")[1]);
                } else {
                    x = long.Parse(coords[0].Split("+")[1]);
                    y = long.Parse(coords[1].Split("+")[1]);
                }
            }

            public override string ToString()
            {
                return $"{x}-{y}";
            }
        }

        private class Game {
            public Coord BtnA;
            public Coord BtnB;
            public Coord Target;
            public Game(Coord a, Coord b, Coord t) {
                BtnA = a;
                BtnB = b;
                Target = t;
            }

            public long? Solve() {
                var aFirst = Optimize(BtnA, BtnB, 3, 1);
                Console.WriteLine($"A token value: {aFirst}");
                var bFirst = Optimize(BtnB, BtnA, 1, 3);
                Console.WriteLine($"B token value: {bFirst}");

                if(!aFirst.HasValue && !bFirst.HasValue) {
                    return null;
                } else if(aFirst.HasValue && !bFirst.HasValue) {
                    return aFirst.Value;
                } else if(!aFirst.HasValue && bFirst.HasValue) {
                    return bFirst.Value;
                } else return long.Min(aFirst.Value, bFirst.Value);
            }

            public long? Optimize(Coord main, Coord alt, int mc, int ac) {
                long? lowestTokens = null;
                Console.WriteLine($"{main} -> {alt}");
                for(int a = 0; a < 101; a++) {
                    var ax = main.x * a;
                    var ay = main.y * a;
                    var tx = Target.x - ax;
                    var ty = Target.y - ay;
                    

                    if(tx % alt.x == 0 && ty % alt.y == 0) {
                        Console.WriteLine($"path to alt with main tokens {a}");
                        if(ty/alt.y == tx/alt.x) {
                            var altTokens = tx/alt.x;
                            var cost = a * mc + altTokens * ac;
                            if(!lowestTokens.HasValue) {lowestTokens = cost;}
                            if(cost < lowestTokens) {lowestTokens = cost;}
                        }
                    }
                }
                return lowestTokens;
            }
        }

        public override string P2()
        {
            var lines = InputAsLines();
            var total = 0L;
           
            return $"{total}";
        }

    }
}