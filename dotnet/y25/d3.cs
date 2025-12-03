using System.Globalization;

namespace y25 {
    public class D3: AoCDay {

        public D3(): base(25, 3) {
            _DebugPrinting = false;
            _UseSample = false;
        }
        public override string P1()
        {
            _DebugPrinting = false;
            var lines = InputAsLines();
            var total = 0L;
            foreach (var line in lines)
            {
                var digs = line.ToCharArray();
                var nums = digs.Select(c => int.Parse(c.ToString())).ToList();
                var (battery, digit) = findHighest(nums);
                PrintLn($"{line} => {battery} , {digit}");
                total += battery;
            }

            return $"total {total}";
        }


        // returns highest num
        private (int, int) findHighest(List<int> nums)
        {
            if (nums.Count == 2)
            {
                var max = int.Max(nums[0], nums[1]);
                var batt = nums[0] * 10 + nums[1];
                return (batt, max);
            }
            else
            {
                var thisDig = nums.First();
                var remaining = nums.Skip(1).ToList();
                var (subBattery, subDig) = findHighest(remaining);

                var bestDig = int.Max(thisDig, subDig);
                var thisBattery = thisDig * 10 + subDig;
                var bestBattery = int.Max(subBattery, thisBattery);
                return (bestBattery, bestDig);
            }
        }
        
        public override string P2()
        {
            _DebugPrinting = false;
            var lines = InputAsLines();
            var total = 0L;
            foreach (var line in lines)
            {
                var digs = line.ToCharArray();
                var nums = digs.Select(c => long.Parse(c.ToString())).ToList();
                var battery = findHighestTwelve(nums);
                PrintLn($"{line} => {battery}");
                total += battery;
            }

            return $"total {total}";
        }
        
        private long findHighestTwelve(List<long> nums)
        {
            var memos = new List<List<long>>();
            for (var i = 0; i < 12; i++)
            {
                var row = new List<long>();
                for (var j = 0; j < nums.Count; j++)
                {
                    row.Add(0L);
                }
                memos.Add(row);
            }
            
            //populate first row: max digit on the right
            memos[0][nums.Count - 1] = nums.Last();
            for (int i = nums.Count - 2; i >= 0; i--)
            {
                memos[0][i] = long.Max(nums[i], memos[0][i + 1]);
            }

            for (int row = 1; row < 12; row++)
            {
                var mulFactor = (long)Math.Pow(10, row);
                for (int i = nums.Count - row - 1; i >= 0; i--)
                {
                    
                    var thisBattery = nums[i] * mulFactor + memos[row - 1][i+1];
                    memos[row][i] = long.Max(thisBattery, memos[row][i + 1]);
                }
            }

            return memos[11][0];
        }

    }
}