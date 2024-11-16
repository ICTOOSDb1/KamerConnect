using System.Text.RegularExpressions;
using KamerConnect.Models;

namespace KamerConnect;

public class AuthenticationService
{

    public Person Person { get; set; }

    private void authenticate(Person person)
    {
        //do sql call to check user
    }
    
    public static bool IsValidEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        Regex regex = new Regex(pattern);
        return regex.IsMatch(email);
    }
}