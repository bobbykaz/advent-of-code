using Grids;

namespace y24 {
    public class D21: AoCDay {

        public D21(): base(24, 21) {
            _DebugPrinting = true;
        }
        public override string P1()
        {
            var lines = InputAsLines();
            var total = 0L;
            foreach(var code in lines){
                PrintLn($"{code}");
                var np = new Numpad();
                var dirPads = new List<DirPad>();
                dirPads.Add(new DirPad());
                dirPads.Add(new DirPad());
                var result = "";
                foreach(var c in code.ToCharArray()) {
                    var numPadPaths = np.GetPossiblePaths(c);

                    var best = "";
                    var currentDpPos = dirPads[0].Ptr;
                    foreach(var option in numPadPaths) {
                        
                    }

                    for(int i = 0; i < dirPads.Count(); i++) {
                        next = dirPads[i].DoPathForCode(next);
                        PrintLn($"  {i}  {next}");
                    }
                    result += next;
                }
                
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
            public Grid<char> G;
            public Pos Ptr;

            public List<string> GetPossiblePaths(char t) { 
                var rowFirst = RowFirstPathToTarget(t);
                var colFirst = ColFirstPathToTarget(t);
                var rslt = new List<string>();
                if(PathIsSafe(rowFirst, Ptr)) rslt.Add(rowFirst);
                if(PathIsSafe(colFirst, Ptr)) rslt.Add(colFirst);
                Ptr = FindTarget(t);
                return rslt;
            }

            public bool PathIsSafe(string path, Pos start) {
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
            public string RowFirstPathToTarget(char t){
                var result = new List<char>();
                var end = FindTarget(t);
                var rDiff = Ptr.R - end.R;
                if(rDiff < 0) {
                    for(int i = 0; i < Math.Abs(rDiff); i++) {
                        result.Add('v');
                    }
                }else if(rDiff > 0) {
                    for(int i = 0; i < Math.Abs(rDiff); i++) {
                        result.Add('^');
                    }
                }

                var cDiff = Ptr.C - end.C;
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
            public string ColFirstPathToTarget(char t){
                var result = new List<char>();
                var end = FindTarget(t);

                var cDiff = Ptr.C - end.C;
                if(cDiff < 0) {
                    for(int i = 0; i < Math.Abs(cDiff); i++) {
                        result.Add('>');
                    }
                }else if(cDiff > 0) {
                    for(int i = 0; i < Math.Abs(cDiff); i++) {
                        result.Add('<');
                    }
                }

                var rDiff = Ptr.R - end.R;
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
                var lines = new List<string>() {"789", "456", "123", "#0A"};
                G = Utilties.RectangularCharGridFromLines(lines);
                Ptr = new Pos(3,2);
            }
        }

        private record struct Memo(Pos Start, string Path);
        private record struct MemoResult(Pos End, string Result);

        private class DirPad: Pad {
            public static Dictionary<Memo, MemoResult> Cache = [];
            public DirPad() {
                var lines = new List<string>() {"#^A", "<v>"};
                G = Utilties.RectangularCharGridFromLines(lines);
                Ptr = new Pos(0,2);
            }

            private string OptimizeFullPath(string path) {
                var pathsSplitOnA = SubPaths(path);
                var result = "";
                var cacheKey = new Memo(Ptr, path);
                if(Cache.ContainsKey(cacheKey)) {
                    var mr = Cache[cacheKey];
                    Ptr = mr.End;
                    return mr.Result;
                }

                foreach(var p in pathsSplitOnA){
                    result += OptimizeSinglePressPath(p);
                }

                var end = Ptr;
                Cache[cacheKey] = new MemoResult(end, result);

                return result;
            }

            private string OptimizeSinglePressPath(string path) {
                var current = Ptr;
                
                var result = "";
                var cacheKey = new Memo(Ptr, path);
                if(Cache.ContainsKey(cacheKey)) {
                    var mr = Cache[cacheKey];
                    Ptr = mr.End;
                    return mr.Result;
                }

                // Try current version of Path
                // Try Row/Col reversed version of Path

                var end = Ptr;
                Cache[cacheKey] = new MemoResult(end, result);

                throw new Exception();
            }

            private List<string> SubPaths(string path) {
                List<string> rslt = [];
                var next = "";
                foreach( var c in path.ToCharArray()) {
                    if(c!= 'A') {
                        next += c;
                    } else {
                        next += c;
                        rslt.Add(next);
                        next = "";
                    }
                }
                if(next != "") {
                    throw new Exception($"part of path ends without A!\n full: {path}\n end: {next}");
                }
                return rslt;
            }

            private string ReverseRowColSinglePressPath(string path) {
                if (path.Count(c => c == 'A') != 1) {
                    throw new Exception($"Encountered more than 1 A in single press reversal \n {path}");
                }

                if (path[path.Length - 1] != 'A') {
                    throw new Exception($"last char is not A \n {path}");
                }
                

            }
        }

        

        public override string P2()
        {
            var lines = InputAsLines();
            var total = 0L;
           
            return $"{total}";
        }

    }
}