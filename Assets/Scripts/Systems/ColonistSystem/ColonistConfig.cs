using System.Collections.Generic;
using Systems.NeedsSystem;
using UnityEngine;

namespace Systems.ColonistSystem {
    [CreateAssetMenu(fileName = "New ColonistController Config", menuName = "DigDeeper/ColonistController Config")]
    public class ColonistConfig : ScriptableObject
    {
        public string Id = "colonist";
        
        [Header("Basic Info")]
        public Sprite Sprite;
        public int height = 2;
        
        [Header("Skills")]
        public List<SkillConfig> startingSkills = new List<SkillConfig>();

        [Header("NeedConfig Configuration")]
        public List<NeedConfig> baseNeeds = new List<NeedConfig>();

        public float InitialHunger { get; set; }
        public float InitialThirst { get; set; }
        public string Name { get; set; }
        public float MaxVelocity { get; set; }
    }
}