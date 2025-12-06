namespace y25 {
    public class D6: AoCDay {

        public D6(): base(25, 6) {
            _DebugPrinting = true;
            _UseSample = false;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var rows = new List<List<string>>();
            foreach (var line in lines)
            {
                var row = new List<string>();
                var pts = line.Split(' ');
                foreach (var pt in pts)
                {
                    if (!string.IsNullOrEmpty(pt))
                        row.Add(pt);
                }
                rows.Add(row);
            }
            
            var total = 0L;
            int finalRow = rows.Count - 1;
            for (int col = 0; col < rows[0].Count; col++)
            {
                long allCols = long.Parse(rows[0][col]);
                bool isMult = rows[finalRow][col] == "*";
                var op = rows[finalRow][col];
                Print($"Col {col} - {allCols} {op}");
                for (int row = 1; row < rows.Count - 1; row++)
                {
                    var thisVal = long.Parse(rows[row][col]);
                    Print($" {thisVal} {op}");
                    if (isMult)
                        allCols*=thisVal;
                    else
                        allCols += thisVal;
                }
                PrintLn($" = {allCols}");
                total += allCols;
            }

            return $"total {total}";
        }
        
        public override string P2()
        {
            _DebugPrinting = true;
            var lines = InputAsLines();
            
            // all rows are equal length.
            // operator in the final row indicates a new column start

            var ops = lines.Last();
            PrintLn(ops);
            var nums = lines.Take(lines.Count - 1).ToList();
            PrintLn("" + nums.Count);
            var total = 0L;
            var current = 0L;
            bool isMult = false;
            for (int c = 0; c < ops.Length; c++)
            {
                if (ops[c] != ' ')
                {
                    PrintLn($" ={current}");
                    PrintLn($"\n{c}: {ops[c]} : starting new func");
                    total += current;
                    isMult = ops[c] == '*';
                    current = 0L;
                    if (isMult)
                        current = 1L;
                }
                
                //build a number top down
                var numStr = "";
                for (int i = 0; i < nums.Count; i++)
                {
                    //PrintLn($"    {nums[i][c]}   (row {i}, letter {c})");
                    numStr += nums[i][c];
                }

                numStr = numStr.Trim();
                if (!string.IsNullOrEmpty(numStr))
                {
                    var thisNum = long.Parse(numStr);
                    PrintLn($"  {thisNum}");
                    if(isMult)
                        current *= thisNum;
                    else current += thisNum;
                }
            }
            PrintLn($" ={current}");
            total += current;
            
            return $"total {total}";
        }
    }
}