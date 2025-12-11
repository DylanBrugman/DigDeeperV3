using System;
using DigDeeper.ColonistSystem;
using UnityEngine;

namespace Systems.ColonistSystem {
    [Serializable]
    public class SkillConfig
    {
        public SkillType Type;
        [Range(0, 100)] public int InitialLevel = 1;
        [Range(0, 100)] public int InitialExperience = 0;
    }
}