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
    
    public void UpdatePerson(Guid personId,  string fieldNameForIdCheck, List<string> fieldsToUpdate, List<NpgsqlParameter> paramaterNotations, string tableName)
    {
        _repository.UpdatePerson(personId, fieldNameForIdCheck, fieldsToUpdate, paramaterNotations, tableName);
    }

    public void InsertTableIfIdIfNotExist(Guid personId, string tableName)
    {
        _repository.InsertTableIfIdIfNotExist(personId, tableName);
    }
}