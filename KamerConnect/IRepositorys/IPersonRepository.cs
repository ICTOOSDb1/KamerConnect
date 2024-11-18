using System.Collections;
using KamerConnect.Models;

namespace KamerConnect.Repository;

public interface IPersonRepository
{
    List<Person> GetAll();
    Person GetPersonById(string id);
}