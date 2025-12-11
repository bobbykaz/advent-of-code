namespace y25 {
    public class D1: AoCDay {

        public D1(): base(25, 1) {
            _DebugPrinting = false;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var dial = 50;
            var zerosHit = 0;

            foreach (var line in lines)
            {
                var val = parseLine(line);
                dial = dial + val;
                if (dial < 0)
                {
                    dial = 100 + dial;
                }

                dial %= 100;
                
                if (dial == 0)
                {
                    zerosHit++;
                }
                PrintLn($"Dial turned {val} to {dial} ; zeros hit: {zerosHit}");
            }
            

            return $"zeros hit {zerosHit}";
        }

        private int parseLine(string s)
        {
            var num = s.Substring(1);
            var val = int.Parse(num);
            if (s[0] == 'R') 
                return val;
            return -val;
        }

        public override string P2()
        {
            var lines = InputAsLines();
            var dial = 50;
            var zerosHit = 0;
            _DebugPrinting = true;
            foreach (var line in lines)
            {
                var val = parseLine(line);
                //PrintLn($"...Val is {val}");
                while (val > 100)
                {
                    val -= 100;
                    zerosHit++;
                    PrintLn($"Rotating right through 0; zeros hit: {zerosHit}; new val {val}");
                }
                
                while (val < -100)
                {
                    val += 100;
                    zerosHit++;
                    PrintLn($"Rotating left through 0; zeros hit: {zerosHit}; new val {val}");
                }

                var wasZero = dial == 0;
                
                dial = dial + val;
                if (dial < 0)
                {
                    dial = 100 + dial;
                    if (!wasZero)
                    {
                        zerosHit++;
                    }
                }

                if (dial == 0)
                {
                    zerosHit++;
                }
                
                if (dial >= 100)
                {
                    dial %= 100;
                    zerosHit++;
                }
                
                
                PrintLn($"Dial turned {line} ( {val} ) to {dial} ; zeros hit: {zerosHit}");
            }
            

            return $"zeros hit {zerosHit}";
        }

    }
}