// using System;
// using System.Collections.Generic;
// using System.Linq;
// using DigDeeper.ColonistSystem;
// using UnityEngine;
//
// namespace Systems.ColonistSystem {
//     public class ColonistNeeds : MonoBehaviour
//     {
//         
//         
//         // Properties
//         public bool HasCriticalNeeds => needs.Values.Any(need => need.IsCritical);
//         public NeedConfig GetMostCriticalNeed() => needs.Values.Where(n => n.IsCritical).OrderBy(n => n.InitialValue).FirstOrDefault();
//         
//         // Events
//         public event Action<NeedType, float> OnNeedChanged;
//         public event Action<NeedConfig> OnNeedBecameCritical;
//         
//         public void Initialize(List<NeedConfig> baseNeeds)
//         {
//             needs.Clear();
//             
//             foreach (var need in baseNeeds)
//             {
//                 needs[need.type] = new NeedConfig
//                 {
//                     type = need.type,
//                     InitialValue = need.InitialValue,
//                     DecayRatePerMinute = need.DecayRatePerMinute,
//                     CriticalThreshold = need.CriticalThreshold
//                 };
//             }
//         }
//         
//         private void Update()
//         {
//             UpdateNeeds(Time.deltaTime);
//         }
//         
//         private void UpdateNeeds(float deltaTime)
//         {
//             foreach (var need in needs.Values)
//             {
//                 float oldValue = need.InitialValue;
//                 need.Decay(deltaTime);
//                 
//                 if (need.InitialValue != oldValue)
//                 {
//                     OnNeedChanged?.Invoke(need.type, need.InitialValue);
//                     
//                     if (!need.IsCritical && oldValue > need.CriticalThreshold && need.InitialValue <= need.CriticalThreshold)
//                     {
//                         OnNeedBecameCritical?.Invoke(need);
//                     }
//                 }
//             }
//         }
//         
//         public float GetNeedValue(NeedType needType)
//         {
//             return needs.ContainsKey(needType) ? needs[needType].InitialValue : 0f;
//         }
//         
//         public void SatisfyNeed(NeedType needType, float amount)
//         {
//             if (needs.ContainsKey(needType))
//             {
//                 needs[needType].Satisfy(amount);
//                 OnNeedChanged?.Invoke(needType, needs[needType].InitialValue);
//             }
//         }
//         
//         public NeedConfig GetNeed(NeedType needType)
//         {
//             return needs.ContainsKey(needType) ? needs[needType] : null;
//         }
//         
//         public List<NeedConfig> GetAllNeeds()
//         {
//             return needs.Values.ToList();
//         }
//     }
// }