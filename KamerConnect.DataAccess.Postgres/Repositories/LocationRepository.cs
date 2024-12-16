using KamerConnect.Models;
using KamerConnect.Utils;
using NetTopologySuite.Geometries;
using Npgsql;
using Coordinate = KamerConnect.Models.Coordinate;
using Location = KamerConnect.Models.Location;

namespace KamerConnect.DataAccess.Postgres.Repositories;

public class LocationRepository
{
    
    private readonly string _connectionString = EnvironmentUtils.GetConnectionString();
    
    
    public async Task<string> Save(Location location)
    {
        return "";
    }
    
    public Task<Location> Get(Guid id)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();

            using (var command =
                   new NpgsqlCommand("""
                                    SELECT person.range, person.profile, person.coordinates FROM location 
                                        LEFT JOIN house_preference ON location.id = house_preference.location_id 
                                             WHERE house_preference.id == @id::uuid
                                     """,
                       connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Location newLocation = new Location("", Profile.DrivingCar, new List<Coordinate>());
                        reader.Close();
                        
                        return Task.FromResult(newLocation);
                    }
                }
            }
        }

        return null;
    }
}