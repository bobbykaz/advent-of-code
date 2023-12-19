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
            // completed ranges are known to be filled in - partial ranges need to figure out whether "inside" is to the left or right
            // in between ranges also need to be figured out.
            // 'missing rows' in the list are the same as the previous row
            // the top row should have no incomplete ranges, and  
            var rowRanges = new List<RowRange>();

            var (extRow,extCol) = (0L,0L);
            var (rn,cn,rx,cx) = (0L,0L,0L,0L);
            var rri = 0;
            rowRanges.Add(new RowRange(0));
            rowRanges[0].AddPoint(0);
            foreach(var line in lines) {
                var ins = new Inst2(line);
                PrintLn($"{ins.dir} - {ins.num}");
                
                rri = rowRanges.FindIndex((rr) => rr.r == extRow);
                if(rri == -1) throw new Exception("uh oh");

                var nextRri = rri;

                switch(ins.dir){
                    case Dir.N:
                        extRow += ins.num; 
                        while(rri > -1 && rowRanges[rri].r < extRow){
                            rowRanges[rri].AddPoint(extCol);
                            rri--;
                        }
                        // -1 rri means new row to add at the very beginning - just need one data point
                        // non-(-1) rri with non-equal R means we need a new row duped
                        if(rri == -1){
                            var newUpRow = new RowRange(extRow);
                            newUpRow.AddPoint(extCol);
                            rowRanges.Add(newUpRow);
                            rowRanges = rowRanges.OrderByDescending(rr => rr.r).ToList();
                        }else if (!(rowRanges[rri].r < extRow)) {
                            var newUpRow = rowRanges[rri+1].DupeToRow(extRow);
                            newUpRow.AddPoint(extCol);
                            rowRanges.Add(newUpRow);
                            rowRanges = rowRanges.OrderByDescending(rr => rr.r).ToList();
                        }
                        break;
                    case Dir.S:
                        extRow -= ins.num; 
                        while(rri < rowRanges.Count && rowRanges[rri].r > extRow){
                            rowRanges[rri].AddPoint(extCol);
                            rri++;
                        }
                        // Count rri means new row to add at the very end - just need one data point
                        // non-(-1) rri with non-equal R means we need a new row duped
                        if(!(rri < rowRanges.Count)){
                            var newDownRow = new RowRange(extRow);
                            newDownRow.AddPoint(extCol);
                            rowRanges.Add(newDownRow);
                            rowRanges = rowRanges.OrderByDescending(rr => rr.r).ToList();
                        }else if(!(rowRanges[rri].r > extRow)) {
                            var newUpRow = rowRanges[rri-1].DupeToRow(extRow);
                            newUpRow.AddPoint(extCol);
                            rowRanges.Add(newUpRow);
                            rowRanges = rowRanges.OrderByDescending(rr => rr.r).ToList();
                        }
                        break;
                    case Dir.E:
                        rowRanges[rri].Add(extCol, extCol+ins.num);
                        extCol += ins.num; 
                        break;
                    case Dir.W: 
                    default:
                        rowRanges[rri].Add(extCol-ins.num, extCol);
                        extCol -= ins.num;
                        break;
                }
            }
            PrintLn($"RRs: {rowRanges.Count}");
            foreach(var rr in rowRanges){
                rr.ranges = rr.ranges.OrderBy(ran => ran.start).ToList();
                foreach(var ran in rr.ranges) {
                    Print(ran.ToString());
                    Print(", ");
                }
                PrintLn("");
            }
            return $"{CountInterior(rowRanges)}";
        }

        public long CountInterior(List<RowRange> rowRanges) {
            var curRR = rowRanges[0];
            var curRRSize = curRR.ranges[0].Length ?? throw new Exception("first row must be complete");

            return 0L;
        }
        // (1,4) len 4 .... (4,7) len 4
        public class Range {
            public long start;
            public long? end;

            public Range(long s, long? e = null) {
                start = s;
                end = e;
            }

            public long? Length {get {
                if(end.HasValue){ return (end.Value - start) + 1;}
                return null;
                }
            }

            public bool IsPartial { get {return !end.HasValue;}}
            public bool IsWhole {get {return !IsPartial;}}

            public Range Copy() {
                return new Range(start, end);
            }
            public bool Overlaps(Range other) {
                if(IsPartial && other.IsPartial) {
                    return this.start == other.start;
                } else if (IsWhole && other.IsPartial) {
                    return other.start <= this.end && other.start >= this.start;
                } else if (IsPartial && other.IsWhole) {

                } else { //both whole

                }
            }

            public override string ToString()
            {
                var endCh = end.HasValue? $"{end}" : "?";
                return $"({start}, {endCh})";
            }
        }

        public class RowRange {
            public long r;
            public List<Range> ranges = new List<Range>();
            public RowRange(long r) {
                this.r = r;
            }

            public void Add(long left, long right){
                ranges.Add(new Range(left, right));
                ranges = ranges
                        .Where(r => !(r.IsPartial && (r.start == left || r.start == right)))
                        .ToList();
            }

            public void AddPoint(long c) {
                if(!ranges.Any(r => r.start == c || r.end == c))
                    ranges.Add(new Range(c, null));
            }

            public RowRange DupeToRow(long r) {
                var other = new RowRange(r)
                {
                    ranges = this.ranges.Select(ran => ran.Copy()).ToList()
                };
                return other;
            }
        }
    }
}