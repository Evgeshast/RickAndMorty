using Allure.NUnit;
using Allure.NUnit.Attributes;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RickAndMorty;

namespace Tests;

[TestFixture]
[AllureNUnit]
public class CharacterTest
{
    private ApiClient _apiClient;

    [SetUp]
    public void Setup()
    {
        _apiClient = new ApiClient("https://rickandmortyapi.com/api");
    }

    [Test]
    [AllureTag("API", "Regression")]
    [AllureSeverity]
    [AllureFeature("Character")]
    public async Task GetAllCharacters_ShouldReturnSuccess()
    {
        var response = await _apiClient.GetAsync("/character");
        Assert.That((int)response.StatusCode, Is.EqualTo(200));

        var data = JObject.Parse(response.Content!);

        data["results"].Should().NotBeNull();
    }

    [Test]
    public async Task FilterCharactersByName_ShouldReturnCorrectResults()
    {
        var response = await _apiClient.GetAsync("/character/?name=rick");
        Assert.That((int)response.StatusCode, Is.EqualTo(200));

        var data = JObject.Parse(response.Content);
        foreach (var character in data["results"])
        {
            character["name"].ToString().Should().Contain("Rick");
        }
    }
}