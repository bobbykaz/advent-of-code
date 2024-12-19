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

        private class Boxmap {
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

            public List<Vec2> GetBoxesAboveOrBelow(long r, long c, bool isAbove) {
                // if a box is directly above this one, that is it;
                // if there is nothing _directly_ above this box, check the left and right
                // .[]...     .[][].    ...[].    .[]... 
                // .[]...     ..[]..    ..[]..    ..[].. 

                var targetR = r+1;
                if(isAbove) targetR = r-1;
            
                if(Boxes[targetR].Contains(c))
                    return new List<Vec2>(){new Vec2(targetR, c)};
                
                var possible = new List<Vec2>();
                if(Boxes[targetR].Contains(c-1))
                    possible.Add(new Vec2(targetR, c-1));
                
                if(Boxes[targetR].Contains(c+1))
                    possible.Add(new Vec2(targetR, c+1));

                return possible;
            }

            public List<(Thing, Vec2)> GetMoveableBoxesInLRChain(Vec2 start, Vec2 dir) {
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
                    Bot = next;
                } else if(nextThing == Thing.Wall) {

                } else if(nextThing == Thing.Box) {
                    var toMove = GetMoveableBoxesInLRChain(nextThingAt, dir);
                    if(toMove.Any()) {
                        //move everything
                        // move bot
                    } else {
                        //do nothing
                    }
                }
            }

            // putting on hold - doing a separate MoveLR and MoveUD above first, then this should just call those
            // public void Move(Vec2 dir) {
            //     var next = Bot + dir;
            //     var (nextThing, nextThingPos) = GetThingAt(next);
            //     if(nextThing == Thing.Wall) {
            //         //Do nothing
            //     } else if (nextThing == Thing.Box) {
            //         //find all neighboring boxes in chain
            //         var toMove = new HashSet<Vec2>();
            //         var stack = new Stack<Vec2>();
            //         var init = GetBoxAt(next);
            //         toMove.Add(init);
            //         stack.Push(init);
            //         bool wallInChain = false;
            //         while(stack.Any()) {
            //             var nextBox = stack.Pop();
            //             var nextCheck = nextBox + dir;
            //             if(SpotContainsWall(nextCheck)){
            //                 wallInChain = true;
            //                 stack.Clear();
            //                 break;
            //             }
            //             if(SpotContainsBox(nextCheck)){
            //                 var nextToMove = GetBoxAt(nextCheck);
            //                 toMove.Add(nextToMove);
            //                 stack.Push(nextToMove);
            //             }
            //         }
            //         //Move self and all boxes
            //         if(!wallInChain){
            //             Bot = next;
            //             foreach(var moved in toMove){
            //                 Boxes[moved.X].Remove(moved.Y);
            //             }
            //             foreach(var moved in toMove){
            //                 var nb = moved + dir;
            //                 Boxes[nb.X].Add(nb.Y);
            //             }
            //         }
            //     } else {
            //         //its clear!
            //         Bot = next;
            //     }
            // }
        }
        private Boxmap DoubleGrid(Grid<char> source) {
            var result = new Boxmap();
            source.ForEachColRow((r,c,v)=>{
                switch(v) {
                    case 'O': 
                        if(!result.Boxes.ContainsKey(r)) { result.Walls[r] = new HashSet<long>();}
                        result.Boxes[r].Add(2*c);
                        break;
                    case '#': 
                        if(!result.Walls.ContainsKey(r)) { result.Walls[r] = new HashSet<long>();}
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