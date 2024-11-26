using System.Data.Common;
using KamerConnect.EnvironmentVariables;
using KamerConnect.Models;
using KamerConnect.Repositories;
using KamerConnect.Utils;
using Npgsql;

namespace KamerConnect.DataAccess.Postgres.Repositories;


public class PersonRepository : IPersonRepository
{
    private readonly string connectionString;

    public PersonRepository()
    {
        connectionString = EnvironmentUtils.GetConnectionString();
    }
    public Person GetPersonById(Guid id)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            using (var command =
                   new NpgsqlCommand($"SELECT * FROM person " +
                                     $"LEFT JOIN personality ON person.id = personality.person_id " +
                                     $"WHERE person.id = @id::uuid",
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
    public Person GetPersonByEmail(string email)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            using (var command =
                   new NpgsqlCommand("""
                                     SELECT * FROM person
                                     LEFT JOIN personality ON person.id = personality.person_id
                                        WHERE person.email = @email
                                     """,
                       connection))
            {
                command.Parameters.AddWithValue("@email", email);

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
    public Guid CreatePerson(Person person)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            using (var command =
                   new NpgsqlCommand($"""
                                      INSERT INTO person (    
                                        email, first_name, middle_name, surname, phone_number, birth_date, gender, role, profile_picture_path, house_preferences_id)
                                        VALUES (@Email,
                                        @FirstName,
                                        @MiddleName,
                                        @Surname,
                                        @PhoneNumber,
                                        @BirthDate,
                                        @Gender::gender,
                                        @Role::user_role,
                                        @ProfilePicturePath,
                                        @housePreferences_id::uuid
                                      ) RETURNING id;
                                      """,
                       connection))
            {
                command.Parameters.AddWithValue("@Email", person.Email);
                command.Parameters.AddWithValue("@FirstName", person.FirstName);
                command.Parameters.AddWithValue("@MiddleName", person.MiddleName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Surname", person.Surname);
                command.Parameters.AddWithValue("@PhoneNumber", person.PhoneNumber ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@BirthDate", person.BirthDate);
                command.Parameters.AddWithValue("@Gender", person.Gender.ToString());
                command.Parameters.AddWithValue("@Role", person.Role.ToString());
                command.Parameters.AddWithValue("@ProfilePicturePath", person.ProfilePicturePath ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@housePreferences_id", person.HousePreferencesId ?? (object)DBNull.Value);

                var result = command.ExecuteScalar() ?? throw new InvalidOperationException();
                return (Guid)result;
            }
        }
    }
    private Person ReadToPerson(DbDataReader reader)
    {
        var person = new Person
        (
            reader.GetString(1),
            reader.GetString(2),
            reader.IsDBNull(3) ? null : reader.GetString(3),
            reader.GetString(4),
            reader.IsDBNull(5) ? null : reader.GetString(5),
            reader.GetDateTime(6),
            EnumUtils.Validate<Gender>(reader.GetString(7)),
            EnumUtils.Validate<Role>(reader.GetString(8)),
            reader.IsDBNull(9) ? null : reader.GetString(9),
            reader.GetGuid(0),
            reader.IsDBNull(10) ? null : reader.GetGuid(10)
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

    public Guid CreateHousePreferences(HousePreferences housePreferences)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            
            using (var command = new NpgsqlCommand(
                       """
                       INSERT INTO house_preferences (type, price, surface, residents)
                       VALUES (@Type::house_type, @Price, @Surface, @Residents)
                       RETURNING id;
                      """, connection))
            {
                command.Parameters.AddWithValue("@Type", housePreferences.Type.ToString());
                command.Parameters.AddWithValue("@Price", housePreferences.Budget);
                command.Parameters.AddWithValue("@Surface", housePreferences.SurfaceArea);
                command.Parameters.AddWithValue("@Residents", housePreferences.Residents);
                
                
                return Guid.Parse(command.ExecuteScalar()?.ToString() ?? throw new InvalidOperationException());
            }
        }
    }
}