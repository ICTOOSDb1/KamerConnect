using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using KamerConnect.Models;
using KamerConnect.Repositories;
using KamerConnect.Utils;

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
        _repository.AuthenticatePerson(email, HashPassword(password, out byte[] salt, _repository.GetSaltFromPerson(email)));
    }

    public void Register(Person person, string password)
    {
        byte[] salt;
        
        if (!Validations.IsValidEmail(person.Email))
            throw new InvalidOperationException("Email in person is invalid.");
        
        if (!Validations.IsValidPerson(person))
            throw new InvalidOperationException("Some required values are null or empty");
        
        //validate password
        
        _repository.CreatePerson(person, HashPassword(password, out salt), salt);
    }
    
    private string HashPassword(string password, out byte[] salt, byte[] existingSalt = null)
    {
        if (existingSalt == null) {
            salt = new byte[keySize];
            salt = RandomNumberGenerator.GetBytes(keySize);
        }
        else {
            salt = existingSalt;
        }

        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            iterations,
            hashAlgorithm,
            keySize);

        return Convert.ToHexString(hash);
    }
}