using System.Configuration;
using RestSharp;

namespace RickAndMorty;

public class ApiClient(string endpoint, string baseUrl = "https://rickandmortyapi.com/api")
{
    private readonly RestClient _client = new(baseUrl + endpoint);

    public async Task<RestResponse> GetAsync(string resource = null!)
    {
        var request = new RestRequest(resource);
        return await _client.ExecuteAsync(request);
    }

    public string BuildQueryParameter(string key, string value)
    {
        return $"{key}={Uri.EscapeDataString(value)}";
    }
}