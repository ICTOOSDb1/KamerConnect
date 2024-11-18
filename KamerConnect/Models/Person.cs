namespace KamerConnect.Models;

public class Person
{
    //Person
    public string Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string Surname { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public Role Role { get; set; }
    public string? ProfilePicturePath { get; set; }
    
    //Personality
    public string School { get; set; }
    public string Study { get; set; }
    public string Description { get; set; }
    
    public Person(string email, string firstName, string? middleName, string surname, string? phoneNumber,
        DateTime birthDate, Gender gender, Role role, string? profilePicturePath, string school,
        string study, string description, string id = "")
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
        School = school;
        Study = study;
        Description = description;
    }
    
    public override string ToString()
    {
        return $@"Id: {Id}
        Email: {Email}
        First Name: {FirstName}
        Middle Name: {MiddleName ?? "N/A"}
        Surname: {Surname}
        Phone Number: {PhoneNumber ?? "N/A"}
        Birth Date: {BirthDate.ToShortDateString()}
        Gender: {Gender}
        Role: {Role}
        Profile Picture Path: {ProfilePicturePath ?? "N/A"}
        School: {School}
        Study: {Study}
        Description: {Description}";
    }

    
}

public enum Role
{
    seeking, 
    offering
}
public enum Gender
{
    male, 
    female,
    other
}

public enum HouseType
{
    Apartment, 
    House,
    Studio
}

public enum SocialType
{
    LinkedIn,
    X,
    Instagram,
    Facebook
}