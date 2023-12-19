using System.ComponentModel;
using System.Runtime.CompilerServices;
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
            var blocks = Utilties.GroupInputAsBlocks(lines);
            var wfs = new List<Workflow>();
            var wfMap = new Dictionary<string,Workflow>();
            foreach(var line in blocks[0]){
                var wf = parseWf(line);
                wfs.Add(wf);
                wfMap[wf.ID] = wf;
            }

            var p = new PartRange();
            PrintLn(p.ToString());
            var combos = AcceptParts(wfMap, "in", p);
            return $"{combos}";
        }

        public class Range {
            public long l;
            public long r;
            public Range(long ll, long rr) {
                l = ll;
                r = rr;
            }
            public bool Contains(long i) {
                return l <= i && i <= r;
            }
            public (Range, Range) SplitRightInclude(long i) {
                var all = new long[]{l,i,r}.ToList();
                all.Sort();
                return (new Range(all[0],all[1]-1), new Range(all[1],all[2]));
            }

            public (Range, Range) SplitLeftInclude(long i) {
                var all = new long[]{l,i,r}.ToList();
                all.Sort();
                return (new Range(all[0],all[1]), new Range(all[1]+1,all[2]));
            }

            public Range Copy() {
                return new Range(l,r);
            }

            public override string ToString()
            {
                return $"[{l}-{r})";
            }
            public long Length {get {return r-l+1;}}
        }

        public class PartRange {
            public Range X;
            public Range M;
            public Range A;
            public Range S;
            public PartRange(){
                X = new Range(1,4000);
                M = new Range(1,4000);
                A = new Range(1,4000);
                S = new Range(1,4000);
            }
            public PartRange Copy() {
                return new PartRange {
                    X = this.X.Copy(),
                    M = this.M.Copy(),
                    A = this.A.Copy(),
                    S = this.S.Copy()
                };
            }

            public Range Get(Cat cat) {
                return cat switch {
                    Cat.X => X,
                    Cat.M => M,
                    Cat.A => A,
                    Cat.S => S,
                    _ => throw new Exception()
                };
            }
            public void Set(Cat cat, Range r) {
                switch(cat) {
                    case Cat.X:
                        X = r;
                        break;
                    case Cat.M:
                        M = r;
                        break;
                    case Cat.A:
                        A = r;
                        break;
                    case Cat.S:
                        S = r;
                        break;
                    default: throw new Exception();
                }
            }

            public override string ToString()
            {
                return $"X:{X} M:{M} A:{A} S:{S} ;; Combos{CombosRemain}";
            }
            public long CombosRemain {get {return X.Length*M.Length*A.Length*S.Length;}}
        }

        public long AcceptParts(Dictionary<string,Workflow> wfmap, string dest, PartRange? p) {
            if(p == null) return 0L;
            var total = 0L;

            if(dest == "A") return p.CombosRemain;
            if(dest == "R") return 0;
            var wf = wfmap[dest];
            PartRange? current = p;
            foreach(var r in wf.Rules) {
                if(current == null) return total;
                var (pass,fail) = EvalRuleRange(r, current);
                total += AcceptParts(wfmap, r.dest, pass);
                current = fail;
            }
            return total;
        }

        public (PartRange?, PartRange?) EvalRuleRange(Rule r, PartRange p) {
            if(r.category == Cat.None) {return (p,null);}
            if(r.opIsGreater) {
                if (p.Get(r.category).Contains(r.target)) {
                    PrintLn($"gt than; {p}; target {r.target}; {r.category}");
                    //is p.X > r.target? x == target fails
                    var (fail, pass) = p.Get(r.category).SplitLeftInclude(r.target);
                    var pp = p.Copy();
                    var fp = p.Copy();
                    pp.Set(r.category, pass);
                    fp.Set(r.category, fail);
                    return (pp,fp);
                }
                if(p.Get(r.category).l > r.target) {
                    return (p,null);
                } else {
                    return (null, p);
                }
            } else {
                if (p.Get(r.category).Contains(r.target)) {
                    PrintLn($"less than; {p}; target {r.target}; {r.category}");
                    //is p.X < r.target? x == target fails
                    var (pass, fail) = p.Get(r.category).SplitRightInclude(r.target);
                    var pp = p.Copy();
                    var fp = p.Copy();
                    pp.Set(r.category, pass);
                    fp.Set(r.category, fail);
                    PrintLn(pass.ToString());
                    PrintLn(fail.ToString());
                    return (pp,fp);
                }
                if(p.Get(r.category).l < r.target) {
                    return (p,null);
                } else {
                    return (null, p);
                }
            }
        }
    }
}
