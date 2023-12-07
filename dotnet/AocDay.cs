public abstract class AoCDay {
    private int _year;
    private int _day;
    protected bool _DebugPrinting { get; set; }
    protected List<string> _InputAsLines;

    protected InputLoader _InputLoader;
    public AoCDay(int year, int day) {
        _year = year;
        _day = day;
        _DebugPrinting = false;
        _InputLoader = new InputLoader();
        _InputAsLines = new List<string>();
    }

    public async Task Run(){
            _InputAsLines = await _InputLoader.GetInput(_year, _day);
            var p1 = P1();
            Console.WriteLine($"20{_year} Day {_day} P1: {p1}");
            var p2 = P2();
            Console.WriteLine($"20{_year} Day {_day} P2: {p2}");
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