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
}