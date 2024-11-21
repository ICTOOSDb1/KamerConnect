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

3. **Minio setup**
   If you go to your minio instance on localhost:9000 you can login with the MINIO_USER and MINIO_PASS you entered in your local.env file.
   When you are logged in you need to go to the access key section and create a new access key. You don't need to change any settings here and you can just click on create. You will get a pop up now that will show the generated key and secret one last time, copy these credentials in the MINIO_KEY and MINIO_SECRET in your local.env file.

4. **Starting application**  
    Open the project in your preferred IDE and start the .NET application.
