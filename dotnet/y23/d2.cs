namespace y23 {
    public class D2 {
        public void Run(){
            P1();
            P2();
        }
        public void P1() {
            var lines = Utilties.ReadFileToLines("../input/y23/d2.txt");
            var pGames = new List<int>();
            foreach (var line in lines) {
                var header = line.Split(": ");
                var gameId = int.Parse(header[0].Split(" ")[1]);
                Console.WriteLine(gameId);

                var games = header[1].Split("; ");
                bool possible = true;
                foreach(var g in games) {
                    var draws = g.Split(", ");
                    foreach(var d in draws){
                        var pts = d.Split(" ");
                        var num = int.Parse(pts[0]);
                        ///12 red cubes, 13 green cubes, and 14 blue cubes
                        switch(pts[1]){
                            case "red":
                                if (num > 12) {possible = false;}
                                break;
                            case "blue":
                                if (num > 14) {possible = false;}
                                break;
                            case "green":
                                if (num > 13) {possible = false;}
                                break;
                        }
                    }

                    if( !possible) {break;}
                }
                if(possible) {
                    pGames.Add(gameId);
                }
            }
            Console.WriteLine(pGames.Sum());
        }

        public void P2() {
            var lines = Utilties.ReadFileToLines("../input/y23/d2.txt");
            //rgb
            var pGames = new List<int>();
            foreach (var line in lines) {
                var header = line.Split(": ");
                var gameId = int.Parse(header[0].Split(" ")[1]);
                Console.WriteLine(gameId);

                var games = header[1].Split("; ");
                var mr = 0;
                var mb = 0;
                var mg = 0;
                foreach(var g in games) {
                    var draws = g.Split(", ");
                    foreach(var d in draws){
                        var pts = d.Split(" ");
                        var num = int.Parse(pts[0]);
                        ///12 red cubes, 13 green cubes, and 14 blue cubes
                        switch(pts[1]){
                            case "red":
                                if (num > mr) {mr = num;}
                                break;
                            case "blue":
                                if (num > mb) {mb = num;}
                                break;
                            case "green":
                                if (num > mg) {mg = num;}
                                break;
                        }
                    }
                }
                pGames.Add(mr * mb * mg);
            }
            Console.WriteLine(pGames.Sum());
        }
    }
}