using System.Data.Common;
using KamerConnect.EnvironmentVariables;
using KamerConnect.Models;
using KamerConnect.Repositories;
using Npgsql;
using NpgsqlTypes;

namespace KamerConnect.DataAccess.Postgres.Repositys;

public class PersonRepository : IPersonRepository
{
    private readonly string connectionString;

    public PersonRepository()
   {
       connectionString = GetConnectionString();
   }
    public Person GetPersonById(string id)
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
    public string CreatePerson(Person person)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            using (var command =
                   new NpgsqlCommand($"""
                                      INSERT INTO person (    
                                        email, first_name, middle_name, surname, phone_number, birth_date, gender, role, profile_picture_path)
                                        VALUES (@email,
                                        @firstName,
                                        @middleName,
                                        @surname,
                                        @phoneNumber,
                                        @birthDate,
                                        @gender::gender,
                                        @role::user_role,
                                        @profilePicturePath
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

                var results = command.ExecuteScalar().ToString() ?? throw new InvalidOperationException();
                return results?.ToString();
            }
        }
    }
    private string GetConnectionString()
    {
        var host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
        var port = Environment.GetEnvironmentVariable("POSTGRES_PORT");
        var database = Environment.GetEnvironmentVariable("POSTGRES_DB");
        var username = Environment.GetEnvironmentVariable("POSTGRES_USER");
        var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
    
        if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(port) ||
            string.IsNullOrEmpty(database) || string.IsNullOrEmpty(username) ||
            string.IsNullOrEmpty(password))
        {
            throw new("Database environment variables are missing. Please check your .env file.");
        }
    
        return $"Host={host};Port={port};Database={database};Username={username};Password={password};";
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
            ValidateEnum<Gender>(reader.GetString(7)),
            ValidateEnum<Role>(reader.GetString(8)),
            reader.IsDBNull(9) ? null : reader.GetString(9),
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

    public void UpdatePerson(Person person)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand(
                       """
                       UPDATE person SET
                           email = @Email,
                           first_name = @FirstName,
                           middle_name = @MiddleName,
                           surname = @Surname,
                           phone_number = @PhoneNumber,
                           birth_date = @BirthDate,
                           gender = @gender::gender,
                           role = @Role::user_role,
                           profile_picture_path = @ProfilePicturePath
                       WHERE id = @Id;
                       """, connection))
            {
                command.Parameters.AddWithValue("@Id", Guid.Parse(person.Id));
                command.Parameters.AddWithValue("@Email", person.Email);
                command.Parameters.AddWithValue("@FirstName", person.FirstName);
                command.Parameters.AddWithValue("@MiddleName", person.MiddleName ?? string.Empty);
                command.Parameters.AddWithValue("@Surname", person.Surname);
                command.Parameters.AddWithValue("@PhoneNumber", person.PhoneNumber ?? string.Empty);
                command.Parameters.AddWithValue("@BirthDate", person.BirthDate);
                command.Parameters.AddWithValue("@Gender", person.Gender.ToString());
                command.Parameters.AddWithValue("@Role", person.Role.ToString());
                command.Parameters.AddWithValue("@ProfilePicturePath", person.ProfilePicturePath ?? string.Empty);

                command.ExecuteNonQuery();
            }
        }
    }
    
    public void UpdatePersonality(Person person)
{
    using (var connection = new NpgsqlConnection(connectionString))
    {
        connection.Open();
        
        using (var transaction = connection.BeginTransaction())
        {
            using (var updateCommand = new NpgsqlCommand(
                """
                UPDATE personality
                SET school = @School,
                    study = @Study,
                    description = @Description
                WHERE person_id = @PersonalityId;
                """, connection))
            {
                updateCommand.Parameters.AddWithValue("@PersonalityId", Guid.Parse(person.Id));
                updateCommand.Parameters.AddWithValue("@School", person.Personality.School ?? string.Empty);
                updateCommand.Parameters.AddWithValue("@Study", person.Personality.Study ?? string.Empty);
                updateCommand.Parameters.AddWithValue("@Description", person.Personality.Description ?? string.Empty);

                var rowsAffected = updateCommand.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    using (var insertCommand = new NpgsqlCommand(
                        """
                        INSERT INTO personality (person_id, school, study, description)
                        VALUES (@PersonalityId, @School, @Study, @Description);
                        """, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@PersonalityId", Guid.Parse(person.Id));
                        insertCommand.Parameters.AddWithValue("@School", person.Personality.School ?? string.Empty);
                        insertCommand.Parameters.AddWithValue("@Study", person.Personality.Study ?? string.Empty);
                        insertCommand.Parameters.AddWithValue("@Description", person.Personality.Description ?? string.Empty);

                        insertCommand.ExecuteNonQuery();
                    }
                }
            }
            transaction.Commit();
        }
    }
}


    public void UpdateSocial(Person person)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            
            using (var transaction = connection.BeginTransaction())
            {
                using (var updateCommand = new NpgsqlCommand(
                           "UPDATE social SET type = @Type::social_type, url = @Url WHERE person_id = @PersonalityId", connection))
                {
                    updateCommand.Parameters.AddWithValue("@PersonalityId", Guid.Parse(person.Id));
                    updateCommand.Parameters.AddWithValue("@Type", person.Social.Type.ToString() ?? null);
                    updateCommand.Parameters.AddWithValue("@Url", person.Social.Url ?? null);
                    var rowsAffected = updateCommand.ExecuteNonQuery();
                    
                    if (rowsAffected == 0)
                    {
                        using (var insertCommand = new NpgsqlCommand(
                                   "INSERT INTO social (person_id, type, url) VALUES (@PersonalityId, @Type::social_type, @Url)", connection))
                        {
                            insertCommand.Parameters.AddWithValue("@PersonalityId", Guid.Parse(person.Id));
                            insertCommand.Parameters.AddWithValue("@Type", person.Social.Type.ToString() ?? null);
                            insertCommand.Parameters.AddWithValue("@Url", person.Social.Url ?? null);
                            insertCommand.ExecuteNonQuery();
                        }
                    }
                }
                transaction.Commit();
            }
        }
    }
    public void UpdateHousePreferences(Person person)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            
            using (var updateCommand = new NpgsqlCommand(
                       """
                       UPDATE house_preferences
                       SET type = @Type,
                           price = @Price,
                           surface = @Surface
                       WHERE id = @HousePreferencesId;
                       """, connection))
            {
                updateCommand.Parameters.AddWithValue("@HousePreferencesId", person.HousePreferencesId);
                updateCommand.Parameters.AddWithValue("@Type", person.HousePreferences.Type.ToString() ?? string.Empty);
                updateCommand.Parameters.AddWithValue("@Price", person.HousePreferences.Budget ?? string.Empty);
                updateCommand.Parameters.AddWithValue("@Surface", person.HousePreferences.SurfaceArea ?? string.Empty);
                
                updateCommand.ExecuteNonQuery();
            }
        }
    }

}