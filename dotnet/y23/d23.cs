using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks.Dataflow;
using Grids;

namespace y23 {
    public class D23 : AoCDay
    {
        public D23(): base(23, 23) {
            _DebugPrinting = true;
        }

        public override string P1()
        {
            var lines = InputAsLines();
            var grid = Utilties.RectangularCharGridFromLines(lines);
            var (sr, sc) = (0,0);
            var (fr,fc) = (grid.LastRowIndex, grid.LastColIndex - 1);
            var vm = new VisitedMap();
            vm.Visit(sr,sc);
            var q = new Queue<(long, VisitedMap, Cell<char>)>();
            foreach(var neigh in NextSpots(grid, vm, sr, sc)) {
                q.Enqueue((0, vm, neigh));
            }
            var maxD = 0L;
            while(q.Any()) {
                var (dist, cvm, next) = q.Dequeue();
                //PrintLn($"starting branch {dist}: {next}");
                var (newDist, poss, done,_) = WalkUntilBranch(grid, cvm, dist, next.R, next.C);
                if(done) {
                    PrintLn($"Branch complete at Dist {newDist}");
                    if(newDist > maxD) maxD = newDist;
                } else {
                    foreach( var np in poss) {
                        var nvm = cvm.Copy();
                        nvm.Visit(np.R, np.C);
                        q.Enqueue((newDist + 1, nvm,np));
                    }
                }
            }
            return $"{maxD}";
        }

        public (long, List<Cell<char>>, bool,(int,int)) WalkUntilBranch(Grid<char> g, VisitedMap vm, long distToNow, int sr, int sc, bool p2 = false) {
            var (cr,cc) = (sr, sc);
            long curDist = distToNow;
            var nextList = NextSpots(g,vm,sr,sc, p2);
            while (nextList.Count == 1) {
                curDist++;
                var n = nextList[0];
                vm.Visit(n.R, n.C);
                (cr,cc) = (n.R, n.C);
                nextList = NextSpots(g,vm,cr,cc, p2);
            }
            if (cr == g.LastRowIndex && cc == g.LastColIndex-1) {
                return (curDist, nextList, true, (cr, cc));
            } 
            return (curDist, nextList, false, (cr,cc));
        }
        public List<Cell<char>> NextSpots(Grid<char> g, VisitedMap vm, int sr, int sc, bool p2 = false) {
            return g.CardinalNeighborsWithDir(sr,sc)
                            .Where(p => p.Item1.V != '#' 
                                    && !vm.WasVisited(p.Item1.R,  p.Item1.C)
                                    && !CellIsUphill(p.Item1, p.Item2, p2))
                                    .Select(p => p.Item1).ToList();

        }

        public bool CellIsUphill(Cell<char> c, Dir d, bool p2= false) {
            if (p2) return false;

            if(c.V == '.') return false;
            switch(c.V){
                case '>':
                    return d != Dir.E;
                case '<':
                    return d != Dir.W;
                case '^':
                    return d != Dir.N;
                case 'v':
                    return d != Dir.S;
                default:
                    throw new Exception("unexpected");
            }
        }

        public class Node {
            public Cell<char> point;
            public List<(long, Node)> Edges = new List<(long, Node)>();
            public Node(Cell<char> c) {
                point = c;
            }
        }

        public string BuildGraphs() {
            var lines = InputAsLines();
            var grid = Utilties.RectangularCharGridFromLines(lines);
            var nodes = new List<Node>();
            var nodeLookup = new Dictionary<string, Node>();
            var startNode = new Node(new Cell<char>(0,1,'.'));
            nodeLookup[startNode.point.Key] = startNode;
            nodes.Add(startNode);
            PrintLn("finding nodes");
            grid.ForEachColRow((r,c,v) =>{
                if(v != '#' 
                    && grid.CardinalNeighbors(r,c)
                        .Where(cell => cell.V != '#')
                        .ToList()
                        .Count > 2) {
                    var node = new Node(new Cell<char>(r,c,v));
                    nodes.Add(node);
                    nodeLookup[node.point.Key] = node;
                 }
            });
            var endNode = new Node(new Cell<char>(grid.LastRowIndex,grid.LastColIndex-1,'.'));
            nodeLookup[endNode.point.Key] = endNode;
            nodes.Add(endNode);
            PrintLn("Nodes: ");
            nodes.ForEach(n => Print($"{n.point.Key},"));
            PrintLn("");
            
            PrintLn("Building Node map");
            foreach(var n in nodes) {
                var vm = new VisitedMap();
                vm.Visit(n.point.R, n.point.C);
                PrintLn($"finding edges for node {n.point.Key}");
                foreach(var pathStart in NextSpots(grid,vm, n.point.R, n.point.C, true)) {
                    vm.Visit(pathStart.R, pathStart.C);
                    var (newDist, _, _, end) = WalkUntilBranch(grid, vm, 0, pathStart.R, pathStart.C, true);
                    PrintLn($"  found {end} at dist {newDist}");
                    var endCell = new Cell<char>(end.Item1,end.Item2, '.');
                    if (!nodeLookup.ContainsKey(endCell.Key)){
                        throw new Exception("uh oh");
                    }
                    n.Edges.Add((newDist+1, nodeLookup[endCell.Key]));
                }
            }
            var q = new Stack<(long,Node,List<string>)>();
            q.Push((0,startNode,new List<string>(){startNode.point.Key}));
            var maxD = 0L;
            while(q.Any()) {
                var (dist, node, seen) = q.Pop();
                if(node.point.Key == endNode.point.Key){
                    //PrintLn($"Branch complete at Dist {dist}");
                    if(dist > maxD) maxD = dist;
                } else {
                    var next = node.Edges.Where(nn => !seen.Contains(nn.Item2.point.Key));
                    foreach(var (segLength,newNode) in next) {
                        var newSeen = new List<string>(seen);
                        newSeen.Add(newNode.point.Key);
                        q.Push((dist+segLength,newNode,newSeen));
                    }       
                }
            }

            return $"{maxD}";
        }

        public override string P2()
        {
            return BuildGraphs();
        }
    }
}
