
using System.Text.RegularExpressions;

public class D6 {
    public void Run(){
        P1();
        P2();
    }
    public void P1() {
        var lines = Utilties.ReadFileToLines("../input/y15/d6.txt");
        var grid = Utilties.NGrid(1000,1000,0);
        var rslt = parse(lines[0]);
        foreach(string l in lines) {
            var parsed = parse(l);
            for( int x = parsed.Item2.fromX; x <= parsed.Item2.toX; x++) {
                for( int y = parsed.Item2.fromY; y <= parsed.Item2.toY; y++) {
                    switch(parsed.Item1) {
                        case Cmd.Off:
                            grid.G[x][y] = 0;
                            break;
                        case Cmd.On:
                            grid.G[x][y] = 1;
                            break;
                        case Cmd.Toggle:
                            grid.G[x][y] = (grid.G[x][y] == 0 ? 1 : 0);
                            break;
                    }
                }
            }
        }

        int c = 0;

        for(int x = 0; x < grid.Height; x++) {
            for(int y = 0; y < grid.Width; y++) {
                c += grid.G[x][y];
            }
        }

        Console.WriteLine($"Total lit: {c}");
    }

    public void P2() {
        var lines = Utilties.ReadFileToLines("../input/y15/d6.txt");
        var grid = Utilties.NGrid(1000,1000,0);
        var rslt = parse(lines[0]);
        foreach(string l in lines) {
            var parsed = parse(l);
            for( int x = parsed.Item2.fromX; x <= parsed.Item2.toX; x++) {
                for( int y = parsed.Item2.fromY; y <= parsed.Item2.toY; y++) {
                    switch(parsed.Item1) {
                        case Cmd.Off:
                            grid.G[x][y] -= 1;
                            if (grid.G[x][y] < 0) {
                                grid.G[x][y] = 0;
                            }
                            break;
                        case Cmd.On:
                            grid.G[x][y] += 1;
                            break;
                        case Cmd.Toggle:
                            grid.G[x][y] += 2;
                            break;
                    }
                }
            }
        }

        int c = 0;

        for(int x = 0; x < grid.Height; x++) {
            for(int y = 0; y < grid.Width; y++) {
                c += grid.G[x][y];
            }
        }

        Console.WriteLine($"PArt 2: Total lit: {c}");
    }

    (Cmd, Rect) parse(string line) {
        Regex regex = new Regex("([a-z\\s]+) (\\d+),(\\d+) through (\\d+),(\\d+)");
    
        Match match = regex.Match(line);
        //DisplayMatchResults(match);
        if(match.Groups.Count == 6) {
            var cmd = Cmd.Off;
            switch(match.Groups[1].Captures[0].Value) {
                case "toggle":
                    cmd = Cmd.Toggle;
                    break;
                case "turn off":
                    cmd = Cmd.Off;
                    break;
                case "turn on":
                    cmd = Cmd.On;
                    break;
                default:
                    DisplayMatchResults(match);
                    throw new Exception($"wrong match results: {line}");

            }

            var rect = new Rect(
                int.Parse(match.Groups[2].Captures[0].Value),
                int.Parse(match.Groups[3].Captures[0].Value),
                int.Parse(match.Groups[4].Captures[0].Value),
                int.Parse(match.Groups[5].Captures[0].Value)
            );

            return (cmd,rect);
        } else {
            DisplayMatchResults(match);
            throw new Exception($"unexpecte number of groups: {match.Groups.Count} in line {line}");
        }
        
    }

    public static void DisplayMatchResults(Match match)
{
    Console.WriteLine("Match has {0} captures", match.Captures.Count);

    int groupNo = 0;
    foreach (Group mm in match.Groups)
    {
        Console.WriteLine("  Group {0,2} has {1,2} captures '{2}'", groupNo, mm.Captures.Count, mm.Value);

        int captureNo = 0;
        foreach (Capture cc in mm.Captures)
        {
            Console.WriteLine("       Capture {0,2} '{1}'", captureNo, cc);
            captureNo++;
        }
        groupNo++;
    }

    groupNo = 0;
    foreach (Group mm in match.Groups)
    {
        Console.WriteLine("    match.Groups[{0}].Value == \"{1}\"", groupNo, match.Groups[groupNo].Value); //**
        groupNo++;
    }

    groupNo = 0;
    foreach (Group mm in match.Groups)
    {
        int captureNo = 0;
        foreach (Capture cc in mm.Captures)
        {
            Console.WriteLine("    match.Groups[{0}].Captures[{1}].Value == \"{2}\"", groupNo, captureNo, match.Groups[groupNo].Captures[captureNo].Value); //**
            captureNo++;
        }
        groupNo++;
    }
}

    public struct Rect {
        public int fromX;
        public int fromY;
        public int toX;
        public int toY;

        public Rect(int a, int b, int c, int d) {
            fromX = a;
            fromY = b;
            toX = c;
            toY = d;
        }
    }
    public enum Cmd {
        Toggle,
        Off,
        On
    }
}