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
            var resource = ApiClient.BuildQueryParameter("page", page.ToString());

            // Act
            var response = await ApiClient.GetAsync($"?{resource}");
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
            var response = await ApiClient.GetAsync();
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
            var resource = ApiClient.BuildQueryParameter("name", name);

            // Act
            var response = await ApiClient.GetAsync($"?{resource}");
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
            var response = await ApiClient.GetAsync();
            var content = SerializationHelper.Deserialize<CharactersResponse>(response.Content!);
            var expectedCharacter = content.Results[0];
            
            var resource = string.Join("&", 
                ApiClient.BuildQueryParameter("name", expectedCharacter.Name),
                ApiClient.BuildQueryParameter("status", expectedCharacter.Status),
                ApiClient.BuildQueryParameter("species", expectedCharacter.Species),
                ApiClient.BuildQueryParameter("type", expectedCharacter.Type),
                ApiClient.BuildQueryParameter("gender", expectedCharacter.Gender));

            // Act
            response = await ApiClient.GetAsync($"?{resource}");
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
            var resource = ApiClient.BuildQueryParameter(filter, value);

            // Act
            var response = await ApiClient.GetAsync($"?{resource}");
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
            var resource = ApiClient.BuildQueryParameter("status", "NotExisting");

            // Act
            var response = await ApiClient.GetAsync($"?{resource}");
            var content = response.Content;

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            content.Should().Contain("\"error\":\"There is nothing here\"");
        }
        
        [Test, AllureTag("API"), AllureSeverity(SeverityLevel.critical)]
        [AllureFeature("Character API")]
        public async Task GetCharacters_WithNullEnumValue_ShouldThrowNotFoundException()
        { 
            // Act
            var response = await ApiClient.GetAsync("?status=null");
            var content = response.Content;

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            content.Should().Contain("\"error\":\"There is nothing here\"");
        }
    }