using System.Collections;
using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IPersonRepository
{
    List<Person> GetAll();
    Person GetPersonById(string id);
}