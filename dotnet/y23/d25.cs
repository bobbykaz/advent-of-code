using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace y23 {
    public class D25 : AoCDay
    {
        public D25(): base(23, 25) {
            _DebugPrinting = true;
        }

        public class Node {
            public string name;
            public HashSet<string> connected;
            public Node(string name) {
                this.name = name; connected = new HashSet<string>();
            }
        }

        public override string P1()
        {
            var lines = InputAsLines();
            var wm = new Dictionary<string,Node>();
            var wires = new HashSet<string>();
            foreach (var line in lines) {
                var pts = line.Split(": ");
                var connects = pts[1].Split(" ");
                var wa = pts[0];
                if (! wm.ContainsKey(wa)) {
                    wm[wa] = new Node(wa);
                }
                var na = wm[wa];
                foreach(var wb in connects) {
                    if (! wm.ContainsKey(wb)) {
                        wm[wb] = new Node(wb);
                    }
                    var nb = wm[wb];
                    na.connected.Add(nb.name);
                    nb.connected.Add(na.name);
                    wires.Add(na.name);
                    wires.Add(nb.name);
                }
            }
            //PrintToViz(wm);
            wm["zkv"].connected.Remove("zxb");
            wm["zxb"].connected.Remove("zkv");
            wm["mtl"].connected.Remove("pgl");
            wm["pgl"].connected.Remove("mtl");
            wm["lkf"].connected.Remove("scf");
            wm["scf"].connected.Remove("lkf");

            PrintLn($"Total set size: {wires.Count}");

            var size1 = ClusterSize(wm, "pgl");
            PrintLn($"Size 1: {size1}");
            var size2 = ClusterSize(wm, "gjs");
            PrintLn($"Size 2: {size2}");
            return $"{size1*size2}";
        }

        public int ClusterSize(Dictionary<string,Node> wm, string node){
            var q = new Queue<string>();
            q.Enqueue(node);
            var count = 0;
            var seen = new HashSet<string>();
            seen.Add(node);
            while(q.Any()) {
                var n = q.Dequeue();
                count++;
                var nn = wm[n];
                foreach(var next in nn.connected){
                    if(!seen.Contains(next)){
                        seen.Add(next);
                        q.Enqueue(next);
                    }
                }
            }
            return count;
        }

        public void PrintToViz(Dictionary<string,Node> wm) {
            //"C:\Program Files\Graphviz\bin\sfdp.exe" -x -Goverlap=scale -Tpng -o out.png ./test.dot
            PrintLn("graph {");
            foreach (var n in wm.Values) {
                foreach(var next in n.connected) {
                    PrintLn($"{n.name} [label=\"{n.name}\"] -- {next}");
                }
            }
            PrintLn("}");
        }

        public List<string>? BFS(Dictionary<string,Node> wm, string from, string to) {
            var q = new Queue<(string, List<string>)>();
            q.Enqueue((from, new List<string>()));
            var seen = new HashSet<string>();
            while(q.Any()) {
                var (f,c) = q.Dequeue();
                var fn = wm[f];
                if (fn.connected.Contains(to)) {
                    return c;
                }
                foreach(var next in fn.connected){
                    if(!seen.Contains(next)){
                        seen.Add(next);
                        var nc = new List<string>(c);
                        nc.Add(next);
                        q.Enqueue((next, nc));
                    }
                }
            }
            return null;
        }

        public override string P2()
        {
            var lines = InputAsLines();
            //<form method="post" action="25/answer"><input type="hidden" name="level" value="2"/><input type="hidden" name="answer" value="0"/><p>You have enough stars to <input type="submit" value="[Push The Big Red Button]"/>.</p></form>
            return $"{0}";
        }
    }
}
