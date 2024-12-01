using System.Numerics;

namespace y24 {
    public class D1: AoCDay {

        public D1(): base(24, 1) {
            _DebugPrinting = false;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var total = 0;
            var l1 = new List<int>();
            var l2 = new List<int>();
            foreach (var line in lines) {
                var pts = line.Split("   ").Select(int.Parse).ToList();
                l1.Add(pts[0]);
                l2.Add(pts[1]);
            }

            l1.Sort();
            l2.Sort();

            for(int i = 0; i < l1.Count; i++) {
                var d = Math.Abs(l1[i] - l2[i]);
                total += d;
            }

            return $"{total}";
        }

        public override string P2()
        {
            var lines = InputAsLines();
            long total = 0;
            var l1 = new List<int>();
            var l2 = new List<int>();
            foreach (var line in lines) {
                var pts = line.Split("   ").Select(int.Parse).ToList();
                l1.Add(pts[0]);
                l2.Add(pts[1]);
            }

            l1.Sort();
            l2.Sort();

            for(int i = 0; i < l1.Count; i++) {
                var t = l1[i];
                var oc = l2.FindAll( n => n == t ).Count;
                total += (t*oc);
            }


            return $"{total}";
        }

    }
}