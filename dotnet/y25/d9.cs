using System.Numerics;
using Grids;
using Vec;

namespace y25 {
    public class D9: AoCDay {
        
        public D9(): base(25, 9) {
            _DebugPrinting = false;
            _UseSample = false;
        }
        
        public override string P1()
        {
            var lines = InputAsLines();
            var vecs = lines.Select(l => Vec2.Parse(l)).ToList();
            var max = 0L;
            for (int i = 0; i < vecs.Count; i++)
            {
                for (int j = i; j < vecs.Count; j++)
                {
                    if (i != j)
                    {
                        var a = vecs[i];
                        var b = vecs[j];
                        var width = long.Abs(a.X - b.X) + 1;
                        var height = long.Abs(a.Y - b.Y) + 1;
                        var area = width * height;
                        if (area > max)
                        {
                            PrintLn($"new max {area} from {a}, {b}");
                            max = area;
                        }
                    }
                }
            }

            

            var total = max;
            return $"total {total}";
        }


        // https://www.xjavascript.com/blog/check-if-polygon-is-inside-a-polygon/
        public record Edge(Vec2 A, Vec2 B)
        {
            public bool IsVertical()
            {
                return A.X == B.X && A.Y != B.Y;
            }

            public bool IsHorizontal()
            {
                return A.X != B.X && A.Y == B.Y;
            }
            public bool Intersects(Edge other)
            {
                var a = this.A;
                var b = this.B;
                var c = other.A;
                var d = other.B;

                var o1 = Orientation(a, b, c);
                var o2 = Orientation(a, b, d);
                var o3 = Orientation(c, d, a);
                var o4 = Orientation(c, d, b);
                
                // General case: segments intersect in their interiors
                if (o1 != o2 && o3 != o4)
                    return true;
                
                // Special cases: collinear and overlapping segments
                if (o1 == Collinear && onSegment(a, new Edge(c, b)))
                    return true;
                if (o2 == Collinear && onSegment(a, new Edge(d, b)))
                    return true;
                if (o3 == Collinear && onSegment(c, new Edge(a, d)))
                    return true;
                if (o4 == Collinear && onSegment(c, new Edge(b, b)))
                    return true;

                return false;
            }
            
            private const int Collinear = 0;
            private const int Clockwise = 1;
            private const int Counterclockwise = 2;

            private int Orientation(Vec2 a, Vec2 b, Vec2 c)
            {
                var rslt = (b.Y - a.Y) * (c.X - b.X) - (b.X - a.X) * (c.Y - b.Y);
                if (rslt == 0) return Collinear;
                if (rslt > 0) return Counterclockwise;
                return Clockwise;
            }

            private bool onSegment(Vec2 a, Edge seg)
            {
                var b = seg.A;
                var c = seg.B;
                return b.X <= long.Max(a.X, c.X) &&
                       b.X >= long.Min(a.X, c.X) 
                       && b.Y <= long.Max(a.Y, c.Y) 
                       && b.Y >= long.Min(a.Y, c.Y);
            }

            public override string ToString()
            {
                return $"{A} | {B}";
            }
        }


        private Dictionary<string, bool> SeenRectIntersects = [];
        
        private bool CheckRectangle(Vec2 a, Vec2 b)
        {
            var xMin = long.Min(a.X, b.X);
            var xMax = long.Max(a.X, b.X);
            var yMin = long.Min(a.Y, b.Y);
            var yMax = long.Max(a.Y, b.Y);

            //"Shrink doubled rectangle inward by 1 to eliminate rectangle edges overlapping with polygon edges"
            var r1 = new Vec2(xMin + 1, yMin + 1); //DL
            var r2 = new Vec2(xMax - 1, yMin + 1); //DR
            var r3 = new Vec2(xMax - 1, yMax - 1); //UR
            var r4 = new Vec2(xMin + 1, yMax - 1); //UL
            
            var key = $"{r1}-{r2}-{r3}-{r4}";
            if (SeenRectIntersects.ContainsKey(key))
            {
                PrintLn($"Checking rectangle {a}, {b}...already seen!");
                return SeenRectIntersects[key];
            }
            
            var rectEdges = new List<Edge>();
            rectEdges.Add(new Edge(r1, r2));
            rectEdges.Add(new Edge(r2, r3));
            rectEdges.Add(new Edge(r3, r4));
            rectEdges.Add(new Edge(r4, r1));
            bool anyEdgesIntersectShape = false;
            foreach (var rEdge in rectEdges)
            {
                foreach (var pEdge in _polygonEdges)
                {
                    if (rEdge.Intersects(pEdge))
                    {
                        //PrintLn($"Checking rectangle {a}, {b}...Edge {rEdge} intersects {pEdge}");
                        anyEdgesIntersectShape = true;
                        break;
                    }
                }
                if(anyEdgesIntersectShape)
                    break;
            }
            
            SeenRectIntersects[key] = anyEdgesIntersectShape;

            if (!anyEdgesIntersectShape)
            {
                PrintLn($"Checking rectangle {a}, {b}......clear!");
            }
            
            return anyEdgesIntersectShape;
        }



        private List<Edge> _polygonEdges = [];
        private List<Edge> _verticalEdges = [];
        private List<Edge> _horizontalEdges = [];
        public override string P2()
        {
            _DebugPrinting = true;
            var lines = InputAsLines();
            var vecs = lines.Select(l => Vec2.Parse(l)).ToList();
            //PrintSvg(vecs);
            
            var doubledVecs = vecs.Select(v => new Vec2(v.X * 2, v.Y * 2)).ToList();
            
            var max = 0L;
            for (int i = 0; i < vecs.Count - 1; i++)
            {
                var a = doubledVecs[i];
                var b = doubledVecs[i + 1];
                _polygonEdges.Add(new Edge(a,b));
            }
            _polygonEdges.Add(new Edge(vecs.Last(), vecs[0]));
            
            _horizontalEdges = _polygonEdges.Where(e => e.IsHorizontal()).ToList();
            _verticalEdges = _polygonEdges.Where(e => e.IsVertical()).ToList();
            
            //is given rectangle of red tiles (points) wholely included in the polygon of all points?

            for (int i = 0; i < vecs.Count; i++)
            {
                for (int j = i; j < vecs.Count; j++)
                {
                    if (i != j)
                    {
                        var a = vecs[i];
                        var b = vecs[j];
                        if (!CheckRectangle(doubledVecs[i],doubledVecs[j]))
                        {
                            var width = long.Abs(a.X - b.X) + 1;
                            var height = long.Abs(a.Y - b.Y) + 1;
                            var area = width * height;
                            if (area > max)
                            {
                                PrintLn($"new max {area} from {a}, {b}");
                                max = area;
                            }
                        }
                    }
                }
            }
            
            return $"max {max}";
        }
        
        public void PrintSvg(List<Vec2> points)
        {
            var pts = string.Join(" ", points.Select(p => $"{p.X },{p.Y}"));
            var template = $"""
                            <svg height="100" width="100" xmlns="http://www.w3.org/2000/svg">
                              <polygon points="{pts}" style="fill:lime;stroke:purple;stroke-width:3" />
                            </svg>
                            """;
            PrintLn(template);
        }
    }
}