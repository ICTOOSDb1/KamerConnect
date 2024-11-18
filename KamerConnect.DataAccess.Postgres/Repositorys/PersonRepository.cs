using System.Data.Common;
using KamerConnect;
using KamerConnect.Models;
using KamerConnect.Repository;
using Npgsql;

namespace KamerConnect.DataAccess.Postgres.Repositys;

public class PersonRepository : IPersonRepository
{
    private readonly string ConnectionString = "Host=localhost;Username=niekvandenberg;Password=password;Database=kamer-connect";

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
                        persons.Add(ReadToPerson(reader));
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
            reader.IsDBNull(3) ? null : reader.GetString(3),
            reader.GetString(4),
            reader.GetString(5),
            reader.GetDateTime(6),
            ValidateEnum<Gender>(reader.GetString(7)), 
            ValidateEnum<Role>(reader.GetString(8)), 
            reader.GetString(9),
            reader.GetGuid(0).ToString()
        );


        if (reader.IsDBNull(11)) return person; //If personality is not present with user
        
        person.Personality = new Personality(
            reader.IsDBNull(13) ? null : reader.GetString(13),
            reader.IsDBNull(14) ? null : reader.GetString(14),
            reader.IsDBNull(15) ? null : reader.GetString(15)
        );

        return person;
    }
    
    private static T ValidateEnum<T>(string value) where T : struct
    {
        if (Enum.TryParse(value, out T result) && Enum.IsDefined(typeof(T), result))
        {
            return result;
        }
    
        throw new ArgumentException($"Invalid value '{value}' for enum {typeof(T).Name}");
    }
}