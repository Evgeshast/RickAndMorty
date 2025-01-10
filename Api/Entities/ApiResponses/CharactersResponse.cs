namespace RickAndMorty.Entities.ApiResponses;

public class CharactersResponse
{
    public Info Info { get; set; } = new Info();
    public List<Character> Results { get; set; } = new();
}