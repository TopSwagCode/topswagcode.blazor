namespace TopSwagCode.Blazor.Client
{
    public class ServerlessHttpClient
    {
        private readonly HttpClient _httpClient;

        public ServerlessHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<string> GetUserInfoAsync()
        {
            return _httpClient.GetStringAsync("userfunction");
        }
    }
}
