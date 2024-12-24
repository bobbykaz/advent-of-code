using System.Data.Common;
using System.Xml.XPath;

namespace y24 {
    public class D24: AoCDay {

        public D24(): base(24, 24) {
            _DebugPrinting = true;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var groups = Utilties.GroupInputAsBlocks(lines);
            var gates = parseInput(groups[0]);

            PrintLn($"Input gates: {groups[0].Count}");
            PrintLn($"Output gates: {groups[1].Count}");
            parseLogic(groups[1], gates);
            bool anyNotReady = true;
            while(anyNotReady){
                anyNotReady = false;
                foreach(var (id, gate) in gates){
                    if (!gate.Ready(gates)) {
                        anyNotReady = true;
                    }
                }
            }
            
            var outputBinary = GetBinaryValueOfGates(gates, 'z');
            PrintLn($"Output: {outputBinary}");
            var answer = Convert.ToInt64(outputBinary, 2);
           
            return $"{answer}";
        }

        private abstract record class Gate(string ID) {
            public int Clock = 0;
            protected int? _Value;

            public abstract bool Ready(Dictionary<string, Gate> lookup);
            public virtual int? Value() {
                return _Value;
            }

            public abstract (string, string)? GetInputs();
        }

        private record class InputGate(string ID, int InitialValue) : Gate(ID)
        {
            public int InternalNum = InitialValue;
            public override int? Value()
            {
                return InternalNum;
            }
            public override bool Ready(Dictionary<string, Gate> lookup)
            {
                return true;
            }

            public override (string, string)? GetInputs()
            {
                return null;
            }
        }

        private record class AndGate(string ID, string Left, string Right) : Gate(ID)
        {
            private bool isReady = false;
            public override bool Ready(Dictionary<string, Gate> lookup)
            {
                if(!isReady){
                    var checkReady = lookup[Left].Ready(lookup) && lookup[Right].Ready(lookup);
                    if(checkReady){
                        isReady = true;
                        _Value = lookup[Left].Value() & lookup[Right].Value();
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    return true;
                }
            }

            public override (string, string)? GetInputs()
            {
                return (Left, Right);
            }
        }

        private record class OrGate(string ID, string Left, string Right) : Gate(ID)
        {
            private bool isReady = false;
            public override bool Ready(Dictionary<string, Gate> lookup)
            {
                if(!isReady){
                    var checkReady = lookup[Left].Ready(lookup) && lookup[Right].Ready(lookup);
                    if(checkReady){
                        isReady = true;
                        _Value = lookup[Left].Value() | lookup[Right].Value();
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    return true;
                }
            }

            public override (string, string)? GetInputs()
            {
                return (Left, Right);
            }
        }

        private record class XorGate(string ID, string Left, string Right) : Gate(ID)
        {
            private bool isReady = false;
            public override bool Ready(Dictionary<string, Gate> lookup)
            {
                if(!isReady){
                    var checkReady = lookup[Left].Ready(lookup) && lookup[Right].Ready(lookup);
                    if(checkReady){
                        isReady = true;
                        _Value = (lookup[Left].Value() ^ lookup[Right].Value()) & 1;
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    return true;
                }
            }

            public override (string, string)? GetInputs()
            {
                return (Left, Right);
            }
        }

        private Dictionary<string, Gate> parseInput(List<string> lines) {
            var result = new Dictionary<string, Gate>();
            foreach(var l in lines){
                var pts = l.Split(": ");
                result[pts[0]] = new InputGate(pts[0], int.Parse(pts[1]));
            }
            return result;
        }

        private void parseLogic(List<string> lines, Dictionary<string, Gate> gates) {
            foreach(var l in lines){
                var pts = l.Split(" -> ");
                var id = pts[1];
                var logic = pts[0].Split(" ");
                var left = logic[0];
                var right = logic[2];
                switch(logic[1]){
                    case "AND":
                        gates[id] = new AndGate(id, left, right);
                        break;
                    case "OR":
                        gates[id] = new OrGate(id, left, right);
                        break;
                    case "XOR":
                        gates[id] = new XorGate(id, left, right);
                        break;
                    default: 
                    throw new Exception();
                }
            }
        }

        private string GetBinaryValueOfGates(Dictionary<string, Gate> gates, char startingWith) {
            var result = new List<Gate>(); 
            foreach(var (id, gate) in gates){
                if (id.StartsWith(startingWith)) {
                    result.Add(gate);
                }
            }

            result = result.OrderByDescending(g => g.ID).ToList();
            var resultValues = result.Select(r => r.Value());
            var outputBinary = string.Join("", resultValues);
            PrintLn($"Output for Gates {startingWith}: {outputBinary}");
            return outputBinary;
        }

        private void PrintRelatedGates(List<string> inputLogic, List<string> gates) {
            foreach(var l in inputLogic) {
                if(l.Split(" ").Any(s => gates.Contains(s))){
                    PrintLn(l);
                }
            }
        }

        private List<string> FindGatesFromOutput(Dictionary<string, Gate> gates, string zGate) {
            List<string> rslt = [];
            rslt.Add(zGate);
            var queue = new Queue<string>();
            queue.Enqueue(zGate);
            while(queue.Any()) {
                var next = queue.Dequeue();
                var inputs = gates[next].GetInputs();
                if(inputs.HasValue) {
                    var (l,r) = inputs.Value;
                    queue.Enqueue(l);
                    queue.Enqueue(r);
                    rslt.Add(l);
                    rslt.Add(r);
                }
            }

            return rslt;
        }

        private List<string> FindSomeGatesFromOutput(Dictionary<string, Gate> gates, string zGate, string NotGate) {
            var r1 = FindGatesFromOutput(gates, NotGate);
            var r2 = FindGatesFromOutput(gates, zGate);
            return r2.Except(r1).ToList();
        }

        private bool RandomNumberAdd() {
            var lines = InputAsLines();
            var groups = Utilties.GroupInputAsBlocks(lines);
            var gates = parseInput(groups[0]);
            var rand = new Random();
            foreach(var k in gates.Keys) {
                var val = rand.Next() % 2;
                (gates[k] as InputGate).InternalNum = val;
            }

            parseLogic(groups[1], gates);
            bool anyNotReady = true;
            while(anyNotReady){
                anyNotReady = false;
                foreach(var (id, gate) in gates){
                    if (!gate.Ready(gates)) {
                        anyNotReady = true;
                    }
                }
            }
            var xbinary = GetBinaryValueOfGates(gates, 'x');
            var ybinary = GetBinaryValueOfGates(gates, 'y');
            var expected = Convert.ToString(Convert.ToInt64(xbinary, 2) + Convert.ToInt64(ybinary, 2), 2);
            var zbinary = GetBinaryValueOfGates(gates, 'z');
            while(expected.Length < zbinary.Length) {
                expected = "0" + expected;
            }
            PrintLn($"Expected............{expected}");
            if(expected == zbinary){
                PrintLn($"\n===\nThey match!\n===\n");
                return true;
            } else {
                PrintLn($"\n===\nWRONG!!!!!\n===\n");
                return false;
            }
        }

        public override string P2()
        {
            var lines = InputAsLines();
            var groups = Utilties.GroupInputAsBlocks(lines);
            var gates = parseInput(groups[0]);

            PrintLn($"Input gates: {groups[0].Count}");
            PrintLn($"Output gates: {groups[1].Count}");
            parseLogic(groups[1], gates);
            bool anyNotReady = true;
            while(anyNotReady){
                anyNotReady = false;
                foreach(var (id, gate) in gates){
                    if (!gate.Ready(gates)) {
                        anyNotReady = true;
                    }
                }
            }
            var xbinary = GetBinaryValueOfGates(gates, 'x');
            var ybinary = GetBinaryValueOfGates(gates, 'y');
            var expected = Convert.ToString(Convert.ToInt64(xbinary, 2) + Convert.ToInt64(ybinary, 2), 2);
            PrintLn($"Expected............{expected}");
            var zbinary = GetBinaryValueOfGates(gates, 'z');
            if(expected == zbinary){
                PrintLn($"\n===\nThey match!\n===\n");
            }
            int count = 0;
            for(int i = 0; i < 1; i++) {
                if (RandomNumberAdd()) {
                    count++;
                }
            }
            PrintLn($"correct adds: {count}");

            //reverse expected, zBinary to get possible gates where something is amiss
            //attempt to swap gates in those chains only

            List<string> crossedWires = [];
            var re = expected.Reverse().ToArray();
            var rz = zbinary.Reverse().ToArray();
            for(int i = 0; i < re.Length; i++) {
                if(re[i] != rz[i]){
                    var name = i < 10? $"z0{i}" : $"z{i}";
                    crossedWires.Add(name);
                    PrintLn($"Compared {name}: expected {re[i]}, got {rz[i]}");
                }

            }

            //Figure out z27
            PrintLn($"z26");
            PrintRelatedGates(groups[1], FindSomeGatesFromOutput(gates, "z26", "z25"));

            PrintLn($"z27");
            PrintRelatedGates(groups[1], FindSomeGatesFromOutput(gates, "z27", "z26"));

            // z09 - kfp, hbs
            // z18 - dhq, z18
            // z22 - pdg, z22
            // z27 - jcp, z27
            var final = new List<string>(){"kfp", "hbs", "dhq", "z18", "pdg", "z22", "jcp", "z27"};
            final.Sort();

            return $"{string.Join(",", final)}";
        }

    }
}