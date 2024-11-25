using System.Text.RegularExpressions;
using KamerConnect.Models;

namespace KamerConnect.Utils;

public static class Validations
{
    public static bool IsValidEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        Regex regex = new Regex(pattern);
        return regex.IsMatch(email);
    }

    public static bool IsValidPerson(Person person)
    {
        if (string.IsNullOrEmpty(person.Email) || 
            string.IsNullOrEmpty(person.FirstName) || 
            string.IsNullOrEmpty(person.Surname) || 
            person.BirthDate == default)
        {
            return false;
        }

        return true;
    }
}