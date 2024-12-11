using System.ComponentModel;
using Grids;

namespace y24 {
    public class D11: AoCDay {

        public D11(): base(24, 11) {
            _DebugPrinting = true;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var nums = Utilties.StringToNums<long>(lines[0], " ");
            for(int i = 0; i < 25; i++) {
                var newNums = new List<long>();
                for(int n = 0; n < nums.Count; n++) {
                    var pts = eval(nums[n]);
                    newNums.AddRange(pts);
                }
                nums = newNums;
            }
            var total = nums.Count;
           
            return $"{total}";
        }

        private List<long> eval(long num) {
            var result = new List<long>();
            var nStr = $"{num}";
            if (num == 0) {
                result.Add(1);
            } else if( nStr.Length % 2 == 0) {
                var str = $"{num}";
                var len = str.Length;
                var p1 = int.Parse(str.Substring(0,len/2));
                var p2 = int.Parse(str.Substring(len/2));
                result.Add(p1);
                result.Add(p2);
            } else {
                result.Add(num*2024);
            }

            return result;
        }

        public override string P2()
        {
            var lines = InputAsLines();
            var nums = Utilties.StringToNums<long>(lines[0], " ");
            for(int i = 0; i < 75; i++) {
                PrintLn($"Round {i}");
                var newNums = new List<long>();
                for(int n = 0; n < nums.Count; n++) {
                    var pts = eval(nums[n]);
                    newNums.AddRange(pts);
                }
                nums = newNums;
            }
            var total = nums.Count;
           
            return $"{total}";
        }

    }
}