using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KamerConnect.Models;

public class Person
{
    public Guid Id { get; set; }
    private string email;

    public string Email
    {
        get => email;
        set => email = value.ToLowerInvariant();
    }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string Surname { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public Role Role { get; set; }
    public string? ProfilePicturePath { get; set; }
    public Personality? Personality { get; set; }
    public string? fullName;
    public string? FullName
    {
        get => fullName;
        set
        {
            fullName = string.Join(" ", new[] { FirstName, MiddleName, Surname });
        }
    }

    public Person(string email, string firstName, string? middleName, string surname, string? phoneNumber,
        DateTime birthDate, Gender gender, Role role, string? profilePicturePath, Guid id)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        MiddleName = middleName;
        Surname = surname;
        PhoneNumber = phoneNumber;
        BirthDate = birthDate;
        Gender = gender;
        Role = role;
        ProfilePicturePath = profilePicturePath;
        FullName = $"{firstName} {middleName} {surname}";
    }
}

public enum Role
{
    Seeking,
    Offering
}
public enum Gender
{
    [Display(Name = "Man")]
    Male,
    [Display(Name = "Vrouw")]
    Female,
    [Display(Name = "Overig")]
    Other
}
