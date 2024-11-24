using System.Collections;
using KamerConnect.Models;
using Npgsql;

namespace KamerConnect.Repositories;

public interface IPersonRepository
{
    Person GetPersonById(string id);
    void CreatePerson(Person person, string password, byte[] salt);
    void AddPasswordToPerson(Guid personId, string password, string salt);
    Person AuthenticatePerson(string email, string password);
    byte[] GetSaltFromPerson(string email);
    void UpdatePerson(Guid personId, string fieldNameForIdCheck, List<string> fieldsToUpdate, List<NpgsqlParameter> paramaterNotations, string tableName);
    void InsertTableIfIdIfNotExist(Guid personId, string tableName);
}