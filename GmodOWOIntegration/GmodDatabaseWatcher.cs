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
    private DateTime lastModifiedTime;
    private DateTime lastReadTime;

    public void StartWatching()
    {
        // Initialize SQLite provider
        Batteries.Init();
        Console.WriteLine($"Checking for database file at {dbPath}");

        // Use FileSystemWatcher to monitor the file
        if (File.Exists(dbPath))
        {
            Console.WriteLine("\nDatabase file found. Starting to watch for changes...");
            WatchFile();
        }
        else
        {
            Console.WriteLine("\nDatabase file not found.");
        }
    }

    private void WatchFile()
    {
        var fileWatcher = new FileSystemWatcher(Path.GetDirectoryName(dbPath))
        {
            Filter = Path.GetFileName(dbPath),
            NotifyFilter = NotifyFilters.LastWrite
        };

        fileWatcher.Changed += (sender, args) =>
        {
            // Ensure the change is from our file and not some other process
            if (args.FullPath == dbPath)
            {
                DateTime currentModifiedTime = File.GetLastWriteTime(dbPath);

                // Only process if the file's modification time is after the last read time
                if (currentModifiedTime > lastReadTime)
                {
                    // Add a small debounce delay (e.g., 100ms) to ensure the file is fully written before processing
                    Thread.Sleep(100);

                    // Update the last read time
                    lastReadTime = currentModifiedTime;

                    Console.WriteLine("Database change detected. Processing new data...");
                    ProcessNewData();
                }
            }
        };

        fileWatcher.EnableRaisingEvents = true;

        // Keep the program alive to watch for file changes
        Console.ReadLine();
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
