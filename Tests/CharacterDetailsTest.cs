using System.Net;
using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using Common.Utils;
using NUnit.Framework;
using FluentAssertions;
using RickAndMorty.Entities;
using RickAndMorty.Entities.ApiResponses;

namespace Tests;

    [TestFixture]
    [AllureNUnit]
    public class CharacterDetailsTest : BaseTest
    {
        [Test, AllureTag("API"), AllureSeverity]
        [AllureFeature("Character API")]
        public async Task GetCharacterDetails_ShouldReturnCorrectDetails()
        {
            // Arrange
            var getCharactersResponse = await ApiClient.GetAsync();
            var charactersResponse = SerializationHelper.Deserialize<CharactersResponse>(getCharactersResponse.Content!);
            var expectedCharacter = charactersResponse.Results[0];
            
            // Act
            var getCharacterDetailsResponse = await ApiClient.GetAsync($"/{expectedCharacter.Id}");
            var character = SerializationHelper.Deserialize<Character>(getCharacterDetailsResponse.Content!);

            // Assert
            getCharacterDetailsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            character.Should().BeEquivalentTo(expectedCharacter);
        }

        [Test, AllureTag("API"), AllureSeverity]
        [AllureFeature("Character API")]
        public async Task GetCharacters_ByMultipleIds_ShouldReturnCorrectDetails()
        {
            // Arrange
            var getCharacterDetailsResponse = await ApiClient.GetAsync();
            var charactersResponse = SerializationHelper.Deserialize<CharactersResponse>(getCharacterDetailsResponse.Content!);
            var expectedCharacters = charactersResponse.Results.Take(3).ToList();

            // Act
            var getCharactersDetailsResponse = await ApiClient.GetAsync("/1,2,3");
            var characters = SerializationHelper.Deserialize<List<Character>>(getCharactersDetailsResponse.Content!);

            // Assert
            getCharactersDetailsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            characters.Should().BeEquivalentTo(expectedCharacters);
        }
        
        [Test, AllureTag("API"), AllureSeverity]
        [AllureFeature("Character API")]
        public async Task GetCharacterDetails_WithNegativeId_ShouldReturnNotFound()
        {
            // Act
            var getCharacterDetailsResponse = await ApiClient.GetAsync("/-1");
            var content = getCharacterDetailsResponse.Content;

            // Assert
            getCharacterDetailsResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
            content.Should().Contain("\"error\":\"Character not found\"");
        }
        
        [Test, AllureTag("API"), AllureSeverity]
        [AllureFeature("Character API")]
        public async Task GetCharacters_ByNotExistingId_ShouldReturnCorrectDetails()
        {
            // Arrange
            var getCharacterDetailsResponse = await ApiClient.GetAsync();
            var charactersResponse = SerializationHelper.Deserialize<CharactersResponse>(getCharacterDetailsResponse.Content!);
            var charactersCount = charactersResponse.Info.Count;

            // Act
            var getCharactersDetailsResponse = await ApiClient.GetAsync($"/{charactersCount + 1}");
            var content = getCharactersDetailsResponse.Content;
    
            // Assert
            getCharactersDetailsResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
            content.Should().Contain("\"error\":\"Character not found\"");
        }
    }