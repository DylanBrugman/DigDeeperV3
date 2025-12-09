using Core.ECS;
using DigDeeper.ColonistSystem;
using Systems.ColonistSystem;

namespace GamePlay.Needs {
    public struct Need
    {
        public NeedType Type; // The type of need (e.g., hunger, thirst, sleep)
        public float Value; // Represents the current level of the need (e.g., hunger, thirst)
        public float MaxValue; // Represents the maximum level of the need
        public float DecayRatePerMinute; // How quickly the need decays over time
        public float ActionThreshold; // The level at which an action is triggered to satisfy the need
        public float CriticalThreshold; // The level at which the need becomes critical and may cause negative effects
        public NeedLevel Level;
    }
}