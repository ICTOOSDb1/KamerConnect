using System;

namespace KamerConnect.Utils;

public class EnvironmentUtils
{
    public static string GetConnectionString()
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
