namespace y23 {
    public class D1 {
        public void Run(){
            P1();
            P2();
        }
        public void P1() {
            var lines = Utilties.ReadFileToLines("../input/y23/d1.txt");
            var total = 0;
            foreach (var line in lines) {
                char? f = null;
                char? l = null;
                foreach(var c in line) {
                    if (char.IsDigit(c)) {
                        if (!f.HasValue) {
                            f = c;
                        }
                        l = c;
                    }
                }
                var num = int.Parse($"{f}{l}");
                total += num;
            }
            Console.WriteLine(total);
        }

        public void P2() {
            var lines = Utilties.ReadFileToLines("../input/y23/d1.txt");
            var total = 0;
            foreach (var line in lines) {
                var ints = new List<(int,char)>();
                (int,char)? f = null;
                (int,char)? l = null;
                int i = 0;
                ints.Add( (line.IndexOf("one"),'1'));
                ints.Add( (line.IndexOf("two"),'2'));
                ints.Add( (line.IndexOf("three"),'3'));
                ints.Add( (line.IndexOf("four"),'4'));
                ints.Add( (line.IndexOf("five"),'5'));
                ints.Add( (line.IndexOf("six"),'6'));
                ints.Add( (line.IndexOf("seven"),'7'));
                ints.Add( (line.IndexOf("eight"),'8'));
                ints.Add( (line.IndexOf("nine"),'9'));
                ints.Add( (line.LastIndexOf("one"),'1'));
                ints.Add( (line.LastIndexOf("two"),'2'));
                ints.Add( (line.LastIndexOf("three"),'3'));
                ints.Add( (line.LastIndexOf("four"),'4'));
                ints.Add( (line.LastIndexOf("five"),'5'));
                ints.Add( (line.LastIndexOf("six"),'6'));
                ints.Add( (line.LastIndexOf("seven"),'7'));
                ints.Add( (line.LastIndexOf("eight"),'8'));
                ints.Add( (line.LastIndexOf("nine"),'9'));
                foreach(var ch in line) {
                    if (char.IsDigit(ch)) {
                        if (!f.HasValue) {
                            f = (i,ch);
                        }
                        l = (i,ch);
                    }
                    i++;
                }
                
                if (!(f.HasValue && l.HasValue)) {
                    throw new Exception("didnt find digits");
                }

                ints.Add(f.Value);
                ints.Add(l.Value);
                var fi = ints.Where( i => i.Item1 != -1).MinBy(s => s.Item1);
                var li = ints.Where( i => i.Item2 != -1).MaxBy(s => s.Item1);

                var num = int.Parse($"{fi.Item2}{li.Item2}");
                total += num;
            }
            Console.WriteLine(total);
        }
    }
}