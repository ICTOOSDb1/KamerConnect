using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using KamerConnect.Models;
using KamerConnect.Repositories;

namespace KamerConnect;

public class AuthenticationService
{
    private IPersonRepository _repository;
   
    /// <summary>
    /// Moet nog in .env
    /// </summary>
    const int keySize = 32;
    const int iterations = 100_000;
    HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256;
    
    public AuthenticationService(IPersonRepository repository)
    {
        _repository = repository;
    }

    public void Authenticate(string email, string password)
    {
        _repository.AuthenticatePerson(email, password);
    }

    public void Register(Person person, string password)
    {
        byte[] salt;
        
        if (!IsValidPerson(person))
            throw new InvalidOperationException("Person is invalid");
        
        _repository.CreatePerson(person, HashPassword(password, out salt), salt);
    }
    
    string HashPassword(string password, out byte[] salt)
    {
        salt = new byte[keySize];
        salt = RandomNumberGenerator.GetBytes(keySize);

        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            iterations,
            hashAlgorithm,
            keySize);

        return Convert.ToHexString(hash);
    }
    
    private bool IsValidEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        Regex regex = new Regex(pattern);
        return regex.IsMatch(email);
    }

    private bool IsValidPerson(Person person)
    {
        if (!IsValidEmail(person.Email))
            throw new InvalidOperationException("Email in person is invalid.");

        //Add more needed validations
        return true;
    }
    
}