using System.ComponentModel;
using System.Runtime.Intrinsics.Arm;
using Grids;
using Microsoft.VisualBasic;
using Vec;

namespace y24 {
    public class D20: AoCDay {

        public D20(): base(24, 20) {
            _DebugPrinting = true;
        }
        public override string P1()
        {
            var grid = initGrid();

            var cheats = new HashSet<Cheat>();
            grid.ForEachRowCol((r,c,v) => {
                var possibleCheats = GetCellsWithinManhattenN(grid, new Pos(r,c), 2);
                foreach(var cheatCell in possibleCheats) {
                    if (cheatCell.V != -100 && cheatCell.V < (v - 2)) {
                        cheats.Add(new Cheat(new Pos(r,c), new Pos(cheatCell.R, cheatCell.C), (v-cheatCell.V) - 2));
                    }
                }
            });

            PrintLn($"total cheats: {cheats.Count()}");
            var gt100 = cheats.Where(c => c.Saved >= 100).Count();
            PrintLn($">100 cheats: {gt100}");


           
            return $"{gt100}";
        }
        
        private List<Cell<long>> GetCellsWithin2(Grid<long> g, Pos target) {
            var n1 = g.CardinalNeighbors(target.R, target.C);
            var n2 = new HashSet<Pos>();
            foreach(var n in n1){
                var l2neighbors = g.CardinalNeighbors(n.R, n.C);
                foreach(var nn in l2neighbors){
                    n2.Add(new Pos(nn.R, nn.C));
                }
            }
            var rslt = n2
                .Where(p => p != target)
                .Select(p => {
                    var cell = g.GetCellIfValid(p.R, p.C);
                    if(cell != null) return cell.Value;
                    throw new Exception();
                    })
                .ToList();

            return rslt;
        }

        private List<Cell<long>> GetCellsWithinManhattenN(Grid<long> g, Pos target, int manhattenDistance) {
            var set = new HashSet<Pos>();
            for(int r = target.R - manhattenDistance; r <= target.R + manhattenDistance; r++){
                for(int c = target.C - manhattenDistance; c <= target.C + manhattenDistance; c++){
                    var cellExists = g.GetCellIfValid(r,c);
                    if(cellExists.HasValue){
                        var other = new Pos(r,c);
                        var dist = target.ManhattenDistance(other);
                        if(dist <= manhattenDistance && dist > 0) {
                            set.Add(other);
                        }
                    }
                }
            }
            
            var rslt = set
                .Where(p => p != target)
                .Select(p => {
                    var cell = g.GetCellIfValid(p.R, p.C);
                    if(cell != null) return cell.Value;
                    throw new Exception();
                    })
                .ToList();

            return rslt;
        }


        private record struct Cheat(Pos Start, Pos End, long Saved);

        private Grid<long> initGrid() {
            var lines = InputAsLines();
            var cgrid = Utilties.RectangularCharGridFromLines(lines);
            var start = new Pos(-1,-1);
            var end = new Pos(-1,-1);
            var grid = new Grid<long>(cgrid.Width, cgrid.Height, -1);
            cgrid.ForEachRowCol((r,c,v) => {
                if(v == 'S') {
                    start = new Pos(r, c);
                    grid.G[r][c] = -1;
                }
                if(v == 'E')
                {
                    end = new Pos(r, c);
                    grid.G[r][c] = 0;
                }
                if(v == '#') {
                    grid.G[r][c] = -100;
                }

                if(v == '.') {
                    grid.G[r][c] = -1;
                }
            });

            var bfs = new Queue<Pos>();
            bfs.Enqueue(end);
            while(bfs.Any()) {
                var current = bfs.Dequeue();
                var currentVal = grid.G[current.R][current.C];
                var neigh = grid.CardinalNeighbors(current.R, current.C)
                                .Where( c => c.V == -1)
                                .ToList();

                foreach(var n in neigh){
                    grid.G[n.R][n.C] = currentVal + 1;
                    bfs.Enqueue(new Pos(n.R,n.C));
                }
            }
            PrintLn($"Dist from start to end: {grid.G[start.R][start.C]}");

            return grid;
        }

        public override string P2()
        {
            var grid = initGrid();

            var cheats = new HashSet<Cheat>();
            grid.ForEachRowCol((r,c,v) => {
                var thisPos = new Pos(r,c);
                var possibleCheats = GetCellsWithinManhattenN(grid, thisPos, 20);
                foreach(var cheatCell in possibleCheats) {
                    if (cheatCell.V != -100) {
                        var endPos = new Pos(cheatCell.R, cheatCell.C);
                        var dist = thisPos.ManhattenDistance(endPos);
                        if(cheatCell.V < (v - dist)) {
                            cheats.Add(new Cheat(thisPos, new Pos(cheatCell.R, cheatCell.C), (v-cheatCell.V) - dist));
                        }
                    }
                }
            });

            PrintLn($"total cheats: {cheats.Count()}");
            var gt100 = cheats.Where(c => c.Saved >= 100).Count();
            PrintLn($">100 cheats: {gt100}");


           
            return $"{gt100}";
        }

    }
}