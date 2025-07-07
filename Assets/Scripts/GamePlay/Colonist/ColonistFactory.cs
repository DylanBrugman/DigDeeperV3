using System.Collections.Generic;
using Core.ECS;
using Core.ECS.Core.ECS;
using ECSImpl.Components;
using GamePlay.Inventory;
using GamePlay.Jobs;
using GamePlay.Movement;
using GamePlay.Needs;
using GamePlay.Skills;
using GamePlay.View;
using Systems.ColonistSystem;
using Systems.NeedsSystem;
using Unity.Mathematics;

namespace GamePlay.Colonist {
    public static class ColonistFactory {
        
        public static EntityId Create(ECSWorld ecsWorld, ColonistConfig config, float2 position) {
            var colonist = ecsWorld.CreateEntity();
            
            ecsWorld.Add(colonist, new ColonistTag {Name = config.Name});
            ecsWorld.Add(colonist, new PositionComponent { Position = position });
            ecsWorld.Add(colonist, new VelocityComponent { Velocity = float2.zero, MaxVelocity = config.MaxVelocity });
            ecsWorld.Add(colonist, new PersonalJobsComponent { Jobs = new Buffer<JobData>() });
            ecsWorld.Add(colonist, CreateNeedsComponent(config.baseNeeds));
            ecsWorld.Add(colonist, new InventoryComponent { Items = new Buffer<EntityId>() });
            ecsWorld.Add(colonist, CreatSkillsComponent(config.startingSkills));
            ecsWorld.Add(colonist, new SpriteComponent { Sprite = config.Sprite });
            return colonist;
        }

        private static SkillsComponent CreatSkillsComponent(List<SkillConfig> skillConfigs) {
            SkillsComponent skillsComponent = new() {
                Skills = new Buffer<Skill>()
            };
            
            foreach (var skillConfig in skillConfigs) {
                skillsComponent.Skills.Add(new Skill {
                    Type = skillConfig.Type,
                    Level = skillConfig.InitialLevel,
                    Experience = skillConfig.InitialExperience
                });
            }
            return skillsComponent;
            
        }

        private static NeedsComponent CreateNeedsComponent(List<NeedConfig> needConfigs) {
            NeedsComponent needsComponent = new() {
                Needs = new Buffer<Need>()
            };
            
            foreach (var needConfig in needConfigs) {
                needsComponent.Needs.Add(new Need {
                    Type = needConfig.type,
                    Value = needConfig.InitialValue,
                    MaxValue = needConfig.MaxValue,
                    ActionThreshold = needConfig.ActionThreshold,
                    CriticalThreshold = needConfig.CriticalThreshold,
                    DecayRatePerMinute = needConfig.DecayRatePerMinute
                });
            }
            return needsComponent;
        }
    }
}