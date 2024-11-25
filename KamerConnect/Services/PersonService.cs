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

    public Person GetPerson(string id)
    {
        return _repository.GetPersonById(id);
    }

    public void CreatePerson(Person person, string password, byte[] salt)
    {
        _repository.CreatePerson(person, password, salt);
    }

    public void UpdatePerson(Person person)
    {
        _repository.UpdatePerson(person);
    }
    
    public void UpdatePersonality(Person person)
    {
        _repository.UpdatePersonality(person);
    }

    public void UpdateSocial(Person person)
    {
        _repository.UpdateSocial(person);
    }

    public void UpdateHousePreferences(Person person)
    {
        _repository.UpdateHousePreferences(person);
    }
}