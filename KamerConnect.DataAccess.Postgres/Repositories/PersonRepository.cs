using System.Data.Common;
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

    public Person? GetPersonById(Guid id)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            using (var command =
                   new NpgsqlCommand("""
                                     SELECT 
                                         person.id,                        
                                         person.email,                     
                                         person.first_name,                
                                         person.middle_name,               
                                         person.surname,                   
                                         person.phone_number,              
                                         person.birth_date,                
                                         person.gender,                    
                                         person.role,                      
                                         person.profile_picture_path,      
                                         personality.id,            
                                         personality.school,               
                                         personality.study,                
                                         personality.description           
                                     FROM person
                                     LEFT JOIN personality ON person.id = personality.person_id
                                     WHERE person.id = @id::uuid;

                                     """,
                       connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read()) return ReadToPerson(reader);
                }
            }
        }

        return null;
    }

    public Person? GetPersonByHouseId(Guid id)
    {
        try
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var command =
                       new NpgsqlCommand("""
                                         SELECT 
                                             person.id,                        
                                             person.email,                     
                                             person.first_name,                
                                             person.middle_name,               
                                             person.surname,                   
                                             person.phone_number,              
                                             person.birth_date,                
                                             person.gender,                    
                                             person.role,                      
                                             person.profile_picture_path,
                                             personality.id,            
                                             personality.school,               
                                             personality.study,                
                                             personality.description 
                                         FROM person
                                         LEFT JOIN personality ON person.id = personality.person_id
                                         LEFT JOIN house ON person.house_id = house.id
                                         WHERE person.house_id = @id::uuid;
                                         """,
                           connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) return ReadToPerson(reader);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return null;
    }

    public Person? GetPersonByEmail(string email)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            using (var command =
                   new NpgsqlCommand("""
                                     SELECT 
                                         person.id,                        
                                         person.email,                     
                                         person.first_name,                
                                         person.middle_name,               
                                         person.surname,                   
                                         person.phone_number,              
                                         person.birth_date,                
                                         person.gender,                    
                                         person.role,                      
                                         person.profile_picture_path,      
                                         personality.id,            
                                         personality.school,               
                                         personality.study,                
                                         personality.description           
                                     FROM person
                                     LEFT JOIN personality ON person.id = personality.person_id
                                     WHERE person.email = @email;

                                     """,
                       connection))
            {
                command.Parameters.AddWithValue("@email", email);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read()) return ReadToPerson(reader);
                }
            }
        }

        return null;
    }

    public Guid CreatePerson(Person person)
    {
        try
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var command =
                       new NpgsqlCommand($"""
                                          INSERT INTO person (
                                            id, email, first_name, middle_name, surname, phone_number, birth_date, gender, role, profile_picture_path)
                                            VALUES (@Id, @Email,
                                            @FirstName,
                                            @MiddleName,
                                            @Surname,
                                            @PhoneNumber,
                                            @BirthDate,
                                            @Gender::gender,
                                            @Role::user_role,
                                            @ProfilePicturePath
                                          ) RETURNING id;
                                          """,
                           connection))
                {
                    command.Parameters.AddWithValue("@Id", person.Id);
                    command.Parameters.AddWithValue("@Email", person.Email);
                    command.Parameters.AddWithValue("@FirstName", person.FirstName);
                    command.Parameters.AddWithValue("@MiddleName", person.MiddleName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Surname", person.Surname);
                    command.Parameters.AddWithValue("@PhoneNumber", person.PhoneNumber ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@BirthDate", person.BirthDate);
                    command.Parameters.AddWithValue("@Gender", person.Gender.ToString());
                    command.Parameters.AddWithValue("@Role", person.Role.ToString());
                    command.Parameters.AddWithValue("@ProfilePicturePath",
                        person.ProfilePicturePath ?? (object)DBNull.Value);

                    var result = command.ExecuteScalar() ?? throw new InvalidOperationException();
                    return (Guid)result;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
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
            reader.GetGuid(0)
        );


        if (reader.IsDBNull(10)) return person;

        person.Personality = new Personality(
            reader.IsDBNull(11) ? null : reader.GetString(11),
            reader.IsDBNull(12) ? null : reader.GetString(12),
            reader.IsDBNull(13) ? null : reader.GetString(13)
        );

        return person;
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
                command.Parameters.AddWithValue("@Id", person.Id);
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

    public void UpdatePersonality(Guid personId, Personality personality)
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
                    updateCommand.Parameters.AddWithValue("@PersonalityId", personId);
                    updateCommand.Parameters.AddWithValue("@School", personality.School ?? string.Empty);
                    updateCommand.Parameters.AddWithValue("@Study", personality.Study ?? string.Empty);
                    updateCommand.Parameters.AddWithValue("@Description", personality.Description ?? string.Empty);

                    var rowsAffected = updateCommand.ExecuteNonQuery();

                    if (rowsAffected == 0)
                        using (var insertCommand = new NpgsqlCommand(
                                   """
                                   INSERT INTO personality (person_id, school, study, description)
                                   VALUES (@PersonalityId, @School, @Study, @Description);
                                   """, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@PersonalityId", personId);
                            insertCommand.Parameters.AddWithValue("@School", personality.School ?? string.Empty);
                            insertCommand.Parameters.AddWithValue("@Study", personality.Study ?? string.Empty);
                            insertCommand.Parameters.AddWithValue("@Description", personality.Description ?? string.Empty);

                            insertCommand.ExecuteNonQuery();
                        }
                }

                transaction.Commit();
            }
        }
    }

    public List<Person> GetPersonsByChatId(Guid chatId)
    {
        
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            using (var command =
                   new NpgsqlCommand("""
                                     SELECT 
                                         person.id,                        
                                         person.email,                     
                                         person.first_name,                
                                         person.middle_name,               
                                         person.surname,                   
                                         person.phone_number,              
                                         person.birth_date,                
                                         person.gender,                    
                                         person.role,                      
                                         person.profile_picture_path,      
                                         personality.id,            
                                         personality.school,               
                                         personality.study,                
                                         personality.description           
                                     FROM person
                                     LEFT JOIN person_chat pc ON person.id = person_chat.person_id
                                     LEFT JOIN personality ON person.id = personality.person_id
                                     WHERE person_chat.chat_id = @chatId::uuid;

                                     """,
                       connection))
            {
                command.Parameters.AddWithValue("@id", chatId);
                List<Person> persons = new List<Person>();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        persons.Add(ReadToPerson(reader));
                    }
                }
                return persons;
            }
        }
    }
    public bool CheckIfEmailExists(string email)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand("""
                                                   SELECT EXISTS (
                                                       SELECT 1
                                                       FROM person
                                                       WHERE email = @email
                                                   );
                                                   """, connection))
            {
                command.Parameters.AddWithValue("@email", email);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return reader.GetBoolean(0);
                    }
                }
            }
        }

        return false;
    }
}