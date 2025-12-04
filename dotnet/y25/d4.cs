using System.Globalization;
using Grids;

namespace y25 {
    public class D4: AoCDay {

        public D4(): base(25, 4) {
            _DebugPrinting = false;
            _UseSample = false;
        }
        public override string P1()
        {
            _DebugPrinting = false;
            var lines = InputAsLines();

            var total = 0L;
            var grid = Utilties.RectangularCharGridFromLines(lines);
            grid.ForEachRowCol((r, c, v) =>
            {
                var neighbors = grid.AllNeighbors(r, c);
                int rolls = neighbors.Count(cell => cell.V == '@');
                if (rolls < 4 && v == '@')
                    total++;
            });


            return $"total {total}";
        }

        public override string P2()
        {
            _DebugPrinting = false;
            var lines = InputAsLines();

            var total = 0L;
            var grid = Utilties.RectangularCharGridFromLines(lines);
            bool anyRemoved = true;
            while (anyRemoved)
            {
                anyRemoved = false;
                grid.ForEachRowCol((r, c, v) =>
                {
                    var neighbors = grid.AllNeighbors(r, c);
                    int rolls = neighbors.Count(cell => cell.V == '@');
                    if (rolls < 4 && v == '@')
                    {
                        total++;
                        grid.G[r][c] = '.';
                        anyRemoved = true;
                    }
                });
            }
            
            return $"total {total}";
        }
    }
}