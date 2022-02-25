namespace TopSwagCode.Blazor.Client
{
    public class LocalHttpClient
    {
        public HttpClient HttpClient { get; set; }

        public LocalHttpClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }
    }

    public class WasmVersionService : IVersionService
    {
        public string Version { get; set; } = null!;

        public string GetVersion() => Version;
    }

    public interface IVersionService
    {
        string GetVersion();
    }
}
