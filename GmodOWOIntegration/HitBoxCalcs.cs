using OWOGame;

namespace GmodOWOIntegration
{
    internal class HitBoxCalcs
    {
        public static void FallDamage(ref Muscle[]? _currentEntryMuscles, ref IntensitySettings _intensities)
        {
            // currently only set for fall damage
            _currentEntryMuscles =
            [
                Muscle.Abdominal_L.WithIntensity(_intensities.Abdominal_L),
                Muscle.Abdominal_R.WithIntensity(_intensities.Abdominal_R),
                Muscle.Lumbar_L.WithIntensity(_intensities.Lumbar_L),
                Muscle.Lumbar_R.WithIntensity(_intensities.Lumbar_R),
            ];
        }
        public static void BulletDamage(ref Muscle[]? _currentEntryMuscles, ref Muscle[]? _currentBleedMuscles, ref IntensitySettings _intensities, ref string? _currentDirection)
        {
            Dictionary<string, (Muscle[], Muscle[])> directionMuscleMap = new()
            {
                {"Front", (
                        [
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.50f)),
                            Muscle.Pectoral_L.WithIntensity(_intensities.Pectoral_L),
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.50f)),
                            Muscle.Pectoral_R.WithIntensity(_intensities.Pectoral_R)
                        ],
                        [
                            Muscle.Pectoral_L.WithIntensity(_intensities.Pectoral_L),
                            Muscle.Pectoral_R.WithIntensity(_intensities.Pectoral_R)
                        ])
                },
                {"Right Front", (
                        [
                            Muscle.Arm_R.WithIntensity(Convert.ToInt32(_intensities.Arm_R * 0.50f)),
                            Muscle.Pectoral_L.WithIntensity(Convert.ToInt32(_intensities.Pectoral_L * 0.25f)),
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.50f)),
                            Muscle.Pectoral_R.WithIntensity(_intensities.Pectoral_R)
                        ],
                        [
                            Muscle.Pectoral_R.WithIntensity(_intensities.Pectoral_R)
                        ])
                },
                {"Front Right", (
                        [
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.50f)),
                            Muscle.Pectoral_L.WithIntensity(Convert.ToInt32(_intensities.Pectoral_L * 0.75f)),
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.75f)),
                            Muscle.Pectoral_R.WithIntensity(_intensities.Pectoral_R)
                        ],
                        [
                            Muscle.Pectoral_L.WithIntensity(Convert.ToInt32(_intensities.Pectoral_L * 0.75f)),
                            Muscle.Pectoral_R.WithIntensity(_intensities.Pectoral_R)
                        ])
                },
                {"Left Front",(
                    [
                        Muscle.Arm_L.WithIntensity(Convert.ToInt32(_intensities.Arm_L*0.50f)),
                        Muscle.Pectoral_R.WithIntensity(Convert.ToInt32(_intensities.Pectoral_R*0.25f)),
                        Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L*0.50f)),
                        Muscle.Pectoral_L.WithIntensity(_intensities.Pectoral_L)
                    ],
                    [
                        Muscle.Pectoral_L.WithIntensity(_intensities.Pectoral_L)
                    ])
                },
                {"Front Left",(
                    [
                        Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R*0.50f)),
                        Muscle.Pectoral_R.WithIntensity(Convert.ToInt32(_intensities.Pectoral_R*0.75f)),
                        Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L*0.75f)),
                        Muscle.Pectoral_L.WithIntensity(_intensities.Pectoral_L)
                    ],
                    [
                        Muscle.Pectoral_R.WithIntensity(Convert.ToInt32(_intensities.Pectoral_R*0.75f)),
                        Muscle.Pectoral_L.WithIntensity(_intensities.Pectoral_L)
                    ])
                },
                {"Back",(
                    [
                        Muscle.Lumbar_L.WithIntensity(Convert.ToInt32(_intensities.Lumbar_L*0.50f)),
                        Muscle.Dorsal_L.WithIntensity(_intensities.Dorsal_L),
                        Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R*0.50f)),
                        Muscle.Dorsal_R.WithIntensity(_intensities.Dorsal_R)
                    ],
                    [
                        Muscle.Dorsal_L.WithIntensity(_intensities.Dorsal_L),
                        Muscle.Dorsal_R.WithIntensity(_intensities.Dorsal_R)
                    ])
                },
                {"Right Back",(
                    [
                        Muscle.Arm_R.WithIntensity(Convert.ToInt32(_intensities.Arm_R*0.50f)),
                        Muscle.Dorsal_L.WithIntensity(Convert.ToInt32(_intensities.Dorsal_L*0.75f)),
                        Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R*0.75f)),
                        Muscle.Dorsal_R.WithIntensity(_intensities.Dorsal_R)
                    ],
                    [
                        Muscle.Dorsal_R.WithIntensity(_intensities.Dorsal_R)
                    ])
                },
                {"Back Right",(
                    [
                        Muscle.Arm_R.WithIntensity(Convert.ToInt32(_intensities.Arm_R*0.50f)),
                        Muscle.Dorsal_L.WithIntensity(Convert.ToInt32(_intensities.Dorsal_L*0.75f)),
                        Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R*0.75f)),
                        Muscle.Dorsal_R.WithIntensity(_intensities.Dorsal_R)
                    ],
                    [
                        Muscle.Dorsal_L.WithIntensity(Convert.ToInt32(_intensities.Dorsal_L*0.50f)),
                        Muscle.Dorsal_R.WithIntensity(_intensities.Dorsal_R)
                    ])
                },
                {"Left Back",(
                    [
                        Muscle.Arm_L.WithIntensity(Convert.ToInt32(_intensities.Arm_L*0.50f)),
                        Muscle.Dorsal_R.WithIntensity(Convert.ToInt32(_intensities.Dorsal_R*0.75f)),
                        Muscle.Lumbar_L.WithIntensity(Convert.ToInt32(_intensities.Lumbar_L*0.75f)),
                        Muscle.Dorsal_L.WithIntensity(_intensities.Dorsal_L)
                    ],
                    [
                        Muscle.Dorsal_L.WithIntensity(_intensities.Dorsal_L)
                    ])
                },
                {"Back Left",(
                    [
                        Muscle.Arm_L.WithIntensity(Convert.ToInt32(_intensities.Arm_L*0.50f)),
                        Muscle.Dorsal_R.WithIntensity(Convert.ToInt32(_intensities.Dorsal_R*0.75f)),
                        Muscle.Lumbar_L.WithIntensity(Convert.ToInt32(_intensities.Lumbar_L*0.75f)),
                        Muscle.Dorsal_L.WithIntensity(_intensities.Dorsal_L)
                    ],
                    [
                        Muscle.Dorsal_R.WithIntensity(Convert.ToInt32(_intensities.Dorsal_R*0.50f)),
                        Muscle.Dorsal_L.WithIntensity(_intensities.Dorsal_L)
                    ])
                },
                {"Left",(
                    [
                        Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L*0.50f)),
                        Muscle.Pectoral_L.WithIntensity(_intensities.Pectoral_L),
                        Muscle.Arm_L.WithIntensity(Convert.ToInt32(_intensities.Arm_L*0.50f))
                    ],
                    [
                        Muscle.Pectoral_L.WithIntensity(_intensities.Pectoral_L)
                    ])
                },
                {"Right",(
                    [
                        Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R*0.50f)),
                        Muscle.Pectoral_R.WithIntensity(_intensities.Pectoral_R),
                        Muscle.Arm_R.WithIntensity(Convert.ToInt32(_intensities.Arm_R*0.50f))
                    ],
                    [
                        Muscle.Pectoral_R.WithIntensity(_intensities.Pectoral_R)
                    ])
                },
            };
            if (_currentDirection != null && directionMuscleMap.TryGetValue(_currentDirection, out (Muscle[], Muscle[]) value))
            {
                (_currentEntryMuscles, _currentBleedMuscles) = value;
            }
        }
    }
}
