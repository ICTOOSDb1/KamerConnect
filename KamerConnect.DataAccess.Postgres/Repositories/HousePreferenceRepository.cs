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
                        price = @Price,
                        surface = @Surface
                    WHERE id = @HousePreferencesId::uuid;
                    """, connection))
            {
                updateCommand.Parameters.AddWithValue("@HousePreferencesId", housePreferences.Id);
                updateCommand.Parameters.AddWithValue("@Type", housePreferences.Type.ToString());
                updateCommand.Parameters.AddWithValue("@Price", housePreferences.Budget);
                updateCommand.Parameters.AddWithValue("@Surface", housePreferences.SurfaceArea);

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
                   SELECT hp.id, hp.type, hp.price, hp.surface, hp.residents
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
                            reader.GetDouble(2),
                            reader.GetDouble(3),
                            EnumUtils.Validate<HouseType>(reader.GetString(1)),
                            reader.GetInt32(4),
                            reader.GetGuid(0)
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
