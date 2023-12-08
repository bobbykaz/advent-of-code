namespace y23 {
    public class D8 : AoCDay
    {
        public D8(): base(23, 8) {
            _DebugPrinting = true;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var dirsStr = lines[0];

            var nmap = new Dictionary<string, Node>();
            for (var l = 2; l < lines.Count(); l++) {
                var pts = Utilties.Split(lines[l], new List<string>{" = (", ", ", ")"}).Where(s => s.Length > 0).ToList();
                var n = new Node(pts[0], pts[1], pts[2]);
                nmap[n.ID] = n;
            }

            var curr = nmap["AAA"];
            var i = 0;
            var steps = 0;
            var dirs = dirsStr.ToCharArray();
            while(curr.ID != "ZZZ") {
                switch(dirs[i]) {
                    case 'L':
                        curr = nmap[curr.Left];
                        break;
                    case 'R':
                        curr = nmap[curr.Right];
                        break;
                    default:
                        throw new Exception("non rl dir");
                }

                steps++;
                i++;
                i = i% dirs.Length;
            }


            return $"{steps}";
        }

        public override string P2()
        {
            var lines = InputAsLines();
            var dirsStr = lines[0];
            
            var nmap = new Dictionary<string, Node>();
            var startNodes = new List<Node>();
            for (var l = 2; l < lines.Count(); l++) {
                var pts = Utilties.Split(lines[l], new List<string>{" = (", ", ", ")"}).Where(s => s.Length > 0).ToList();
                var n = new Node(pts[0], pts[1], pts[2]);
                nmap[n.ID] = n;
                if(n.ID[2] == 'A') {
                    startNodes.Add(n);
                }
            }
            var thingsToLcm = new List<long>();
            foreach(var n in startNodes) {
                PrintLn($"Starting with Node {n.ID}");
                var i = 0;
                var steps = 0L;
                var dirs = dirsStr.ToCharArray();
                var curr = n;
                
                bool found = false;
                while(!found) {
                    curr = dirs[i] switch {
                        'L' => nmap[curr.Left],
                        'R' => nmap[curr.Right],
                        _ =>throw new Exception("non rl dir")
                    };

                    steps++;
                    i++;
                    i = i% dirs.Length;

                    if(curr.ID[2] == 'Z') {
                        PrintLn($"  {curr.ID} @ {steps} / {i}");
                        found = true;
                        var next = dirs[i] switch {
                        'L' => nmap[curr.Left],
                        'R' => nmap[curr.Right],
                        _ =>throw new Exception("non rl dir")
                        };
                        PrintLn($"next = {next.ID}");
                    };
        
                }
                PrintLn($"... ended at {steps}");
                thingsToLcm.Add(steps);
            }

            thingsToLcm.ForEach(l => Print($"{l}, "));
            PrintLn("");
            var lcm = Utilties.LCM(thingsToLcm.ToArray());

            return $"{lcm}";
        }

        public class Node {
            public string ID;
            public string Left;
            public string Right;

            public Node(string i, string l, string r) {
                ID = i;
                Left = l;
                Right = r;
            }
        }
    }
}