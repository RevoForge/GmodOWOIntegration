namespace GmodOWOIntegration
{
    public class Settings
    {
        public required string Owo_ip { get; set; }
        public required int Gmod_port { get; set; }
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
        private static int gmodPortNumber;
        private static GmodUdpServer udpServer = new();
        private static bool debugRunning = true;

        static void Main()
        {
            Console.WriteLine("GmodOWOIntegration v0.5 By RevoForge");

            gmodPortNumber = owoIntegration.Start();

            try
            {
                udpServer.StartServer(gmodPortNumber);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred starting UDP Server: " + ex.Message);
            }

            while (true)
            {
                Console.WriteLine("Type 'debug' to start debugging or 'exit' to quit:");
                string? userInput = Console.ReadLine();

                if (userInput != null)
                {
                    string response = userInput.ToLower();
                    if (response == "exit")
                    {
                        break; // Exit the program
                    }
                    else if (response == "debug")
                    {
                        debugRunning = true;
                        DebugLoop();
                    }
                }
            }
            udpServer.StopServer();
        }


        static void DebugLoop()
        {
            while (debugRunning)
            {
                string damageType = GetUserInput("DamageType", "DMG_BULLET");
                string hitbox = GetUserInput("Hitbox", "2");
                string direction = GetUserInput("Direction", "Front");

                OWOIntegration.TestSensations(damageType, hitbox, direction);

                if (!AskToRunAgain())
                {
                    break;
                }
            }
        }

        static string GetUserInput(string prompt, string defaultValue)
        {
            Console.WriteLine($"Debug Feature - Enter {prompt}:");
            string? userInput = Console.ReadLine();

            if (string.IsNullOrEmpty(userInput))
            {
                Console.WriteLine($"Debug Feature - Using default value: {defaultValue}");
                return defaultValue;
            }

            return userInput;
        }

        static bool AskToRunAgain()
        {
            Console.WriteLine("Do you want to run the program again? (type yes to continue or any other input to stop debugging)");
            string? response = Console.ReadLine();

            if (response != null && response.Equals("yes", StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
