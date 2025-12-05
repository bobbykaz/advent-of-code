using Range = RangeUtils.Range;

namespace y25 {
    public class D5: AoCDay {

        public D5(): base(25, 5) {
            _DebugPrinting = false;
            _UseSample = false;
        }
        public override string P1()
        {
            _DebugPrinting = false;
            var lines = InputAsLines();
            var blocks = Utilties.GroupInputAsBlocks(lines);
            var ranges = blocks[0].Select(s =>
            {
                var pts = s.Split('-');
                var low = long.Parse(pts[0]);
                var high = long.Parse(pts[1]);
                return (low, high);
            }).ToList();

            var ids = blocks[1].Select(long.Parse);

            var total = 0L;
            
            foreach (var id in ids)
            {
                foreach (var range in ranges)
                {
                    if (id >= range.low && id <= range.high)
                    {
                        total++;
                        break;
                    }
                }
            }
            

            return $"total {total}";
        }

        public override string P2()
        {
            _DebugPrinting = false;
            var lines = InputAsLines();
            var blocks = Utilties.GroupInputAsBlocks(lines);
            var ranges = blocks[0].Select(s =>
            {
                var pts = s.Split('-');
                var low = long.Parse(pts[0]);
                var high = long.Parse(pts[1]);
                return new Range(low, high);
            })
            .OrderBy(r => r.Low)
            .ThenBy(r => r.High)
            .ToList();

            var candidate = ranges[0];
            var uniqs = new List<Range>();
            foreach (var r in ranges.Skip(1))
            {
                if (candidate.Intersects(r))
                {
                    candidate = candidate.GetMergedRange(r);
                }
                else
                {
                    uniqs.Add(candidate.Copy());
                    candidate = r.Copy();
                }
            }
            uniqs.Add(candidate.Copy());
            var total = uniqs.Sum(r => r.Length);
            return $"total {total}";
        }
    }
}