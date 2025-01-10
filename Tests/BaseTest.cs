using NUnit.Framework;
using RickAndMorty;

namespace Tests;

public class BaseTest
{
    protected ApiClient _apiClient;

    [SetUp]
    public void SetUp()
    {
        _apiClient = new ApiClient(Endpoints.Character);
    }
}