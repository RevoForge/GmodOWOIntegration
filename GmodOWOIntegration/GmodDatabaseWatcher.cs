using GmodOWOIntegration;
using Microsoft.Data.Sqlite;
using Microsoft.Win32;
using System.IO;


[Serializable]
class GmodOWOData
{
    public required string damageType;
    public required string hitbox;
    public required string direction;
}

class GmodDatabaseWatcher
{
    private static string FindGarrysModDirectory()
    {
        string steamPath = GetSteamPath();
        if (steamPath != null)
        {
            string garrysModPath = Path.Combine(steamPath, "steamapps", "common", "GarrysMod");
            if (Directory.Exists(garrysModPath))
            {
                return garrysModPath;
            }
        }
        return ""; // Return null if the directory is not found
    }

    private static string GetSteamPath()
    {
#if WINDOWS
    // Check the registry for the Steam installation path
    using RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam");
    if (key != null)
    {
        return key.GetValue("SteamPath") as string;
    }
#endif
        return ""; // Return null if the registry key is not found
    }

    // Usage
    private static string FindDatabaseFile()
    {
        string garrysModDirectory = FindGarrysModDirectory();
        if (Directory.Exists(garrysModDirectory))
        {
            string[] files = Directory.GetFiles(garrysModDirectory, "cl.db", SearchOption.AllDirectories);

            if (files.Length > 0)
            {
                return files[0]; // Return the first found instance of cl.db
            }
        }
        return ""; // Return empty string if cl.db not found
    }

    private string dbPath = FindDatabaseFile();
    private DateTime lastModifiedTime;

    public void StartWatching()
    {
        int maxRetries = 60; // Check every 5 seconds for 5 minutes (60 retries * 5 seconds = 300 seconds)
        int retries = 0;

        // Continuously check for the database file
        while (!File.Exists(dbPath) && retries < maxRetries)
        {
            // Show progress on the same line
            Console.Write($"\rDatabase file not found. Retrying in 5 seconds... ({retries + 1}/{maxRetries})");
            dbPath = FindDatabaseFile();
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
                    Console.WriteLine("Database change detected. Processing new data...");
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
            using SqliteConnection connection = new($"Data Source={dbPath};Version=3;");
            connection.Open();

            // Query the database for the damage data (assumes auto-incrementing id for unique rows)
            string query = "SELECT damage_type, hitbox, direction FROM damage_data ORDER BY id DESC LIMIT 1"; // Get the latest entry
            using SqliteCommand command = new(query, connection);
            using SqliteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                GmodOWOData jsonData = new()
                {
                    damageType = (string)reader["damage_type"],
                    hitbox = (string)reader["hitbox"],
                    direction = (string)reader["direction"],
                };
                if (jsonData.damageType != "" && jsonData.hitbox != "" && jsonData.direction != "")
                {
                    // Process the retrieved data
                    OWOIntegration.ParseOWOData(jsonData.damageType, jsonData.hitbox, jsonData.direction);
                    Console.WriteLine($"Processed data: Damage Type = {jsonData.damageType}, Hitbox = {jsonData.hitbox}, Direction = {jsonData.direction}");
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
