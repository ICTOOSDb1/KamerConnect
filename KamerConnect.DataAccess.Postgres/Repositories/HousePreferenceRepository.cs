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
