using KamerConnect.Models;
using KamerConnect.Repositories;
using KamerConnect.Utils;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Npgsql;

namespace KamerConnect.DataAccess.Postgres.Repositories;

public class HousePreferenceRepository : IHousePreferenceRepository
{
    private readonly string _connectionString;

    public HousePreferenceRepository()
    {
        _connectionString = EnvironmentUtils.GetConnectionString();
    }

    public void UpdateHousePreferences(HousePreferences housePreferences)
    {
        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var updateCommand = new NpgsqlCommand(
                            """
                    UPDATE house_preferences
                    SET type = @Type::house_type,
                        min_price = @MinPrice,
                        max_price = @MaxPrice,
                        city = @City,
                        city_geolocation = ST_SetSRID(ST_MakePoint(@x, @y), 4326),
                        surface = @Surface,
                        residents = @Residents,
                        smoking = @Smoking::preference_choice,
                        pet = @Pet::preference_choice,
                        interior = @Interior::preference_choice,
                        parking = @Parking::preference_choice,
                        search_area_id = @SearchAreaId::uuid
                    WHERE id = @Id::uuid;
                    """, connection))
                {
                    Guid newSearchAreaId = CreateSearchArea(housePreferences.SearchArea, connection);

                    updateCommand.Parameters.AddWithValue("@Type", housePreferences.Type.ToString());
                    updateCommand.Parameters.AddWithValue("@MinPrice", housePreferences.MinBudget);
                    updateCommand.Parameters.AddWithValue("@MaxPrice", housePreferences.MaxBudget);
                    updateCommand.Parameters.AddWithValue("@City", housePreferences.City);
                    updateCommand.Parameters.AddWithValue("@x", housePreferences.CityGeolocation.X);
                    updateCommand.Parameters.AddWithValue("@y", housePreferences.CityGeolocation.Y);
                    updateCommand.Parameters.AddWithValue("@Surface", housePreferences.SurfaceArea);
                    updateCommand.Parameters.AddWithValue("@Residents", housePreferences.Residents);
                    updateCommand.Parameters.AddWithValue("@Smoking", housePreferences.Smoking.ToString());
                    updateCommand.Parameters.AddWithValue("@Pet", housePreferences.Pet.ToString());
                    updateCommand.Parameters.AddWithValue("@Interior", housePreferences.Interior.ToString());
                    updateCommand.Parameters.AddWithValue("@Parking", housePreferences.Parking.ToString());
                    updateCommand.Parameters.AddWithValue("@SearchAreaId", newSearchAreaId);

                    updateCommand.Parameters.AddWithValue("@Id", housePreferences.Id);

                    updateCommand.ExecuteNonQuery();
                    RemoveSearchArea(housePreferences.SearchArea.Id, connection);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public HousePreferences? GetHousePreferences(Guid personId)
    {
        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(
                           """
                   SELECT hp.min_price, hp.max_price, hp.city, ST_AsText(hp.city_geolocation), hp.surface, hp.type, hp.residents, hp.smoking, hp.pet, hp.interior, hp.parking, hp.id, sa.id, sa.range, sa.profile, ST_AsText(sa.geometry)
                   FROM house_preferences hp
                   INNER JOIN person p ON p.house_preferences_id = hp.id
                   LEFT JOIN search_area sa ON hp.search_area_id = i.id
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
                                new WKTReader().Read(reader.GetString(3)) as Point,
                                reader.GetDouble(4),
                                EnumUtils.Validate<HouseType>(reader.GetString(5)),
                                reader.GetInt32(6),
                                EnumUtils.Validate<PreferenceChoice>(reader.GetString(7)),
                                EnumUtils.Validate<PreferenceChoice>(reader.GetString(8)),
                                EnumUtils.Validate<PreferenceChoice>(reader.GetString(9)),
                                EnumUtils.Validate<PreferenceChoice>(reader.GetString(10)),
                                reader.GetGuid(11),
                                new SearchArea(reader.GetGuid(12), reader.GetInt32(13), EnumUtils.Validate<Profile>(reader.GetString(14)), new WKTReader().Read(reader.GetString(15)) as Polygon)
                            );
                        }
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

    public Guid Create(HousePreferences housePreferences, Guid personId)
    {
        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        Guid iscochroneId = CreateSearchArea(housePreferences.SearchArea, connection);
                        Guid housePreferenceId = CreateHousePreferencesRecord(housePreferences, iscochroneId, connection);
                        AddHousePreferencesToPerson(personId, housePreferenceId, connection);

                        transaction.Commit();

                        return housePreferenceId;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        catch (NpgsqlException e)
        {
            Console.WriteLine($"Error occurred while creating housePreference in DB: {e.Message}");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error occurred while creating housePreference: {e.Message}");
            throw;
        }
    }

    private Guid CreateHousePreferencesRecord(HousePreferences housePreferences, Guid iscochroneId, NpgsqlConnection connection)
    {
        try
        {
            using (var command = new NpgsqlCommand(
                       """
                            INSERT INTO house_preferences (id, type, min_price, max_price, city, city_geolocation, surface, residents, smoking, pet, interior, parking, search_area_id)
                            VALUES (@Id::uuid,
                                    @Type::house_type,
                                    @MinPrice,
                                    @MaxPrice,
                                    @City,
                                    ST_SetSRID(ST_MakePoint(@x, @y), 4326),
                                    @Surface,
                                    @Residents,
                                    @Smoking::preference_choice,
                                    @Pet::preference_choice,
                                    @Interior::preference_choice,
                                    @Parking::preference_choice,
                                    @IscochroneId::uuid)
                            RETURNING id;
                           """, connection))
            {

                command.Parameters.AddWithValue("@Id", housePreferences.Id);
                command.Parameters.AddWithValue("@Type", housePreferences.Type.ToString());
                command.Parameters.AddWithValue("@MinPrice", housePreferences.MinBudget);
                command.Parameters.AddWithValue("@MaxPrice", housePreferences.MaxBudget);
                command.Parameters.AddWithValue("@City", housePreferences.City);
                command.Parameters.AddWithValue("@x", housePreferences.CityGeolocation.X);
                command.Parameters.AddWithValue("@y", housePreferences.CityGeolocation.Y);
                command.Parameters.AddWithValue("@Surface", housePreferences.SurfaceArea);
                command.Parameters.AddWithValue("@Residents", housePreferences.Residents);
                command.Parameters.AddWithValue("@Smoking", housePreferences.Smoking.ToString());
                command.Parameters.AddWithValue("@Pet", housePreferences.Pet.ToString());
                command.Parameters.AddWithValue("@Interior", housePreferences.Interior.ToString());
                command.Parameters.AddWithValue("@Parking", housePreferences.Parking.ToString());
                command.Parameters.AddWithValue("@IscochroneId", iscochroneId);

                var result = command.ExecuteScalar() ?? throw new InvalidOperationException();
                return (Guid)result;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void AddHousePreferencesToPerson(Guid personId, Guid housePreferencesId, NpgsqlConnection connection)
    {
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

    private Guid CreateSearchArea(SearchArea searchArea, NpgsqlConnection connection)
    {
        try
        {
            using (var command = new NpgsqlCommand($"""
                                                        INSERT INTO search_area (
                                                            range,
                                                            profile,
                                                            geometry
                                                        )
                                                        VALUES (
                                                            @range,
                                                            @profile::travel_profile,
                                                            ST_SetSRID(ST_GeomFromText(@geometry), 4326)
                                                        ) RETURNING id;
                                                        """, connection))
            {
                command.Parameters.AddWithValue("@range", searchArea.Range);
                command.Parameters.AddWithValue("@profile", searchArea.Profile.ToString());
                command.Parameters.AddWithValue("@geometry", searchArea.Geometry.ToText());

                var searchAreaId = command.ExecuteScalar() as Guid?;
                if (!searchAreaId.HasValue)
                    throw new InvalidOperationException("Failed to retrieve the ID of the created search area.");

                return searchAreaId.Value;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void RemoveSearchArea(Guid searchAreaId, NpgsqlConnection connection)
    {
        try
        {
            using (var command = new NpgsqlCommand(
                """
            DELETE FROM search_area
            WHERE id = @SearchAreaId;
            """, connection))
            {
                command.Parameters.AddWithValue("@SearchAreaId", searchAreaId);
                command.ExecuteNonQuery();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error occurred while removing searchArea: {e.Message}");
            throw;
        }
    }

}
