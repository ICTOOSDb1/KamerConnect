# Kamer Connect

## Getting Started with the Project

### Initial Setup

1. **Configure Environment Variables**  
   Locate the `.env.example` file in the KamerConnect.EnvironmentVariables directory. Copy this file and rename the copy to `local.env`. Then, open the `local.env` file and fill in the necessary variables to match your setup. MINIO_KEY and MINIO_SECRET can be left empty until one of the next steps.

2. **Starting the application requirements**  
   Make sure Docker is running. Then, open a terminal in the project’s root directory and run the following command to start the required services for the application:

   ```bash
   docker-compose up -d
   ```

3. **Exectute database migrations**
   To get your database up-to-date with the current database version we created a project KamerConnect.Migrate.Postgres. When executing this projects Program.cs it automatically runs all the database upscripts. If there is a change in the database later on you can just execute this project again, because it keeps track off your executed upscripts.

4. **Starting application**  
    Open the project in your preferred IDE and start the .NET application.

## Working on the project

### Code conventions
   We have a few rules about our code conventions which are all described in our technical documentation.

### Adding a migration
   If you want to create a new upscript you can just add a .sql file in the scripts folder in the KamerConnect.Migrate.Postgres project. To add a new upscript for a change in the database we have a few rules
   1. Add a version number in the upscript name like this: V1_DESCRIPTION_HERE.
   2. If possible make the upscript reusable for example add a IF NOT EXISTS.
   3. Never change an already existing migration.