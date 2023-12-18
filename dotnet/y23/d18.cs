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
            foreach(var line in lines) {
                var ins = new Inst2(line);
                PrintLn($"{ins.dir} - {ins.num}");
            }
            
            return $"{0}";
        }
    }
}