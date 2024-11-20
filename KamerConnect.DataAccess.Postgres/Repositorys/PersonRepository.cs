using System.Data.Common;
using KamerConnect;
using KamerConnect.Models;
using KamerConnect.Repositories;
using Npgsql;

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

    public void CreatePerson(Person person, string password, byte[] salt)
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

                var personId = (Guid)(command.ExecuteScalar() ?? throw new InvalidOperationException());
                
                AddPasswordToPerson(personId, password, Convert.ToBase64String(salt));
            }
        }
    }

    public void AddPasswordToPerson(Guid personId, string password, string salt)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand(
                       """
                       INSERT INTO password (salt, hashed_password, person_id)
                                     VALUES (@Salt, @HashedPassword, @PersonId);
                       """, connection))
            {
                command.Parameters.AddWithValue("@Salt", salt);
                command.Parameters.AddWithValue("@HashedPassword", password);
                command.Parameters.AddWithValue("@PersonId", personId);

                command.ExecuteNonQuery();
            }
        }
    }

    public Person AuthenticatePerson(string email, string password)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            using (var command =
                   new NpgsqlCommand("""
                                     SELECT * FROM person 
                                     LEFT JOIN personality ON person.id = personality.person_id 
                                     LEFT JOIN password ON person.id = password.person_id
                                        WHERE person.email = @email
                                     """,
                       connection))
            {
                command.Parameters.AddWithValue("@email", email);

                using (var reader = command.ExecuteReader())
                {
                    
                    while (reader.Read())
                    {
                        if (reader.GetString(18) == password)
                        {
                            Console.WriteLine("You are logged in");
                            return ReadToPerson(reader);
                        }
                    }
                    
                }
            }
        }
        return null;
    }

    public byte[] GetSaltFromPerson(string email)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            using (var command =
                   new NpgsqlCommand($"SELECT salt FROM person LEFT JOIN password ON person.id = password.person_id WHERE person.email = @email",
                       connection))
            {
                command.Parameters.AddWithValue("@email", email);
                try
                {
                    byte[] salt = Convert.FromBase64String(command.ExecuteScalar().ToString());
                    return salt;
                }
                catch(NullReferenceException ex)
                {
                    Console.WriteLine($"Email not valid: {ex.Message}");
                    return null;
                }
                
            }
        }

        return null;
    }

    public void SaveSession(Guid personId, DateTime startingDate, string sessionToken)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand(
                       """
                       INSERT INTO session (session, startingDate, person_id)
                                     VALUES (@Session, @StartingDate, @PersonId);
                       """, connection))
            {
                command.Parameters.AddWithValue("@Session", sessionToken);
                command.Parameters.AddWithValue("@StartingDate", startingDate);
                command.Parameters.AddWithValue("@PersonId", personId);

                command.ExecuteNonQuery();
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
}