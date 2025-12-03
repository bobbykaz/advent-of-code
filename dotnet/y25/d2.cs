namespace y25 {
    public class D2: AoCDay {

        public D2(): base(25, 2) {
            _DebugPrinting = false;
            _UseSample = false;
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
            var asStr = $"{n}";
            for (var i = 1; i < numTimes; i++)
            {
                asStr += $"{n}";
            }
            return long.Parse(asStr);
        }
        
        public override string P2()
        {
            _DebugPrinting = true;
            var lines = InputAsLines();
            var initranges = lines[0].Split(",")
                .Select(s => parseRange(s))
                .OrderBy(r => r.Item1)
                .ThenBy(r => r.Item2);
            
            var rangeDict = new Dictionary<long, List<(long, long)>>();
            for (var i = 1L; i <= 10L; i++)
            {
                rangeDict[i] = [];
            }

            foreach (var range in initranges)
            {
                var ld = Utilties.CountDigits(range.Item1);
                var ud = Utilties.CountDigits(range.Item2);
                if (ud == ld)
                {
                    PrintLn($"Adding range {range} to map of {ud}");
                    rangeDict[ud].Add(range);
                }
                else
                {
                    var middle = baseOfDigits(ud);
                    var middleMinusOne = middle - 1;
                    var lowerRange = (range.Item1, middleMinusOne);
                    var upperRange = (middle, range.Item2);
                    PrintLn($"Splitting range {range} to {lowerRange} | {upperRange}");
                    rangeDict[ld].Add(lowerRange);
                    rangeDict[ud].Add(upperRange);
                }
            }

            PrintLn($"...............");
            
            var total = 0L;
            // single digit ids ignored
            var seen = new HashSet<long>();
            for (var n = 2L; n <= 10L; n++)
            {
                var rangesOfnDigits = rangeDict[n];
                
                // number of digits in sequence, time to repeat sequence
                PrintLn($"Checking all ranges of digit length {n} ( {rangesOfnDigits.Count} )");
                var rstr = fmtRanges(rangesOfnDigits);
                PrintLn($"  {rstr}");
                var howManyDigitsHowManyTimes = new List<(long, long)>();
                howManyDigitsHowManyTimes.Add((1, n));
                PrintLn($"  (will use 1 digit {n} times)");
                for (var i = 1L; i <= 10L; i++)
                {
                    // longest 10 digit sequence is 2 digits, repeated 5 times
                    for (var j = 2L; j <= 5L; j++)
                    {
                        if ((i * j) == n)
                        {
                            PrintLn($"  (will use {i} digits {j} times)");
                            howManyDigitsHowManyTimes.Add((i, j));
                        }
                    }
                }
                
                
                foreach (var seqsToRepeat in howManyDigitsHowManyTimes)
                {
                    total += findAllBadIds(rangesOfnDigits, seqsToRepeat.Item1, seqsToRepeat.Item2, seen);
                }
            }
            
            return $"p2= {total}";
        }

        private long findAllBadIds(List<(long, long)> ranges, long digitsInSequence, long sequenceRepeats, HashSet<long> seen)
        {
            PrintLn($"..Checking all numbers with {digitsInSequence} in sequence repeating {sequenceRepeats} times");
            var seqStart = baseOfDigits(digitsInSequence);
            var seqEnd = baseOfDigits(digitsInSequence + 1);
            PrintLn($"..sequences are from {seqStart} to {seqEnd}");
            var total = 0L;

            for (var i = seqStart; i < seqEnd; i++)
            {
                var targetId = duplicate(i, sequenceRepeats);
                if (idIsInAnyRange(targetId, ranges))
                {
                    if (!seen.Contains(targetId))
                    {
                        PrintLn($"....ID {targetId} is in a range");
                        seen.Add(targetId);
                        total += targetId;
                    }
                }
            }

            return total;
        }
        
        private bool idIsInAnyRange(long id, List<(long, long)> ranges)
        {
            foreach (var r in ranges)
            {
                if (id >= r.Item1 && id <= r.Item2)
                    return true;
            }

            return false;
        }

        private string fmtRanges(List<(long, long)> ranges)
        {
            var str = "";
            foreach (var r in ranges)
            {
                str += $"({r.Item1} - {r.Item2}),";
            }

            return str;
        }

    }
}