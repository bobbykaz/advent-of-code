using System.Security;
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
                //PrintLn($"{ins.dir} - {ins.num}");
                
                rri = rowRanges.FindIndex((rr) => rr.row == extRow);
                if(rri == -1) throw new Exception("uh oh");

                var nextRri = rri;

                switch(ins.dir){
                    case Dir.N:
                        extRow += ins.num; 
                        while(rri > -1 && rowRanges[rri].row < extRow){
                            rowRanges[rri].AddPoint(extCol);
                            rri--;
                        }
                        // -1 rri means new row to add at the very beginning - just need one data point
                        // non-(-1) rri with non-equal R means we need a new row duped
                        if(rri == -1){
                            var newUpRow = new RowRange(extRow);
                            newUpRow.AddPoint(extCol);
                            rowRanges.Add(newUpRow);
                            rowRanges = rowRanges.OrderByDescending(rr => rr.row).ToList();
                        }else if (!(rowRanges[rri].row < extRow)) {
                            var newUpRow = rowRanges[rri+1].DupeToRow(extRow);
                            newUpRow.AddPoint(extCol);
                            rowRanges.Add(newUpRow);
                            rowRanges = rowRanges.OrderByDescending(rr => rr.row).ToList();
                        }
                        break;
                    case Dir.S:
                        extRow -= ins.num; 
                        while(rri < rowRanges.Count && rowRanges[rri].row > extRow){
                            rowRanges[rri].AddPoint(extCol);
                            rri++;
                        }
                        // Count rri means new row to add at the very end - just need one data point
                        // non-(-1) rri with non-equal R means we need a new row duped
                        if(!(rri < rowRanges.Count)){
                            var newDownRow = new RowRange(extRow);
                            newDownRow.AddPoint(extCol);
                            rowRanges.Add(newDownRow);
                            rowRanges = rowRanges.OrderByDescending(rr => rr.row).ToList();
                        }else if(!(rowRanges[rri].row > extRow)) {
                            var newUpRow = rowRanges[rri-1].DupeToRow(extRow);
                            newUpRow.AddPoint(extCol);
                            rowRanges.Add(newUpRow);
                            rowRanges = rowRanges.OrderByDescending(rr => rr.row).ToList();
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
            PrintLn("\nCleaning\n");
            rowRanges = CleanUpRRs(rowRanges);

            foreach(var rr in rowRanges){
                rr.ranges = rr.ranges.OrderBy(ran => ran.start).ToList();
                foreach(var ran in rr.ranges) {
                    Print(ran.ToString());
                    Print(", ");
                }
                PrintLn("");
            }

            PrintLn("\n Counting \n");
            return $"{CountInterior(rowRanges)}";
        }

        public List<RowRange> CleanUpRRs(List<RowRange> rrs) {
            return rrs.Select(CleanUp).ToList();
        }

        public RowRange CleanUp(RowRange rr){
            //find same start / end
            for (int i = 1; i < rr.ranges.Count; i++) {
                var cur = rr.ranges[i];
                var prev = rr.ranges[i-1];
                if(prev.IsWhole && cur.start == prev.end) {
                    PrintLn($"Merging {prev} and {cur}");
                    prev.end = cur.end;
                    rr.ranges.RemoveAt(i);
                    PrintLn($"...Merged to {rr.ranges[i-1]}");
                    i--;
                }
            }
            //any dupes?
            for (int i = 1; i < rr.ranges.Count; i++) {
                var cur = rr.ranges[i];
                var prev = rr.ranges[i-1];
                if(prev.start == cur.start && prev.end == cur.end) {
                    PrintLn($"Dupe found: {cur}");
                    rr.ranges.RemoveAt(i);
                    i--;
                }
            }

            
            if(rr.ranges.Count > 1){
                //if the first is partial it has to be merged right?
                if(rr.ranges[0].IsPartial) {
                    if(rr.ranges[1].IsPartial) {
                        PrintLn($"Merging partial start {rr.ranges[0]} to partial right: {rr.ranges[1]}");
                        rr.ranges[0].end = rr.ranges[1].start;
                        rr.ranges.RemoveAt(1);
                        PrintLn($" Merged:{rr.ranges[0]}");
                    } else {
                        PrintLn($"Merging partial start {rr.ranges[0]} to whole right: {rr.ranges[1]}");
                        rr.ranges[1].start = rr.ranges[0].start;
                        rr.ranges.RemoveAt(0);
                        PrintLn($" Merged:{rr.ranges[0]}");
                    }
                }
            }

            if(rr.ranges.Count > 1){
                //if the last is partial it has to be merged right?
                var lastIndex = rr.ranges.Count - 1;
                if(rr.ranges[lastIndex].IsPartial) {
                    PrintLn($"Merging partial end {rr.ranges[lastIndex]} to the left: {rr.ranges[lastIndex - 1]}");
                    rr.ranges[lastIndex - 1].end = rr.ranges[lastIndex].start;
                    rr.ranges.RemoveAt(lastIndex);
                    PrintLn($" Merged:{rr.ranges[lastIndex - 1]}");
                }
            }

            return rr;
        }

        public long CountInterior(List<RowRange> rowRanges) {
            var curRR = rowRanges[0];
            var curRRSize = curRR.ranges[0].Length;
            var totalSize = 0L;
            PrintLn($"Initial row size :{curRRSize} ({curRR})");
            for(int i = 1; i < rowRanges.Count; i++) {
                PrintLn("Row " + i);
                var next = rowRanges[i];
                PrintLn($"   Init: {next}");
                // copy the previous line all the way down to this one
                totalSize += curRRSize * (curRR.row - next.row -1);
                //test left and right of each range in this row to see if its inside a range in the prev row
                // if so, merge the ranges to the left/right
                //the goal is to have only 'completed' not connecting rows in "next" before repeating

                //clear partials - first/last row already cant be
                for(int r = 1; r < next.ranges.Count - 1; r++) {
                    var nextRange = next.ranges[r];
                    PrintLn($"      Testing {nextRange}");
                    if(nextRange.IsPartial) {
                        // should it merge left?
                        var testLeft = nextRange.start - 1;
                        var testRight = nextRange.start + 1;
                        if (curRR.ranges.Any(rr => rr.ContainsPoint(testLeft))) {
                            next.ranges[r-1].end = nextRange.start;
                            next.ranges.RemoveAt(r);
                            r--;
                        } else if (curRR.ranges.Any(rr => rr.ContainsPoint(testRight))){ // should it merge right?
                            next.ranges[r+1].start = nextRange.start;
                            next.ranges.RemoveAt(r);
                            r--;
                        } else {
                            //
                            throw new Exception("Wasnt expecting to get here");
                        }
                    }
                }
                                
                // There should no longer be any partial ranges
                //check in between remaining rows
                for(int r = 0; r < next.ranges.Count - 1; r++) {
                    var nextRange = next.ranges[r];
                    // should it merge left?
                    var testRight = nextRange.end.Value + 1;
                    if (curRR.ranges.Any(rr => rr.ContainsPoint(testRight))){ // should it merge right?
                        next.ranges[r+1].start = nextRange.end ?? throw new Exception("partial range unexpected at end step");
                        next.ranges.RemoveAt(r);
                        r--;
                    }
                }

                PrintLn($"   Custom cleaned: {next}");
                var finalCleaned = CleanUp(next);

                PrintLn($"   Final cleaned: {finalCleaned}");
                var totalRowSize = 0L;
                foreach(var range in finalCleaned.ranges) {
                    totalRowSize += range.Length;
                };


            }

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

            public long Length {get {
                if(end.HasValue){ return (end.Value - start) + 1;}
                throw new Exception("Partial range cant have length");
                }
            }

            public bool IsPartial { get {return !end.HasValue;}}
            public bool IsWhole {get {return !IsPartial;}}

            public Range Copy() {
                return new Range(start, end);
            }
            public bool CompletelyContains(Range other) {
                if(IsPartial && other.IsPartial) {
                    return this.start == other.start;
                } else if (IsWhole && other.IsPartial) {
                    return other.start <= this.end && other.start >= this.start;
                } else if (IsPartial && other.IsWhole) {
                    return this.start >= other.start && this.start <= other.Length;
                } else { //both whole
                    return this.start <= other.start && this.end >= other.end;
                }
            }

            public bool ContainsPoint(long pt) {
                if(IsPartial)
                    throw new Exception("Partial range cant contain anything");
                return start <= pt && end >= pt;
            }

            public override string ToString()
            {
                var endCh = end.HasValue? $"{end}" : "?";
                return $"({start}, {endCh})";
            }
        }

        public class RowRange {
            public long row;
            public List<Range> ranges = new List<Range>();
            public RowRange(long r) {
                this.row = r;
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

            public override string ToString()
            {   
                var pts = string.Join(',',ranges.Select(r => r.ToString()).ToList());
                return $"{row} : [{pts}]";
            }
        }
    }
}