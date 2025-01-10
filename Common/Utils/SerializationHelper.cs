using System.Text.Json;

namespace Common.Utils;

public class SerializationHelper
{
    public static T Deserialize<T>(string json)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        
        return JsonSerializer.Deserialize<T>(json, options)!;
    }
}