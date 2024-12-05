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

    public Match? GetMatchById(Guid id)
    {
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var command =
                       new NpgsqlCommand("""
                                         SELECT *
                                         FROM matchrequests
                                         WHERE person_id = @id::uuid;

                                         """,
                           connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return readToMatch(reader);
                        }
                    }
                }
            }

            return null;
        }
    }

    public Match[]? GetMatchesByHouseId(Guid Houseid)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            using (var command = new NpgsqlCommand("""
                                                   SELECT *
                                                   FROM matchrequests
                                                   WHERE (house_id = @id::uuid or person_id = @id::uuid) and status = 'Pending'
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

                    return matches.Count > 0 ? matches.ToArray() : null;
                }
            }
        }
    }


    public void AddMatch(Match match)
    {
        throw new NotImplementedException();
    }

    public void UpdateMatch(Match match , status status)
    {
        if (match.matchId == Guid.Empty)
            throw new ArgumentException("MatchId must not be null for update.");
        
        try
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
        
                string updateQuery = @"
            UPDATE matchrequests
            SET status = @status::matchrequest_status
            WHERE id = @id";

                using (var command = new NpgsqlCommand(updateQuery, connection))
                {
                    // Set the parameters, converting the enum to its string representation
                    command.Parameters.AddWithValue("@status", status.ToString());
                    command.Parameters.AddWithValue("@id", match.matchId);
            
                    try
                    {
                        // Execute the command
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Handle the error (e.g., log it)
                        Console.WriteLine($"Error updating match request status: {ex.Message}");
                    }
                }
            }
        }
        catch (NpgsqlException e)
        {
            Console.WriteLine($"Error occurred while updating Match in DB: {e.Message}");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error occurred while updating Match: {e.Message}");
            throw;
        }
        
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