using System.Text;

namespace AppTask.API.Libraries.Text;

public static class StringExtension
{
    public static string GenerateHash(this string s, int length)
    {
        Random random = new Random();
        StringBuilder sb = new StringBuilder();
        
        for (int i = 0; i < length; i++)
        {
            sb.Append(random.Next(0, 9));
        }

        return sb.ToString();
    }
}
