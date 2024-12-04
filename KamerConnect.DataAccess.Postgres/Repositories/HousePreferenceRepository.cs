using KamerConnect.Models;
using KamerConnect.Repositories;
using KamerConnect.Utils;
using Npgsql;

namespace KamerConnect.DataAccess.Postgres.Repositories;

public class HousePreferenceRepository : IHousePreferenceRepository
{
    private readonly string connectionString;

    public HousePreferenceRepository()
    {
        connectionString = EnvironmentUtils.GetConnectionString();
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
                        city = @City,
                        surface = @Surface,
                        residents = @Residents,
                        smoking = @Smoking::preference_choice,
                        pet = @Pet::preference_choice,
                        interior = @Interior::preference_choice,
                        parking = @Parking::preference_choice
                    WHERE id = @Id::uuid;
                    """, connection))
            {
                updateCommand.Parameters.AddWithValue("@Type", housePreferences.Type.ToString());
                updateCommand.Parameters.AddWithValue("@MinPrice", housePreferences.MinBudget);
                updateCommand.Parameters.AddWithValue("@MaxPrice", housePreferences.MaxBudget);
                updateCommand.Parameters.AddWithValue("@City", housePreferences.City);
                updateCommand.Parameters.AddWithValue("@Surface", housePreferences.SurfaceArea);
                updateCommand.Parameters.AddWithValue("@Residents", housePreferences.Residents);
                updateCommand.Parameters.AddWithValue("@Smoking", housePreferences.Smoking.ToString());
                updateCommand.Parameters.AddWithValue("@Pet", housePreferences.Pet.ToString());
                updateCommand.Parameters.AddWithValue("@Interior", housePreferences.Interior.ToString());
                updateCommand.Parameters.AddWithValue("@Parking", housePreferences.Parking.ToString());

                updateCommand.Parameters.AddWithValue("@Id", housePreferences.Id);

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
                   SELECT hp.min_price, hp.max_price, hp.city, hp.surface, hp.type, hp.residents, hp.smoking, hp.pet, hp.interior, hp.parking, hp.id
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
                            reader.GetString(2),
                            reader.GetDouble(3),
                            EnumUtils.Validate<HouseType>(reader.GetString(4)),
                            reader.GetInt32(5),
                            EnumUtils.Validate<PreferenceChoice>(reader.GetString(6)),
                            EnumUtils.Validate<PreferenceChoice>(reader.GetString(7)),
                            EnumUtils.Validate<PreferenceChoice>(reader.GetString(8)),
                            EnumUtils.Validate<PreferenceChoice>(reader.GetString(9)),
                            reader.GetGuid(10)
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
                        INSERT INTO house_preferences (id, type, min_price, max_price, city, surface, residents, smoking, pet, interior, parking)
                        VALUES (@Id::uuid, 
                                @Type::house_type, 
                                @MinPrice, 
                                @MaxPrice, 
                                @City,
                                @Surface, 
                                @Residents, 
                                @Smoking::Preference_choice,
                                @Pet::Preference_choice,
                                @Interior::Preference_choice, 
                                @Parking::Preference_choice)
                        RETURNING id;
                       """, connection))
            {
                
                command.Parameters.AddWithValue("@Id", housePreferences.Id);
                command.Parameters.AddWithValue("@Type", housePreferences.Type.ToString());
                command.Parameters.AddWithValue("@MinPrice", housePreferences.MinBudget);
                command.Parameters.AddWithValue("@MaxPrice", housePreferences.MaxBudget);
                command.Parameters.AddWithValue("@City", housePreferences.City);
                command.Parameters.AddWithValue("@Surface", housePreferences.SurfaceArea);
                command.Parameters.AddWithValue("@Residents", housePreferences.Residents);
                command.Parameters.AddWithValue("@Smoking", housePreferences.Smoking.ToString());
                command.Parameters.AddWithValue("@Pet", housePreferences.Pet.ToString());
                command.Parameters.AddWithValue("@Interior", housePreferences.Interior.ToString());
                command.Parameters.AddWithValue("@Parking", housePreferences.Parking.ToString());
                var result = command.ExecuteScalar() ?? throw new InvalidOperationException();
                return (Guid)result;
            }
        }
    }

    public void AddHousePreferences(Guid personId, Guid housePreferencesId)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            using (var updateCommand = new NpgsqlCommand(
                       """
                       UPDATE person
                       SET house_preferences_id = @HousePreferencesId::uuid
                       WHERE id = @PersonId::uuid;
                       """, connection))
            {
                
                updateCommand.Parameters.AddWithValue("@HousePreferencesId", housePreferencesId);
                updateCommand.Parameters.AddWithValue("@PersonId", personId);
                
                updateCommand.ExecuteNonQuery();
            }
        }
    }
}
