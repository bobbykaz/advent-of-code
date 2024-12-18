using Grids;
using Vec;

namespace y24 {
    public class D18: AoCDay {

        public D18(): base(24, 18) {
            _DebugPrinting = true;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var grid = BuildGrid(lines, 71, 1024);
            //grid.Print();

            var result = BFS(grid, 70);
            return $"{result}";
        }

        private int? BFS(Grid<int> grid, int targetIndex) {
            var start = new Branch(new Pos(0,0), new VisitedMap());
            start.VM.Visit(0,0);
            var target = new Pos(targetIndex,targetIndex);
            var bfs = new Queue<Branch>();
            bfs.Enqueue(start);

            var cache = new Dictionary<string, int>();
            cache[start.Pos.Key] = 0;
            long eval = 0;
            long skipped = 0;
            while(bfs.Any()) {
                var next = bfs.Dequeue();
                if(next.Pos == target) {
                    return next.VM.VisitedCount() - 1;
                }

                if(eval % 100 == 0) {
                    //PrintLn($"{eval} ({skipped}) - remaining: {bfs.Count()}: {next.Pos}");
                }
                var neighbors = grid.CardinalNeighbors(next.Pos.R, next.Pos.C)
                                    .Where(c => c.V == 0 && !next.VM.WasVisited(c.R, c.C))
                                    .ToList();
                //PrintLn($"{next.Pos}");
                foreach(var n in neighbors) {
                    //PrintLn($"  moving to {n.R}, {n.C}");
                    var newBranch = new Branch(new Pos(n.R, n.C), next.VM.Copy());
                    newBranch.VM.Visit(newBranch.Pos);                    
                    var steps = newBranch.VM.VisitedCount();
                    if(newBranch.Pos == target) {
                        return newBranch.VM.VisitedCount() - 1;
                    }
                    if(cache.ContainsKey(newBranch.Pos.Key)) {
                        if(steps < cache[newBranch.Pos.Key]){
                            cache[newBranch.Pos.Key] = steps;
                            bfs.Enqueue(newBranch);
                        } else {
                            skipped++;
                        }
                    } else {
                        cache[newBranch.Pos.Key] = steps;
                        bfs.Enqueue(newBranch);
                    }
                }

                var prunedBranches = bfs.ToList().OrderBy(b => b.VM.VisitedCount()).ToList();
                bfs.Clear();
                foreach(var b in prunedBranches) bfs.Enqueue(b);

                eval++;
            }
           
            return null;
        }

        private Grid<int> BuildGrid(List<string> lines, int size, int threshold) {
            var grid = new Grid<int>(size, size, 0);
            int count = 0;
            foreach(var l in lines) {
                var nums = Utilties.StringToNums<int>(l);
                var c = nums[0];
                var r = nums[1];
                grid.G[r][c] = 1;
                count++;
                if(count>= threshold) 
                    return grid;
            }

            return grid;
        }

        record struct Branch(Pos Pos, VisitedMap VM);

        public override string P2()
        {
            var lines = InputAsLines();
            var low = 1024;
            var high = lines.Count;

            while(high - low > 1) {
                PrintLn($"{low} - {high}");
                var target = high - ((high - low) / 2);
                var grid = BuildGrid(lines, 71, target);

                var result = BFS(grid, 70);
                if(result.HasValue) {
                    low = target;
                } else {
                    high = target;
                }
            }

            var higrid = BuildGrid(lines, 71, high);
            var hiresult = BFS(higrid, 70);
            if(hiresult.HasValue) {
                PrintLn($"uh oh high {high}");
            }

            var lowgrid = BuildGrid(lines, 71, low);
            var lowresult = BFS(lowgrid, 70);

            if(!lowresult.HasValue) {
                PrintLn($"uh oh low {low}");
            }

            return $"{high} : {lines[high-1]}";
        }

    }
}