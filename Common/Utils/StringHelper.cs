using System.Text;

namespace Common.Utils;

public class StringHelper
{
    private static Random _random = new();

    public static string GenerateRandomString(int length)
    {
        const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        
        var randomString = new StringBuilder(length);
        
        for (var i = 0; i < length; i++)
        {
            var randomChar = characters[_random.Next(characters.Length)];
            randomString.Append(randomChar);
        }

        return randomString.ToString();
    }
}