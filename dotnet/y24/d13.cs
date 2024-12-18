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
            public Game(Coord a, Coord b, Coord t, bool isP2 = false) {
                BtnA = a;
                BtnB = b;
                Target = t;
                if(isP2) {
                    Target.x += 10000000000000L;
                    Target.y += 10000000000000L;
                }
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
                } 
                if(!aFirst.HasValue || !bFirst.HasValue){
                    throw new Exception();
                }
                return long.Min(aFirst.Value, bFirst.Value);
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

            public (bool, long) Math() {
                //Button A: X+26, Y+66
                //Button B: X+67, Y+21
                //Prize: X=10000000012748, Y=10000000012176

                //10000000012748 = APresses*26 + BPresses*67
                //10000000012176 = Apresses*66 + BPresses*21
                //| 26   67  |    |Apresses|   = [TX]
                //| 66   21  |    |BPresses|   = [TY]

                //| a1   b1  |    |x|   = [c1]
                //| a2   b2  |    |y|   = [c2]
                var a1 = BtnA.x;
                var b1 = BtnB.x;
                var a2 = BtnA.y;
                var b2 = BtnB.y;
                var c1 = Target.x;
                var c2 = Target.y;

                var D = a1*b2 -a2*b1;
                var Dx = c1*b2 - c2*b1;
                var Dy = a1*c2 - a2*c1;
                if (D!= 0 && Dx % D == 0 && Dy % D ==0) {
                    var X = Dx/D;
                    var Y = Dy/D;
                    return (true, (3 * X) + Y);
                }
                return (false, 0L);

            }
        }

        public override string P2()
        {
            var lines = InputAsLines();
            var blocks = Utilties.GroupInputAsBlocks(lines);
            var games = new List<Game>();
            var total = 0L;
            foreach(var block in blocks) {
                var ba = new Coord(block[0], false);
                var bb = new Coord(block[1], false);
                var t = new Coord(block[2], true);
                var g = new Game(ba,bb,t, isP2: true);
                PrintLn($"{ba}, {bb}, : {t}");
                
                var (works, tokens) = g.Math();
                if(works){
                    PrintLn($"{works} : {tokens}");
                    total += tokens;
                }
                    
            }

            
           
            return $"{total}";
        }

    }
}