using NUnit.Framework;
using RickAndMorty;

namespace Tests;

public class BaseTest
{
    protected ApiClient ApiClient;

    [SetUp]
    public void SetUp()
    {
        ApiClient = new ApiClient(Endpoints.Character);
    }
}