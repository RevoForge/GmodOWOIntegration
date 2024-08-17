using OWOGame;

namespace GmodOWOIntegration
{
    internal class HitBoxCalcs
    {
        public static void HeadHitBoxCalculation(ref Muscle[]? _currentEntryMuscles, ref Muscle[]? _currentBleedMuscles, ref IntensitySettings _intensities)
        {
            _currentEntryMuscles =
            [
                Muscle.Abdominal_L.WithIntensity(_intensities.Abdominal_L),
                Muscle.Abdominal_R.WithIntensity(_intensities.Abdominal_R),
                Muscle.Pectoral_L.WithIntensity(_intensities.Pectoral_L),
                Muscle.Pectoral_R.WithIntensity(_intensities.Pectoral_R),
                Muscle.Dorsal_L.WithIntensity(_intensities.Dorsal_L),
                Muscle.Dorsal_R.WithIntensity(_intensities.Dorsal_R),
                Muscle.Lumbar_L.WithIntensity(_intensities.Lumbar_L),
                Muscle.Lumbar_R.WithIntensity(_intensities.Lumbar_R),
                Muscle.Arm_L.WithIntensity(_intensities.Arm_L),
                Muscle.Arm_R.WithIntensity(_intensities.Arm_R)
            ];
            _currentBleedMuscles = _currentEntryMuscles;
        }
        public static void ChestHitBoxCalculation(ref Muscle[]? _currentEntryMuscles, ref Muscle[]? _currentBleedMuscles, ref IntensitySettings _intensities, ref string? _currentDirection)
        {
            Dictionary<string, (Muscle[], Muscle[])> directionMuscleMap = new Dictionary<string, (Muscle[], Muscle[])>
            {
                {"Front", (
                        new Muscle[]
                        {
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.50f)),
                            Muscle.Pectoral_L.WithIntensity(_intensities.Pectoral_L),
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.50f)),
                            Muscle.Pectoral_R.WithIntensity(_intensities.Pectoral_R)
                        },
                        new Muscle[]
                        {
                            Muscle.Pectoral_L.WithIntensity(_intensities.Pectoral_L),
                            Muscle.Pectoral_R.WithIntensity(_intensities.Pectoral_R)
                        })
                },
                {"Right Front", (
                        new Muscle[]
                        {
                            Muscle.Arm_R.WithIntensity(Convert.ToInt32(_intensities.Arm_R * 0.50f)),
                            Muscle.Pectoral_L.WithIntensity(Convert.ToInt32(_intensities.Pectoral_L * 0.25f)),
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.50f)),
                            Muscle.Pectoral_R.WithIntensity(_intensities.Pectoral_R)
                        },
                        new Muscle[]
                        {
                            Muscle.Pectoral_R.WithIntensity(_intensities.Pectoral_R)
                        })
                },
                {"Front Right", (
                        new Muscle[]
                        {
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.50f)),
                            Muscle.Pectoral_L.WithIntensity(Convert.ToInt32(_intensities.Pectoral_L * 0.75f)),
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.75f)),
                            Muscle.Pectoral_R.WithIntensity(_intensities.Pectoral_R)
                        },
                        new Muscle[]
                        {
                            Muscle.Pectoral_L.WithIntensity(Convert.ToInt32(_intensities.Pectoral_L * 0.75f)),
                            Muscle.Pectoral_R.WithIntensity(_intensities.Pectoral_R)
                        })
                },
                {"Left Front",(
                    new Muscle[]
                    {
                        Muscle.Arm_L.WithIntensity(Convert.ToInt32(_intensities.Arm_L*0.50f)),
                        Muscle.Pectoral_R.WithIntensity(Convert.ToInt32(_intensities.Pectoral_R*0.25f)),
                        Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L*0.50f)),
                        Muscle.Pectoral_L.WithIntensity(_intensities.Pectoral_L)
                    },
                    new Muscle[]
                    {
                        Muscle.Pectoral_L.WithIntensity(_intensities.Pectoral_L)
                    })
                },
                {"Front Left",(
                    new Muscle[]
                    {
                        Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R*0.50f)),
                        Muscle.Pectoral_R.WithIntensity(Convert.ToInt32(_intensities.Pectoral_R*0.75f)),
                        Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L*0.75f)),
                        Muscle.Pectoral_L.WithIntensity(_intensities.Pectoral_L)
                    },
                    new Muscle[]
                    {
                        Muscle.Pectoral_R.WithIntensity(Convert.ToInt32(_intensities.Pectoral_R*0.75f)),
                        Muscle.Pectoral_L.WithIntensity(_intensities.Pectoral_L)
                    })
                },
                {"Back",(
                    new Muscle[]
                    {
                        Muscle.Lumbar_L.WithIntensity(Convert.ToInt32(_intensities.Lumbar_L*0.50f)),
                        Muscle.Dorsal_L.WithIntensity(_intensities.Dorsal_L),
                        Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R*0.50f)),
                        Muscle.Dorsal_R.WithIntensity(_intensities.Dorsal_R)
                    },
                    new Muscle[]
                    {
                        Muscle.Dorsal_L.WithIntensity(_intensities.Dorsal_L),
                        Muscle.Dorsal_R.WithIntensity(_intensities.Dorsal_R)
                    })
                },
                {"Right Back",(
                    new Muscle[]
                    {
                        Muscle.Arm_R.WithIntensity(Convert.ToInt32(_intensities.Arm_R*0.50f)),
                        Muscle.Dorsal_L.WithIntensity(Convert.ToInt32(_intensities.Dorsal_L*0.75f)),
                        Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R*0.75f)),
                        Muscle.Dorsal_R.WithIntensity(_intensities.Dorsal_R)
                    },
                    new Muscle[]
                    {
                        Muscle.Dorsal_R.WithIntensity(_intensities.Dorsal_R)
                    })
                },
                {"Back Right",(
                    new Muscle[]
                    {
                        Muscle.Arm_R.WithIntensity(Convert.ToInt32(_intensities.Arm_R*0.50f)),
                        Muscle.Dorsal_L.WithIntensity(Convert.ToInt32(_intensities.Dorsal_L*0.75f)),
                        Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R*0.75f)),
                        Muscle.Dorsal_R.WithIntensity(_intensities.Dorsal_R)
                    },
                    new Muscle[]
                    {
                        Muscle.Dorsal_L.WithIntensity(Convert.ToInt32(_intensities.Dorsal_L*0.50f)),
                        Muscle.Dorsal_R.WithIntensity(_intensities.Dorsal_R)
                    })
                },
                {"Left Back",(
                    new Muscle[]
                    {
                        Muscle.Arm_L.WithIntensity(Convert.ToInt32(_intensities.Arm_L*0.50f)),
                        Muscle.Dorsal_R.WithIntensity(Convert.ToInt32(_intensities.Dorsal_R*0.75f)),
                        Muscle.Lumbar_L.WithIntensity(Convert.ToInt32(_intensities.Lumbar_L*0.75f)),
                        Muscle.Dorsal_L.WithIntensity(_intensities.Dorsal_L)
                    },
                    new Muscle[]
                    {
                        Muscle.Dorsal_L.WithIntensity(_intensities.Dorsal_L)
                    })
                },
                {"Back Left",(
                    new Muscle[]
                    {
                        Muscle.Arm_L.WithIntensity(Convert.ToInt32(_intensities.Arm_L*0.50f)),
                        Muscle.Dorsal_R.WithIntensity(Convert.ToInt32(_intensities.Dorsal_R*0.75f)),
                        Muscle.Lumbar_L.WithIntensity(Convert.ToInt32(_intensities.Lumbar_L*0.75f)),
                        Muscle.Dorsal_L.WithIntensity(_intensities.Dorsal_L)
                    },
                    new Muscle[]
                    {
                        Muscle.Dorsal_R.WithIntensity(Convert.ToInt32(_intensities.Dorsal_R*0.50f)),
                        Muscle.Dorsal_L.WithIntensity(_intensities.Dorsal_L)
                    })
                },
                {"Left",(
                    new Muscle[]
                    {
                        Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L*0.50f)),
                        Muscle.Pectoral_L.WithIntensity(_intensities.Pectoral_L),
                        Muscle.Arm_L.WithIntensity(Convert.ToInt32(_intensities.Arm_L*0.50f))
                    },
                    new Muscle[]
                    {
                        Muscle.Pectoral_L.WithIntensity(_intensities.Pectoral_L)
                    })
                },
                {"Right",(
                    new Muscle[]
                    {
                        Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R*0.50f)),
                        Muscle.Pectoral_R.WithIntensity(_intensities.Pectoral_R),
                        Muscle.Arm_R.WithIntensity(Convert.ToInt32(_intensities.Arm_R*0.50f))
                    },
                    new Muscle[]
                    {
                        Muscle.Pectoral_R.WithIntensity(_intensities.Pectoral_R)
                    })
                },
            };
            if (_currentDirection != null && directionMuscleMap.ContainsKey(_currentDirection))
            {
                (_currentEntryMuscles, _currentBleedMuscles) = directionMuscleMap[_currentDirection];
            }
        }
        public static void StomachHitBoxCalculation(ref Muscle[]? _currentEntryMuscles, ref Muscle[]? _currentBleedMuscles, ref IntensitySettings _intensities, ref string? _currentDirection)
        {
            Dictionary<string, (Muscle[], Muscle[])> directionMuscleMap = new()
            {
                {"Front", (
                        new Muscle[]
                        {
                            Muscle.Pectoral_L.WithIntensity(Convert.ToInt32(_intensities.Pectoral_L*0.50f)),
                            Muscle.Pectoral_R.WithIntensity(Convert.ToInt32(_intensities.Pectoral_R*0.50f)),
                            Muscle.Abdominal_L.WithIntensity(_intensities.Abdominal_L),
                            Muscle.Abdominal_R.WithIntensity(_intensities.Abdominal_R)
                        },
                        new Muscle[]
                        {
                            Muscle.Abdominal_L.WithIntensity(_intensities.Abdominal_L),
                            Muscle.Abdominal_R.WithIntensity(_intensities.Abdominal_R)
                        })
                },
                {"Right Front", (
                        new Muscle[]
                        {
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L*0.25f)),
                            Muscle.Pectoral_R.WithIntensity(Convert.ToInt32(_intensities.Pectoral_R*0.50f)),
                            Muscle.Abdominal_R.WithIntensity(_intensities.Abdominal_R)
                        },
                        new Muscle[]
                        {
                            Muscle.Abdominal_R.WithIntensity(_intensities.Abdominal_R)
                        })
                },
                {"Front Right", (
                        new Muscle[]
                        {
                            Muscle.Pectoral_L.WithIntensity(Convert.ToInt32(_intensities.Pectoral_L*0.25f)),
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L*0.75f)),
                            Muscle.Pectoral_R.WithIntensity(Convert.ToInt32(_intensities.Pectoral_R*0.75f)),
                            Muscle.Abdominal_R.WithIntensity(_intensities.Abdominal_R)
                        },
                        new Muscle[]
                        {
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L*0.75f)),
                            Muscle.Abdominal_R.WithIntensity(_intensities.Abdominal_R)
                        })
                },
                {"Left Front",(
                    new Muscle[]
                    {
                        Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R*0.25f)),
                        Muscle.Pectoral_L.WithIntensity(Convert.ToInt32(_intensities.Pectoral_L*0.50f)),
                        Muscle.Abdominal_L.WithIntensity(_intensities.Abdominal_L)
                    },
                    new Muscle[]
                    {
                        Muscle.Abdominal_L.WithIntensity(_intensities.Abdominal_L)
                    })
                },
                {"Front Left",(
                    new Muscle[]
                    {
                        Muscle.Pectoral_R.WithIntensity(Convert.ToInt32(_intensities.Pectoral_R*0.25f)),
                        Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R*0.75f)),
                        Muscle.Pectoral_L.WithIntensity(Convert.ToInt32(_intensities.Pectoral_L*0.50f)),
                        Muscle.Abdominal_L.WithIntensity(_intensities.Abdominal_L)
                    },
                    new Muscle[]
                    {
                        Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R*0.75f)),
                        Muscle.Abdominal_L.WithIntensity(_intensities.Abdominal_L)
                    })
                },
                {"Back",(
                    new Muscle[]
                    {
                        Muscle.Dorsal_L.WithIntensity(Convert.ToInt32(_intensities.Dorsal_L*0.50f)),
                        Muscle.Lumbar_L.WithIntensity(_intensities.Lumbar_L),
                        Muscle.Dorsal_R.WithIntensity(Convert.ToInt32(_intensities.Dorsal_R*0.50f)),
                        Muscle.Lumbar_R.WithIntensity(_intensities.Lumbar_R)
                    },
                    new Muscle[]
                    {
                        Muscle.Lumbar_L.WithIntensity(_intensities.Lumbar_L),
                        Muscle.Lumbar_R.WithIntensity(_intensities.Lumbar_R)
                    })
                },
                {"Right Back",(
                    new Muscle[]
                    {
                        Muscle.Lumbar_L.WithIntensity(Convert.ToInt32(_intensities.Lumbar_L*0.50f)),
                        Muscle.Dorsal_R.WithIntensity(Convert.ToInt32(_intensities.Dorsal_R*0.75f)),
                        Muscle.Lumbar_R.WithIntensity(_intensities.Dorsal_R)
                    },
                    new Muscle[]
                    {
                        Muscle.Lumbar_R.WithIntensity(_intensities.Lumbar_R)
                    })
                },
                {"Back Right",(
                    new Muscle[]
                    {
                        Muscle.Lumbar_L.WithIntensity(Convert.ToInt32(_intensities.Lumbar_L*0.75f)),
                        Muscle.Dorsal_R.WithIntensity(Convert.ToInt32(_intensities.Dorsal_R*0.75f)),
                        Muscle.Lumbar_R.WithIntensity(_intensities.Lumbar_R)
                    },
                    new Muscle[]
                    {
                        Muscle.Lumbar_L.WithIntensity(Convert.ToInt32(_intensities.Lumbar_L*0.50f)),
                        Muscle.Lumbar_R.WithIntensity(_intensities.Lumbar_R)
                    })
                },
                {"Left Back",(
                    new Muscle[]
                    {
                        Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R*0.75f)),
                        Muscle.Dorsal_L.WithIntensity(Convert.ToInt32(_intensities.Dorsal_L*0.75f)),
                        Muscle.Lumbar_L.WithIntensity(_intensities.Lumbar_L)
                    },
                    new Muscle[]
                    {
                        Muscle.Lumbar_L.WithIntensity(_intensities.Lumbar_L)
                    })
                },
                {"Back Left",(
                    new Muscle[]
                    {
                        Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R*0.75f)),
                        Muscle.Dorsal_L.WithIntensity(Convert.ToInt32(_intensities.Dorsal_L*0.75f)),
                        Muscle.Lumbar_L.WithIntensity(_intensities.Lumbar_L)
                    },
                    new Muscle[]
                    {
                        Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R*0.50f)),
                        Muscle.Lumbar_L.WithIntensity(_intensities.Lumbar_L)
                    })
                },
                {"Left",(
                    new Muscle[]
                    {
                        Muscle.Pectoral_L.WithIntensity(Convert.ToInt32(_intensities.Pectoral_L*0.50f)),
                        Muscle.Abdominal_L.WithIntensity(_intensities.Abdominal_L)
                    },
                    new Muscle[]
                    {
                        Muscle.Abdominal_L.WithIntensity(_intensities.Abdominal_L)
                    })
                },
                {"Right",(
                    new Muscle[]
                    {
                        Muscle.Pectoral_R.WithIntensity(Convert.ToInt32(_intensities.Pectoral_R*0.50f)),
                        Muscle.Abdominal_R.WithIntensity(_intensities.Abdominal_R)
                    },
                    new Muscle[]
                    {
                        Muscle.Abdominal_R.WithIntensity(_intensities.Abdominal_R)
                    })
                },
            };
            if (_currentDirection != null && directionMuscleMap.ContainsKey(_currentDirection))
            {
                (_currentEntryMuscles, _currentBleedMuscles) = directionMuscleMap[_currentDirection];
            }
        }
        public static void LeftArmHitBoxCalculation(ref Muscle[]? _currentEntryMuscles, ref Muscle[]? _currentBleedMuscles, ref IntensitySettings _intensities, ref string? _currentDirection)
        {
            Dictionary<string, (Muscle[], Muscle[])> directionMuscleMap = new()
            {
                {
                    "Front",
                    (
                        new Muscle[]
                        {
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.50f)),
                            Muscle.Pectoral_L.WithIntensity(Convert.ToInt32(_intensities.Pectoral_L * 0.50f)),
                            Muscle.Arm_L.WithIntensity(_intensities.Arm_L)
                        },
                        new Muscle[]
                        {
                            Muscle.Arm_L.WithIntensity(_intensities.Arm_L)
                        }
                    )
                },
                {
                    "Right Front",
                    (
                        new Muscle[]
                        {
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.50f)),
                            Muscle.Pectoral_L.WithIntensity(Convert.ToInt32(_intensities.Pectoral_L * 0.50f)),
                            Muscle.Arm_L.WithIntensity(_intensities.Arm_L)
                        },
                        new Muscle[]
                        {
                            Muscle.Arm_L.WithIntensity(_intensities.Arm_L)
                        }
                    )
                },
                {
                    "Front Right",
                    (
                        new Muscle[]
                        {
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.50f)),
                            Muscle.Pectoral_L.WithIntensity(Convert.ToInt32(_intensities.Pectoral_L * 0.50f)),
                            Muscle.Arm_L.WithIntensity(_intensities.Arm_L)
                        },
                        new Muscle[]
                        {
                            Muscle.Arm_L.WithIntensity(_intensities.Arm_L)
                        }
                    )
                },
                {
                    "Left Front",
                    (
                        new Muscle[]
                        {
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.50f)),
                            Muscle.Pectoral_L.WithIntensity(Convert.ToInt32(_intensities.Pectoral_L * 0.50f)),
                            Muscle.Arm_L.WithIntensity(_intensities.Arm_L)
                        },
                        new Muscle[]
                        {
                            Muscle.Arm_L.WithIntensity(_intensities.Arm_L)
                        }
                    )
                },
                {
                    "Front Left",
                    (
                        new Muscle[]
                        {
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.50f)),
                            Muscle.Pectoral_L.WithIntensity(Convert.ToInt32(_intensities.Pectoral_L * 0.50f)),
                            Muscle.Arm_L.WithIntensity(_intensities.Arm_L)
                        },
                        new Muscle[]
                        {
                            Muscle.Arm_L.WithIntensity(_intensities.Arm_L)
                        }
                    )
                },
                {
                    "Back",
                    (
                        new Muscle[]
                        {
                            Muscle.Lumbar_L.WithIntensity(Convert.ToInt32(_intensities.Lumbar_L * 0.50f)),
                            Muscle.Dorsal_L.WithIntensity(Convert.ToInt32(_intensities.Dorsal_L * 0.50f)),
                            Muscle.Arm_L.WithIntensity(_intensities.Arm_L)
                        },
                        new Muscle[]
                        {
                            Muscle.Arm_L.WithIntensity(_intensities.Arm_L)
                        }
                    )
                },
                {
                    "Right Back",
                    (
                        new Muscle[]
                        {
                            Muscle.Lumbar_L.WithIntensity(Convert.ToInt32(_intensities.Lumbar_L * 0.50f)),
                            Muscle.Dorsal_L.WithIntensity(Convert.ToInt32(_intensities.Dorsal_L * 0.50f)),
                            Muscle.Arm_L.WithIntensity(_intensities.Arm_L)
                        },
                        new Muscle[]
                        {
                            Muscle.Arm_L.WithIntensity(_intensities.Arm_L)
                        }
                    )
                },
                {
                    "Back Right",
                    (
                        new Muscle[]
                        {
                            Muscle.Lumbar_L.WithIntensity(Convert.ToInt32(_intensities.Lumbar_L * 0.50f)),
                            Muscle.Dorsal_L.WithIntensity(Convert.ToInt32(_intensities.Dorsal_L * 0.50f)),
                            Muscle.Arm_L.WithIntensity(_intensities.Arm_L)
                        },
                        new Muscle[]
                        {
                            Muscle.Arm_L.WithIntensity(_intensities.Arm_L)
                        }
                    )
                },
                {
                    "Left Back",
                    (
                        new Muscle[]
                        {
                            Muscle.Lumbar_L.WithIntensity(Convert.ToInt32(_intensities.Lumbar_L * 0.50f)),
                            Muscle.Dorsal_L.WithIntensity(Convert.ToInt32(_intensities.Dorsal_L * 0.50f)),
                            Muscle.Arm_L.WithIntensity(_intensities.Arm_L)
                        },
                        new Muscle[]
                        {
                            Muscle.Arm_L.WithIntensity(_intensities.Arm_L)
                        }
                    )
                },
                {
                    "Back Left",
                    (
                        new Muscle[]
                        {
                            Muscle.Lumbar_L.WithIntensity(Convert.ToInt32(_intensities.Lumbar_L * 0.50f)),
                            Muscle.Dorsal_L.WithIntensity(Convert.ToInt32(_intensities.Dorsal_L * 0.50f)),
                            Muscle.Arm_L.WithIntensity(_intensities.Arm_L)
                        },
                        new Muscle[]
                        {
                            Muscle.Arm_L.WithIntensity(_intensities.Arm_L)
                        }
                    )
                },
                {
                    "Left",
                    (
                        new Muscle[]
                        {
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.25f))
                        },
                        new Muscle[]
                        {
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.25f))
                        }
                    )
                },
                {
                    "Right",
                    (
                        new Muscle[]
                        {
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.25f))
                        },
                        new Muscle[]
                        {
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.25f))
                        }
                    )
                }
            };
            if (_currentDirection != null && directionMuscleMap.ContainsKey(_currentDirection))
            {
                (_currentEntryMuscles, _currentBleedMuscles) = directionMuscleMap[_currentDirection];
            }
        }
        public static void RightArmHitBoxCalculation(ref Muscle[]? _currentEntryMuscles, ref Muscle[]? _currentBleedMuscles, ref IntensitySettings _intensities, ref string? _currentDirection)
        {
            Dictionary<string, (Muscle[], Muscle[])> directionMuscleMap = new()
            {
                {
                    "Front", (
                        new[]
                        {
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.50f)),
                            Muscle.Pectoral_R.WithIntensity(Convert.ToInt32(_intensities.Pectoral_R * 0.50f)),
                            Muscle.Arm_R.WithIntensity(_intensities.Arm_R)
                        },
                        new[]
                        {
                            Muscle.Arm_R.WithIntensity(_intensities.Arm_R)
                        }
                    )
                },
                {
                    "Right Front", (
                        new[]
                        {
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.50f)),
                            Muscle.Pectoral_R.WithIntensity(Convert.ToInt32(_intensities.Pectoral_R * 0.50f)),
                            Muscle.Arm_R.WithIntensity(_intensities.Arm_R)
                        },
                        new[]
                        {
                            Muscle.Arm_R.WithIntensity(_intensities.Arm_R)
                        }
                    )
                },
                {
                    "Front Right", (
                        new[]
                        {
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.50f)),
                            Muscle.Pectoral_R.WithIntensity(Convert.ToInt32(_intensities.Pectoral_R * 0.50f)),
                            Muscle.Arm_R.WithIntensity(_intensities.Arm_R)
                        },
                        new[]
                        {
                            Muscle.Arm_R.WithIntensity(_intensities.Arm_R)
                        }
                    )
                },
                {
                    "Left Front", (
                        new[]
                        {
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.50f)),
                            Muscle.Pectoral_R.WithIntensity(Convert.ToInt32(_intensities.Pectoral_R * 0.50f)),
                            Muscle.Arm_R.WithIntensity(_intensities.Arm_R)
                        },
                        new[]
                        {
                            Muscle.Arm_R.WithIntensity(_intensities.Arm_R)
                        }
                    )
                },
                {
                    "Front Left", (
                        new[]
                        {
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.50f)),
                            Muscle.Pectoral_R.WithIntensity(Convert.ToInt32(_intensities.Pectoral_R * 0.50f)),
                            Muscle.Arm_R.WithIntensity(_intensities.Arm_R)
                        },
                        new[]
                        {
                            Muscle.Arm_R.WithIntensity(_intensities.Arm_R)
                        }
                    )
                },
                {
                    "Back", (
                        new[]
                        {
                            Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R * 0.50f)),
                            Muscle.Dorsal_R.WithIntensity(Convert.ToInt32(_intensities.Dorsal_R * 0.50f)),
                            Muscle.Arm_R.WithIntensity(_intensities.Arm_R)
                        },
                        new[]
                        {
                            Muscle.Arm_R.WithIntensity(_intensities.Arm_R)
                        }
                    )
                },
                {
                    "Right Back", (
                        new[]
                        {
                            Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R * 0.50f)),
                            Muscle.Dorsal_R.WithIntensity(Convert.ToInt32(_intensities.Dorsal_R * 0.50f)),
                            Muscle.Arm_R.WithIntensity(_intensities.Arm_R)
                        },
                        new[]
                        {
                            Muscle.Arm_R.WithIntensity(_intensities.Arm_R)
                        }
                    )
                },
                {
                    "Back Right", (
                        new[]
                        {
                            Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R * 0.50f)),
                            Muscle.Dorsal_R.WithIntensity(Convert.ToInt32(_intensities.Dorsal_R * 0.50f)),
                            Muscle.Arm_R.WithIntensity(_intensities.Arm_R)
                        },
                        new[]
                        {
                            Muscle.Arm_R.WithIntensity(_intensities.Arm_R)
                        }
                    )
                },
                {
                    "Left Back", (
                        new[]
                        {
                            Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R * 0.50f)),
                            Muscle.Dorsal_R.WithIntensity(Convert.ToInt32(_intensities.Dorsal_R * 0.50f)),
                            Muscle.Arm_R.WithIntensity(_intensities.Arm_R)
                        },
                        new[]
                        {
                            Muscle.Arm_R.WithIntensity(_intensities.Arm_R)
                        }
                    )
                },
                {
                    "Back Left", (
                        new[]
                        {
                            Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R * 0.50f)),
                            Muscle.Dorsal_R.WithIntensity(Convert.ToInt32(_intensities.Dorsal_R * 0.50f)),
                            Muscle.Arm_R.WithIntensity(_intensities.Arm_R)
                        },
                        new[]
                        {
                            Muscle.Arm_R.WithIntensity(_intensities.Arm_R)
                        }
                    )
                },
                {
                    "Left", (
                        new[]
                        {
                            Muscle.Arm_R.WithIntensity(_intensities.Arm_R)
                        },
                        new[]
                        {
                            Muscle.Arm_R.WithIntensity(_intensities.Arm_R)
                        }
                    )
                },
                {
                    "Right", (
                        new[]
                        {
                            Muscle.Arm_R.WithIntensity(_intensities.Arm_R)
                        },
                        new[]
                        {
                            Muscle.Arm_R.WithIntensity(_intensities.Arm_R)
                        }
                    )
                }
            };

            if (_currentDirection != null && directionMuscleMap.ContainsKey(_currentDirection))
            {
                (_currentEntryMuscles, _currentBleedMuscles) = directionMuscleMap[_currentDirection];
            }
        }

        public static void LeftLegHitBoxCalculation(ref Muscle[]? _currentEntryMuscles, ref Muscle[]? _currentBleedMuscles, ref IntensitySettings _intensities, ref string? _currentDirection)
        {
            Dictionary<string, (Muscle[], Muscle[])> directionMuscleMap = new()
            {
                {
                    "Front",
                    (
                        new Muscle[]
                        {
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.50f)),
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.30f)),
                        },
                        new Muscle[]
                        {
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.25f)),
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.10f))
                        }
                    )
                },
                {
                    "Front Right",
                    (
                        new Muscle[]
                        {
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.50f)),
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.30f)),
                        },
                        new Muscle[]
                        {
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.25f)),
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.10f))
                        }
                    )
                },
                {
                    "Front Left",
                    (
                        new Muscle[]
                        {
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.50f)),
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.30f)),
                        },
                        new Muscle[]
                        {
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.25f)),
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.10f))
                        }
                    )
                },
                {
                    "Right Front",
                    (
                        new Muscle[]
                        {
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.25f)),
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.30f)),
                        },
                        new Muscle[]
                        {
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.25f))
                        }
                    )
                },
                {
                    "Left Front",
                    (
                        new Muscle[]
                        {
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.50f)),
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.15f)),
                        },
                        new Muscle[]
                        {
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.25f))
                        }
                    )
                },
                {
                    "Back",
                    (
                        new Muscle[]
                        {
                            Muscle.Lumbar_L.WithIntensity(Convert.ToInt32(_intensities.Lumbar_L * 0.50f)),
                            Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R * 0.30f))
                        },
                        new Muscle[]
                        {
                            Muscle.Lumbar_L.WithIntensity(Convert.ToInt32(_intensities.Lumbar_L * 0.25f)),
                            Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R * 0.15f))
                        }
                    )
                },
                {
                    "Back Right",
                    (
                        new Muscle[]
                        {
                            Muscle.Lumbar_L.WithIntensity(Convert.ToInt32(_intensities.Lumbar_L * 0.50f)),
                            Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R * 0.30f))
                        },
                        new Muscle[]
                        {
                            Muscle.Lumbar_L.WithIntensity(Convert.ToInt32(_intensities.Lumbar_L * 0.25f))
                        }
                    )
                },
                {
                    "Back Left",
                    (
                        new Muscle[]
                        {
                            Muscle.Lumbar_L.WithIntensity(Convert.ToInt32(_intensities.Lumbar_L * 0.50f)),
                            Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R * 0.15f))
                        },
                        new Muscle[]
                        {
                            Muscle.Lumbar_L.WithIntensity(Convert.ToInt32(_intensities.Lumbar_L * 0.25f))
                        }
                    )
                },
                {
                    "Left",
                    (
                        new Muscle[]
                        {
                            Muscle.Lumbar_L.WithIntensity(Convert.ToInt32(_intensities.Lumbar_L * 0.50f)),
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.50f)),
                        },
                        new Muscle[]
                        {
                            Muscle.Lumbar_L.WithIntensity(Convert.ToInt32(_intensities.Lumbar_L * 0.25f)),
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.25f)),
                        }
                    )
                },
                {
                    "Right",
                    (
                        new Muscle[]
                        {
                            Muscle.Lumbar_L.WithIntensity(Convert.ToInt32(_intensities.Lumbar_L * 0.50f)),
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.50f)),
                        },
                        new Muscle[]
                        {
                            Muscle.Lumbar_L.WithIntensity(Convert.ToInt32(_intensities.Lumbar_L * 0.25f)),
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.25f)),
                        }
                    )
                }
            };
            if (_currentDirection != null && directionMuscleMap.ContainsKey(_currentDirection))
            {
                (_currentEntryMuscles, _currentBleedMuscles) = directionMuscleMap[_currentDirection];
            }
        }

        public static void RightLegHitBoxCalculation(ref Muscle[]? _currentEntryMuscles, ref Muscle[]? _currentBleedMuscles, ref IntensitySettings _intensities, ref string? _currentDirection)
        {
            Dictionary<string, (Muscle[], Muscle[])> directionMuscleMap = new()
            {
                {
                    "Front", (
                        new Muscle[]
                        {
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.50f)),
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.30f)),
                        },
                        new Muscle[]
                        {
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.25f)),
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.10f))
                        }
                    )
                },
                {
                    "Front Right", (
                        new Muscle[]
                        {
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.50f)),
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.30f)),
                        },
                        new Muscle[]
                        {
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.25f)),
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.10f))
                        }
                    )
                },
                {
                    "Front Left", (
                        new Muscle[]
                        {
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.50f)),
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.30f)),
                        },
                        new Muscle[]
                        {
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.25f)),
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.10f))
                        }
                    )
                },
                {
                    "Right Front", (
                        new Muscle[]
                        {
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.25f)),
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.30f)),
                        },
                        new Muscle[]
                        {
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.25f))
                        }
                    )
                },
                {
                    "Left Front", (
                        new Muscle[]
                        {
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.50f)),
                            Muscle.Abdominal_L.WithIntensity(Convert.ToInt32(_intensities.Abdominal_L * 0.15f)),
                        },
                        new Muscle[]
                        {
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.25f))
                        }
                    )
                },
                {
                    "Back", (
                        new Muscle[]
                        {
                            Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R * 0.50f)),
                            Muscle.Lumbar_L.WithIntensity(Convert.ToInt32(_intensities.Lumbar_L * 0.30f))
                        },
                        new Muscle[]
                        {
                            Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R * 0.25f)),
                            Muscle.Lumbar_L.WithIntensity(Convert.ToInt32(_intensities.Lumbar_L * 0.15f))
                        }
                    )
                },
                {
                    "Back Right", (
                        new Muscle[]
                        {
                            Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R * 0.50f)),
                            Muscle.Lumbar_L.WithIntensity(Convert.ToInt32(_intensities.Lumbar_L * 0.30f))
                        },
                        new Muscle[]
                        {
                            Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R * 0.25f))
                        }
                    )
                },
                {
                    "Back Left", (
                        new Muscle[]
                        {
                            Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R * 0.50f)),
                            Muscle.Lumbar_L.WithIntensity(Convert.ToInt32(_intensities.Lumbar_L * 0.30f))
                        },
                        new Muscle[]
                        {
                            Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R * 0.25f))
                        }
                    )
                },
                {
                    "Right Back", (
                        new Muscle[]
                        {
                            Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R * 0.50f)),
                            Muscle.Lumbar_L.WithIntensity(Convert.ToInt32(_intensities.Lumbar_L * 0.30f))
                        },
                        new Muscle[]
                        {
                            Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R * 0.25f))
                        }
                    )
                },
                {
                    "Left Back", (
                        new Muscle[]
                        {
                            Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R * 0.50f)),
                            Muscle.Lumbar_L.WithIntensity(Convert.ToInt32(_intensities.Lumbar_L * 0.30f))
                        },
                        new Muscle[]
                        {
                            Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R * 0.25f))
                        }
                    )
                },
                {
                    "Left", (
                        new Muscle[]
                        {
                            Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R * 0.50f)),
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.50f))
                        },
                        new Muscle[]
                        {
                            Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R * 0.25f)),
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.25f))
                        }
                    )
                },
                {
                    "Right", (
                        new Muscle[]
                        {
                            Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R * 0.50f)),
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.50f))
                        },
                        new Muscle[]
                        {
                            Muscle.Lumbar_R.WithIntensity(Convert.ToInt32(_intensities.Lumbar_R * 0.25f)),
                            Muscle.Abdominal_R.WithIntensity(Convert.ToInt32(_intensities.Abdominal_R * 0.25f))
                        }
                    )
                },
            };
            if (_currentDirection != null && directionMuscleMap.ContainsKey(_currentDirection))
            {
                (_currentEntryMuscles, _currentBleedMuscles) = directionMuscleMap[_currentDirection];
            }
        }

    }
}
