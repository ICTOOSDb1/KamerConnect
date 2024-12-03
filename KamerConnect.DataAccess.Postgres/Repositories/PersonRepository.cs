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
                    while (reader.Read())
                    {
                        return ReadToPerson(reader);
                    }
                }
            }
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
                command.Parameters.AddWithValue("@ProfilePicturePath", person.ProfilePicturePath ?? (object)DBNull.Value);

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
                    {
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
                }
                transaction.Commit();
            }
        }
    }
    
    public void UpdateHousePreferences(HousePreferences housePreferences)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            using (var updateCommand = new NpgsqlCommand(
                        """
                    UPDATE house_preferences
                    SET type = @Type::house_type,
                        min_price = @MinPrice,
                        max_price = @MaxPrice,
                        surface = @Surface,
                        residents = @Residents,
                        smoking = @Smoking::preference_choice,
                        smoking = @Pet::preference_choice,
                        smoking = @Interior::preference_choice,
                        smoking = @Parking::preference_choice,
                    WHERE id = @HousePreferencesId::uuid;
                    """, connection))
            {
                updateCommand.Parameters.AddWithValue("@HousePreferencesId", housePreferences.Id);
                updateCommand.Parameters.AddWithValue("@MinPrice", housePreferences.MinBudget);
                updateCommand.Parameters.AddWithValue("@MaxPrice", housePreferences.MaxBudget);
                updateCommand.Parameters.AddWithValue("@Surface", housePreferences.SurfaceArea);
                updateCommand.Parameters.AddWithValue("@Residents", housePreferences.Residents);
                updateCommand.Parameters.AddWithValue("@Smoking", housePreferences.Smoking.ToString());
                updateCommand.Parameters.AddWithValue("@Pet", housePreferences.Pet.ToString());
                updateCommand.Parameters.AddWithValue("@Interior", housePreferences.Interior.ToString());
                updateCommand.Parameters.AddWithValue("@Parking", housePreferences.Parking.ToString());

                updateCommand.ExecuteNonQuery();
            }
        }
    }

    public HousePreferences? GetHousePreferences(Guid personId)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            using (var command = new NpgsqlCommand(
                       """
                   SELECT hp.min_price, hp.max_price, hp.surface, hp.type, hp.residents, hp.smoking, hp.pet, hp.interior, hp.parking, hp.id
                   FROM house_preferences hp
                   INNER JOIN person p ON p.house_preferences_id = hp.id
                   WHERE p.id = @PersonId;
                   """, connection))
            {
                command.Parameters.AddWithValue("@PersonId", personId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new HousePreferences(
                            reader.GetDouble(0),
                            reader.GetDouble(1),
                            reader.GetDouble(2),
                            EnumUtils.Validate<HouseType>(reader.GetString(3)),
                            reader.GetInt32(4),
                            EnumUtils.Validate<PreferenceChoice>(reader.GetString(5)),
                            EnumUtils.Validate<PreferenceChoice>(reader.GetString(6)),
                            EnumUtils.Validate<PreferenceChoice>(reader.GetString(7)),
                            EnumUtils.Validate<PreferenceChoice>(reader.GetString(8)),
                            reader.GetGuid(9)
                        );
                    }
                }
            }
        }

        return null;
    }

    public Guid CreateHousePreferences(HousePreferences housePreferences)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand(
                       """
                       INSERT INTO house_preferences (type, min_price, max_price, surface, residents, smoking, pet, interior, parking)
                       VALUES (@Type::house_type, @MinPrice, @MaxPrice, @Surface, @Residents, @Smoking::Preference_choice, @Pet::Preference_choice, @Interior::Preference_choice, @Parking::Preference_choice)
                       RETURNING id;
                      """, connection))
            {
                command.Parameters.AddWithValue("@Type", housePreferences.Type.ToString());
                command.Parameters.AddWithValue("@MinPrice", housePreferences.MinBudget);
                command.Parameters.AddWithValue("@MaxPrice", housePreferences.MaxBudget);
                command.Parameters.AddWithValue("@Surface", housePreferences.SurfaceArea);
                command.Parameters.AddWithValue("@Residents", housePreferences.Residents);
                command.Parameters.AddWithValue("@Smoking", housePreferences.Smoking.ToString());
                command.Parameters.AddWithValue("@Pet", housePreferences.Pet.ToString());
                command.Parameters.AddWithValue("@Interior", housePreferences.Interior.ToString());
                command.Parameters.AddWithValue("@Parking", housePreferences.Parking.ToString());


                return Guid.Parse(command.ExecuteScalar()?.ToString() ?? throw new InvalidOperationException());
            }
        }
    }
}