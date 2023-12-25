namespace y23 {
    public class D24 : AoCDay
    {
        public D24(): base(23, 24) {
            _DebugPrinting = false;
        }

        public static int NextID = 0;
        public static int GenID() {NextID++; return NextID;}

        public class Point3D {
            public double x;
            public double y;
            public double z;
            public Point3D(double xx,double yy, double zz) {
                x = xx; y = yy; z = zz;
            }

            public Point3D(string line) {
                var pts = line.Split(", ");
                x = double.Parse(pts[0]);
                y = double.Parse(pts[1]);
                z = double.Parse(pts[2]);
            }

            public Point3D Copy() {
                return new Point3D(x,y,z);
            }

            public override string ToString()
            {
                return $"({x},{y},{z})";
            }

            public override bool Equals(object? obj)
            {
                //
                // See the full list of guidelines at
                //   http://go.microsoft.com/fwlink/?LinkID=85237
                // and also the guidance for operator== at
                //   http://go.microsoft.com/fwlink/?LinkId=85238
                //
                
                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }
                
                var other = obj as Point3D;
                if (other == null) return false;

                return this.x == other.x && this.y == other.y && this.z == other.z;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(x, y, z);
            }
        }

        public override string P1()
        {
            var lines = InputAsLines();
            var hails = new List<Hail>();
            foreach(var line in lines) {
                var h = new Hail(line);
                PrintLn($"{h.ID}: {line}");
                hails.Add(h);
            }
            int collisions = 0;
            var seen = new Dictionary<string, bool>();
            foreach(var h in hails) {
                foreach(var other in hails) {
                    if (h.ID != other.ID && !seen.ContainsKey($"{other.ID}-{h.ID}")) {
                        if (P1TestArea(h, other)) {
                            collisions++;
                        }
                        seen[$"{h.ID}-{other.ID}"] = true;
                    }
                }
            }
            return $"{collisions}";
        }

        public bool P1TestArea(Hail a, Hail b) {
            var testMin = 200000000000000.0;
            var testMax = 400000000000000.0;
            var (x,y,timea,timeb) = HailsCrossXY(a,b);
            PrintLn("");
                        PrintLn(a.ToString());
                        PrintLn(b.ToString());
                        PrintLn($"  {x},{y}, {timea}, {timeb}");
            if(a.M_P1 == b.M_P1) {
                PrintLn($"{a.ID}, {b.ID} are parallel");
                return false;
            } else if (timea >= 0 && timeb >=0) {
                if (x <= testMax && x >= testMin) {
                    if (y <= testMax && y >= testMin) {
                        
                        PrintLn($"  Hails {a.ID}, {b.ID} intersect in the test area!");
                        return true;
                    }
                }
                PrintLn($"  Hails {a.ID}, {b.ID} cross outside the test area ({x},{y})");
            } else {
                PrintLn($"  Hails {a.ID}, {b.ID} intersect in past");
                return false;
            }
            return false;
        }
        // time, x, y
        public (double, double, double, double) HailsCrossXY(Hail a, Hail b) {
            //Max + ba = Mbx + bb
            //x (Ma - Mb) = bb - ba
            var xPt = (b.B_P1 - a.B_P1) / (a.M_P1 - b.M_P1);
            var yPt = a.M_P1 * xPt + a.B_P1;
            var timeA = (xPt - a.pos.x) / a.vel.x;
            var timeB = (xPt - b.pos.x) / b.vel.x;
            return (xPt, yPt, timeA, timeB);
        }

        public class Hail {
            public string ID = $"{GenID()}";
            public Point3D pos;
            public Point3D vel;

            public double M_P1;
            public double B_P1;

            public string line;
            public Hail(string line) {
                var pts = line.Split(" @ ");
                pos = new Point3D(pts[0]);
                vel = new Point3D(pts[1]);
                this.line = line;
                MxB();
            }

            public Point3D PosAt(double time) {
                var r = pos.Copy();
                r.x += vel.x * time;
                r.y += vel.y * time;
                r.z += vel.z * time;

                return r;
            }

            public void MxB() {
                var (x1,y1) = (pos.x,pos.y);
                var (x2,y2) = (x1 + vel.x, y1 + vel.y);
                M_P1 = (y2-y1) / (x2-x1);
                B_P1 = y1 - x1*M_P1;
            }

            public override string ToString()
            {
                return $"{ID}: {line}";
            }
        }

        public override string P2()
        {
            var lines = InputAsLines();
            return $"{0}";
        }
    }
}
