namespace y23 {
    public class D12 : AoCDay
    {
        public D12(): base(23, 12) {
            _DebugPrinting = false;
        }

        public override string P1()
        {
            var lines = InputAsLines();
            var totalPerms = 0;
            foreach(var line in lines) {
                var sm = new SM(line);
                int perms = Permute(sm.Springs, sm.Conditions);
                totalPerms += perms;
            }
            
            return $"{totalPerms}";
        }

        public class SM {
            public string Springs;
            public List<int> Conditions;
            public SM(string line) {
                var pts = line.Split(" ");
                Springs = pts[0];
                Conditions = Utilties.StringToNums<int>(pts[1]);
            }
        }

        public int Permute(string spr, List<int> conditions) {
            var qs = spr.Count(c => c == '?');
            if(qs == 0){ 
                if(Valid(spr, conditions)) 
                    return 1;  
                else
                    return 0;
            }

            var i = spr.IndexOf('?');

            var sl = spr.ToCharArray();
            sl[i] = '.';
            var sr = spr.ToCharArray();
            sr[i] = '#';
            return Permute(new string(sl), conditions) + Permute(new string(sr), conditions);

        }

        public bool Valid(string spr, List<int> conditions) {
            if (spr.Contains('?')) 
                return false;

            var s = spr.Trim('.');
            for(int i = 0; i < conditions.Count; i++) {
                var num = conditions[i];
                var nextDot = s.IndexOf('.');
                if(nextDot == -1 && i == conditions.Count -1) 
                    return s.Length == num;

                if(nextDot != num) 
                    return false;
                s = s[nextDot..];
                s = s.Trim('.');
            }
            return false;
        }

        public class SM2 {
            public string Springs;
            public List<int> Conditions;
            public SM2(string line) {
                var pts = line.Split(" ");
                //add a dot to the end to help with counting completed runs below
                Springs = $"{pts[0]}?{pts[0]}?{pts[0]}?{pts[0]}?{pts[0]}.";
                var conStr = $"{pts[1]},{pts[1]},{pts[1]},{pts[1]},{pts[1]}";
                Conditions = Utilties.StringToNums<int>(conStr);
            }
        }

        private Dictionary<string, long> Memos = new Dictionary<string, long>();
        public override string P2()
        {
            var lines = InputAsLines();
            var totalPerms = 0L;
            int c = 1;
            foreach(var line in lines) {
                var sm = new SM2(line);
                Memos = new Dictionary<string, long>();
                long perms = dynRecurse(sm.Springs,sm.Conditions,0,0,0);
                PrintLn($"{sm.Springs} - {line} - {perms}");
                totalPerms += perms;
                c++;
            }
            
            return $"{totalPerms}";
        }
        public long dynRecurse(string spr, List<int> conditions, 
                                int i, int curRun, int completedRuns) {

            string key = $"{i}-{curRun}-{completedRuns}";

            if(Memos.ContainsKey(key))
                return Memos[key];

            if(i == spr.Length) {
                var rslt = 0;
                if(conditions.Count == completedRuns)
                    rslt = 1;
                Memos[key] = rslt;
                return rslt;
            } else if (spr[i] == '#') {
                var rslt = dynRecurse(spr, conditions, i+1, curRun+1, completedRuns);
                Memos[key] = rslt;
                return rslt;
            } else if(spr[i] == '.' || completedRuns == conditions.Count) {
                var rslt = 0L;//midrun and stopped short of target, fail the run
                if (completedRuns < conditions.Count && curRun == conditions[completedRuns]) {
                    //just finished a group
                    rslt = dynRecurse(spr, conditions, i+1, 0, completedRuns+1);
                } else if (curRun == 0) {
                    // no new group yet, keep on moving
                    rslt = dynRecurse(spr, conditions, i+1, 0, completedRuns);
                }
                Memos[key] = rslt;
                return rslt;
            } else { // must be '?'
                var nextIsHash = dynRecurse(spr, conditions, i+1, curRun + 1, completedRuns);
                var nextIsDot = 0L; // midrun and stopped short of target, fail the run
                if (curRun == conditions[completedRuns]) {
                    //complete the run
                    nextIsDot = dynRecurse(spr, conditions, i+1, 0, completedRuns+1);
                } else if (curRun == 0) {
                    nextIsDot = dynRecurse(spr, conditions, i+1, 0, completedRuns);
                }
                Memos[key] = nextIsDot + nextIsHash;
                return nextIsDot + nextIsHash;
            }
        }
    }
}