using KamerConnect.Models;
using KamerConnect.Repositories;
using Npgsql;

namespace KamerConnect.DataAccess.Postgres.Repositys;

public class AuthenticationRepository : IAuthenticationRepository
{
    private readonly string connectionString;

    public AuthenticationRepository()
    {
        connectionString = GetConnectionString();
    }

    public void SaveSession(string personId, DateTime startingDate, string sessionToken)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand(
                       "INSERT INTO session (sessiontoken, startingDate, person_id) VALUES (@Session, @StartingDate, @PersonId::uuid); ",
                       connection))
            {
                command.Parameters.AddWithValue("@Session", sessionToken);
                command.Parameters.AddWithValue("@StartingDate", startingDate);
                command.Parameters.AddWithValue("@PersonId", personId);

                command.ExecuteNonQuery();
            }
        }
    }

    public Session? GetSession(string personId)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand(
                       "SELECT * FROM session WHERE person_id = @personId::uuid;", connection))
            {
                command.Parameters.AddWithValue("@personId", personId);

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("No session found for the given personId.");
                        return null;
                    }

                    while (reader.Read())
                    {
                        return new Session(
                            reader.GetString(0),
                            reader.GetDateTime(1),
                            reader.GetString(2)
                        );
                    }
                }
            }
        }

        return null;
    }

    public Session? GetSessionWithLocalToken(string? localSessionToken)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand(
                       "SELECT * FROM session WHERE sessiontoken = @sessiontoken;", connection))
            {
                command.Parameters.AddWithValue("@sessiontoken", localSessionToken);

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("No session found for the given token.");
                        return null;
                    }

                    while (reader.Read())
                    {
                        return new Session(
                            reader.GetString(0),
                            reader.GetDateTime(1),
                            reader.GetGuid(2).ToString()
                        );
                    }
                }
            }
        }

        return null;
    }

    public void UpdateSessionDate(string sessionToken)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand(
                       "UPDATE session SET startingdate = @startingdate WHERE sessiontoken = @sessionToken",
                       connection))
            {
                command.Parameters.AddWithValue("@sessionToken", sessionToken);
                command.Parameters.AddWithValue("@startingdate", DateTime.Now);

                command.ExecuteNonQuery();
            }
        }
    }

    public void RemoveSession(string? sessionToken)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand(
                       "DELETE FROM session WHERE sessiontoken = @sessionToken", connection))
            {
                command.Parameters.AddWithValue("@sessionToken", sessionToken);


                command.ExecuteNonQuery();
            }
        }
    }

    public byte[]? GetSaltFromPerson(string email)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            using (var command =
                   new NpgsqlCommand(
                       $"SELECT salt FROM person LEFT JOIN password ON person.id = password.person_id WHERE person.email = @email",
                       connection))
            {
                command.Parameters.AddWithValue("@email", email);
                try
                {
                    byte[]? salt = Convert.FromBase64String(command.ExecuteScalar().ToString());
                    return salt;
                }
                catch (NullReferenceException ex)
                {
                    Console.WriteLine($"Email not valid: {ex.Message}");
                    return null;
                }
            }
        }

        return null;
    }

    public void AddPassword(string? personId, string password, string salt)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand(
                       """
                       INSERT INTO password (salt, hashed_password, person_id)
                                     VALUES (@Salt, @HashedPassword, @PersonId::uuid);
                       """, connection))
            {
                command.Parameters.AddWithValue("@Salt", salt);
                command.Parameters.AddWithValue("@HashedPassword", password);
                command.Parameters.AddWithValue("@PersonId", personId);

                command.ExecuteNonQuery();
            }
        }
    }

    public string? GetPassword(string personId)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            using (var command =
                   new NpgsqlCommand($"SELECT * FROM password WHERE person_id = @personId::uuid",
                       connection))
            {
                command.Parameters.AddWithValue("@personId", personId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return reader.GetString(2);
                    }
                }
            }
        }

        return null;
    }

    private string GetConnectionString()
    {
        var host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
        var port = Environment.GetEnvironmentVariable("POSTGRES_PORT");
        var database = Environment.GetEnvironmentVariable("POSTGRES_DB");
        var username = Environment.GetEnvironmentVariable("POSTGRES_USER");
        var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");

        if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(port) ||
            string.IsNullOrEmpty(database) || string.IsNullOrEmpty(username) ||
            string.IsNullOrEmpty(password))
        {
            throw new("Database environment variables are missing. Please check your .env file.");
        }

        return $"Host={host};Port={port};Database={database};Username={username};Password={password};";
    }
}