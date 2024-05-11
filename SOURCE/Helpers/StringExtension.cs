namespace SOURCE.Helpers;

public static class StringExtensions
{
    public static string FirstCharToLowerCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        return char.ToLower(input[0]) + input[1..];
    }
}