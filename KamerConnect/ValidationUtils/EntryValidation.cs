namespace KamerConnect.ValidationUtils;

public class EntryValidation
{
    public static bool IsValidEmail(string email)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    public static bool IsNumeric(string text)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(text, @"^[\d/]+$");
    }

}