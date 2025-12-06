internal class Program
{
    private static async Task Main(string[] args)
    {
        var token = System.Environment.GetEnvironmentVariable("AOC_TOKEN");
        var inputRoot = "../input";
        InputLoader.Init(token, inputRoot);

        await new y25.D6().Run();
    }
}