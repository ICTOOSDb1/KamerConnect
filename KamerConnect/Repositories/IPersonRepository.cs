using System.Collections;
using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IPersonRepository
{
    Person? GetPersonById(Guid id);
    Person? GetPersonByEmail(string email);
    Guid? CreatePerson(Person person);
}