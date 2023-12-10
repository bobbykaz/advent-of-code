namespace y23 {
    public class D6 : AoCDay
    {
        public D6(): base(23, 6) {
            _DebugPrinting = true;
        }
        public override string P1()
        {
            List<int> times = new List<int>{47, 70, 75, 66};
            List<int> dist = new List<int>{282, 1079, 1147, 1062};

            var a = bfRace(times[0], dist[0]);
            var b = bfRace(times[1], dist[1]);
            var c = bfRace(times[2], dist[2]);
            var d = bfRace(times[3], dist[3]);
            var total = a * b * c * d;
            return $"{total}";
        }

        public int bfRace(int time, int dist) {
            int recordBeats = 0;
            for(int i = 0; i < time; i++) {
                if(i *(time-i) > dist)
                    recordBeats++;
            }
            return recordBeats;
        }

        public long bfRaceL(long time, long dist) {
            long recordBeats = 0;
            for(long i = 0; i < time; i++) {
                if(i *(time-i) > dist)
                    recordBeats++;
                if(i % 1000000 == 0)
                    PrintLn($"{i}");
            }
            return recordBeats;
        }

        public override string P2()
        {
            var lines = InputAsLines();

            var a = bfRaceL(47707566, 282107911471062);
        
            return $"{a}";
        }
    }
}