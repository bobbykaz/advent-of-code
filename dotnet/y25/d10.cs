using System.Security.AccessControl;

namespace y25 {
    public class D10: AoCDay {

        public D10(): base(25, 10) {
            _DebugPrinting = true;
            _UseSample = false;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var total = 0L;
            foreach (var line in lines)
            {
             total += solveMachineP1(line);
            }

            return $"total {total}";
        }

        private long solveMachineP1(string line)
        {
            PrintLn($"\nraw: {line}");
            var pts = Utilties.Split(line, ["] ", " {"]);
            var target = pts[0].Substring(1);// skip leading '['
            var totalLights = target.Length;
            
            var buttons = ButtonResult.Parse(pts[1],totalLights);
            //ignore joltage at end

            return Solve(target, buttons);
        }

        private long Solve(string target, List<ButtonResult> oneButtons)
        {
            Dictionary<string, ButtonResult> seen = [];
            foreach (var b in oneButtons)
            {
                seen[b.LightStr()] = b;
                if (b.LightStr() == target)
                {
                    PrintLn($"found at {b} (one shot!)");
                    return b.Presses;
                }
            }

            var pressLevel = 1;
            var newButtonsThisRound = oneButtons.ToList();
            while (true)
            {
                var iterButtons = new List<ButtonResult>();
                foreach (var b in newButtonsThisRound)
                {
                 iterButtons.Add(b);   
                }
                newButtonsThisRound.Clear();
                
                pressLevel++;
                PrintLn($".. checking all {pressLevel} press combos");
                var allPrevButtons = seen.Values.ToList().OrderBy(b => b.Presses);
                var found = false;
                foreach (var b in iterButtons.OrderBy(b => b.Presses))
                {
                    foreach (var prevButton in allPrevButtons)
                    {
                        if (b != prevButton)
                        {
                            var resultButton = b.Combine(prevButton);
                            var existingButton = seen.ContainsKey(resultButton.LightStr()) ? seen[resultButton.LightStr()] : null;
                            if ( existingButton == null || existingButton.Presses > resultButton.Presses)
                            {
                                PrintLn($"  Combining \n   {b} with \n   {prevButton}");
                                PrintLn($" = {resultButton}");
                                seen[resultButton.LightStr()] = resultButton;
                                newButtonsThisRound.Add(resultButton);

                                if (resultButton.LightStr() == target)
                                {
                                    PrintLn($"found at {resultButton}");
                                    return resultButton.Presses;
                                }
                            }
                        }
                    }
                }

                if (pressLevel >= 10)
                {
                    throw new Exception();
                }
            }
        }

        private record ButtonResult(int TotalLights, HashSet<int> Indices, long Presses)
        {
            public static List<ButtonResult> Parse(string buttonStr, int indicators)
            {
                var pts = buttonStr.Split(" ");
                 return pts
                    .Select(str => str.Trim('(', ')'))
                    .Select(trimmed => trimmed.Split(","))
                    .Select(s => s.Select(int.Parse).ToList().ToHashSet())
                    .Select(list => new ButtonResult(indicators, list, 1))
                    .ToList();
            }

            public string LightStr()
            {
                var lights = new string('.', TotalLights).ToCharArray();
                foreach (var i in Indices)
                {
                    lights[i] = '#';
                }

                return string.Join("",lights);
            }
            public ButtonResult Combine(ButtonResult other)
            {
                var totalPresses = this.Presses + other.Presses;
                var combinedIndices = new HashSet<int>();
                foreach (var i in this.Indices)
                {
                    if(!other.Indices.Contains(i))
                        combinedIndices.Add(i);
                }
                
                foreach (var i in other.Indices)
                {
                    if(!this.Indices.Contains(i))
                        combinedIndices.Add(i);
                }
                
                return new ButtonResult(TotalLights, combinedIndices, totalPresses);
            }

            public override string ToString()
            {
                //return $"[{LightStr()}] - {string.Join(",", Indices.ToArray())} - [{Presses} presses]";
                return $"[{LightStr()}] - [{Presses} presses]";
            }
        }
        
        public override string P2()
        {
            _DebugPrinting = true;
            var lines = InputAsLines();
            var total = 0L;

            return $"total {total}";
        }
    }
}