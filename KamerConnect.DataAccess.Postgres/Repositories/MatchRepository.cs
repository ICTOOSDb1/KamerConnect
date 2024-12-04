using System;
using System.Collections.Generic;
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

    public List<MatchRequest> GetMatches(Guid id)
    {
        var matches = new List<MatchRequest>();

        
        using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();

            
            string query = "SELECT * FROM matchrequests WHERE person_id = @id OR house_id = @id";
            using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                
                using (DbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        matches.Add(new MatchRequest(
                            reader.GetGuid(0), 
                            reader.GetGuid(1), 
                            reader.GetString(2)  
                        ));
                    }
                }
            }

            return matches;
        }
    }
}