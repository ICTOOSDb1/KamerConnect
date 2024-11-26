using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IPersonRepository
{
    Person? GetPersonById(Guid id);
    Person? GetPersonByEmail(string email);
    Guid? CreatePerson(Person person);
    void UpdatePerson(Person person);
    void UpdatePersonality(Guid personId, Personality personality);
    void UpdateSocial(Guid personId, Social social);
    public void UpdateHousePreferences(HousePreferences housePreferences);
    Guid CreateHousePreferences(HousePreferences housePreferences);
    HousePreferences? GetHousePreferences(Guid personId);
}