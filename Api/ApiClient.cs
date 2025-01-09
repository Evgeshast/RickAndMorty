using RestSharp;

namespace RickAndMorty;

public class ApiClient
{
    private readonly RestClient _client;

    public ApiClient(string baseUrl)
    {
        _client = new RestClient(baseUrl);
    }

    public async Task<RestResponse> GetAsync(string endpoint)
    {
        var request = new RestRequest(endpoint, Method.Get);
        return await _client.ExecuteAsync(request);
    }
    
    public string BuildQueryParameter(string key, string value)
    {
        return $"{key}={Uri.EscapeDataString(value)}";
    }
}