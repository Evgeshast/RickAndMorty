using System.Net;
using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RickAndMorty;

namespace Tests;

[TestFixture]
    [AllureNUnit]
    public class CharacterApiTests
    {
        private ApiClient _apiClient;
        private const string BaseUrl = "https://rickandmortyapi.com/api/character";

        [SetUp]
        public void SetUp()
        {
            _apiClient = new ApiClient(BaseUrl);
        }
        
        [Test, AllureTag("API"), AllureSeverity(SeverityLevel.normal)]
        [AllureFeature("Character API"), AllureStory("Retrieve a list of characters with pagination")]
        public async Task GetCharacters_WithPagination_ShouldReturnValidResponseAndNavigatePages()
        {
            // Arrange
            var page = 1;
            var resource = _apiClient.BuildQueryParameter("page", page.ToString());

            // Act
            var response = await _apiClient.GetAsync($"?{resource}");
            var content = response.Content;

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().Contain("info");
            content.Should().Contain("results");
            content.Should().Contain("next");
            content.Should().Contain("Rick Sanchez");
            content.Should().Contain("Morty Smith");
        }

        [Test, AllureTag("API"), AllureSeverity(SeverityLevel.normal)]
        [AllureFeature("Character API"), AllureStory("Filter characters by name")]
        public async Task GetCharacters_WithNameFilter_ShouldReturnFilteredResults()
        {
            // Arrange
            var name = "Rick Sanchez";
            var resource = _apiClient.BuildQueryParameter("name", name);

            // Act
            var response = await _apiClient.GetAsync($"?{resource}");
            var content = response.Content;

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().Contain(name);
            content.Should().Contain("Alive");
            content.Should().Contain("Human");
        }

        [Test, AllureTag("API"), AllureSeverity(SeverityLevel.normal)]
        [AllureFeature("Character API"), AllureStory("Filter characters by multiple parameters")]
        public async Task GetCharacters_WithMultipleFilters_ShouldReturnFilteredResults()
        {
            // Arrange
            var filters = new
            {
                name = "Morty Smith",
                status = "Alive",
                species = "Human",
                type = "",
                gender = "Male"
            };
            var resource = string.Join("&", 
                _apiClient.BuildQueryParameter("name", filters.name),
                _apiClient.BuildQueryParameter("status", filters.status),
                _apiClient.BuildQueryParameter("species", filters.species),
                _apiClient.BuildQueryParameter("type", filters.type),
                _apiClient.BuildQueryParameter("gender", filters.gender));

            // Act
            var response = await _apiClient.GetAsync($"?{resource}");
            var content = response.Content;

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().Contain(filters.name);
            content.Should().Contain(filters.status);
            content.Should().Contain(filters.species);
            content.Should().Contain(filters.gender);
        }

        [Test, AllureTag("API"), AllureSeverity(SeverityLevel.normal)]
        [AllureFeature("Character API"), AllureStory("Filter characters by status")]
        public async Task GetCharacters_WithStatusFilter_ShouldReturnFilteredResults()
        {
            // Arrange
            var status = "Dead";
            var resource = _apiClient.BuildQueryParameter("status", status);

            // Act
            var response = await _apiClient.GetAsync($"?{resource}");
            var content = response.Content;

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().Contain(status);
            content.Should().Contain("Adjudicator Rick");
            content.Should().Contain("Human");
        }

        [Test, AllureTag("API"), AllureSeverity(SeverityLevel.normal)]
        [AllureFeature("Character API"), AllureStory("Filter characters by gender")]
        public async Task GetCharacters_WithGenderFilter_ShouldReturnFilteredResults()
        {
            // Arrange
            var gender = "Female";
            var resource = _apiClient.BuildQueryParameter("gender", gender);

            // Act
            var response = await _apiClient.GetAsync($"?{resource}");
            var content = response.Content;

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().Contain(gender);
            content.Should().Contain("Summer Smith");
            content.Should().Contain("Human");
        }

        [Test, AllureTag("API"), AllureSeverity(SeverityLevel.normal)]
        [AllureFeature("Character API"), AllureStory("Filter characters by species")]
        public async Task GetCharacters_WithSpeciesFilter_ShouldReturnFilteredResults()
        {
            // Arrange
            var species = "Alien";
            var resource = _apiClient.BuildQueryParameter("species", species);

            // Act
            var response = await _apiClient.GetAsync($"?{resource}");
            var content = response.Content;

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().Contain(species);
            content.Should().Contain("Abadango Cluster Princess");
            content.Should().Contain("Alive");
        }

        [Test, AllureTag("API"), AllureSeverity(SeverityLevel.normal)]
        [AllureFeature("Character API"), AllureStory("Verify character details")]
        public async Task GetCharacterDetails_ShouldReturnCorrectDetails()
        {
            // Arrange
            var characterId = 1;

            // Act
            var response = await _apiClient.GetAsync($"/{characterId}");
            var content = response.Content;

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().Contain("Rick Sanchez");
            content.Should().Contain("Alive");
            content.Should().Contain("Human");
            content.Should().Contain("Earth (C-137)");
        }
    }