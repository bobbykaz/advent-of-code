using System.ComponentModel.DataAnnotations;

namespace y24 {
    public class D17: AoCDay {

        private long RegA;
        private long RegB;
        private long RegC;
        private long InsP = 0;
        private List<long> Input = [];
        private List<long> Output = [];

        private enum OpCode {
            Adv = 0,  //A Div
            Bxl = 1, // B Bitwise XOR
            Bst = 2,
            Jnz = 3,  //
            Bxc = 4,
            Out = 5,
            Bdv = 6,
            Cdv = 7
        }

        private long ComboOperand(long n) {
            if(n <= 3) {return n;}
            switch(n) {
                case 4: return RegA;
                case 5: return RegB;
                case 6: return RegC;
                default: throw new Exception();
            }
        }

        private long GetInput() {
            if(InsP + 1 >= Input.Count) {
                PrintLn($"Halting;");
                PrintLn($"Output : {string.Join(",", Output)}");
                throw new Exception();
            }
            return Input[(int)InsP + 1];
        }

        private long IntPow(long baseN, long exp) {
            var start = 1L;
            for(int i = 0; i < exp; i++) {
                start *= baseN;
            }

            return start;
        }

        private long Div(long numerator, long op) {
            var opN = ComboOperand(op);
            var upTwo = IntPow(2, opN);
            return numerator / upTwo;
        }

        private void Op_Adv() {
            var input = GetInput();
            var rslt = Div(RegA, input);
            RegA = rslt;
        }

        private void Op_Bdv() {
            var input = GetInput();
            var rslt = Div(RegA, input);
            RegB = rslt;
        }

        private void Op_Cdv() {
            var input = GetInput();
            var rslt = Div(RegA, input);
            RegC = rslt;
        }

        private void Op_Bxl() {
            var input = GetInput();
            var rslt = RegB ^ input;
            RegB = rslt;
        }

        private void Op_Bst() {
            var input = ComboOperand(GetInput());
            RegB = input % 8;
        }

        private void Op_Jnz() {
            if(RegA != 0) {
                InsP = GetInput();

                InsP -=2;
            }
        }

        private void Op_Bxc() {
            var input = GetInput();
            RegB ^= RegC;
        }

        private void Op_Out() {
            var input = ComboOperand(GetInput());
            Output.Add(input % 8);
        }

        private bool DoOp() {
            if(InsP >= Input.Count()) {
                return false;
            }
            var op = (OpCode) Input[(int)InsP];
            //PrintLn($"{op} - {InsP}; {RegA}, {RegB}, {RegC}");
            switch(op) {
                case OpCode.Adv:
                    Op_Adv();
                    break;
                case OpCode.Bxl:
                    Op_Bxl();
                    break;
                case OpCode.Bdv:
                    Op_Bdv();
                    break;
                case OpCode.Bst:
                    Op_Bst();
                    break;
                case OpCode.Jnz:
                    Op_Jnz();
                    break;
                case OpCode.Cdv:
                    Op_Cdv();
                    break;
                case OpCode.Out:
                    Op_Out();
                    break;
                case OpCode.Bxc:
                    Op_Bxc();
                    break;
            }
            InsP += 2;

            return true;
        }

        public D17(): base(24, 17) {
            _DebugPrinting = true;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            RegA = int.Parse(lines[0].Split(": ")[1]);
            RegB = int.Parse(lines[1].Split(": ")[1]);
            RegC = int.Parse(lines[2].Split(": ")[1]);

            Input = Utilties.StringToNums<long>(lines[4].Split(": ")[1]);

            try {
                while(DoOp()) {

                }
            }
            catch(Exception e) {
                PrintLn($"Caught {e}");
            }

            var total = 0L;
            PrintLn($"Output : {string.Join(",", Output)}");
           
            return $"{total}";
        }

        private void Reset() {
            var lines = InputAsLines();
            RegA = int.Parse(lines[0].Split(": ")[1]);
            RegB = int.Parse(lines[1].Split(": ")[1]);
            RegC = int.Parse(lines[2].Split(": ")[1]);

            Input = Utilties.StringToNums<long>(lines[4].Split(": ")[1]);
            InsP = 0;
            Output = [];
        }

        public override string P2()
        {
            var lines = InputAsLines();
            Reset();
            var target = new List<long>(Input);
            //var attempt = 239800000;
            var desired = new List<int>();
            foreach(var n in target) {
                desired.Add(1);
            }

            // digits in RegA in base 8 correspond to number of outputs
            // first digit of RegA seems to correspond to last output (likely because we're %= and /=)
            // Brute Force through all options, solving 1 digit at a time
            for(int i = 0; i < desired.Count(); i++) {
                for(int n = 0; n < 8; n++) {
                    desired[i] = n;
                    Reset();
                    RegA = ToBase8Int(desired);
                    Compute();
                    if(Output.Count() == target.Count() && Output[target.Count() - i - 1] == target[target.Count() - i - 1]) {
                        PrintLn($"Digit {i} B8 value")
                        break;
                    }
                }
            }
            var final = ToBase8Int(desired);
            Reset();
            RegA = final;
            Compute();
            if(Output.SequenceEqual(target)){
                    return $"{final}";
            }

            var total = 0L;
            PrintLn($"Output : {string.Join(",", Output)}");
           
            return $"{total}";
        }

        private void Compute() {
            try 
            {
                while(DoOp()) {}
            }
            catch(Exception e) {
                //PrintLn($"Caught {e}");
            }
        }

        private long ToBase8Int(List<int> digitsBase8) {
            var asStr = string.Join("", digitsBase8);
            var num = Convert.ToInt64(asStr, 8);
            return num;
        }

    }
}