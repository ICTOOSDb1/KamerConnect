using KamerConnect.Models;
using KamerConnect.Repositories;

namespace KamerConnect.Services;

public class PersonService
{
    private IPersonRepository _repository;

    public PersonService(IPersonRepository repository)
    {
        _repository = repository;
    }

    public Person? GetPersonById(Guid id)
    {
        return _repository.GetPersonById(id);
    }
    public Person? GetPersonByEmail(string email)
    {
        return _repository.GetPersonByEmail(email);
    }

    public Guid? CreatePerson(Person person)
    {
        return _repository.CreatePerson(person);
    }


    public void UpdatePerson(Person person)
    {
        _repository.UpdatePerson(person);
    }

    public void UpdatePersonality(Guid personId, Personality personality)
    {
        _repository.UpdatePersonality(personId, personality);
    }

    public void UpdateSocial(Guid personId, Social social)
    {
        _repository.UpdateSocial(personId, social);
    }

    public void UpdateHousePreferences(Guid housePreferencesId, HousePreferences housePreferences)
    {
        _repository.UpdateHousePreferences(housePreferencesId, housePreferences);
    }

    public Guid CreateHousePreferences(HousePreferences housePreferences)
    {
        return _repository.CreateHousePreferences(housePreferences);
    }
}