namespace y24 {
    public class D2: AoCDay {

        public D2(): base(24, 2) {
            _DebugPrinting = false;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var safe = 0;
            var safeL = new List<int>();
            foreach (var (line,l) in lines.ForEachIndex()) {
                var nums = Utilties.StringToNums<int>(line, " ");
                var inc = 0;
                var dec = 0;
                for (int i = 1; i < nums.Count; i++) {
                    var d = nums[i] - nums[i-1];
                    if (d > 0) inc ++;
                    if (d < 0) dec ++;
                    if (Math.Abs(d) >= 1 && Math.Abs(d) <= 3) {
                        if (i == nums.Count - 1) {
                            if(inc == 0 || dec == 0) {
                                safe++;
                                safeL.Add(l);
                            }
                        }
                    } else {
                        break;
                    }
                }
                
            }

            return $"{safe}";
        }

        private LevelCheck checkLevel(int a, int b) {
            var  d = b-a;
            var abs = Math.Abs(d);
            var l = new LevelCheck();
            l.inc = d > 0;
            l.dec = d < 0;
            l.diff = abs >=1 && abs <=3;

            return l;
        }

        private bool checkSafety(List<int> nums) {
            var checks = new List<LevelCheck>();
            for (int i = 1; i < nums.Count; i++) {
                checks.Add(checkLevel(nums[i-1], nums[i]));
            }

            if(checks.Any(c => !c.diff)) {
                return false;
            }

            var anyInc = checks.Any(c => c.inc);
            var anyDec = checks.Any(c => c.dec);

            if (anyInc && anyDec) {
                return false;
            }

            if (!anyInc && !anyDec) {
                return false;
            }

            return true;
        }

        struct LevelCheck {
            public bool inc;
            public bool dec;
            public bool diff;
        }

        public override string P2()
        {
            var lines = InputAsLines();
            var safe = 0;
            foreach (var (line,l) in lines.ForEachIndex()) {
                var nums = Utilties.StringToNums<int>(line, " ");
                if (checkSafety(nums)) {
                    safe++;
                } else {
                    for(int i = 0; i < nums.Count; i++) {
                        var fn = Utilties.StringToNums<int>(line, " ");
                        fn.RemoveAt(i);
                        if(checkSafety(fn)) {
                            safe++;
                            break;
                        }
                    }
                }
            }

            return $"{safe}";
        }

    }
}