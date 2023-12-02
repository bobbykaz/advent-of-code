using System.Net.Http.Headers;

public class InputLoader {
    const String BaseUrl = "https://adventofcode.com";
    protected HttpClient Client;
    protected string AocAuth;

    public InputLoader(string aocCookie) {
        AocAuth = aocCookie;
        Client = new HttpClient() { BaseAddress = new Uri(BaseUrl)};
        Client.DefaultRequestHeaders.Add("User-Agent","github.com/bobbykaz/advent-of-code/tree/master/dotnet");
    }
}