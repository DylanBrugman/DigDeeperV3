using DigDeeper.ColonistSystem;
using Systems.ColonistSystem;
using UnityEngine;

namespace Systems.NeedsSystem {
    [System.Serializable]
    public class NeedConfig : ScriptableObject {
        public NeedType type;
        [Range(0, 100)] public float InitialValue = 100f;
        [Range(0, 100)] public float DecayRatePerMinute = 1f;
        [Range(0, 100)] public float CriticalThreshold = 20f;
        [Range(0, 100)] public float MaxValue = 100f;
        [Range(0, 100)] public float ActionThreshold = 30f;
    }
}