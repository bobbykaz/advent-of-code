namespace y24 {
    public class D7: AoCDay {

        public D7(): base(24, 7) {
            _DebugPrinting = true;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            long total = 0;
            foreach(var l in lines){
                var pts = l.Split(": ");
                var result = long.Parse(pts[0]);
                var ops = Utilties.StringToNums<long>(pts[1], " ");
                if(permute(ops, result))
                    total += result;

            }

            return $"{total}";
        }

        private bool permute(List<long> nums, long total, bool part2 = false) {
            var possibleLists = new List<string> { "" };
            var rounds = nums.Count - 1;
            for (int r = 0; r < rounds; r++) {
                var newPossibleLists = new List<string>();
                foreach(var s in possibleLists) {
                    newPossibleLists.Add(s + "+");
                    newPossibleLists.Add(s + "*");
                    if(part2)
                        newPossibleLists.Add(s + "|");
                }
                possibleLists = newPossibleLists;
            }

            foreach( var opsList in possibleLists) {
                var result = calculate(nums, opsList.ToCharArray().ToList());
                if (result == total) {
                    return true;
                }
            }

            return false;
        }

        private long calculate(List<long> nums, List<char> ops) {
            long total = nums[0];
            for (int i = 0; i < ops.Count; i++) {
                long next = nums[i+1];
                long nextOp = ops[i];
                switch(nextOp){
                    case '+':
                        total = total + next;
                        break;
                    case '*':
                        total = total * next;
                        break;
                    case '|':
                        var str = $"{total}{next}";
                        total = long.Parse(str);
                        break; 
                    default:
                        throw new Exception();
                }
            }

            return total;
        }

       

        public override string P2()
        {
            var lines = InputAsLines();
            long total = 0;
            foreach(var l in lines){
                var pts = l.Split(": ");
                var result = long.Parse(pts[0]);
                var ops = Utilties.StringToNums<long>(pts[1], " ");
                if(permute(ops, result, part2: true))
                    total += result;

            }

            return $"{total}";
        }

    }
}