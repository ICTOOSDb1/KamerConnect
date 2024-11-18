using System;
using System.IO;
using System.Reflection;
using DotNetEnv;
using DbUp;
using KamerConnect.DataAccess.Postgres.Repositys;
using KamerConnect.Models;
using KamerConnect.Services;

class Program
{
    // static int Main()
    // {
    //     var rootEnvPath = Path.Combine(Directory.GetCurrentDirectory(), "..", ".env");
    //     var binEnvPath = Path.Combine(Directory.GetCurrentDirectory(), "../../../..", ".env");
    //
    //     if (File.Exists(rootEnvPath)) { Env.Load(rootEnvPath); } 
    //     else if (File.Exists(binEnvPath)) { Env.Load(binEnvPath); }
    //
    //     var host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
    //     var port = Environment.GetEnvironmentVariable("POSTGRES_PORT");
    //     var database = Environment.GetEnvironmentVariable("POSTGRES_DB");
    //     var username = Environment.GetEnvironmentVariable("POSTGRES_USER");
    //     var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
    //
    //     if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(port) ||
    //         string.IsNullOrEmpty(database) || string.IsNullOrEmpty(username) ||
    //         string.IsNullOrEmpty(password))
    //     {
    //         Console.WriteLine("Database environment variables are missing. Please check your .env file.");
    //         return -1;
    //     }
    //
    //     var connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};";
    //
    //     var upgrader = DeployChanges.To
    //         .PostgresqlDatabase(connectionString)
    //         .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
    //         .LogToConsole()
    //         .Build();
    //
    //     var result = upgrader.PerformUpgrade();
    //
    //     if (!result.Successful)
    //     {
    //         Console.WriteLine($"An error occurred while migrating the PostgreSQL database: {result.Error}");
    //         return -1;
    //     }
    //
    //     Console.WriteLine("Migrated PostgreSQL database successfully.");
    //
    //     return 0;
    // }

    
    static int Main()
    {
        var personDataAccess = new PersonRepository();
        var personService = new PersonService(personDataAccess);
        
        Person person = personService.GetPerson("f525276a-1d32-40fc-aef5-9757ba3bcb23");
        Console.WriteLine(person.ToString());
        return 0;
    }
}
