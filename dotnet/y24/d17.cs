namespace y24 {
    public class D17: AoCDay {

        private int RegA;
        private int RegB;
        private int RegC;
        private int InsP = 0;
        private List<int> Input = [];
        private List<int> Output = [];

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

        private int ComboOperand(int n) {
            if(n <= 3) {return n;}
            switch(n) {
                case 4: return RegA;
                case 5: return RegB;
                case 6: return RegC;
                default: throw new Exception();
            }
        }

        private int GetInput() {
            if(InsP + 1 >= Input.Count) {
                PrintLn($"Halting;");
                PrintLn($"Output : {string.Join(",", Output)}");
                throw new Exception();
            }
            return Input[InsP + 1];
        }

        private int IntPow(int baseN, int exp) {
            var start = 1;
            for(int i = 0; i < exp; i++) {
                start *= baseN;
            }

            return start;
        }

        private int Div(int numerator, int op) {
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
            var op = (OpCode) Input[InsP];
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

            Input = Utilties.StringToNums<int>(lines[4].Split(": ")[1]);

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

            Input = Utilties.StringToNums<int>(lines[4].Split(": ")[1]);
            Output = [];
        }

        public override string P2()
        {
            var lines = InputAsLines();
            Reset();
            var target = new List<int>(Input);
            var attempt = 239800000;
            var keepGoing = true;
            while (keepGoing) {
                if(attempt % 100000 == 0) {
                    PrintLn($"{attempt}");
                }
                RegA = attempt;
                try {
                    while(DoOp()) {

                    }
                }
                catch(Exception e) {
                    //PrintLn($"Caught {e}");
                }
                if(Output.SequenceEqual(target)){
                    return $"{attempt}";
                }
                attempt++;
                Reset();
            }
            

            var total = 0L;
            PrintLn($"Output : {string.Join(",", Output)}");
           
            return $"{total}";
        }

    }
}