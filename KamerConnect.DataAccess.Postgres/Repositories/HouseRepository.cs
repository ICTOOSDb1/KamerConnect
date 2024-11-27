using System.Data.Common;
using KamerConnect.Models;
using KamerConnect.Repositories;
using KamerConnect.Utils;
using Npgsql;
using KamerConnect.Services;
namespace KamerConnect.DataAccess.Postgres.Repositories;

public class HouseRepository : IHouseRepository
{
    private readonly string _connectionString = EnvironmentUtils.GetConnectionString();

    private Guid CreateHouseRecord(House house, NpgsqlConnection connection)
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
        house_number_addition
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
        @houseNumberAddition
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
            command.Parameters.AddWithValue("@houseNumberAddition", (object)house.HouseNumberAddition ?? DBNull.Value);

            var houseId = command.ExecuteScalar() as Guid?;
            if (!houseId.HasValue)
                throw new InvalidOperationException("Failed to retrieve the ID of the created house.");

            return houseId.Value;
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
                throw new InvalidOperationException("Failed to update house ID for the person. Ensure the person exists.");
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

    public House Get(Guid id)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();

            using (var command =
                   new NpgsqlCommand($"SELECT * FROM house " +
                                     $"WHERE house.id = @id::uuid",
                       connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return ReadToHouse(reader);
                    }
                }
            }
        }

        return null;
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
            new List<HouseImage>()
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
                            house_number_addition = @houseNumberAddition
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
                            command.Parameters.AddWithValue("@houseNumberAddition", (object)house.HouseNumberAddition ?? DBNull.Value);

                            command.ExecuteNonQuery();
                        }

                        UpdateHouseImages((Guid)house.Id, house.HouseImages, connection);

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
        // Moet nog geimplementeerd worden
    }


}
