using System.Globalization;
using System.Text.RegularExpressions;
using KamerConnect.Models;

namespace KamerConnect.Utils;

public static class ValidationUtils
{
    public static bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
    }

    public static bool IsValidPassword(string password)
    {
        return password.Length > 8;
    }

    public static bool IsInteger(string text)
    {
        return int.TryParse(text, out _);
    }

    public static bool IsDouble(string text)
    {
        return double.TryParse(text, out _);
    }

    public static bool IsValidPostalCode(string postalCode)
    {
        return Regex.IsMatch(postalCode, @"^\d{4}\s?[A-Z]{2}$");
    }

    public static bool IsValidPhoneNumber(string phoneNumber)
    {
        return Regex.IsMatch(phoneNumber, @"^(\+31|0)?\s?\d{9}$");
    }

    public static bool IsValidDate(string date)
    {
        return DateTime.TryParseExact(
            date,
            "dd/MM/yyyy",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out _);
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