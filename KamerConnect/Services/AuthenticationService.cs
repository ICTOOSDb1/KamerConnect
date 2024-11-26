
using System.Security.Cryptography;
using System.Text;
using KamerConnect.Exceptions;
using KamerConnect.Models;
using KamerConnect.Models.ConfigModels;
using KamerConnect.Repositories;
using KamerConnect.Services;
using KamerConnect.Utils;
using Microsoft.Maui.Storage;

namespace KamerConnect;

public class AuthenticationService
{
    private PersonService _personService;
    private IAuthenticationRepository _repository;

    private PasswordHashingConfig _passwordHashingConfig;

    public AuthenticationService(PersonService personService, IAuthenticationRepository authenticationRepository)
    {
        _personService = personService;
        _repository = authenticationRepository;

        _passwordHashingConfig = GetHashValues();


    }

    public async Task Authenticate(string email, string passwordAttempt)
    {
        try
        {
            Person person = _personService.GetPersonByEmail(email) ?? throw new InvalidCredentialsException();
            string personPassword = _repository.GetPassword(person.Id);

            if (ValidatePassword(HashPassword(passwordAttempt,
                    out byte[] salt,
                    _repository.GetSaltFromPerson(email)), personPassword))
            {
                await SaveSession(person.Id, DateTime.Now, GenerateSessionToken());
            }
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

        if (!ValidationUtils.IsValidEmail(person.Email))
            throw new InvalidOperationException("Email in person is invalid.");

        if (!ValidationUtils.IsValidPerson(person))
            throw new InvalidOperationException("Some required values are null or empty");

     

        string personId = _personService.CreatePerson(person);

        _repository.AddPassword(personId, HashPassword(password, out salt), Convert.ToBase64String(salt));
    }

    public async Task<bool> CheckSession()
    {
        string currentToken = await GetSession();

        if (!string.IsNullOrEmpty(currentToken))
        {
            Session? sessionExpirationDate = _repository.GetSessionWithLocalToken(currentToken);

            if (sessionExpirationDate == null || DateTime.Now >= sessionExpirationDate.startingDate.AddMonths(6))
            {
                RemoveSession(currentToken);
                return false;
            }

            return true;
        }

        return false;
    }

    private async Task SaveSession(string personId, DateTime currentDate, string sessionToken)
    {
        if (_repository.GetSession(personId) == null)
        {
            _repository.SaveSession(personId, currentDate, sessionToken);
            await SecureStorage.Default.SetAsync("session_token", sessionToken);
        }
    }

    private async Task<string?> GetSession()
    {
        return await SecureStorage.Default.GetAsync("session_token");
    }

    private void RemoveSession(string currentToken)
    {
        _repository.RemoveSession(currentToken);
        SecureStorage.Default.Remove("session_token");
    }
    private bool ValidatePassword(string passwordAttempt, string personPassword)
    {
        if (passwordAttempt == personPassword)
        {
            return true;
        }

        throw new InvalidCredentialsException();
    }

    public string HashPassword(string password, out byte[] salt, byte[] existingSalt = null)
    {
        if (existingSalt == null)
        {
            salt = new byte[_passwordHashingConfig.KeySize];
            salt = RandomNumberGenerator.GetBytes(_passwordHashingConfig.KeySize);
        }
        else
        {
            salt = existingSalt;
        }

        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            _passwordHashingConfig.Iterations,
            new HashAlgorithmName(_passwordHashingConfig.HashAlgorithm),
            _passwordHashingConfig.KeySize);

        return Convert.ToHexString(hash);
    }
    private string GenerateSessionToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(_passwordHashingConfig.KeySize));
    }
    private PasswordHashingConfig GetHashValues()
    {
        var keySize = Environment.GetEnvironmentVariable("HASH_KEY_SIZE");
        var iterations = Environment.GetEnvironmentVariable("HASH_ITERATIONS");
        var algorithm = Environment.GetEnvironmentVariable("HASH_ALGORITHM");


        if (string.IsNullOrEmpty(keySize) || string.IsNullOrEmpty(iterations) || string.IsNullOrEmpty(algorithm))
        {
            throw new("Some hash data has not been set");
        }

        return new PasswordHashingConfig(int.Parse(keySize), int.Parse(iterations), algorithm);
    }
}