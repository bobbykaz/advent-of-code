using Grids;

namespace y24 {
    public class D10: AoCDay {

        public D10(): base(24, 10) {
            _DebugPrinting = false;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var chargrid = Utilties.RectangularCharGridFromLines(lines);
            var seenmap = new VisitedMap();
            var grid = new Grid<int>(chargrid.Width, chargrid.Height, -1);
            var total = 0;
            chargrid.ForEachRowCol((r,c,v) => {
                if(v != '.')
                    grid.G[r][c] = int.Parse($"{v}");
            });


            grid.ForEachRowCol((r,c,v) => {
                if(v == 0){
                    var svm = new VisitedMap();
                    var score = visitTrailheads(grid, svm, r,c);
                    PrintLn($"{r},{c} = {score}");
                    total += svm.VisitedCount();
                }
            });
        

           
            return $"{total}";
        }

        private int visitTrailheads(Grid<int> g, VisitedMap vm, int r, int c) {
            return vtr(g, vm, r, c, 0);
        }

        private int vtr(Grid<int> g, VisitedMap vm, int r, int c, int val) {
            if(val == 9) {
                vm.Visit(r,c);
                return 1;
            } else {
                var subtotal = 0;
                foreach(var cell in g.CardinalNeighbors(r,c)) {
                    if (cell.V == (val + 1)) {
                        subtotal += vtr(g,vm,cell.R, cell.C, cell.V);
                    }
                }
                return subtotal;
            }
        }

       

        public override string P2()
        {
            var lines = InputAsLines();
            var chargrid = Utilties.RectangularCharGridFromLines(lines);
            var seenmap = new VisitedMap();
            var grid = new Grid<int>(chargrid.Width, chargrid.Height, -1);
            var total = 0;
            chargrid.ForEachRowCol((r,c,v) => {
                if(v != '.')
                    grid.G[r][c] = int.Parse($"{v}");
            });


            grid.ForEachRowCol((r,c,v) => {
                if(v == 0){
                    var score = visitTrailheads(grid, seenmap, r,c);
                    PrintLn($"{r},{c} = {score}");
                    total += score;
                }
            });
        

           
            return $"{total}";
        }

    }
}