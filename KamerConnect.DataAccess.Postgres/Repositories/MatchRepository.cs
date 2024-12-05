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
                                                   WHERE house_id = @id::uuid;
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
            reader.GetString(4)
        );
    return match;
    }
}