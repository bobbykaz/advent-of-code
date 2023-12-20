using System.Diagnostics.Contracts;

namespace y23 {
    public class D20 : AoCDay
    {
        public D20(): base(23, 20) {
            _DebugPrinting = true;
        }
        
        public override string P1()
        {
            var lines = InputAsLines();
            var mods = new List<Module>();
            mods = lines.Select(l => new Module(l)).ToList();
            var modMap = new Dictionary<string, Module>();
            foreach(var m in mods) {
                modMap[m.Name] = m;
            }
            foreach(var m in mods) {
                foreach(var d in m.Dests) {
                    if(!modMap.ContainsKey(d)) {
                        modMap[d] = new Module(d, MK.Sink);
                    }
                    var md = modMap[d];
                    md.AddSrc(m.Name);
                }
            }
            long lows = 0L;
            long highs = 0L;
            var pq = new Queue<Pulse>();
            for( int i = 0; i < 1000; i++) {
                var initP = new Pulse(false, "button", "broadcaster");
                pq.Enqueue(initP);
                while(pq.Any()) {
                    var p = pq.Dequeue();
                    if(p.IsHigh) {highs++;} else {lows++;}
                    var targetM = modMap[p.To];
                    var newPulses = targetM.RecPulse(p);
                    foreach(var np in newPulses){
                        pq.Enqueue(np);
                    }
                }
                PrintLn($"Button {i} complete");
            }

           
            return $"{lows*highs}";
        }

        public enum MK {
            Broadcaster,
            FlipFlop,
            Conjunc,
            Sink
        }
        public class Module {
            public List<string> Sources;
            public List<string> Dests;
            public string Name;
            public MK mK;

            bool FFOn = false;
            Dictionary<string,bool> ConState = new Dictionary<string, bool>();

            public Module(string line) {
                Sources = new List<string>();
                var pts = line.Split(" -> ");
                if (pts[0] == "broadcaster") {
                    mK = MK.Broadcaster;
                    Name = "broadcaster";
                } else {
                    if(pts[0].Contains("%")) {
                        mK = MK.FlipFlop;
                    } else {
                        mK = MK.Conjunc;
                    }
                    Name = pts[0].Substring(1);
                }

                if (pts[1].Contains(", ")) {
                    Dests = pts[1].Split(", ").ToList();
                } else {
                    Dests = new string[]{pts[1]}.ToList();
                }
            }

            public Module(string name, MK mk) {
                Name = name;
                this.mK = mk;
                Sources = new List<string>();
                Dests = new List<string>();
            }

            public void AddSrc(string name) {
                Sources.Add(name);
                ConState[name] = false;
            }

            public List<Pulse> RecPulse(Pulse p, long buttons = 0) {
                if (mK == MK.Conjunc) {
                    if(Name == "kh") {
                        var old = ConState[p.From];
                        if(old != p.IsHigh) {
                            Console.Write("KH: ");
                            foreach(var k in ConState.Keys){
                                var state = ConState[k] ? 1 : 0;
                                 Console.Write($"{state}");
                            }
                            Console.Write($" ; Buttons {buttons}\n");
                        }
                    }
                    ConState[p.From] = p.IsHigh;
                    var outP = !ConState.Values.All(b => b); // all true -> low
                    return Dests.Select(d => new Pulse(outP, Name, d)).ToList();
                } else if (mK == MK.FlipFlop) {
                    if(!p.IsHigh) {
                        FFOn = !FFOn;
                        return Dests.Select(d => new Pulse(FFOn, Name, d)).ToList();
                    }
                    return new List<Pulse>();
                } else if (mK == MK.Broadcaster) {
                    return Dests.Select(d => new Pulse(false, Name, d)).ToList();
                } else {
                    return new List<Pulse>();
                }
            }
        }

        public class Pulse {
            public bool IsHigh;
            public string From;
            public string To;
            public Pulse(bool isHigh, string f, string t) {
                IsHigh = isHigh; From = f; To = t;
            }
        }

        /*
        It doesnt end, but from logs you can figure out the cycle time of the important parts of RX (the kh node for me)
        KH: 0100 ; Buttons 3761
        KH: 0100 ; Buttons 7522

        KH: 0010 ; Buttons 3931
        KH: 0010 ; Buttons 7862

        KH: 1000 ; Buttons 4049
        KH: 1000 ; Buttons 8098

        KH: 0001 ; Buttons 4079
        KH: 0001 ; Buttons 8158
        */
        public override string P2()
        {
            var lines = InputAsLines();
            var mods = new List<Module>();
            mods = lines.Select(l => new Module(l)).ToList();
            var modMap = new Dictionary<string, Module>();
            foreach(var m in mods) {
                modMap[m.Name] = m;
            }
            foreach(var m in mods) {
                foreach(var d in m.Dests) {
                    if(!modMap.ContainsKey(d)) {
                        modMap[d] = new Module(d, MK.Sink);
                    }
                    var md = modMap[d];
                    md.AddSrc(m.Name);
                }
            }
            long lows = 0L;
            long highs = 0L;
            var pq = new Queue<Pulse>();
            long buttons = 1;
            while(true) {
                var initP = new Pulse(false, "button", "broadcaster");
                pq.Enqueue(initP);
                while(pq.Any()) {
                    var p = pq.Dequeue();
                    if(p.To == "rx" && !p.IsHigh) {
                        PrintLn($"Exiting after {buttons}");
                        return $"{buttons}";
                    }
                    if(p.IsHigh) {highs++;} else {lows++;}
                    var targetM = modMap[p.To];
                    var newPulses = targetM.RecPulse(p, buttons);
                    foreach(var np in newPulses){
                        pq.Enqueue(np);
                    }
                }
                buttons++;
            }
        }

    }
}
