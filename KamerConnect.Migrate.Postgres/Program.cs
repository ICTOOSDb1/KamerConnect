﻿using System.Reflection;
using DbUp;
using KamerConnect.EnvironmentVariables;


class Program
{
    static int Main()
    {
        EnvVariables.Load();

        var host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
        var port = Environment.GetEnvironmentVariable("POSTGRES_PORT");
        var database = Environment.GetEnvironmentVariable("POSTGRES_DB");
        var username = Environment.GetEnvironmentVariable("POSTGRES_USER");
        var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");

        if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(port) ||
            string.IsNullOrEmpty(database) || string.IsNullOrEmpty(username) ||
            string.IsNullOrEmpty(password))
        {
            Console.WriteLine("Database environment variables are missing. Please check your .env file.");
            return -1;
        }

        var connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};";

        var upgrader = DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            Console.WriteLine($"An error occurred while migrating the PostgreSQL database: {result.Error}");
            return -1;
        }

        Console.WriteLine("Migrated PostgreSQL database successfully.");

        return 0;
    }

}
