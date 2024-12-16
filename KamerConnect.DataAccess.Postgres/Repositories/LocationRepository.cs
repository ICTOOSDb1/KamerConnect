using KamerConnect.Models;
using KamerConnect.Utils;
using NetTopologySuite.Geometries;
using Npgsql;
using Coordinate = KamerConnect.Models.Coordinate;

namespace KamerConnect.DataAccess.Postgres.Repositories;

public class LocationRepository
{
    
    private readonly string _connectionString = EnvironmentUtils.GetConnectionString();
    
    
}