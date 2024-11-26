using System.Collections;
using KamerConnect.Models;
using Npgsql;

namespace KamerConnect.Repositories;

public interface IPersonRepository
{
    Person GetPersonById(string id);
    Person GetPersonByEmail(string email);
    string CreatePerson(Person person);
    
    void UpdatePerson(Person person);
    void UpdatePersonality(Guid personId, Personality personality);
    void UpdateSocial(Guid personId, Social social);
    public void UpdateHousePreferences(Guid housePreferencesId, HousePreferences housePreferences);
}