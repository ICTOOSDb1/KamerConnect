using KamerConnect.Models;
using KamerConnect.Repositories;
using Npgsql;

namespace KamerConnect.Services;

public class PersonService
{
    private IPersonRepository _repository;

    public PersonService(IPersonRepository repository)
    {
        _repository = repository;
    }
    
    public Person GetPersonByEmail(string email)
    {
        return _repository.GetPersonByEmail(email);
    }
    
    public string CreatePerson(Person person)
    { 
        return _repository.CreatePerson(person);
    }
    

    public void UpdatePerson(Person person)
    {
        _repository.UpdatePerson(person);
    }
    
    public void UpdatePersonality(string personId, Personality personality)
    {
        _repository.UpdatePersonality(personId, personality);
    }

    public void UpdateSocial(string personId, Social social)
    {
        _repository.UpdateSocial(personId, social);
    }

    public void UpdateHousePreferences(string housePreferencesId, HousePreferences housePreferences)
    {
        _repository.UpdateHousePreferences(housePreferencesId, housePreferences);
    }
}