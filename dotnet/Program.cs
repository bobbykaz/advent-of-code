internal class Program
{
    private static async Task Main(string[] args)
    {
        new y23.D5().Run();
    }

    private static async Task InputTest() {
        var inputRoot = System.Environment.GetEnvironmentVariable("INPUT_ROOT");
        if(inputRoot == null) inputRoot = "../input";
        var token = System.Environment.GetEnvironmentVariable("AOC_TOKEN");

        Console.WriteLine($"Token: {token}");
        Console.WriteLine($"Input Root: {inputRoot}");
        InputLoader.Init(token, inputRoot);
        var il = new InputLoader();

        var input = await il.FetchInput(23, 4);
        Console.WriteLine(input);
    }
}