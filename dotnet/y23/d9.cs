using System.Data;
using System.Xml.XPath;

namespace y23 {
    public class D9 : AoCDay
    {
        public D9(): base(23, 9) {
            _DebugPrinting = true;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var total = 0L;
            foreach(var line in lines) {
                var nums = Utilties.StringToNums<long>(line, " ");
                var exn = extrapolate(nums);
                var ex = nums.Last() + exn;
                PrintLn($"{ex}");
                total += ex;
            }


            return $"{total}";
        }

        public long extrapolate(List<long> nums) {
            var diffs = new List<long>();
            for(int i = 0; i < nums.Count - 1; i++) {
                diffs.Add(nums[i+1] - nums[i]);
            }
            if (diffs.All(l => l == diffs[0])){
                return diffs[0];
            } else {
                var subEx = extrapolate(diffs);
                return diffs.Last() + subEx;
            }
        }

        public long extrapolateR(List<long> nums) {
            var diffs = new List<long>();
            for(int i = 0; i < nums.Count - 1; i++) {
                diffs.Add(nums[i+1] - nums[i]);
            }
            if (diffs.All(l => l == diffs[0])){
                return diffs[0];
            } else {
                var subEx = extrapolateR(diffs);
                return diffs.First() - subEx;
            }
        }

        public override string P2()
        {
            var lines = InputAsLines();
            var total = 0L;
            foreach(var line in lines) {
                var nums = Utilties.StringToNums<long>(line, " ");
                var exn = extrapolateR(nums);
                var ex = nums.First() -exn;
                PrintLn($"{ex}");
                total += ex;
            }


            return $"{total}";


            return $"{total}";
            
            return $"{total}";
        }
    }
}