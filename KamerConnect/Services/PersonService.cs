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

    public Person GetPersonById(string id)
    {
        return _repository.GetPersonById(id);
    }
    public Person GetPersonByEmail(string email)
    {
        return _repository.GetPersonByEmail(email);
    }
    
    public string CreatePerson(Person person)
    { 
        return _repository.CreatePerson(person);
    }
    
    public Guid CreateHousePreferences(HousePreferences housePreferences)
    {
        return _repository.CreateHousePreferences(housePreferences);
    }
}