
using Microsoft.Z3;

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

            return solve(buttons, joltage);
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

            using (Context ctx = new Context())
            {
                Optimize opt = ctx.MkOptimize();
                var buttonExprs = new List<IntExpr>();
                for (int b = 0; b < buttons.Count; b++)
                {
                    char buttonVariable = (char)('a' + b);
                    buttonExprs.Add(ctx.MkIntConst($"{buttonVariable}"));
                }
                
                var presses = ctx.MkIntConst("presses");
                
                opt.Add(
                    ctx.MkEq(
                        presses,
                        ctx.MkAdd(buttonExprs)
                    )
                );
                
                for (int n = 0; n < joltages.Count; n++)
                {
                    var joltageCount = joltages[n];
                    
                    var buttonsContributing = new List<int>();
                    for (int buttonIndex = 0; buttonIndex < buttons.Count; buttonIndex++)
                    {
                        if (buttons[buttonIndex].Indices.Contains(n))
                            buttonsContributing.Add(buttonIndex);
                    }

                    var contributingExprs = buttonsContributing.Select(i => buttonExprs[i]);
                    
                    opt.Add(
                        ctx.MkEq(
                            ctx.MkAdd(contributingExprs),
                            ctx.MkInt(joltageCount)
                            )
                        );
                }
                
                opt.MkMinimize(presses);

                // Check for a solution
                if (opt.Check() == Status.SATISFIABLE)
                {
                    Model model = opt.Model;
                    var rslt = $"{model.Eval(presses)}";
                    PrintLn($"  Min Presses: {model.Eval(presses)}");
                    for (int i = 0; i < buttonExprs.Count; i++)
                    {
                        PrintLn($"  Button {buttons[i]}: {model.Eval(buttonExprs[i])}");
                    }

                    return 0;
                }
                else
                {
                    PrintLn("  No solution found, or problem is unsatisfiable.");
                    throw new Exception();
                }
            }
        }

        private void Z3Solve()
        {
            using (Context ctx = new Context())
            {
                Optimize opt = ctx.MkOptimize();
                // Define integer variables
                IntExpr a = ctx.MkIntConst("a");
                IntExpr b = ctx.MkIntConst("b");
                IntExpr c = ctx.MkIntConst("c");
                IntExpr d = ctx.MkIntConst("d");
                IntExpr e = ctx.MkIntConst("e");
                IntExpr f = ctx.MkIntConst("f");
                IntExpr presses = ctx.MkIntConst("presses");

                // Add constraints
                // e + f = 3
                // b + f = 5
                // c + d = 4
                // a + b + d = 7
                // we're trying to minimize presses = a+b+c+d+e+f
                opt.Add(
                    ctx.MkEq(
                        ctx.MkAdd(e,f), 
                        ctx.MkInt(3)
                    )
                );
                
                opt.Add(
                    ctx.MkEq(
                        ctx.MkAdd(b,f), 
                        ctx.MkInt(5)
                    )
                );
                opt.Add(
                    ctx.MkEq(
                        ctx.MkAdd(c,d), 
                        ctx.MkInt(4)
                    )
                );
                opt.Add(
                    ctx.MkEq(
                        ctx.MkAdd(c,d), 
                        ctx.MkInt(4)
                    )
                );
                opt.Add(
                    ctx.MkEq(
                        ctx.MkAdd(a,b,c,d,e,f), 
                        presses
                    )
                );
                opt.MkMinimize(presses);

                // Check for a solution
                if (opt.Check() == Status.SATISFIABLE)
                {
                    Model model = opt.Model;
                    Console.WriteLine($"Min presses: {model.Eval(presses)}");
                    Console.WriteLine($"a: {model.Eval(a)}");
                    Console.WriteLine($"b: {model.Eval(b)}");
                    Console.WriteLine($"c: {model.Eval(c)}");
                    Console.WriteLine($"d: {model.Eval(d)}");
                    Console.WriteLine($"e: {model.Eval(e)}");
                    Console.WriteLine($"f: {model.Eval(f)}");
                }
                else
                {
                    Console.WriteLine("No solution found, or problem is unsatisfiable.");
                }
            }
        }
    }
}