using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
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


            public bool SpotContainsWall(Vec2 next) {
                var (r,c) = (next.X, next.Y);
                return Walls[r].Contains(c) || Walls[r].Contains(c-1);
            }

            public bool SpotContainsBox(Vec2 next) {
                var (r,c) = (next.X, next.Y);
                return Boxes[r].Contains(c) || Boxes[r].Contains(c-1);
            }

            public List<Vec2> GetBoxesAt(Vec2 next) {
                var result = new List<Vec2>();
                var (r,c) = (next.X, next.Y);
                if(Boxes[r].Contains(c)) {
                    result.Add(new Vec2(r, c));
                }
                if(Boxes[r].Contains(c-1)) {
                    result.Add(new Vec2(r, c-1));
                }
                

                // For L/R - can only be one box to the side.
                // For N/S, Can be possibly two boxes

                throw new Exception($"no box at {next}");
            }
            public void Move(Vec2 dir) {
                var next = Bot + dir;
                if(SpotContainsWall(next)) {
                    //Do nothing
                } else if (SpotContainsBox(next)) {
                    //find all neighboring boxes in chain
                    var toMove = new HashSet<Vec2>();
                    var stack = new Stack<Vec2>();
                    var init = GetBoxAt(next);
                    toMove.Add(init);
                    stack.Push(init);
                    bool wallInChain = false;
                    while(stack.Any()) {
                        var nextBox = stack.Pop();
                        var nextCheck = nextBox + dir;
                        if(SpotContainsWall(nextCheck)){
                            wallInChain = true;
                            stack.Clear();
                            break;
                        }
                        if(SpotContainsBox(nextCheck)){
                            var nextToMove = GetBoxAt(nextCheck);
                            toMove.Add(nextToMove);
                            stack.Push(nextToMove);
                        }
                    }
                    //Move self and all boxes
                    if(!wallInChain){
                        Bot = next;
                        foreach(var moved in toMove){
                            Boxes[moved.X].Remove(moved.Y);
                        }
                        foreach(var moved in toMove){
                            var nb = moved + dir;
                            Boxes[nb.X].Add(nb.Y);
                        }
                    }
                } else {
                    //its clear!
                    Bot = next;
                }
            }
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