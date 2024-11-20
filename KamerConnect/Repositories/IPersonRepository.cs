using System.Collections;
using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IPersonRepository
{
    Person GetPersonById(string id);
    void CreatePerson(Person person, string password, byte[] salt);
    void AddPasswordToPerson(Guid personId, string password, string salt);
    Person AuthenticatePerson(string email, string password);
    byte[] GetSaltFromPerson(string email);
    void SaveSession(Guid personId, DateTime startingDate, string sessionToken);
}