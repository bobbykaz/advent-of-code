using System.Security.Cryptography;
using Grids;

namespace y25 {
    public class D12: AoCDay {

        public D12(): base(25, 12) {
            _DebugPrinting = true;
            _UseSample = false;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var blocks = Utilties.GroupInputAsBlocks(lines);

            _Presents = blocks.Take(6).Select(block => new Present(block)).ToList();

            foreach (var p in _Presents)
            {
                PrintLn(p.ToString());
            }
            
            var regions = blocks.Skip(6).First();
            
            var total = 0L;
            foreach (var line in regions)
            {
                if (CheckRegion(line))
                {
                    total++;
                }
            }
            
            return $"total {total}";
        }

        private List<Present> _Presents = [];

        public bool CheckRegion(string region)
        {
            PrintLn($"{region}");
            var pts = region.Split(": ");
            var wh = pts[0].Split("x");
            var width = int.Parse(wh[0]);
            var height = int.Parse(wh[1]);
            var area = width * height;
            PrintLn($"  {width} x {height} = {area}");

            var reqs = pts[1].Split(" ").Select(s => int.Parse(s)).ToList();
            PrintLn($"  {string.Join(",", reqs)}");
            var rawSpace = 0;
            for (int i = 0; i < reqs.Count; i++)
            {
                rawSpace += reqs[i] * _Presents[i].RawSpace;
            }
            PrintLn($"  min space needed: {rawSpace}");
            if (rawSpace > area)
            {
                return false;
            }
            return true;
        }
        private class Present
        {
            public long Id { get; set; }
            public Grid<char> G { get; set; }

            public int RawSpace { get; set; }

            public Present(List<string> lines)
            {
                var idLine = lines[0];
                Id = long.Parse(idLine.Trim(':'));
                G = Utilties.RectangularCharGridFromLines(lines.Skip(1).ToList());
                var count = 0;
                G.ForEachColRow((r, c, v) =>
                {
                    if (v == '#')
                        count++;
                });
                RawSpace = count;
            }

            public override string ToString()
            {
                var b = $"{Id}: {RawSpace}";
                b += G.ToStringLines();
                b += "\n";
                return b;
            }
        }

        
        public override string P2()
        {
            _DebugPrinting = false;
            var lines = InputAsLines();
            var total = 0L;
            foreach (var line in lines)
            {
                
            }
            
            return $"total {total}";
        }

    }
}