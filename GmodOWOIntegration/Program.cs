
namespace GmodOWOIntegration
{
    public class Settings
    {
        public required string Owo_ip { get; set; }
        public bool UseAutoConnect { get; set; }
        public required IntensitySettings Intensities { get; set; }
    }

    public class IntensitySettings
    {
        public int Pectoral_L { get; set; }
        public int Pectoral_R { get; set; }
        public int Abdominal_L { get; set; }
        public int Abdominal_R { get; set; }
        public int Arm_L { get; set; }
        public int Arm_R { get; set; }
        public int Dorsal_L { get; set; }
        public int Dorsal_R { get; set; }
        public int Lumbar_L { get; set; }
        public int Lumbar_R { get; set; }
    }
    internal class Program
    {
        private static readonly OWOIntegration owoIntegration = new();
        static void Main()
        {
            Console.WriteLine("GmodOWOIntegration v0.7 By RevoForge");

            owoIntegration.Start();

            int mode = 0;
            bool validInput = false;

            // Keep prompting the user until a valid input (1 or 2) is provided
            while (!validInput)
            {
                Console.WriteLine("Choose mode: (1) Single Player or (2) Multiplayer");
                string userInput = Console.ReadLine();

                if (int.TryParse(userInput, out mode) && (mode == 1 || mode == 2))
                {
                    validInput = true; // Valid input, exit the loop
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter 1 for Single Player or 2 for Multiplayer.");
                }
            }

            bool isMultiplayer = mode == 2;
            GmodDatabaseWatcher watcher = new(isMultiplayer);
            watcher.StartWatching();

            while (true)
            {

            }
        }
    }
}
