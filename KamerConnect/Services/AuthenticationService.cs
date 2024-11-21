
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using KamerConnect.Exceptions;
using KamerConnect.Models;
using KamerConnect.Repositories;
using KamerConnect.Services;
using KamerConnect.Utils;

namespace KamerConnect;

public class AuthenticationService
{
    private PersonService _personService;
    private IAuthenticationRepository _repository;
   
    /// <summary>
    /// Moet nog in .env
    /// </summary>
    const int keySize = 32;
    const int iterations = 100_000;
    HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256;
    
    public AuthenticationService(PersonService personService, IAuthenticationRepository authenticationRepository)
    {
        _personService = personService;
        _repository = authenticationRepository;
    }

    public void Authenticate(string email, string passwordAttempt)
    {
        try
        {
            string personPassword = _repository.GetPassword(_personService.GetPersonByEmail(email).Id ?? throw new InvalidCredentialsException());
            ValidatePassword(HashPassword(passwordAttempt, out byte[] salt, _repository.GetSaltFromPerson(email)), personPassword);
            Console.WriteLine("User logged in");
        }
        catch (InvalidCredentialsException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void Register(Person person, string password)
    {
        byte[] salt;
        
        if (!Validations.IsValidEmail(person.Email))
            throw new InvalidOperationException("Email in person is invalid.");
        
        if (!Validations.IsValidPerson(person))
            throw new InvalidOperationException("Some required values are null or empty");
        
        //validate password
        Guid person_id = _personService.CreatePerson(person);

        if (person_id != null)
        {
            _repository.AddPassword(person_id, HashPassword(password, out salt), Convert.ToBase64String(salt));
        }
    }

    private bool ValidatePassword(string passwordAttempt, string personPassword)
    {
        if (passwordAttempt == personPassword)
        {
            return true;
        }

        throw new InvalidCredentialsException();
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