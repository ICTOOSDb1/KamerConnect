namespace KamerConnect.Models;

public class Person
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string Surname { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public Role Role { get; set; }
    public string? ProfilePicturePath { get; set; }
    public Personality? Personality { get; set; }

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
    }
}

public enum Role
{
    Seeking,
    Offering
}
public enum Gender
{
    Male,
    Female,
    Other
}