using KamerConnect.Models;
using KamerConnect.Repositories;
using KamerConnect.Utils;
using Npgsql;

namespace KamerConnect.DataAccess.Postgres.Repositories;

public class HouseRepository : IHouseRepository
{
    private readonly string ConnectionString = EnvironmentUtils.GetConnectionString();

    private string CreateHouseRecord(House house, NpgsqlConnection connection)
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

            return houseId.Value.ToString();
        }
    }


    private void CreateHouseImages(string houseId, List<HouseImage> houseImages, NpgsqlConnection connection)
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

    public string CreateHouse(House house)
    {
        try
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var houseId = CreateHouseRecord(house, connection);

                        CreateHouseImages(houseId, house.HouseImages, connection);

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
}
