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
        private static readonly GmodDatabaseWatcher DatabaseWatcher = new();
        static void Main()
        {
            Console.WriteLine("GmodOWOIntegration v0.5 By RevoForge");

            owoIntegration.Start();

            try
            {
                DatabaseWatcher.StartWatching();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred starting DatabaseWatcher: " + ex.Message);
            }

            while (true)
            {

            }
        }
    }
}
