using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IPersonRepository
{
    Person? GetPersonById(Guid id);
    Person? GetPersonByHouseId(Guid id);
    Person? GetPersonByEmail(string email);
    Guid CreatePerson(Person person);
    void UpdatePerson(Person person);
    void UpdatePersonality(Guid personId, Personality personality);
}