using System.Net.Http.Headers;
using System.Runtime.InteropServices;

public class InputLoader {
    const String BaseUrl = "https://adventofcode.com";
    protected HttpClient Client;
    private static string? AocAuth;
    private static string? InputRoot;

    public static void Init(string token, string inputRoot) {
        AocAuth = token;
        InputRoot = inputRoot;
    }

    public InputLoader() {
        Client = new HttpClient() { BaseAddress = new Uri(BaseUrl)};
        Client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent","github.com/bobbykaz/advent-of-code/tree/master/dotnet");
        Client.DefaultRequestHeaders.Add("Cookie", $"session={AocAuth}");
    }

    public async Task<String> FetchInput(int year, int day) {
        return await Client.GetStringAsync($"20{year}/day/{day}/input");
    }

    public async Task<List<string>> GetInput(int year, int day) {
        var filename = getInputFilePath(year, day);
        if(!System.IO.File.Exists(filename)) {
            Directory.CreateDirectory(getInputFileDir(year));
            var input = await FetchInput(year, day);
            await File.WriteAllTextAsync(filename, input);
        }

        return Utilties.ReadFileToLines(filename);
    }

    private string getInputFileDir(int year) {
        return $"{InputRoot}/y{year}/";
    }
    private string getInputFilePath(int year, int day) {
        return $"{InputRoot}/y{year}/d{day}.txt";
    }
}