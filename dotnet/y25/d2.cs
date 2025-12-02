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
                    total += allBadIdsInRange(range.Item1, range.Item2, 2);
                }
            }
            
            return $"total= {total}";
        }

        private long allBadIdsInRange(long lower, long upper, long dupeAmount)
        {
            var total = 0L;
            var ld = Utilties.CountDigits(lower);
            var ud = Utilties.CountDigits(upper);
            {
                var start = findStart(lower, dupeAmount);
                var next = duplicate(start, dupeAmount);
                PrintLn($"..Starting at {start}: ID {next}");
                while (next <= upper)
                {
                    if (next >= lower)
                    {
                        PrintLn($"....ID {next} is in range");
                        total += next;
                    }

                    start++;
                    next = duplicate(start, dupeAmount);
                }
            }

            if (ud != ld)
            {
                var start = findStart(baseOfDigits(ud));
                var next = duplicate(start, dupeAmount);
                PrintLn($"..Re-Starting at {start}: ID {next}");
                while (next <= upper)
                {
                    if (next >= lower)
                    {
                        PrintLn($"....ID {next} is in range");
                        total += next;
                    }

                    start++;
                    next = duplicate(start, dupeAmount);
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

                for (var d = ld; d <= ud; d++)
                {
                    total += allBadIdsInRange(range.Item1, range.Item2, d);
                }
            }
            
            return $"p2= {total}";
        }

    }
}