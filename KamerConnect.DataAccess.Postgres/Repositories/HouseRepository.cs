using System.Data.Common;
using KamerConnect.Models;
using KamerConnect.Repositories;
using KamerConnect.Utils;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Npgsql;

namespace KamerConnect.DataAccess.Postgres.Repositories;

public class HouseRepository : IHouseRepository
{
    private readonly string _connectionString = EnvironmentUtils.GetConnectionString();

    private Guid CreateHouseRecord(House house, NpgsqlConnection connection)
    {
        try
        {
            using (var command = new NpgsqlCommand($"""
                                                    INSERT INTO house (
                                                        type,
                                                        price,
                                                        description,
                                                        surface,
                                                        residents,
                                                        city,
                                                        street,
                                                        postal_code,
                                                        house_number,
                                                        house_number_addition,
                                                        house_geolocation,
                                                        available,
                                                        smoking,
                                                        pet,
                                                        interior,
                                                        parking
                                                    )
                                                    VALUES (
                                                        @type::house_type,
                                                        @price,
                                                        @description,
                                                        @surface,
                                                        @residents,
                                                        @city,
                                                        @street,
                                                        @postalCode,
                                                        @houseNumber,
                                                        @houseNumberAddition,
                                                        ST_SetSRID(ST_MakePoint(@x, @y), 4326),
                                                        @available,
                                                        @smoking::preference_choice,
                                                        @pet::preference_choice,
                                                        @interior::preference_choice,
                                                        @parking::preference_choice
                                                    ) RETURNING id;
                                                    """, connection))
            {
                command.Parameters.AddWithValue("@type", house.Type.ToString());
                command.Parameters.AddWithValue("@price", house.Price);
                command.Parameters.AddWithValue("@description", (object)house.Description ?? DBNull.Value);
                command.Parameters.AddWithValue("@surface", (object)house.Surface ?? DBNull.Value);
                command.Parameters.AddWithValue("@residents", house.Residents);
                command.Parameters.AddWithValue("@city", house.City);
                command.Parameters.AddWithValue("@street", house.Street);
                command.Parameters.AddWithValue("@postalCode", house.PostalCode);
                command.Parameters.AddWithValue("@houseNumber", house.HouseNumber);
                command.Parameters.AddWithValue("@houseNumberAddition", house.HouseNumberAddition);
                command.Parameters.AddWithValue("@x", house.HouseGeolocation.X);
                command.Parameters.AddWithValue("@y", house.HouseGeolocation.Y);
                command.Parameters.AddWithValue("@available", house.Available);
                command.Parameters.AddWithValue("@smoking", house.Smoking.ToString());
                command.Parameters.AddWithValue("@pet", house.Pet.ToString());
                command.Parameters.AddWithValue("@interior", house.Interior.ToString());
                command.Parameters.AddWithValue("@parking", house.Parking.ToString());

                var houseId = command.ExecuteScalar() as Guid?;
                if (!houseId.HasValue)
                    throw new InvalidOperationException("Failed to retrieve the ID of the created house.");

                return houseId.Value;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void CreateHouseImages(Guid houseId, List<HouseImage> houseImages, NpgsqlConnection connection)
    {
        if (houseImages == null || !houseImages.Any()) return;

        foreach (var image in houseImages)
        {
            using (var command = new NpgsqlCommand($"""
                                                        INSERT INTO house_image (
                                                            house_id,
                                                            path,
                                                            bucket
                                                        )
                                                        VALUES (
                                                            @houseId::uuid,
                                                            @path,
                                                            @bucket
                                                        );
                                                    """, connection))
            {
                command.Parameters.AddWithValue("@houseId", houseId);
                command.Parameters.AddWithValue("@path", image.Path);
                command.Parameters.AddWithValue("@bucket", image.Bucket);

                command.ExecuteNonQuery();
            }
        }
    }

    public void AddHouseToPerson(Guid houseId, Guid personId, NpgsqlConnection connection)
    {
        using (var command = new NpgsqlCommand($"""
                                                    UPDATE person
                                                    SET house_id = @houseId::uuid
                                                    WHERE id = @personId::uuid;
                                                """, connection))
        {
            command.Parameters.AddWithValue("@houseId", houseId);
            command.Parameters.AddWithValue("@personId", personId);

            var rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected == 0)
            {
                throw new InvalidOperationException(
                    "Failed to update house ID for the person. Ensure the person exists.");
            }
        }
    }

    public Guid Create(House house, Guid personId)
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
                        Guid houseId = CreateHouseRecord(house, connection);

                        CreateHouseImages(houseId, house.HouseImages, connection);
                        AddHouseToPerson(houseId, personId, connection);
                        transaction.Commit();

                        return houseId;
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
            Console.WriteLine($"Error occurred while creating house in DB: {e.Message}");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error occurred while creating house: {e.Message}");
            throw;
        }
    }

    public House? Get(Guid id)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();

            using (var command =
                       new NpgsqlCommand("""
                                         SELECT h.id, h.type, h.price, h.description, h.surface,
                                                h.residents, h.city, h.street, h.postal_code,
                                                h.house_number, h.house_number_addition, ST_AsText(h.house_geolocation), h.available, h.smoking, h.pet, h.interior, h.parking
                                         FROM house h
                                         WHERE h.id = @id::uuid;
                                         """,
                           connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var house = ReadToHouse(reader);
                        reader.Close();

                        house.HouseImages = GetHouseImages(house.Id, connection);

                        return house;
                    }
                }
            }
        }

        return null;
    }


    private List<HouseImage> GetHouseImages(Guid houseId, NpgsqlConnection connection)
    {
        var houseImages = new List<HouseImage>();

        using (var command = new NpgsqlCommand($"""
                                                SELECT path, bucket
                                                FROM house_image
                                                WHERE house_id = @houseId::uuid;
                                                """, connection))
        {
            command.Parameters.AddWithValue("@houseId", houseId);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var path = reader.GetString(0);
                    var bucket = reader.GetString(1);

                    houseImages.Add(new HouseImage(path, bucket));
                }
            }
        }

        return houseImages;
    }


    public House? GetByPersonId(Guid personId)
    {
        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var command =
                       new NpgsqlCommand($"""
                                      SELECT h.id, h.type, h.price, h.description, h.surface,
                                             h.residents, h.city, h.street, h.postal_code,
                                             h.house_number, h.house_number_addition, ST_AsText(h.house_geolocation), h.available, h.smoking, h.pet, h.interior, h.parking
                                      FROM house h
                                      INNER JOIN person p ON p.house_id = h.id
                                      WHERE p.id = @PersonId;
                                      """, connection))
                {
                    command.Parameters.AddWithValue("@PersonId", personId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var house = ReadToHouse(reader);
                            reader.Close();

                            house.HouseImages = GetHouseImages(house.Id, connection);

                            return house;
                        }
                    }
                }
            }

            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private House ReadToHouse(DbDataReader reader)
    {
        return new House
        (
            reader.GetGuid(0),
            EnumUtils.Validate<HouseType>(reader.GetString(1)),
            reader.GetDouble(2),
            reader.IsDBNull(3) ? null : reader.GetString(3),
            reader.GetInt32(4),
            reader.GetInt32(5),
            reader.GetString(6),
            reader.GetString(7),
            reader.GetString(8),
            reader.GetInt32(9),
            reader.GetString(10),
            new WKTReader().Read(reader.GetString(11)) as Point,
            new List<HouseImage>(),
            reader.GetBoolean(12),
        EnumUtils.Validate<PreferenceChoice>(reader.GetString(13)),
            EnumUtils.Validate<PreferenceChoice>(reader.GetString(14)),
            EnumUtils.Validate<PreferenceChoice>(reader.GetString(15)),
            EnumUtils.Validate<PreferenceChoice>(reader.GetString(16))
        );
    }

    public void Update(House house)
    {
        if (house.Id == null)
            throw new ArgumentException("House ID must not be null for update.");

        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new NpgsqlCommand($"""
                                                                    UPDATE house
                                                                    SET type = @type::house_type,
                                                                        price = @price,
                                                                        description = @description,
                                                                        surface = @surface,
                                                                        residents = @residents,
                                                                        city = @city,
                                                                        street = @street,
                                                                        postal_code = @postalCode,
                                                                        house_number = @houseNumber,
                                                                        house_number_addition = @houseNumberAddition,
                                                                        house_geolocation = ST_SetSRID(ST_MakePoint(@x, @y), 4326),
                                                                        available = @available,
                                                                        smoking = @smoking::preference_choice,
                                                                        pet = @pet::preference_choice,
                                                                        interior = @interior::preference_choice,
                                                                        parking = @parking::preference_choice
                                                                    WHERE id = @id::uuid;
                                                                """, connection))
                        {
                            command.Parameters.AddWithValue("@id", house.Id);
                            command.Parameters.AddWithValue("@type", house.Type.ToString());
                            command.Parameters.AddWithValue("@price", house.Price);
                            command.Parameters.AddWithValue("@description", (object)house.Description ?? DBNull.Value);
                            command.Parameters.AddWithValue("@surface", (object)house.Surface ?? DBNull.Value);
                            command.Parameters.AddWithValue("@residents", house.Residents);
                            command.Parameters.AddWithValue("@city", house.City);
                            command.Parameters.AddWithValue("@street", house.Street);
                            command.Parameters.AddWithValue("@postalCode", house.PostalCode);
                            command.Parameters.AddWithValue("@houseNumber", house.HouseNumber);
                            command.Parameters.AddWithValue("@houseNumberAddition", house.HouseNumberAddition);
                            command.Parameters.AddWithValue("@x", house.HouseGeolocation.X);
                            command.Parameters.AddWithValue("@y", house.HouseGeolocation.Y);
                            command.Parameters.AddWithValue("@available", house.Available);
                            command.Parameters.AddWithValue("@smoking", house.Smoking.ToString());
                            command.Parameters.AddWithValue("@pet", house.Pet.ToString());
                            command.Parameters.AddWithValue("@interior", house.Interior.ToString());
                            command.Parameters.AddWithValue("@parking", house.Parking.ToString());

                            command.ExecuteNonQuery();
                        }

                        UpdateHouseImages(house.Id, house.HouseImages, connection);

                        transaction.Commit();
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
            Console.WriteLine($"Error occurred while updating house in DB: {e.Message}");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error occurred while updating house: {e.Message}");
            throw;
        }
    }

    private void UpdateHouseImages(Guid houseId, List<HouseImage> houseImages, NpgsqlConnection connection)
    {
        using (var deleteCommand = new NpgsqlCommand("""
                                                         DELETE FROM house_image
                                                         WHERE house_id = @houseId::uuid;
                                                     """, connection))
        {
            deleteCommand.Parameters.AddWithValue("@houseId", houseId);
            deleteCommand.ExecuteNonQuery();
        }

        if (houseImages != null && houseImages.Any())
        {
            foreach (var image in houseImages)
            {
                using (var insertCommand = new NpgsqlCommand("""
                                                                 INSERT INTO house_image (
                                                                     house_id,
                                                                     path,
                                                                     bucket
                                                                 )
                                                                 VALUES (
                                                                     @houseId::uuid,
                                                                     @path,
                                                                     @bucket
                                                                 );
                                                             """, connection))
                {
                    insertCommand.Parameters.AddWithValue("@houseId", houseId);
                    insertCommand.Parameters.AddWithValue("@path", image.Path);
                    insertCommand.Parameters.AddWithValue("@bucket", image.Bucket);

                    insertCommand.ExecuteNonQuery();
                }
            }
        }
    }

    public List<House> GetByPreferences(HousePreferences housePreferences)
    {
        try
        {
            var houses = new List<House>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand($"""
    SELECT 
        id, 
        type, 
        price, 
        description, 
        surface, 
        residents, 
        city, 
        street, 
        postal_code, 
        house_number, 
        house_number_addition, 
        ST_AsText(house_geolocation) AS house_geolocation, 
        available, 
        smoking, 
        pet, 
        interior, 
        parking
    FROM 
        house
    WHERE 
        ST_Contains(ST_SetSRID(ST_GeomFromText(@geometry), 4326), house.house_geolocation)
        AND available = true
        AND residents <= @residents
        AND (
            @smoking::preference_choice = 'No_preference' OR smoking = @smoking::preference_choice OR smoking = 'No_preference'
        )
        AND (
            @pet::preference_choice = 'No_preference' OR pet = @pet::preference_choice OR smoking = 'No_preference'
        )
        AND (
            @interior::preference_choice = 'No_preference' OR interior = @interior::preference_choice OR smoking = 'No_preference'
        )
        AND (
            @parking::preference_choice = 'No_preference' OR parking = @parking::preference_choice OR smoking = 'No_preference'
        );
    """, connection))
                {
                    command.Parameters.AddWithValue("@housePreferenceId", housePreferences.Id);
                    command.Parameters.AddWithValue("@residents", housePreferences.Residents);
                    command.Parameters.AddWithValue("@smoking", housePreferences.Smoking.ToString());
                    command.Parameters.AddWithValue("@pet", housePreferences.Pet.ToString());
                    command.Parameters.AddWithValue("@interior", housePreferences.Interior.ToString());
                    command.Parameters.AddWithValue("@parking", housePreferences.Parking.ToString());
                    command.Parameters.AddWithValue("@geometry", housePreferences.SearchArea.Geometry.ToText());

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var house = ReadToHouse(reader);
                            houses.Add(house);
                        }
                    }
                    houses.ForEach((house) => { house.HouseImages = GetHouseImages(house.Id, connection); });
                }
            }

            return houses;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

}
