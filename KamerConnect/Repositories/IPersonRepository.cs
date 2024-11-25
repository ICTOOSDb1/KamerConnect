using System.Collections;
using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IPersonRepository
{
    Person GetPersonById(string id);
    Person GetPersonByEmail(string email);
    string CreatePerson(Person person);
    Guid CreateHousePreferences(HousePreferences housePreferences);
}