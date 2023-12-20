namespace y23 {
    public class D15 : AoCDay
    {
        public D15(): base(23, 15) {
            _DebugPrinting = true;
        }

        public override string P1()
        {
            var lines = InputAsLines()[0].Split(',');
            var total = lines.Select(s => HashString(s)).Sum();
                       
            return $"{total}";
        }

        public int HashString(string str) {
            int current = 0;
            foreach(var ch in str.ToCharArray()) {
                current += (int) ch;
                current *= 17;
                current = current % 256;
            }
            return current;
        }

        public override string P2()
        {
            var seqs = InputAsLines()[0].Split(',');
            var hashmap = new Dictionary<int, List<(string, int)>>();
            for(int i = 0; i < 256; i++) {
                hashmap[i] = new List<(string, int)>();
            }
            foreach (var seq in seqs) {
                bool isDash = seq.Contains('-');
                var pts = isDash? seq.Split('-') : seq.Split('=');
                var (label, num) = (pts[0], int.Parse(pts[1] == "" ? "-1" : pts[1]));
                var hash = HashString(label);
                if (isDash) {
                    var lens = hashmap[hash] ?? new List<(string, int)>();
                    lens = lens.Where(s => s.Item1 != label).ToList();
                    hashmap[hash] = lens;
                } else {
                    var lens = hashmap[hash] ?? new List<(string, int)>();
                    bool replaced = false;
                    for(var i = 0; i < lens.Count; i++) {
                        if(lens[i].Item1 == label) {
                            lens[i] = (label,num);
                            replaced = true;
                        }
                    }
                    if (!replaced){
                        lens.Add((label, num));
                    }
                    hashmap[hash] = lens;
                }

            }

            var total = 0L;
            for(int i = 0; i < 256; i++) {
                var boxTotal = 0L;
                var lens = hashmap[i] ?? new List<(string, int)>();
                var boxNum = 1+i;

                for(int l = 0; l < lens.Count; l++) {
                    boxTotal += boxNum * (l+1) * lens[l].Item2;
                }

                total += boxTotal;
            }
            
            return $"{total}";
        }
    }
}