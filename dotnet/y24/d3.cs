using System.Diagnostics;
using System.Numerics;

namespace y24 {
    public class D3: AoCDay {

        public D3(): base(24, 3) {
            _DebugPrinting = true;
        }
        public override string P1()
        {

            var temp = parse("xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))");
            PrintLn("" + temp);

            var lines = InputAsLines();
            long total = 0;
            foreach (var (line,l) in lines.ForEachIndex()) {
                total += parse(line);   
            }
            return $"{total}";
        }

        private long parse(string mem) {
            long total = 0;
            var pts = mem.Split("mul(");
            foreach(var pt in pts){
                var sub = Utilties.Split(pt, new List<string>{ "," , ")" });
                var comma = pt.IndexOf(",");
                var paren = pt.IndexOf(")");
                if(sub.Count >= 2 && comma < paren && comma > 0 && paren > 0) {
                    int a = 0;
                    int b = 0;
                    if(int.TryParse(sub[0], out a) && int.TryParse(sub[1], out b)){
                        total += a*b;
                    }
                }
            }
            return total;
        }

        private (long, bool) parse2(string mem, bool startEnabled) {
            long total = 0;
            var pts = mem.Split("mul(");
            var enabled = startEnabled;
            foreach(var pt in pts){
                var sub = Utilties.Split(pt, new List<string>{ "," , ")" });
                var comma = pt.IndexOf(",");
                var paren = pt.IndexOf(")");
                if(sub.Count >= 2 && comma < paren && comma > 0 && paren > 0) {
                    int a = 0;
                    int b = 0;
                    if(int.TryParse(sub[0], out a) && int.TryParse(sub[1], out b)){
                        if(enabled)
                            total += a*b;
                    }
                }
                var lastDo = pt.LastIndexOf("do()");
                var lastDont = pt.LastIndexOf("don't()");
                if(lastDo > lastDont)
                    enabled = true;
                if(lastDont > lastDo)
                    enabled = false;
            }
            return (total, enabled);
        }

        public override string P2()
        {
            var lines = InputAsLines();
            long total = 0;
            var enabled = true;
            foreach (var (line,l) in lines.ForEachIndex()) {
                var (t, b) = parse2(line, enabled);
                total += t;
                enabled = b;
            }

            return $"{total}";
        }

    }
}