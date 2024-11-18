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

    public Person GetPerson(string id)
    {
        return _repository.GetPersonById(id);
    }
}