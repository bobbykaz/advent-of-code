using System.Security.Cryptography;
using Grids;

namespace y24 {
    public class D21: AoCDay {

        public D21(): base(24, 21) {
            _DebugPrinting = false;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var total = 0L;
            foreach(var code in lines){
                PrintLn($"processing {code}");
                var np = new Numpad();
                var d1 = new DirPad("..D1");
                np.NextPad = d1;
                var d2 = new DirPad("....D2");
                d1.NextPad = d2;
                var result = np.PressPath(code);
                PrintLn($"Code {code} result: {result}");
                
                var codeScore = NumValOfCode(code);
                var score = result.Length * codeScore;
                PrintLn($"   {result.Length} * {codeScore} = {score}");
                total += score;
            }
           
            return $"{total}";
        }

        private int NumValOfCode(string code) {
            return int.Parse(code.Substring(0, 3));
        }

        private abstract class Pad {
            public string Name = "";
            public Grid<char> G;
            public Pad? NextPad;
            public Dictionary<Memo, string> Cache = [];
            public string PressPath(string path){
                var result = "";
                // Always starts at A
                var currentChar = 'A';
                foreach(var c in path.ToCharArray()){
                    var nextPath = PressKey(currentChar, c);
                    result += nextPath;
                    currentChar = c;
                }

                return result;
            }
            public string PressKey(char from, char to) {
                if (from == '#' || to == '#') {
                    throw new Exception("Somehow we ended up on the gap key?!?!");
                }

                var cacheKey = new Memo(from, to);
                if(Cache.ContainsKey(cacheKey)) {
                    Console.WriteLine($"{Name} - cached result for {from} => {to} {Cache[cacheKey]}");
                    return Cache[cacheKey];
                }
                var paths = GetPossiblePaths(from, to);
                var results = new List<string>();
                foreach(var p in paths){
                    results.Add(PressPathOnNextPad(p));
                }
                results = results.OrderBy(r => r.Length).ToList();
                foreach(var r in results){
                    Console.WriteLine($"{Name} - possible result {r}");
                }

                var best = results.First();
                Cache[cacheKey] = best;
                return best;
            }

            private string PressPathOnNextPad(string path) {
                if(NextPad != null) {
                    return NextPad.PressPath(path);
                } else {
                    return path;
                }
            }

            public List<string> GetPossiblePaths(char from, char t) { 
                var rowFirst = RowFirstPathToTarget(from, t);
                var colFirst = ColFirstPathToTarget(from, t);
                var rslt = new List<string>();
                if(PathIsSafe(rowFirst, from)) rslt.Add(rowFirst);
                if(PathIsSafe(colFirst, from)) rslt.Add(colFirst);
                return rslt;
            }

            public bool PathIsSafe(string path, char from) {
                var start = FindTarget(from);
                var current = start;
                foreach(var c in path) {
                    var diff = c switch {
                        'v' => new Pos(1,0),
                        '<' => new Pos(0,-1),
                        '>' => new Pos(0,1),
                        '^' => new Pos(-1,0),
                        'A' => new Pos(0,0),
                        _ => throw new Exception()
                    };
                    current += diff;
                    var cell = G.GetCellIfValid(current.R, current.C);
                    if(!cell.HasValue || cell.Value.V == '#') {
                        return false;
                    }
                }
                return true;
            }

            // might end up over illegal gap #, or otherwise may be inefficient
            public string RowFirstPathToTarget(char from, char t){
                var result = new List<char>();
                var start = FindTarget(from);
                var end = FindTarget(t);
                var rDiff = start.R - end.R;
                if(rDiff < 0) {
                    for(int i = 0; i < Math.Abs(rDiff); i++) {
                        result.Add('v');
                    }
                }else if(rDiff > 0) {
                    for(int i = 0; i < Math.Abs(rDiff); i++) {
                        result.Add('^');
                    }
                }

                var cDiff = start.C - end.C;
                if(cDiff < 0) {
                    for(int i = 0; i < Math.Abs(cDiff); i++) {
                        result.Add('>');
                    }
                }else if(cDiff > 0) {
                    for(int i = 0; i < Math.Abs(cDiff); i++) {
                        result.Add('<');
                    }
                }

                result.Add('A');

                return string.Join("", result);
            }

            // might end up over illegal gap #, or otherwise may be inefficient
            public string ColFirstPathToTarget(char from, char t){
                var result = new List<char>();
                var start = FindTarget(from);
                var end = FindTarget(t);

                var cDiff = start.C - end.C;
                if(cDiff < 0) {
                    for(int i = 0; i < Math.Abs(cDiff); i++) {
                        result.Add('>');
                    }
                }else if(cDiff > 0) {
                    for(int i = 0; i < Math.Abs(cDiff); i++) {
                        result.Add('<');
                    }
                }

                var rDiff = start.R - end.R;
                if(rDiff < 0) {
                    for(int i = 0; i < Math.Abs(rDiff); i++) {
                        result.Add('v');
                    }
                }else if(rDiff > 0) {
                    for(int i = 0; i < Math.Abs(rDiff); i++) {
                        result.Add('^');
                    }
                }

                result.Add('A');

                return string.Join("", result);
            }

            private Pos FindTarget(char t) {
                Pos rslt = new Pos(-1,-1);
                bool found = false;
                G.ForEachRowCol((r,c,v) => {
                    if(v == t) {
                        rslt = new Pos(r,c);
                        found = true;
                    }
                });

                if(!found) throw new Exception();
                return rslt;
            }
        }

        private class Numpad: Pad {
            public Numpad() {
                Name = "N0";
                var lines = new List<string>() {"789", "456", "123", "#0A"};
                G = Utilties.RectangularCharGridFromLines(lines);
                //Ptr = new Pos(3,2);
            }
        }

        private record struct Memo(char From, char To);

        private class DirPad: Pad {
            public DirPad(string name) {
                Name = name;
                var lines = new List<string>() {"#^A", "<v>"};
                G = Utilties.RectangularCharGridFromLines(lines);
            }
        }

        

        public override string P2()
        {
            var lines = InputAsLines();
            var total = 0L;
            foreach(var code in lines){
                PrintLn($"processing {code}");
                var np = new Numpad();
                Pad prev = np;
                for (int i = 0; i < 25; i++){
                    var d = new DirPad($"D{i}");
                    prev.NextPad = d;
                    prev = d;
                }
                var result = np.PressPath(code);
                PrintLn($"Code {code} result: {result}");
                
                var codeScore = NumValOfCode(code);
                var score = result.Length * codeScore;
                PrintLn($"   {result.Length} * {codeScore} = {score}");
                total += score;
            }
           
            return $"{total}";
        }

    }
}