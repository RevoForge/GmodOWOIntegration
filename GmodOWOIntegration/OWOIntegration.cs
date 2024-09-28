using Newtonsoft.Json;
using OWOGame;

namespace GmodOWOIntegration
{
    internal class OWOIntegration
    {
        private bool _useAutoConnect;
        private string _Ip = "127.0.0.1";
        private static IntensitySettings _intensities = new();
        private static Sensation? _currentSensation;
        private static readonly Sensation _bleedingSensation = GmodSensations.Bleeding;
        private static Muscle[]? _currentEntryMuscles;
        private static Muscle[]? _currentBleedMuscles;
        private static readonly GmodDamageTypes _damageTypes = new();
        private static string? _currentDirection;
        private static bool _useBleeding = false;
        public void Start()
        {
            LoadOWOSettings();
        }
        public void LoadOWOSettings()
        {
            string exePath = AppContext.BaseDirectory;
            string directoryName = Path.GetDirectoryName(exePath) ?? "";
            string? settingsFilePath = directoryName != null ? Path.Combine(directoryName, "settings.json") : null;

            Settings settings;

            if (File.Exists(settingsFilePath))
            {
                string json = File.ReadAllText(settingsFilePath);
                if (json != null)
                {
                    settings = JsonConvert.DeserializeObject<Settings>(json!)!;
                    Console.WriteLine("Settings loaded from file.");
                }
                else
                {
                    Console.WriteLine("Failed to read settings from file.");
                    Console.WriteLine("Using default settings.");
                    settings = DefaultSettings();
                }
            }
            else
            {
                // Create new settings with default values
                settings = DefaultSettings();

                // Serialize the default settings to JSON and write to file
                string defaultSettingsJson = JsonConvert.SerializeObject(settings, Formatting.Indented);
                if (settingsFilePath != null)
                {
                    Console.WriteLine("Settings file not found. Using default settings.");
                    File.WriteAllText(settingsFilePath, defaultSettingsJson);
                }
                else
                {
                    Console.WriteLine("Failed to write default settings to file.");
                    // Handle the case when settingsFilePath is null, such as logging an error or providing a default file path
                }
            }
            // Access the values
            _Ip = settings.Owo_ip;
            _useAutoConnect = settings.UseAutoConnect;
            _intensities = settings.Intensities;
            ConnectToOWO();
        }
        private static Settings DefaultSettings()
        {
            return new Settings

            {
                Owo_ip = "127.0.0.1",
                UseAutoConnect = true,
                Intensities = new IntensitySettings
                {
                    Pectoral_L = 100,
                    Pectoral_R = 100,
                    Abdominal_L = 100,
                    Abdominal_R = 100,
                    Arm_L = 100,
                    Arm_R = 100,
                    Dorsal_L = 100,
                    Dorsal_R = 100,
                    Lumbar_L = 100,
                    Lumbar_R = 100
                }
            };
        }
        private void ConnectToOWO()
        {
            if (_useAutoConnect)
            {
                OWO.AutoConnect();
                Console.WriteLine("Autoconnect set to true. Autoconnecting to MyOWO App...");
            }
            else
            {
                OWO.Connect(_Ip);
                Console.WriteLine("Autoconnect set to false. Connecting to MyOWO with IP: " + _Ip + "...");
            }
            while (OWO.ConnectionState != ConnectionState.Connected)
            {

            }
            Console.WriteLine("Connected to OWO!");

        }
        public static void ParseOWOData(string DamageType, string Direction)
        {
            _currentSensation = _damageTypes.DamageTypes[DamageType];
            _currentDirection = Direction;
            if (DamageType == "DMG_BULLET")
            {
                _useBleeding = true;
                HitBoxCalcs.BulletDamage(ref _currentEntryMuscles, ref _currentBleedMuscles, ref _intensities, ref _currentDirection);
            }
            else
            {
                _useBleeding = false;
                HitBoxCalcs.FallDamage(ref _currentEntryMuscles, ref _intensities);
            }
            //Console.WriteLine("Sending Sensation: " + DamageType + ", Direction: " + Direction);
            SendOWOData();
        }
        private static void SendOWOData()
        {
            Sensation sensationToSend;
            if (_useBleeding)
            {
                sensationToSend = _currentSensation.WithMuscles(_currentEntryMuscles).Append(_bleedingSensation.WithMuscles(_currentBleedMuscles));
            }
            else
            {
                sensationToSend = _currentSensation.WithMuscles(_currentEntryMuscles);
            }
            OWO.Send(sensationToSend);
        }
    }
}