using System.Data;
using Grids;
using Vec;

namespace y24 {
    public class D15: AoCDay {

        public D15(): base(24, 15) {
            _DebugPrinting = false;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var groups = Utilties.GroupInputAsBlocks(lines);
            var grid = Utilties.RectangularCharGridFromLines(groups[0]);
            var instructions = groups[1];

            var dirIns = string.Join("", instructions).ToCharArray().Select(c => GridUtilities.CardinalDirFromChar(c)).ToList();

            var current = new Pos(-1,-1);
            grid.ForEachRowCol((r,c,v) => {
                if(v == '@') {
                    current = new Pos(r,c);
                }
            });

            foreach(var dir in dirIns){
                PrintLn($"Move: {dir}");
                var nextCell = grid.GetNeighbor(current.R, current.C, dir);
                if(!nextCell.HasValue) {throw new Exception();}

                if(nextCell.Value.V == '#') {
                    // do nothing
                } else if (nextCell.Value.V == '.') {
                    grid.G[current.R][current.C] = '.';
                    grid.G[nextCell.Value.R][nextCell.Value.C] = '@';
                    current = new Pos(nextCell.Value.R,nextCell.Value.C);
                } else if (nextCell.Value.V == 'O') {
                    var final = grid.GetNeighbor(nextCell.Value.R,nextCell.Value.C, dir);
                    while(final.HasValue && final.Value.V == 'O') {
                        final = grid.GetNeighbor(final.Value.R,final.Value.C, dir);
                    }
                    if(final.HasValue && final.Value.V == '.') {
                        grid.G[current.R][current.C] = '.';
                        grid.G[nextCell.Value.R][nextCell.Value.C] = '@';
                        grid.G[final.Value.R][final.Value.C] = 'O';
                        current = new Pos(nextCell.Value.R,nextCell.Value.C);
                    }
                }
                //grid.Print();
                PrintLn("=====");
            }


            var total = scoreGrid(grid);

            //grid.Print();
           
            return $"{total}";
        }

        private long scoreGrid(Grid<char> g){
            long total = 0L;
            g.ForEachRowCol((r,c,v) =>{
                if (v == 'O') {
                    total += 100*r + c;
                }
            });
            return total;
        }

        public override string P2()
        {
            _DebugPrinting = false;
            var lines = InputAsLines();
            var groups = Utilties.GroupInputAsBlocks(lines);
            var grid = Utilties.RectangularCharGridFromLines(groups[0]);
            var instructions = groups[1];

            var dirIns = string.Join("", instructions).ToCharArray().Select(c => GridUtilities.CardinalDirFromChar(c)).ToList();

            var current = new Pos(-1,-1);
            //grid.Print();
            var boxmap = DoubleGrid(grid);
            boxmap.Print(_DebugPrinting);
            boxmap.debug = _DebugPrinting;


            foreach(var dir in dirIns){
                PrintLn($"Move: {dir}");
                boxmap.Move(dir);
                //boxmap.Print(_DebugPrinting);
                //PrintLn("=====");
            }

            boxmap.Print(true);
            return $"{boxmap.Score()}";
        }

        private class Boxmap {
            public int Width;
            public int Height;
            public bool debug = false;
            public Dictionary<long, HashSet<long>> Walls = [];
            public Dictionary<long, HashSet<long>> Boxes = [];
            public Vec2 Bot = new Vec2(0,0);

            public enum Thing {
                Box, Wall, Empty
            }


            public (Thing, Vec2) GetThingAt(Vec2 next) {
                var (r,c) = (next.X, next.Y);
                if(Walls[r].Contains(c)) {
                    return (Thing.Wall, new Vec2(r,c));
                }
                if(Walls[r].Contains(c-1)) {
                    return (Thing.Wall, new Vec2(r,c-1));
                }

                if(Boxes[r].Contains(c)) {
                    return (Thing.Box, new Vec2(r,c));
                }
                if(Boxes[r].Contains(c-1)) {
                    return (Thing.Box, new Vec2(r,c-1));
                }

                return (Thing.Empty, next);
            }

            public List<(Thing,Vec2)> GetThingsUD(Vec2 targetPos) {
                // if a box is directly above this one, that is it;
                // if there is nothing _directly_ above this box, check the left and right
                // .[]...     .[][].    ...[].    .[]... 
                // .[]...     ..[]..    ..[]..    ..[].. 
                var results = new List<(Thing,Vec2)>();

                var targetR = targetPos.X;
                var c = targetPos.Y;
                if(Boxes[targetR].Contains(c)){
                    results.Add((Thing.Box, targetPos));
                    return results;
                }

                if(Walls[targetR].Contains(c)){
                    results.Add((Thing.Wall, targetPos));
                    return results;
                }
                
                if(Boxes[targetR].Contains(c-1))
                    results.Add((Thing.Box,new Vec2(targetR, c-1)));
                
                if(Boxes[targetR].Contains(c+1))
                    results.Add((Thing.Box,new Vec2(targetR, c+1)));

                if(Walls[targetR].Contains(c-1))
                    results.Add((Thing.Wall,new Vec2(targetR, c-1)));
                
                if(Walls[targetR].Contains(c+1))
                    results.Add((Thing.Wall,new Vec2(targetR, c+1)));

                return results;
            }

            public List<(Thing, Vec2)> GetMoveableBoxesInUDChain(Vec2 startingBox, Vec2 dir) {
                var toMove = new List<(Thing, Vec2)>();
                var toEval = new Queue<(Thing, Vec2)>();
                toEval.Enqueue((Thing.Box, startingBox));
                while(toEval.Any()){
                    var (currentThing, currentThingAt) = toEval.Dequeue();
                    toMove.Add((currentThing, currentThingAt));
                    var nextLevelPos = currentThingAt + dir;
                    var thingsAtNextLevel = GetThingsUD(nextLevelPos);
                    if(thingsAtNextLevel.Any(t => t.Item1 == Thing.Wall)) {
                        //any walls, cant move
                        return [];
                    }
                    foreach(var t in thingsAtNextLevel){
                        toEval.Enqueue(t);
                    }
                }
                return toMove;
            }

            public void MoveUD(Dir d) {
                var dir = new Vec2(1,0);
                if(d == Dir.N) 
                    dir = new Vec2(-1,0);
                else if(d != Dir.S) throw new Exception();

                var next = Bot + dir;
                var (nextThing, nextThingAt) = GetThingAt(next);
                if(nextThing == Thing.Empty) {
                    if(debug) {
                        Console.WriteLine($"Moving to empty space at {next}");
                    }
                    Bot = next;
                } else if(nextThing == Thing.Wall) {
                    if(debug) {
                        Console.WriteLine($"Wall at {next}; do nothing");
                    }
                } else if(nextThing == Thing.Box) {
                    if(debug) {
                        Console.WriteLine($"Box at {next}, calculating...");
                    }
                    var toMove = GetMoveableBoxesInUDChain(nextThingAt, dir);
                    if(toMove.Any()) {
                        if(debug) {
                            Console.WriteLine($"Found boxes to move: {string.Join(",", toMove)}");
                        }
                        foreach (var (b, pos) in toMove){
                            if(b != Thing.Box) throw new Exception();
                            Boxes[pos.X].Remove(pos.Y);
                        }
                        foreach (var (b, pos) in toMove){
                            var newPos = pos + dir;
                            Boxes[newPos.X].Add(newPos.Y);
                        }
                        Bot = next;
                    } else {
                        if(debug) {
                            Console.WriteLine($"Wall at in chain; do nothing");
                        }
                    }
                }
            }

            public List<(Thing, Vec2)> GetMoveableBoxesInLRChain(Vec2 start, Dir d) {
                var dir = new Vec2(0,-1);
                if(d == Dir.E){
                    // when moving right, from the position of a box, you always have to skip one spot
                    dir = new Vec2(0,2);
                }
                var toMove = new List<(Thing, Vec2)>();
                var n = GetThingAt(start);
                while(n.Item1 == Thing.Box) {
                    var (currentThing, currentThingAt) = n;
                    toMove.Add(n);
                    n = GetThingAt(currentThingAt + dir);
                }
                if (n.Item1 == Thing.Empty)
                    return toMove;
                else
                 return new List<(Thing, Vec2)>();
            }
            public void MoveLR(Dir d) {
                var dir = new Vec2(0,1);
                if(d == Dir.W) 
                    dir = new Vec2(0,-1);
                else if(d != Dir.E) throw new Exception();

                var next = Bot + dir;
                var (nextThing, nextThingAt) = GetThingAt(next);
                if(nextThing == Thing.Empty) {
                    if(debug){
                        Console.WriteLine($"Moving to empty space at {next}");
                    }
                    Bot = next;
                } else if(nextThing == Thing.Wall) {
                    if(debug){
                        Console.WriteLine($"Wall at {next}; do nothing");
                    }
                } else if(nextThing == Thing.Box) {
                    if(debug){
                        Console.WriteLine($"Box at {next}, calculating...");
                    }
                    var toMove = GetMoveableBoxesInLRChain(nextThingAt, d);
                    if(debug){
                            Console.WriteLine($"Found boxes to move: {string.Join(",", toMove)}");
                        }
                    if(toMove.Any()) {
                        foreach (var (b, pos) in toMove){
                            if(b != Thing.Box) throw new Exception();
                            Boxes[pos.X].Remove(pos.Y);
                        }
                        foreach (var (b, pos) in toMove){
                            var newPos = pos + dir;
                            Boxes[pos.X].Add(newPos.Y);
                        }
                        Bot = next;
                    } else {
                        if(debug){
                            Console.WriteLine($"No move, wall in chain");
                        }
                        //do nothing
                    }
                }
            }

            public void Move(Dir d){
                switch(d) {
                    case Dir.N:
                    case Dir.S:
                        MoveUD(d);
                        break;
                    case Dir.E:
                    case Dir.W:
                        MoveLR(d);
                        break;
                    default:
                        throw new Exception();
                }
            }

            public long Score() {
                var total = 0L;
                foreach(var r in Boxes.Keys) {
                    foreach(var c in Boxes[r]){
                            total += (100 * r) + c;
                        }
                }

                return total;
            }
            public void Print(bool debug){
                if(debug){
                    var g = new Grid<char>(Width, Height, '.');
                    foreach(var r in Walls.Keys) {
                        foreach(var c in Walls[r]){
                            g.G[(int)r][(int)c] = '#';
                            g.G[(int)r][(int)c+1] = '#';
                        }
                    }

                    foreach(var r in Boxes.Keys) {
                        foreach(var c in Boxes[r]){
                            g.G[(int)r][(int)c] = '[';
                            g.G[(int)r][(int)c+1] = ']';
                        }
                    }
                    g.G[(int)Bot.X][(int)Bot.Y] = '@';
                    g.Print();
                }
            }
        }
        private Boxmap DoubleGrid(Grid<char> source) {
            var result = new Boxmap(){Width = source.Width * 2, Height = source.Height};
            source.ForEachColRow((r,c,v)=>{
                if(!result.Boxes.ContainsKey(r)) { result.Boxes[r] = new HashSet<long>();}
                if(!result.Walls.ContainsKey(r)) { result.Walls[r] = new HashSet<long>();}
                switch(v) {
                    case 'O': 
                        result.Boxes[r].Add(2*c);
                        break;
                    case '#': 
                        result.Walls[r].Add(2*c);
                        break;
                    case '@': 
                        result.Bot = new Vec2(r, 2*c);
                        break;
                    default: 
                        break;
                };
            });
            
            return result;
        }
    }
}