// using System;
// using DigDeeper.ColonistSystem;
//
// namespace Systems.ColonistSystem {
//     [Serializable]
//     public class ColonistStats {
//         private ColonistConfig Config;
//
//         public float CurrentHealth { get; set; }
//         public float CurrentStamina { get; set; }
//         public float CurrentHunger { get; set; }
//         public float CurrentThirst { get; set; }
//
//         // Limits
//         private float MaxHealth => Config.BaseHealth;
//         private float MaxStamina => Config.BaseStamina;
//         private float MaxHunger => Config.BaseHunger;
//         private float MaxMovementSpeed => Config.BaseSpeed;
//         private float MaxThirst => Config.BaseThirst;
//
//         // Properties
//         public float HealthPercentage => CurrentHealth / MaxHealth;
//         public float StaminaPercentage => CurrentStamina / MaxStamina;
//
//         public ColonistStats(ColonistConfig config) {
//             if (config == null) throw new ArgumentNullException(nameof(config));
//
//             Config = config;
//
//             // Initialize stats based on config values
//             CurrentHealth = config.BaseHealth;
//             CurrentStamina = config.BaseStamina;
//             CurrentHunger = config.BaseHunger;
//             CurrentThirst = config.BaseThirst;
//         }
//     }
// }