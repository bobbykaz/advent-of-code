using System.ComponentModel;
using System.Data.Common;
using System.Drawing;
using System.Net;

namespace y23 {
    public class D22 : AoCDay
    {
        public D22(): base(23, 22) {
            _DebugPrinting = true;
        }
        public enum Axis {
            x,y,z
        }
        class Point3D {
            public int x;
            public int y;
            public int z;
            public Point3D(int xx,int yy, int zz) {
                x = xx; y = yy; z = zz;
            }

            public Point3D(string line) {
                var pts = line.Split(",");
                x = int.Parse(pts[0]);
                y = int.Parse(pts[1]);
                z = int.Parse(pts[2]);
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

        class Brick {
            public static char NextId = 'A';
            public static string GetId() {
                return Guid.NewGuid().ToString();
                var n = NextId;
                NextId = (char) (NextId + 1);
                return $"{n}";
            }
            public string id;
            public Point3D start;
            public Point3D end;
            public Brick(Point3D p1, Point3D p2) {
                id = GetId();
                var list = new List<Point3D>(){p1,p2};
                list = list.OrderBy(p => p.z).ThenBy(p => p.x).ThenBy(p=>p.y).ToList();
                start = list[0];
                end = list[1];
                //Console.WriteLine(ToString());
            }

            public Brick Copy() {
                var b = new Brick(start.Copy(), end.Copy());
                b.id = id;
                return b;
            }

            public bool OnGround { get {return start.z == 1;} }
            public int Length {get {
                // two of these will be 0;
                return (end.x - start.x) + (end.y - start.y) + (end.z - start.z) + 1;
            }}

            public int Height { get {return end.z - start.z + 1;}}
            public int LowestZ {get {return start.z;}}
            public int HighestZ {get {return end.z;}}
            public bool ContainsPoint(Point3D pt) {
                return PointsInBrick().Any(p => p.Equals(pt));
            }
            public bool ContainsPointQuickMaybeBroken(Point3D pt) {
                // 1,1,1 - 3,1,1
                if (Length == 1) {
                    return pt == start;
                }
                bool shareX = (pt.x - start.x) == 0;
                bool shareY = (pt.y - start.y) == 0;
                bool shareZ = (pt.z - start.z) == 0;
                var sharedAxes = new List<bool>(){shareX, shareY, shareZ}.Where(b => b).ToList();
                if(sharedAxes.Count < 2) {
                    return false;
                }
                bool isBetween =  start.x <= pt.x && pt.x <= end.x &&
                                    start.y <= pt.y && pt.y <= end.y &&
                                    start.z <= pt.z && pt.z <= end.z;
                return isBetween;
            }

            public List<Point3D> PointsInBrick() {
                var rslt = new List<Point3D>();
                for (var i = start.x; i <= end.x; i++) {
                    for (var j = start.y; j <= end.y; j++) {
                        for (var k = start.z; k <= end.z; k++) {
                            rslt.Add(new Point3D(i, j, k));
                        }
                    }
                }
                return rslt;
            }

            public List<Point3D> PointsBelow() {
                var rslt = new List<Point3D>();
                for (var i = start.x; i <= end.x; i++) {
                    for (var j = start.y; j <= end.y; j++) {
                        rslt.Add(new Point3D(i, j , start.z - 1));
                    }
                }
                return rslt;
            }

            public override string ToString()
            {
                return $"{start} ~ {end} -> {id}";
            }
        }

        class BrickMap {
            public int maxHeight = 1;
            public List<Brick> bricks = new List<Brick>();
            public Dictionary<int,List<Brick>> HeightMap = new Dictionary<int, List<Brick>>();

            public void AddBrick(Brick b) {
                bricks.Add(b);
                for (int z = b.start.z; z <= b.end.z; z++) {
                    if(!HeightMap.ContainsKey(z))
                        HeightMap[z] = new List<Brick>();
                    HeightMap[z].Add(b);
                }
                if(b.HighestZ > maxHeight) {maxHeight = b.HighestZ;};
            }

            public void SettleBricks() {
                int i = 1;
                while(i <= maxHeight) {
                    //Console.WriteLine($"Checking bricks at {i}");
                    bool stillFalling = true; 
                    bool anyFell = false;
                    while(stillFalling) {
                        stillFalling = SettleBricksAt(i);
                        if (stillFalling) {
                            anyFell = true;
                        }
                    }
                    if(anyFell) {
                        i--;
                    } else {
                        i++;
                    }
                }
            }

            public bool SettleBricksAt(int z) {
                bool atLeastOneFell = false;
                if (HeightMap.ContainsKey(z)) {
                    foreach(var brick in HeightMap[z].ToList()) {
                        //Console.WriteLine($"  Checking brick {brick.id} at {z}");
                        var brickFell = SettleBrick(brick);
                        if(brickFell) {
                            atLeastOneFell = true;
                        }
                    }
                }
                return atLeastOneFell;
            }

            public bool SettleBrick(Brick b) {
                if(CanFall(b)) {
                    for (int i = b.LowestZ; i <= b.HighestZ; i++) {
                        HeightMap[i].RemoveAll(br => br.id == b.id);
                    }
                    b.start.z -= 1;
                    b.end.z -= 1;
                    for (int i = b.LowestZ; i <= b.HighestZ; i++) {
                        if(!HeightMap.ContainsKey(i))
                            HeightMap[i] = new List<Brick>();

                        HeightMap[i].Add(b);
                    }
                    //Console.WriteLine($"    Brick {b.id} fell");
                    return true;
                } else { 
                    return false;
                }
            }

            public void RemoveBrick(Brick b) {
                for (int i = b.LowestZ; i <= b.HighestZ; i++) {
                    HeightMap[i].RemoveAll(br => br.id == b.id);
                }
                bricks.RemoveAll(br => br.id == b.id);
            }

            public bool CanFall(Brick b, string? brickIgnoreId = null) {
                if(b.OnGround) {
                    return false;
                }
                //Console.WriteLine($"Can {b} fall?");
                var below = b.PointsBelow();
                
                if (!HeightMap.ContainsKey(b.LowestZ - 1)) {
                    //Console.WriteLine($"   Nothing in HM at level {b.LowestZ - 1}, yes");
                    return true;
                } else {
                    var bricksOnLevelBelow = HeightMap[b.LowestZ - 1];
                    if(brickIgnoreId != null) {
                        bricksOnLevelBelow = bricksOnLevelBelow.Where(br => br.id != brickIgnoreId).ToList();
                    }
                    foreach(var p in below){
                        //Console.Write($"   Checking point {p} ");
                        foreach(var bb in bricksOnLevelBelow) {
                            var pointInBB = bb.ContainsPoint(p);
                            //Console.WriteLine($"      {bb} contains? {pointInBB}");
                            if(pointInBB)
                                return false;
                        }
                    }
                    //Console.WriteLine(" no points collide; can fall");
                    return true;
                }
            }
        }

        BrickMap parseBricks() {
            var lines = InputAsLines();
            var BM = new BrickMap();
            foreach(var line in lines) {
                var bPts = line.Split("~").Select(l =>new Point3D(l)).ToList();
                var b = new Brick(bPts[0], bPts[1]);

                BM.AddBrick(b);
            }
            return BM;
        }



        public override string P1()
        {
            var bm = parseBricks();
            bm.SettleBricks();
            PrintLn("Settled\n");
            //bm.bricks.ForEach(b => PrintLn(b.ToString()));

            var nonLoadBearingBricks = new List<Brick>();
            foreach(var b in bm.bricks) {
                int levelToCheck = b.HighestZ + 1;
                bool anyCouldFall = false;
                if(bm.HeightMap.ContainsKey(levelToCheck)) {
                    foreach(var other in bm.HeightMap[levelToCheck]) {
                        if(bm.CanFall(other, b.id)) {
                            anyCouldFall = true;
                            break;
                        }
                    }
                }
                if (!anyCouldFall) {
                    nonLoadBearingBricks.Add(b);
                    //PrintLn($"Brick {b.id} can disintegrate");
                }
            }
            
            return $"{nonLoadBearingBricks.Count}";
        }
        public override string P2()
        {
            var bm = parseBricks();
            bm.SettleBricks();
            PrintLn("Settled\n");
            long total = 0;
            foreach(var brick in bm.bricks){
                var cm = new BrickMap();
                foreach(var bb in bm.bricks){
                    cm.AddBrick(bb.Copy());
                }
                cm.RemoveBrick(brick);
                cm.SettleBricks();
                long diff = 0;
                var ogStarts = bm.bricks.Where(b => b.id != brick.id).Select(b => b.start).ToList();
                var newStarts = cm.bricks.Select(b => b.start).ToList();
                for(int i = 0; i < ogStarts.Count; i++) {
                    if (!ogStarts[i].Equals(newStarts[i])){
                        diff++;
                    }
                }
                total += diff;
            }
            
            return $"{total}";
        }
    }
}
