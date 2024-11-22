using System.Reflection;
using DotNetEnv;
using DbUp;
using KamerConnect;
using KamerConnect.DataAccess.Postgres.Repositys;
using Npgsql;
using KamerConnect.EnvironmentVariables;
using KamerConnect.Models;
using KamerConnect.Services;


class Program
{
    static int Main()
    {

        EnvVariables.Load();

        // var host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
        // var port = Environment.GetEnvironmentVariable("POSTGRES_PORT");
        // var database = Environment.GetEnvironmentVariable("POSTGRES_DB");
        // var username = Environment.GetEnvironmentVariable("POSTGRES_USER");
        // var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
        //
        // if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(port) ||
        //     string.IsNullOrEmpty(database) || string.IsNullOrEmpty(username) ||
        //     string.IsNullOrEmpty(password))
        // {
        //     Console.WriteLine("Database environment variables are missing. Please check your .env file.");
        //     return -1;
        // }
        //
        // var connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};";
        //
        // var upgrader = DeployChanges.To
        //     .PostgresqlDatabase(connectionString)
        //     .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
        //     .LogToConsole()
        //     .Build();
        //
        // var result = upgrader.PerformUpgrade();
        //
        // if (!result.Successful)
        // {
        //     Console.WriteLine($"An error occurred while migrating the PostgreSQL database: {result.Error}");
        //     return -1;
        // }
        //
        // Console.WriteLine("Migrated PostgreSQL database successfully.");
        //
        // return 0;
        var personDataAcces = new PersonRepository();
        var authenDataAcces = new AuthenticationRepository();
        var personSerivce = new PersonService(personDataAcces);
        var authService = new AuthenticationService(personSerivce, authenDataAcces);

        //Console.WriteLine(personSerivce.GetPersonById("7a799ba5-4705-4ab6-8610-14e847df5586").ToString());
        //authService.Authenticate("niek2004@icloud.com", "coolheid");
        //authService.Register(new Person("janjan@gmail.com", "henk", null, "bosman", null, new DateTime(1920, 2, 2), Gender.Female, Role.Offering, null), "henkje");
        authService.Authenticate("email@gmail.com", "henkje");
        
        return 0;
    }

}
