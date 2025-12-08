namespace Vec {
    public record Vec2(long X, long Y) {
        public static Vec2 operator +(Vec2 a, Vec2 b) {
            return new Vec2(a.X + b.X, a.Y + b.Y);
        }

        public static Vec2 operator -(Vec2 a, Vec2 b) {
            return new Vec2(a.X - b.X, a.Y - b.Y);
        }

        public static Vec2 operator *(Vec2 a, long b) {
            return new Vec2(a.X * b, a.Y * b);
        }

        public static Vec2 operator *(Vec2 a, Vec2 b) {
            return new Vec2(a.X * b.X, a.Y * b.Y);
        }

        public static Vec2 operator /(Vec2 a, long b) {
            return new Vec2(a.X / b, a.Y / b);
        }

        public static Vec2 operator /(Vec2 a, Vec2 b) {
            return new Vec2(a.X / b.X, a.Y / b.Y);
        }

        public static Vec2 operator %(Vec2 a, Vec2 b) {
            return new Vec2(a.X % b.X, a.Y % b.Y);
        }
    }
    
    public record Vec3(long X, long Y, long Z) {
        public static Vec3 Parse(string v, string separator = ",")
        {
            var pts = v.Split(separator);
            if(pts.Length != 3) 
                throw new ArgumentException($"{v} when split by {separator} is not 3 parts");
            return new Vec3(long.Parse(pts[0]), long.Parse(pts[1]), long.Parse(pts[2]));
        }
        public long ManhattanDistance(Vec3 other)
        {
            return Math.Abs(other.X - X) + Math.Abs(other.Y - Y) + Math.Abs(other.Z - Z);
        }

        public double EuclideanDistance(Vec3 other)
        {
            var xdiff = other.X - X;
            var ydiff = other.Y - Y;
            var zdiff = other.Z - Z;
            return Math.Sqrt(Math.Pow(xdiff,2) + Math.Pow(ydiff,2) + Math.Pow(zdiff,2));
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }
    }
}