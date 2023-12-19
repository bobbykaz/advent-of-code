using System.Security.Cryptography.X509Certificates;

namespace y23 {
    public class D19 : AoCDay
    {
        public D19(): base(23, 19) {
            _DebugPrinting = true;
        }
        
        public override string P1()
        {
            var lines = InputAsLines();
            var blocks = Utilties.GroupInputAsBlocks(lines);
            var wfs = new List<Workflow>();
            var wfMap = new Dictionary<string,Workflow>();
            foreach(var line in blocks[0]){
                var wf = parseWf(line);
                wfs.Add(wf);
                wfMap[wf.ID] = wf;
            }

            var parts = new List<Part>();
            foreach(var line in blocks[1]){
                parts.Add(new Part(line));
            }

            var total = 0L;
            foreach(var p in parts){
                if(AcceptOrRejectPart(wfMap,p)) {
                    total += p.TotalRating;
                }
            }
           
            return $"{total}";
        }
        public Workflow parseWf(string line) {
            var pts = line.Split("{");
            var name = pts[0];
            var rules = pts[1].TrimEnd('}');
            var rulePts = rules.Split(",");
            var wf = new Workflow(name)
            {
                Rules = rulePts.Select(s => parseRule(s)).ToList()
            };
            return wf;
        }

        public bool AcceptOrRejectPart( Dictionary<string,Workflow> wfmap, Part p) {
            var curWf = wfmap["in"];
            while(true) {
                foreach(var r in curWf.Rules) {
                    if (p.MeetsRule(r)) {
                        if(r.dest == "A") return true;
                        if(r.dest == "R") return false;
                        curWf = wfmap[r.dest];
                        break;
                    }
                }
            }
            throw new Exception("unreachable");
        }

        public Rule parseRule(string rule){
            if(rule.Contains(':')) {
                var pts = Utilties.Split(rule, new string[]{"<", ">", ":"}.ToList());
                return new Rule(pts[0], long.Parse(pts[1]), rule.Contains(">"), pts[2]);
            }
            return new Rule("",0,false,rule);
        }
        public enum Cat {
            X,M,A,S, None
        }
        public class Workflow {
            public string ID;
            public List<Rule> Rules;
            public Workflow(string id) {
                ID = id;
                Rules = new List<Rule>();
            }

        }
        public class Rule {
            public Cat category;
            public long target;
            public bool opIsGreater;
            public string dest;
            public Rule(string c, long t, bool o, string d) {
                category = c switch {
                    "x" => Cat.X,
                    "m" => Cat.M,
                    "a" => Cat.A,
                    "s" => Cat.S,
                    _ => Cat.None
                };
                target = t;
                opIsGreater = o;
                dest = d;
            }
        }

        public class Part {
            public long x;
            public long m;
            public long a;
            public long s;
            public Part(string part) {
                var partPts = part.Substring(1, part.Length-2).Split(",");
                x = long.Parse(partPts[0].Split("=")[1]);
                m = long.Parse(partPts[1].Split("=")[1]);
                a = long.Parse(partPts[2].Split("=")[1]);
                s = long.Parse(partPts[3].Split("=")[1]);
            }

            public long TotalRating {get {return x + m + a + s;}}

            public bool MeetsRule(Rule r) {
                if(r.category == Cat.None)
                    return true;
                var src = r.category switch {
                    Cat.X => this.x,
                    Cat.M => this.m,
                    Cat.A => this.a,
                    Cat.S => this.s,
                    _ => throw new Exception("uh oh")
                };
                if (r.opIsGreater) {
                    return src > r.target;
                } else {
                    return src < r.target;
                }
            }
        }

        
        public override string P2()
        {
            var lines = InputAsLines();
            return $"{0}";
        }
    }
}