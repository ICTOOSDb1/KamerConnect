using KamerConnect.Repositories;
using Microsoft.Maui.Storage;
using Npgsql;

namespace KamerConnect.DataAccess.Postgres.Repositys;

public class AuthenticationRepository : IAuthenticationRepository
{
    private readonly string connectionString;
    
    
    public AuthenticationRepository()
    {
        connectionString = GetConnectionString();
    }
    
    public void SaveSessionInDB(string personId, DateTime startingDate, string sessionToken)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand(
                       """
                       INSERT INTO session (sessiontoken, startingDate, person_id)
                                     VALUES (@Session, @StartingDate, @PersonId::uuid);
                       """, connection))
            {
                command.Parameters.AddWithValue("@Session", sessionToken);
                command.Parameters.AddWithValue("@StartingDate", startingDate);
                command.Parameters.AddWithValue("@PersonId", personId);

                command.ExecuteNonQuery();
            }
        }
    }

    public async Task SaveSessionInLS(string personId, DateTime startingDate, string sessionToken)
    {
        await SecureStorage.SetAsync("session_token", sessionToken);
    }

    public byte[] GetSaltFromPerson(string email)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            using (var command =
                   new NpgsqlCommand($"SELECT salt FROM person LEFT JOIN password ON person.id = password.person_id WHERE person.email = @email",
                       connection))
            {
                command.Parameters.AddWithValue("@email", email);
                try
                {
                    byte[] salt = Convert.FromBase64String(command.ExecuteScalar().ToString());
                    return salt;
                }
                catch(NullReferenceException ex)
                {
                    Console.WriteLine($"Email not valid: {ex.Message}");
                    return null;
                }
                
            }
        }
        return null;
    }
    
    public void AddPassword(string personId, string password, string salt)
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
    
    public string GetPassword(string person_id)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            using (var command =
                   new NpgsqlCommand($"SELECT * FROM password WHERE person_id = @personId::uuid",
                       connection))
            {
                command.Parameters.AddWithValue("@personId", person_id);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return reader.GetString(2);
                    }
                }
            }
        }
        
        return "";
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