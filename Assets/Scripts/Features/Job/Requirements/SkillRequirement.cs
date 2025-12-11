// using System.Collections.Generic;
// using DigDeeper.ColonistSystem;
// using Systems.ColonistSystem;
//
// namespace Systems.JobSystem.Requirements {
//     public class SkillRequirement : IRequirement<Dictionary<SkillType, SkillConfig>> {
//         
//         private readonly SkillType _skillType;
//         private readonly int _requiredLevel;
//         public SkillRequirement(SkillType digging, int level) {
//             _skillType = digging;
//             _requiredLevel = level;
//         }
//
//         public bool IsMet(Dictionary<SkillType, SkillConfig> colonistSkills) {
//             return colonistSkills.TryGetValue(_skillType, out var skill) && skill.Level >= _requiredLevel;
//         }
//
//         public string GetFailDescription() {
//             return $"Colonist does not have the required skill level for {_skillType}. Required: {_requiredLevel}";
//         }
//     }
// }