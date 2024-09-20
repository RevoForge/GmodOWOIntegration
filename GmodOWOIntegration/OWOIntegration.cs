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
        private static Hitbox _currentHitbox;
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
        public static void TestSensations(string DamageType, string Hitbox, string Direction)
        {
            ParseOWOData(DamageType, Hitbox, Direction);
            Console.WriteLine("Sending Sensation: " + DamageType + ", Direction: " + Direction + ", Hitbox: " + Hitbox);
        }
        public static void ParseOWOData(string DamageType, string Hitbox, string Direction)
        {
            if (int.TryParse(Hitbox, out int result))
            {
                if (DamageType == "DMG_BULLET" || DamageType == "DMG_SLASH" || DamageType == "DMG_BUCKSHOT" || DamageType == "DMG_SNIPER")
                {
                    _useBleeding = true;
                }
                else
                {
                    _useBleeding = false;
                }
                _currentHitbox = (Hitbox)result;
                _currentSensation = _damageTypes.DamageTypes[DamageType];
                _currentDirection = Direction;
                CalculateOWOMuscles();
                Console.WriteLine("Sending Sensation: " + DamageType + ", Direction: " + Direction + ", Hitbox: " + _currentHitbox);
            }
            else
            {
                Console.WriteLine("Invalid hitbox value: " + Hitbox);
            }
        }
        private static void CalculateOWOMuscles()
        {
            Dictionary<Hitbox, Action> hitboxCalculations = new()
            {
                {Hitbox.HITGROUP_HEAD, () => HitBoxCalcs.HeadHitBoxCalculation(ref _currentEntryMuscles, ref _currentBleedMuscles, ref _intensities)},
                {Hitbox.HITGROUP_CHEST, () => HitBoxCalcs.ChestHitBoxCalculation(ref _currentEntryMuscles, ref _currentBleedMuscles, ref _intensities, ref _currentDirection)},
                {Hitbox.HITGROUP_STOMACH, () => HitBoxCalcs.StomachHitBoxCalculation(ref _currentEntryMuscles, ref _currentBleedMuscles, ref _intensities, ref _currentDirection)},
                {Hitbox.HITGROUP_LEFTARM, () => HitBoxCalcs.LeftArmHitBoxCalculation(ref _currentEntryMuscles, ref _currentBleedMuscles, ref _intensities, ref _currentDirection)},
                {Hitbox.HITGROUP_RIGHTARM, () => HitBoxCalcs.RightArmHitBoxCalculation(ref _currentEntryMuscles, ref _currentBleedMuscles, ref _intensities, ref _currentDirection)},
                {Hitbox.HITGROUP_LEFTLEG, () => HitBoxCalcs.LeftLegHitBoxCalculation(ref _currentEntryMuscles, ref _currentBleedMuscles, ref _intensities, ref _currentDirection)},
                {Hitbox.HITGROUP_RIGHTLEG, () => HitBoxCalcs.RightLegHitBoxCalculation(ref _currentEntryMuscles, ref _currentBleedMuscles, ref _intensities, ref _currentDirection)}
            };

            if (hitboxCalculations.TryGetValue(_currentHitbox, out Action? value))
            {
                value.Invoke();
                SendOWOData();
            }
            else
            {
                Console.WriteLine("Invalid hitbox value: " + _currentHitbox);
            }

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