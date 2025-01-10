using System.Net;
using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using Common.Utils;
using FluentAssertions;
using NUnit.Framework;
using RickAndMorty.Entities.ApiResponses;

namespace Tests;

    [TestFixture]
    [AllureNUnit]
    public class CharacterApiTests : BaseTest
    {
        
        [Test, AllureTag("API"), AllureSeverity(SeverityLevel.critical)]
        [AllureFeature("Character API")]
        public async Task GetCharacters_WithPagination_ShouldReturnValidResponseAndNavigatePages()
        {
            // Arrange
            const int page = 1;
            var resource = _apiClient.BuildQueryParameter("page", page.ToString());

            // Act
            var response = await _apiClient.GetAsync($"?{resource}");
            var content = SerializationHelper.Deserialize<CharactersResponse>(response.Content!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Info.Should().NotBeNull();
            content.Results.Should().NotBeEmpty();
            content.Info.Next.Should().NotBeNull();
            content.Results.Should().Contain(c => c.Name == "Rick Sanchez");
            content.Results.Should().Contain(c => c.Name == "Morty Smith");
        }

        [Test, AllureTag("API"), AllureSeverity(SeverityLevel.critical)]
        [AllureFeature("Character API")]
        public async Task GetCharacters_WithoutParameters_ShouldReturnValidResponse()
        {
            // Act
            var response = await _apiClient.GetAsync();
            var content = SerializationHelper.Deserialize<CharactersResponse>(response.Content!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Info.Should().NotBeNull();
            content.Results.Should().HaveCountGreaterThan(0);
            content.Info.Next.Should().NotBeNull();
            content.Results.Should().Contain(c => c.Name == "Rick Sanchez");
            content.Results.Should().Contain(c => c.Name == "Morty Smith");
        }

        [Test, AllureTag("API"), AllureSeverity(SeverityLevel.critical)]
        [AllureFeature("Character API")]
        public async Task GetCharacters_WithNameFilter_ShouldReturnFilteredResults()
        {
            // Arrange
            var name = "Rick Sanchez";
            var resource = _apiClient.BuildQueryParameter("name", name);

            // Act
            var response = await _apiClient.GetAsync($"?{resource}");
            var content = SerializationHelper.Deserialize<CharactersResponse>(response.Content!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Results.Should().Contain(c => c.Name == name);
            content.Results.Should().Contain(c => c.Status == "Alive");
            content.Results.Should().Contain(c => c.Species == "Human");
        }

        [Test, AllureTag("API"), AllureSeverity]
        [AllureFeature("Character API")]
        [AllureLink("Empty string for type filter returns characters with not empty type", "https://github.com/allure-issue/Allure-Issue")]
        public async Task GetCharacters_WithMultipleFilters_ShouldReturnFilteredResults()
        {
            // Arrange
            var response = await _apiClient.GetAsync();
            var content = SerializationHelper.Deserialize<CharactersResponse>(response.Content!);
            var expectedCharacter = content.Results[0];
            
            var resource = string.Join("&", 
                _apiClient.BuildQueryParameter("name", expectedCharacter.Name),
                _apiClient.BuildQueryParameter("status", expectedCharacter.Status),
                _apiClient.BuildQueryParameter("species", expectedCharacter.Species),
                _apiClient.BuildQueryParameter("type", expectedCharacter.Type),
                _apiClient.BuildQueryParameter("gender", expectedCharacter.Gender));

            // Act
            response = await _apiClient.GetAsync($"?{resource}");
            content = SerializationHelper.Deserialize<CharactersResponse>(response.Content!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Results.Should().NotBeEmpty().And.HaveCountGreaterOrEqualTo(1);
            content.Results[0].Should().BeEquivalentTo(expectedCharacter);
        }

        [TestCase("status", "Dead")]
        [TestCase("gender", "Female")]
        [TestCase("species", "Alien")]
        [Test, AllureTag("API"), AllureSeverity]
        [AllureFeature("Character API")]
        public async Task GetCharacters_WithDifferentFilters_ShouldReturnFilteredResults(string filter, string value)
        {
            // Arrange
            var resource = _apiClient.BuildQueryParameter(filter, value);

            // Act
            var response = await _apiClient.GetAsync($"?{resource}");
            var content = SerializationHelper.Deserialize<CharactersResponse>(response.Content!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Results.Should().NotBeEmpty()
                .And.OnlyContain(character =>
                    (filter == "status" && character.Status == value) ||
                    (filter == "gender" && character.Gender == value) ||
                    (filter == "species" && character.Species == value)
                );
        }
        
        
        [Test, AllureTag("API"), AllureSeverity(SeverityLevel.critical)]
        [AllureFeature("Character API")]
        public async Task GetCharacters_WithNotExistingEnumValue_ShouldThrowNotFoundException()
        {
            // Arrange
            var resource = _apiClient.BuildQueryParameter("status", "NotExisting");

            // Act
            var response = await _apiClient.GetAsync($"?{resource}");
            var content = response.Content;

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            content.Should().Contain("\"error\": \"There is nothing here\"");
        }
        
        [Test, AllureTag("API"), AllureSeverity(SeverityLevel.critical)]
        [AllureFeature("Character API")]
        public async Task GetCharacters_WithNullEnumValue_ShouldThrowNotFoundException()
        { 
            // Act
            var response = await _apiClient.GetAsync("?status=null");
            var content = response.Content;

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            content.Should().Contain("\"error\":\"There is nothing here\"");
        }
    }