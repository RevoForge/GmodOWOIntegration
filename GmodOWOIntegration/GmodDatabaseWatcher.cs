using GmodOWOIntegration;
using Newtonsoft.Json;
using SQLitePCL;
using static GmodOWOIntegration.GameFinder;
[Serializable]
class GmodOWOData
{
    public required string damage_type { get; set; }
    public required string direction { get; set; }
}

class GmodDatabaseWatcher
{
    private readonly string dbPath = Path.Combine(GetGarrysModInstallPath(), "garrysmod", "data", "damage_data.json");
    private DateTime currentModifiedTime;
    private DateTime lastReadTime;

    public void StartWatching()
    {
        Console.WriteLine($"Checking for database file at {dbPath}");

        // Check if the file exists, and if not, create it
        if (File.Exists(dbPath))
        {
            Console.WriteLine("\nDatabase file found. Starting to watch for changes...");
        }
        else
        {
            // If the file does not exist, create it with initial data
            Console.WriteLine("\nDatabase file not found. Creating new file...");
            CreateInitialFile();
        }

        // Start watching the file for changes
        WatchFile();
    }
    // Method to create the initial file with default JSON data
    private void CreateInitialFile()
    {
        // Create an initial damage data object (can be adjusted as needed)
        var initialData = new GmodOWOData
        {
            damage_type = "None",
            direction = "None"
        };

        // Serialize the data to JSON
        string jsonData = JsonConvert.SerializeObject(initialData, Formatting.Indented);

        // Write the JSON data to the file
        try
        {
            File.WriteAllText(dbPath, jsonData);
            Console.WriteLine("Initial database file created with default data.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating the file: {ex.Message}");
        }
    }
    private void WatchFile()
    {
        Task.Run(() =>
        {
            var fileWatcher = new FileSystemWatcher(Path.GetDirectoryName(dbPath))
            {
                Filter = Path.GetFileName(dbPath),
                NotifyFilter = NotifyFilters.LastWrite
            };

            fileWatcher.Changed += (sender, args) =>
            {
                if (args.FullPath == dbPath)
                {
                    currentModifiedTime = File.GetLastWriteTime(dbPath);

                    if (currentModifiedTime > lastReadTime)
                    {
                        Thread.Sleep(100);
                        lastReadTime = currentModifiedTime;

                        //Console.WriteLine("Database change detected. Processing new data...");
                        ProcessNewData();
                    }
                }
            };

            fileWatcher.EnableRaisingEvents = true;

            // Keep the watcher alive until the program exits
            while (true) Thread.Sleep(100);
        });
    }


    private void ProcessNewData()
    {
        try
        {
            // Read and process the data from the JSON file
            if (File.Exists(dbPath))
            {
                string jsonData = File.ReadAllText(dbPath).Trim();

                // Deserialize the JSON data
                GmodOWOData damageData = JsonConvert.DeserializeObject<GmodOWOData>(jsonData);

                if (damageData != null && !string.IsNullOrEmpty(damageData.damage_type) && !string.IsNullOrEmpty(damageData.direction))
                {
                    // Process the retrieved data
                    OWOIntegration.ParseOWOData(damageData.damage_type, damageData.direction);
                    Console.WriteLine($"Processed data: Damage Type = {damageData.damage_type}, Direction = {damageData.direction}");
                }
                else
                {
                    Console.WriteLine("JSON data is empty or malformed. No data to process.");
                }
            }
            else
            {
                Console.WriteLine($"File {dbPath} not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while processing the data: " + ex.Message);
        }
    }
}
