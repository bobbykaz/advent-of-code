using System.Security.AccessControl;

namespace y25 {
    public class D10: AoCDay {

        public D10(): base(25, 10) {
            _DebugPrinting = false;
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
            var queue = new Queue<ButtonResult>();
            var emptyButton = new ButtonResult(target.Length, new HashSet<int>(), 0);
            queue.Enqueue(emptyButton);
            seen[emptyButton.LightStr()] = emptyButton;
            while (queue.Any())
            {
                var next = queue.Dequeue();
                foreach (var pressedButton in oneButtons)
                {
                    var resultButton = next.Combine(pressedButton);
                    var existingButton = seen.ContainsKey(resultButton.LightStr()) ? seen[resultButton.LightStr()] : null;
                    if ( existingButton == null || existingButton.Presses > resultButton.Presses)
                    {
                        PrintLn($"  Combining \n   {next} with \n   {pressedButton}");
                        PrintLn($" = {resultButton}");
                        seen[resultButton.LightStr()] = resultButton;
                        queue.Enqueue(resultButton);

                        if (resultButton.LightStr() == target)
                        {
                            PrintLn($"found at {resultButton}");
                            return resultButton.Presses;
                        }
                    }
                }
            }
            
            throw new Exception("Unreachable");
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
            foreach (var line in lines)
            {
                total += solveMachineP2(line);
            }

            return $"total {total}";
        }
        
        private long solveMachineP2(string line)
        {
            PrintLn($"raw: {line}");
            var pts = Utilties.Split(line, ["] ", " {"]);
            var target = pts[0].Substring(1);// skip leading '['
            var totalLights = target.Length;
            
            var buttons = ButtonResult.Parse(pts[1],totalLights);
            var joltage = pts[2].Trim('}').Split(",").Select(s => long.Parse(s)).ToList();
            if (joltage.Count != totalLights)
                throw new Exception("Joltage count mismatch");

            return 0L;
        }

        private long solve(List<ButtonResult> buttons, List<long> joltages)
        {
            // each button is a variable
            // a   b     c   d      e    f
            //(3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}
            // e + f = 3
            // b + f = 5
            // c + d = 4
            // a + b + d = 7
            for (int n = 0; n < joltages.Count; n++)
            {
                var joltageCount = joltages[n];
                var variablesContributing = new List<char>();
                for (int buttonIndex = 0; buttonIndex < buttons.Count; buttonIndex++)
                {
                    if (buttons[buttonIndex].Indices.Contains(n))
                    {
                        char buttonVariable = (char) ('a' + buttonIndex);
                        variablesContributing.Add(buttonVariable);
                    }
                }
            }

            return 0L;
        }
    }
}