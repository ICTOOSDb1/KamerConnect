using System.Data.Common;
using KamerConnect.Models;
using KamerConnect.Repositories;
using KamerConnect.Utils;
using Npgsql;

namespace KamerConnect.DataAccess.Postgres.Repositories;

public class MatchRepository : IMatchRepository
{
    private readonly string connectionString;

    public MatchRepository()
    {
        connectionString = EnvironmentUtils.GetConnectionString();
    }

    

    public Match[]? GetMatchesById(Guid Houseid)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            using (var command = new NpgsqlCommand("""
                                                   SELECT *
                                                   FROM matchrequests
                                                   WHERE house_id = @id::uuid or person_id = @id::uuid
                                                   """, 
                       connection))
            {
                command.Parameters.AddWithValue("@id", Houseid);

                using (var reader = command.ExecuteReader())
                {
                    var matches = new List<Match>();

                    while (reader.Read())
                    {
                        var match = readToMatch(reader);
                        if (match != null)
                        {
                            matches.Add(match);
                        }
                    }

                    try
                    {
                        return matches.ToArray();   
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                    
                }
            }
        }
    }


    public void AddMatch(Match match)
    {
        throw new NotImplementedException();
    }

    public void UpdateMatch(Match match)
    {
        throw new NotImplementedException();
    }

    public void DeleteMatch(Match match)
    {
        throw new NotImplementedException();
    }

    public Match readToMatch(DbDataReader reader)
    {
        var match = new Match(
            reader.GetGuid(0),
            reader.GetGuid(1),
            reader.GetGuid(2),
            EnumUtils.Validate<status>(reader.GetString(3)),
            "test"
        );
    return match;
    }
}