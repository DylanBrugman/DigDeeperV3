// using System.Collections.Generic;
//
// namespace Systems.ColonistSystem {
//     public class Tool : Item {
//
//         public ToolType Type { get; set; }
//         readonly HashSet<ToolAction> _actions;   // e.g. { Dig, Chop }
//
//         public Tool(params ToolAction[] actions) => _actions = new HashSet<ToolAction>(actions);
//
//         public bool CanDo(ToolAction a) => _actions.Contains(a);
//     }
//     
//     public enum ToolType {
//         Pickaxe,
//         Axe,
//         Shovel,
//         Hammer,
//         Saw,
//         Wrench,
//         Drill,
//     }
//
//     public enum ToolAction {
//         Dig,
//         Chop,
//         Build,
//         Repair,
//         Saw,
//         Wrench,
//         Drill
//     }
// }