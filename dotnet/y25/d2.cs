namespace y25 {
    public class D2: AoCDay {

        public D2(): base(25, 2) {
            _DebugPrinting = false;
            _UseSample = true;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var ranges = lines[0].Split(",").Select(s => parseRange(s));
            
            var total = 0L;

            foreach (var range in ranges)
            {
                var ld = Utilties.CountDigits(range.Item1);
                var ud = Utilties.CountDigits(range.Item2);
                PrintLn($"Starting range {range} ( digits: {ld}, {ud} )");

                if (ld % 2 != 0 && ud % 2 != 0)
                {
                    PrintLn("... skipping since both odd");
                }
                else
                {
                    total += allBadIdsInRange(range.Item1, range.Item2);
                }
            }
            
            return $"total= {total}";
        }

        private long allBadIdsInRange(long lower, long upper)
        {
            var total = 0L;
            var ld = Utilties.CountDigits(lower);
            var ud = Utilties.CountDigits(upper);
            {
                var start = findStart(lower, 2);
                var next = duplicate(start, 2);
                PrintLn($"..Starting at {start}: ID {next}");
                while (next <= upper)
                {
                    if (next >= lower)
                    {
                        PrintLn($"....ID {next} is in range");
                        total += next;
                    }

                    start++;
                    next = duplicate(start, 2);
                }
            }

            if (ud != ld)
            {
                var start = findStart(baseOfDigits(ud));
                var next = duplicate(start, 2);
                PrintLn($"..Re-Starting at {start}: ID {next}");
                while (next <= upper)
                {
                    if (next >= lower)
                    {
                        PrintLn($"....ID {next} is in range");
                        total += next;
                    }

                    start++;
                    next = duplicate(start, 2);
                }
            }

            return total;
        }
        
        private (long, long) parseRange(string s)
        {
            var pts = s.Split("-");
            var lower = long.Parse(pts[0]);
            var upper = long.Parse(pts[1]);
            return (lower, upper);
        }

        private long baseOfDigits(long d)
        {
            return (long) Math.Pow(10, (int)d-1);
        }

        private long findStart(long n, long digitMod = 2)
        {
            var d = Utilties.CountDigits(n);
            var toReduce = 1;
            for (int i = 0; i < d/digitMod; i++)
            {
                toReduce *= 10;
            }
            return n / toReduce;
        }

        private long duplicate(long n, long numTimes)
        {
            var digits = Utilties.CountDigits(n);
            var mod = 1;
            for (int i = 0; i < digits; i++)
            {
                mod *= 10;
            }

            var total = n;
            for (int i = 1; i < numTimes; i++)
            {
                total *= mod;
                total += n;
            }
            
            return total;
        }

        private long findAllBadIdsOfDigitLength(long lower, long upper, long maxDigits, long digitsToRepeat)
        {
            if (maxDigits % digitsToRepeat != 0 && maxDigits != digitsToRepeat)
                return 0L;

            PrintLn($"..Checking all numbers with {digitsToRepeat} repeating up to {maxDigits} digits ({lower} - {upper})");
            var repeatTimes = maxDigits / digitsToRepeat;
            var start = baseOfDigits(digitsToRepeat);
            var max = baseOfDigits(digitsToRepeat + 1);
            var next = duplicate(start, repeatTimes);
            var total = 0L;
            PrintLn($"....start seq = {start}; max seq = {max}; repeat times = {repeatTimes}; init next = {next} ");
            while (next <= upper && start < max)
            {
                if (next >= lower)
                {
                    PrintLn($"......ID {next} is in range");
                    total += next;
                }
                start++;
                next = duplicate(start, 2);
            }

            return total;
        }
        
        public override string P2()
        {
            _DebugPrinting = true;
            var lines = InputAsLines();
            var ranges = lines[0].Split(",").Select(s => parseRange(s));
            
            var total = 0L;

            foreach (var range in ranges)
            {
                var ld = Utilties.CountDigits(range.Item1);
                var ud = Utilties.CountDigits(range.Item2);
                PrintLn($"Starting range {range} ( digits: {ld}, {ud} )");

                if (ud != ld)
                {
                    var middle = baseOfDigits(ud);
                    var middleMinusOne = middle - 1;
                    
                    for (var d = 1; d <= ld; d++)
                    {
                        total += findAllBadIdsOfDigitLength(range.Item1, middleMinusOne, ld, d);
                    }  
                    
                    for (var d = 1; d <= ud; d++)
                    {
                        total += findAllBadIdsOfDigitLength(middle, range.Item2, ud, d);
                    }   
                }
                else
                {
                    for (var d = 1; d <= ud; d++)
                    {
                        total += findAllBadIdsOfDigitLength(range.Item1, range.Item2, ud, d);
                    }   
                }
            }
            
            return $"p2= {total}";
        }

    }
}