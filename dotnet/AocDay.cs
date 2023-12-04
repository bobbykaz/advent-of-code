public abstract class AoCDay {
    private int _year;
    private int _day;
    protected bool _DebugPrinting { get; set; }
    public AoCDay(int year, int day) {
        _year = year;
        _day = day;
        _DebugPrinting = false;
    }

    public void Run(){
            var p1 = P1();
            Console.WriteLine($"20{_year} Day {_day} P1: {p1}");
            var p2 = P2();
            Console.WriteLine($"20{_year} Day {_day} P2: {p2}");
        }

    public List<string> InputAsLines(string? filename = null) {
        var file = filename ?? $"../input/y{_year}/d{_day}.txt";
        return Utilties.ReadFileToLines(file);
    }


    public abstract string P1();

    public abstract string P2();

    public void PrintLn(string str) {
        if(_DebugPrinting)
            Console.WriteLine(str);
    }

    public void Print(string str) {
        if(_DebugPrinting)
            Console.Write(str);
    }
}