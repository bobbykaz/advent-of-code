using System.Data;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Grids;
using Vec;

namespace y24 {
    public class D14: AoCDay {

        public D14(): base(24, 14) {
            _DebugPrinting = true;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var FieldDim = new Vec2(11,7);
            FieldDim = new Vec2(101,103);
            var bots = new List<Bot>();
            foreach(var l in lines) {
                bots.Add(ParseLine(l));
            }

            for(int i = 0; i < 100; i++) {
                for (int n = 0; n < bots.Count(); n++) {
                    var b = bots[n];
                    b.Pos += b.Velocity;
                    b.Pos = Wrap(b.Pos, FieldDim);
                    bots[n] = b;
                }
            }
            var score = new List<long>(){0,0,0,0,0};
            foreach(var b in bots) {
                var q = Quadrant(b.Pos, FieldDim);
                score[q] += 1;
            }
            PrintLn($"Scores: {string.Join(',', score)}");
            var total = score[1] * score[2] * score[3] * score[4];
            return $"{total}";
        }

        private Bot ParseLine(string line){
            var pts = line.Split(" ");
            var posNum = pts[0].Split("=")[1].Split(",");
            var velNum = pts[1].Split("=")[1].Split(",");

            var pos = new Vec2(long.Parse(posNum[0]), long.Parse(posNum[1]));
            var vel = new Vec2(long.Parse(velNum[0]), long.Parse(velNum[1]));
            return new Bot(pos, pos, vel);
        }

        private Vec2 Wrap(Vec2 toWrap, Vec2 bounds) {
            var temp = toWrap % bounds;
            if (temp.X < 0) {
                temp = new Vec2(temp.X + bounds.X, temp.Y);
            }

            if (temp.Y < 0) {
                temp = new Vec2(temp.X, temp.Y + bounds.Y);
            }

            return temp;
        }
        private int Quadrant(Vec2 pos, Vec2 dim) {
            //Dim must be odd!
            var xBound = dim.X/2;
            var yBound = dim.Y/2;
            if (pos.X == xBound || pos.Y == yBound) {
                return 0;
            }

            if(pos.X < xBound && pos.Y < yBound) {
                return 1;
            } else if (pos.X > xBound && pos.Y < yBound) {
                return 2;
            } else if(pos.X < xBound && pos.Y > yBound) {
                return 3;
            } else { return 4; }
        }

        public record struct Bot(Vec2 Start, Vec2 Pos, Vec2 Velocity);

        private void Print(List<Bot> bots, Vec2 dim) {
            var g = new Grid<char>((int)dim.X, (int)dim.Y, ' ');
            foreach(var b in bots){
                g.G[(int)b.Pos.Y][(int)b.Pos.X] = 'X';
            }
            g.Print();
        } 

        private Vec2 AvgPos(List<Bot> bots) {
            var p = new Vec2(0,0);
            foreach(var b in bots) {
                p += b.Pos;
            }

            p /= bots.Count();
            return p;
        }

        private Vec2 Delta(List<Bot> bots, Vec2 target) {
            var d = new Vec2(0,0);
            foreach(var b in bots) {
                var diff = b.Pos - target;
                d += new Vec2(Math.Abs(diff.X), Math.Abs(diff.Y));
            }
            return d;
        }
        public override string P2()
        {
            var lines = InputAsLines();
            var FieldDim = new Vec2(11,7);
            FieldDim = new Vec2(101,103);
            var bots = new List<Bot>();
            foreach(var l in lines) {
                bots.Add(ParseLine(l));
            }

            var minDelta = long.MaxValue;
            var minDeltaRound = -1;

            for(int i = 0; i < 10000; i++) {
                for (int n = 0; n < bots.Count(); n++) {
                    var b = bots[n];
                    b.Pos += b.Velocity;
                    b.Pos = Wrap(b.Pos, FieldDim);
                    bots[n] = b;
                }
                var avg = AvgPos(bots);
                var delt = Delta(bots, avg);
                if ((delt.X + delt.Y) < minDelta) {
                    minDelta = (delt.X + delt.Y);
                    minDeltaRound = i;
                    if (i == 6474) {
                        Print(bots, FieldDim);
                    }
                }
            }
            return $"{minDelta}, {minDeltaRound+1}";
        }

    }
}