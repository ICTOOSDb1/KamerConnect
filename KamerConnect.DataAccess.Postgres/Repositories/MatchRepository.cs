using System.Data.Common;
using KamerConnect.Models;
using KamerConnect.Repositories;
using KamerConnect.Utils;
using Npgsql;

namespace KamerConnect.DataAccess.Postgres.Repositories;

public class MatchRepository : IMatchRepository
{
    private readonly string _connectionString;

    public MatchRepository()
    {
        _connectionString = EnvironmentUtils.GetConnectionString();
    }

    public List<Match> GetPendingMatchesById(Guid Id)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();

            using (var command = new NpgsqlCommand("""
                                                   SELECT *
                                                   FROM matchrequests
                                                   WHERE (house_id = @id::uuid or person_id = @id::uuid) and status = 'Pending'
                                                   """,
                       connection))
            {
                command.Parameters.AddWithValue("@id", Id);

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

                    return matches;
                }
            }
        }
    }

    public void UpdateStatusMatch(Match match, status status)
    {
        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string updateQuery = @"
                UPDATE matchrequests
                SET status = @status::matchrequest_status
                WHERE id = @id";

                using (var command = new NpgsqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@status", status.ToString());
                    command.Parameters.AddWithValue("@id", match.matchId);

                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred while updating Match in DB: {ex.Message}");
            throw;
        }
    }

    public Match readToMatch(DbDataReader reader)
    {
        var match = new Match(
            reader.GetGuid(0),
            reader.GetGuid(1),
            reader.GetGuid(2),
            EnumUtils.Validate<status>(reader.GetString(3)),
            "Placeholder motivation"
        );
        return match;
    }
}