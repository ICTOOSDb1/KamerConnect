using System.Collections;
using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IPersonRepository
{
    List<Person> GetAll();
    Person GetPersonById(string id);
    void CreatePerson(Person person, string password, byte[] salt);
    void AddPasswordToPerson(Guid personId, string password, string salt);
    void AuthenticatePerson(string email, string password);
}