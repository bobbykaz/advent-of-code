using System.Runtime.CompilerServices;
using Grids;

namespace y23 {
    public class D18 : AoCDay
    {
        public D18(): base(23, 18) {
            _DebugPrinting = true;
        }
        
        public override string P1()
        {
            var lines = InputAsLines();
            var grid = new Grid<char>(1000,1000, '.');
            var (r,c) = (500,500);
            grid.G[r][c] = '#';
            var count = 0;
            foreach(var line in lines) {
                var ins = new Inst(line);
                for(int i = 0; i < ins.num; i++) {
                    var next = grid.GetNeighbor(r,c, ins.dir) ?? throw new Exception($"uh oh {count}");
                    (r,c) = (next.R, next.C);
                    grid.G[r][c] = '#';
                }
                count++;
            }
            //grid.Print();
            var vm = new VisitedMap();
            var flood = new Queue<(int,int)>();
            flood.Enqueue((407,506));
            grid.G[407][506] = '#';
            while(flood.Any()) {
                var (nr,nc) = flood.Dequeue();
                //PrintLn($"{nr}-{nc}: {grid.G[nr][nc]}");

                var neighbors = grid.CardinalNeighbors(nr,nc);
                foreach(var cell in neighbors) {
                    if(cell.V == '.') {
                        //PrintLn($"   neighbor {nr}-{nc}: {cell.R}{cell.C}");
                        vm.Visit(cell.R, cell.C);
                        grid.G[cell.R][cell.C] = '#';
                        flood.Enqueue((cell.R,cell.C));
                    }
                }
            }

            var numhash = 0;
            grid.ForEachColRow((r,c,v) =>{
                if(v == '#') {
                    numhash++;
                }
            });
            
            return $"{numhash}";
        }

        public class Inst {
            public Dir dir;
            public int num;
            public string color;
            public Inst( string line) {
                var pts = line.Split(' ');
                dir = GridUtilities.DirFromChar(pts[0][0]);
                num = int.Parse(pts[1]);
                color = pts[2];
            }
        }

        public class Inst2 {
            public Dir dir;
            public long num;
            public Inst2( string line) {
                var pts = line.Split(' ');
                dir = GridUtilities.DirFromChar(pts[0][0]);
                num = int.Parse(pts[1]);
                //(#70c710)
                var color = pts[2].Substring(2,6);
                dir = color[5] switch {
                    '0' => Dir.E,
                    '1' => Dir.S,
                    '2' => Dir.W,
                    '3' => Dir.N,
                    _ => throw new Exception("uh oh bad dir")
                };
                num = Convert.ToInt64(color.Substring(0,5), 16);
            }
        }

        public override string P2()
        {
            var lines = InputAsLines();
            // each rowRange is a list of all the distinct parts of that row
            // should always have an even number, and the filled lines in that row is just the sum of the pairs of points(adding 1 to each pair)
            //ex : 0,3,7,8,11,13 = ####...##..###
            // 'missing rows' in the range are the same as the previous row
            //BUT..... rows can had an odd number of points, iff there was a l/R draw!
            var rowRanges = new List<RowRange>();

            var (r,c) = (0L,0L);
            var (rn,cn,rx,cx) = (0L,0L,0L,0L);
            var count = 0;
            var rri = 0;
            rowRanges.Add(new RowRange(0));
            rowRanges[0].AddC(0);
            foreach(var line in lines) {
                var ins = new Inst2(line);
                PrintLn($"{ins.dir} - {ins.num}");
                
                rri = rowRanges.FindIndex((rr) => rr.r == r);
                if(rri == -1) throw new Exception("uh oh");

                var nextRri = rri;

                switch(ins.dir){
                    case Dir.N:
                        r += ins.num; 
                        while(rri > -1 && rowRanges[rri].r < r){
                            rowRanges[rri].AddC(c);
                            rri--;
                        }
                        // -1 rri means new row to add at the very beginning - just need one data point
                        // non-(-1) rri with non-equal R means we need a new row duped
                        if(rri == -1){
                            var newUpRow = new RowRange(r);
                            newUpRow.AddC(c);
                            rowRanges.Add(newUpRow);
                            rowRanges = rowRanges.OrderByDescending(rr => rr.r).ToList();
                        }else if (!(rowRanges[rri].r < r)) {
                            var newUpRow = rowRanges[rri+1].DupeToRow(r);
                            newUpRow.AddC(c);
                            rowRanges.Add(newUpRow);
                            rowRanges = rowRanges.OrderByDescending(rr => rr.r).ToList();
                        }
                        break;
                    case Dir.S:
                        r -= ins.num; 
                        while(rri < rowRanges.Count && rowRanges[rri].r > r){
                            rowRanges[rri].AddC(c);
                            rri++;
                        }
                        // Count rri means new row to add at the very end - just need one data point
                        // non-(-1) rri with non-equal R means we need a new row duped
                        if(!(rri < rowRanges.Count)){
                            var newDownRow = new RowRange(r);
                            newDownRow.AddC(c);
                            rowRanges.Add(newDownRow);
                            rowRanges = rowRanges.OrderByDescending(rr => rr.r).ToList();
                        }else if(!(rowRanges[rri].r > r)) {
                            var newUpRow = rowRanges[rri-1].DupeToRow(r);
                            newUpRow.AddC(c);
                            rowRanges.Add(newUpRow);
                            rowRanges = rowRanges.OrderByDescending(rr => rr.r).ToList();
                        }
                        break;
                    case Dir.E:
                        c += ins.num; 
                        rowRanges[rri].AddC(c);
                        break;
                    case Dir.W: 
                    default:
                        c -= ins.num;
                        rowRanges[rri].AddC(c); 
                        break;
                }
            }
            PrintLn($"RRs: {rowRanges.Count}");
            rowRanges.ForEach(rr => {if (rr.colPts.Count % 2 != 0) PrintLn($"uh oh {rr.r}");});
            return $"{0}";
        }

        public class RowRange {
            public long r;
            public List<long> colPts = new List<long>();
            public RowRange(long r) {
                this.r = r;
            }

            public void AddC(long c) {
                if(!colPts.Contains(c)){
                    colPts.Add(c);
                    colPts.Sort();
                }
            }

            public RowRange DupeToRow(long r) {
                var other = new RowRange(r);
                other.colPts = this.colPts.ToList();
                return other;
            }
        }
    }
}