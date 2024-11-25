using System.Collections;
using KamerConnect.Models;
using Npgsql;

namespace KamerConnect.Repositories;

public interface IPersonRepository
{
    Person GetPersonById(string id);
    Person GetPersonByEmail(string email);
    string CreatePerson(Person person);
    
    void AddPasswordToPerson(Guid personId, string password, string salt);
    Person AuthenticatePerson(string email, string password);
    byte[] GetSaltFromPerson(string email);
    void UpdatePerson(Person person);
    void UpdatePersonality(Person person);
    void UpdateSocial(Person person);
    public void UpdateHousePreferences(Person person);
}