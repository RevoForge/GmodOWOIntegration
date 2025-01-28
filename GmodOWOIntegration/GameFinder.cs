using Microsoft.Win32;

namespace GmodOWOIntegration
{
    public class GameFinder
    {
        public static string GetGarrysModInstallPath()
        {
            const int maxRetries = 10; // Maximum number of retry attempts
            const int retryDelay = 2000; // Delay between retries in milliseconds (2 seconds)

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                Console.WriteLine($"Attempt {attempt} to locate Garry's Mod installation...");

                try
                {
                    // Get Steam installation path
                    string steamPath = GetSteamPath();

                    if (string.IsNullOrEmpty(steamPath))
                    {
                        Console.WriteLine("Steam installation not found.");
                        return string.Empty;
                    }

                    // Get libraryfolders.vdf file
                    string libraryFile = Path.Combine(steamPath, "steamapps", "libraryfolders.vdf");
                    if (!File.Exists(libraryFile))
                    {
                        Console.WriteLine("libraryfolders.vdf not found.");
                        return string.Empty;
                    }

                    // Search for Garry's Mod folder
                    string garrysModPath = FindGarrysModPath(libraryFile);

                    if (!string.IsNullOrEmpty(garrysModPath))
                    {
                        Console.WriteLine($"Garry's Mod installation found: {garrysModPath}");
                        return garrysModPath;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while locating Garry's Mod installation: {ex.Message}");
                }

                Console.WriteLine("Retrying...");
                Thread.Sleep(retryDelay); // Wait before retrying
            }

            Console.WriteLine("Failed to locate Garry's Mod installation after 10 attempts.");
            return string.Empty; // Return empty if all retries fail
        }

        private static string GetSteamPath()
        {
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Valve\Steam");
            return key?.GetValue("SteamPath") as string ?? string.Empty;
        }

        private static string FindGarrysModPath(string libraryFile)
        {
            string[] lines = File.ReadAllLines(libraryFile);

            foreach (string line in lines)
            {
                if (line.Contains("path"))
                {
                    // Extract library folder path
                    string[] split = line.Split('"');
                    if (split.Length >= 5)
                    {
                        string libraryPath = split[3].Replace("\\\\", "\\"); // Normalize path

                        // Check for Garry's Mod installation
                        string garrysModPath = Path.Combine(libraryPath, "steamapps", "common", "GarrysMod");
                        if (Directory.Exists(garrysModPath))
                        {
                            return garrysModPath;
                        }
                    }
                }
            }

            // Check default library folder as a fallback
            string defaultPath = Path.Combine(Path.GetDirectoryName(libraryFile), "common", "GarrysMod");
            return Directory.Exists(defaultPath) ? defaultPath : null;
        }
    }
}
