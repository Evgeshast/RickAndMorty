namespace RickAndMorty.Entities;

public class Character
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Species { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public Origin Origin { get; set; } = new Origin();
    public Location Location { get; set; } = new Location();
    public string Image { get; set; } = string.Empty;
    public List<string> Episode { get; set; } = new List<string>();
    public string Url { get; set; } = string.Empty;
    public DateTime Created { get; set; }
}