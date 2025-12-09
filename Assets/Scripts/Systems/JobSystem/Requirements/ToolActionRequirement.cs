// using System.Linq;
// using GamePlay.InventoryComponent;
// using Systems.ColonistSystem;
//
// namespace Systems.JobSystem.Requirements {
//     public class ToolActionRequirement : IRequirement<InventoryComponent> {
//         
//         private readonly ToolAction _requiredToolAction;
//         
//         public ToolActionRequirement(ToolAction requiredToolAction) {
//             _requiredToolAction = requiredToolAction;
//         }
//
//         public bool IsMet(InventoryComponent inventory) {
//             return inventory.(_requiredToolAction);
//         }
//
//         public string GetFailDescription() {
//             return $"Colonist does not have a tool for action {_requiredToolAction} or the inventory is empty.";
//         }
//     }
// }