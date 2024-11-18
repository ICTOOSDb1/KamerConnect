using System.Data.Common;
using KamerConnect;
using KamerConnect.Models;
using KamerConnect.Repository;
using Npgsql;

namespace KamerConnect.DataAccess.Postgres.Repositys;

public class PersonRepository : IPersonRepository
{
    public string ConnectionString { get; set; }

    public PersonRepository()
    {
        ConnectionString = "Host=localhost;Username=niekvandenberg;Password=password;Database=kamers-connect";;
    }

    public List<Person> GetAll()
    {
        var persons = new List<Person>();

        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            connection.Open();

            using (var command =
                   new NpgsqlCommand("SELECT * FROM person LEFT JOIN personality ON person.id = personality.person_id ",
                       connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Person person = ReadToPerson(reader);
                        
                        persons.Add(person);
                    }
                }
            }
        }

        return persons;
    }

    public Person GetPersonById(string id)
    {
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            connection.Open();

            using (var command =
                   new NpgsqlCommand($"SELECT * FROM person LEFT JOIN personality ON person.id = personality.person_id WHERE person.id = @id::uuid",
                       connection))
            {
                command.Parameters.AddWithValue("@id", id);
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return ReadToPerson(reader);
                    }
                }
            }
        }

        return null;
    }

    private Person ReadToPerson(DbDataReader reader)
    {
        var person = new Person
        (
            reader.GetString(1),
            reader.GetString(2),
            reader.GetString(3),
            reader.GetString(4),
            reader.GetString(5),
            reader.GetDateTime(6),
            (Gender)Enum.Parse(typeof(Gender), reader.GetString(7)),
            (Role)Enum.Parse(typeof(Role), reader.GetString(8)),
            reader.GetString(9),
            reader.GetString(11),
            reader.GetString(12),
            reader.GetString(13),
            reader.GetString(0)
        );

        return person;
    }
}