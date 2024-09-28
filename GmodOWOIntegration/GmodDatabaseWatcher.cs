using GmodOWOIntegration;
using Microsoft.Data.Sqlite;
using SQLitePCL;

[Serializable]
class GmodOWOData
{
    public required string damageType;
    public required string direction;
}

class GmodDatabaseWatcher
{

    private readonly string dbPath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\GarrysMod\\garrysmod\\cl.db";
    private DateTime lastModifiedTime;

    public void StartWatching()
    {
        // Initialize the SQLite provider
        Batteries.Init();
        int maxRetries = 60; // Check every 5 seconds for 5 minutes (60 retries * 5 seconds = 300 seconds)
        int retries = 0;

        // Continuously check for the database file
        while (!File.Exists(dbPath) && retries < maxRetries)
        {
            // Show progress on the same line
            Console.Write($"\rDatabase file not found. Retrying in 5 seconds... ({retries + 1}/{maxRetries})");
            // Wait for 5 seconds before trying again
            Thread.Sleep(5000);

            retries++;
        }

        if (!File.Exists(dbPath))
        {
            Console.WriteLine("\nDatabase file not found after 5 minutes. Exiting...");
            return;
        }

        Console.WriteLine("\nDatabase file found. Starting to watch for changes...");

        Task.Run(() =>
        {
            lastModifiedTime = File.GetLastWriteTime(dbPath);

            while (true)
            {
                DateTime currentModifiedTime = File.GetLastWriteTime(dbPath);

                // Check if the database file has been modified
                if (currentModifiedTime > lastModifiedTime)
                {
                    //Console.WriteLine("Database change detected. Processing new data...");
                    lastModifiedTime = currentModifiedTime;

                    // Process new data in the database
                    ProcessNewData();
                }

                // Sleep for 0.1 seconds before checking again
                Thread.Sleep(100);  // Polling interval set to 0.1 seconds
            }
        });
    }

    private void ProcessNewData()
    {
        try
        {
            using SqliteConnection connection = new($"Data Source={dbPath};Mode=ReadOnly;");
            connection.Open();

            // Query the database for the damage data (assumes auto-incrementing id for unique rows)
            string query = "SELECT damage_type, direction FROM damage_data ORDER BY id DESC LIMIT 1"; // Get the latest entry
            using SqliteCommand command = new(query, connection);
            using SqliteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                GmodOWOData jsonData = new()
                {
                    damageType = (string)reader["damage_type"],
                    direction = (string)reader["direction"],
                };
                if (jsonData.damageType != "" && jsonData.direction != "")
                {
                    // Process the retrieved data
                    OWOIntegration.ParseOWOData(jsonData.damageType, jsonData.direction);
                    Console.WriteLine($"Processed data: Damage Type = {jsonData.damageType}, Direction = {jsonData.direction}");
                }
                else
                {
                    Console.WriteLine("Database data is empty. No data to process.");
                }

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while reading from the database: " + ex.Message);
        }
    }
}
