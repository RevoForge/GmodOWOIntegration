using GmodOWOIntegration;
using Microsoft.Data.Sqlite;
using SQLitePCL;
using static GmodOWOIntegration.GameFinder;

[Serializable]
class GmodOWOData
{
    public required string damageType;
    public required string direction;
}

class GmodDatabaseWatcher
{
    private string dbPath;
    private DateTime lastModifiedTime;

    public GmodDatabaseWatcher(bool isMultiplayer)
    {
        dbPath = isMultiplayer
            ? GetGarrysModInstallPath() + "\\garrysmod\\sv.db"  // Multiplayer database path
            : GetGarrysModInstallPath() + "\\garrysmod\\cl.db"; // Single-player database path
    }

    public void StartWatching()
    {
        // Initialize the SQLite provider
        Batteries.Init();
        Console.WriteLine($"Checking for database file at {dbPath}");
        if (!WaitForDatabaseFile())
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
                    lastModifiedTime = currentModifiedTime;
                    ProcessNewData();
                }

                // Sleep for 0.1 seconds before checking again
                Thread.Sleep(100);
            }
        });
    }

    private bool WaitForDatabaseFile()
    {
        int maxRetries = 60; // Check every 5 seconds for 5 minutes
        int retries = 0;

        while (!File.Exists(dbPath) && retries < maxRetries)
        {
            Console.Write($"\rDatabase file not found. Retrying in 5 seconds... ({retries + 1}/{maxRetries})");
            Thread.Sleep(5000); // Wait for 5 seconds
            retries++;
        }

        return File.Exists(dbPath);
    }

    private void ProcessNewData()
    {
        try
        {
            using SqliteConnection connection = new($"Data Source={dbPath};Mode=ReadOnly;");
            connection.Open();

            string query = "SELECT damage_type, direction FROM damage_data ORDER BY id DESC LIMIT 1";
            using SqliteCommand command = new(query, connection);
            using SqliteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                GmodOWOData jsonData = new()
                {
                    damageType = (string)reader["damage_type"],
                    direction = (string)reader["direction"],
                };

                if (!string.IsNullOrEmpty(jsonData.damageType) && !string.IsNullOrEmpty(jsonData.direction))
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
