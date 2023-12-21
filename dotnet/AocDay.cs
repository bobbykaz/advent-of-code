using System.Diagnostics;

public abstract class AoCDay {
    private int _year;
    private int _day;
    protected bool _DebugPrinting { get; set; }
    protected List<string> _InputAsLines;

    protected long _ElapsedMillis {get { return _Timer.ElapsedMilliseconds; } }

    private Stopwatch _Timer;

    protected InputLoader _InputLoader;
    public AoCDay(int year, int day) {
        _year = year;
        _day = day;
        _DebugPrinting = false;
        _InputLoader = new InputLoader();
        _InputAsLines = new List<string>();
        _Timer = new Stopwatch();
    }

    public async Task Run(){
            _InputAsLines = await _InputLoader.GetInput(_year, _day);
            _Timer.Restart();
            var p1 = P1();
            _Timer.Stop();
            Console.WriteLine($"20{_year} Day {_day} P1: {p1}");
            Console.WriteLine($"... {_Timer.ElapsedMilliseconds}ms");
            _Timer.Restart();
            var p2 = P2();
            _Timer.Stop();
            Console.WriteLine($"20{_year} Day {_day} P2: {p2}");
            Console.WriteLine($"... {_Timer.ElapsedMilliseconds}ms");
        }


    public abstract string P1();

    public abstract string P2();

    public List<string> InputAsLines() {
        return _InputAsLines.ToList();
    }

    public void PrintLn(string str) {
        if(_DebugPrinting)
            Console.WriteLine(str);
    }

    public void Print(string str) {
        if(_DebugPrinting)
            Console.Write(str);
    }
}