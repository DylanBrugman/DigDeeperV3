// using System;
// using System.Runtime.CompilerServices;
// using GamePlay.InventoryComponent.GamePlay.InventoryComponent;
//
// namespace GamePlay.InventoryComponent {
//     /// Static lookup table: O(1) by enum value, zero allocations at run-time.
//     public static class ItemDB {
//         static readonly ItemDef[] _defs;
//
//         static ItemDB() {
//             _defs = new ItemDef[Enum.GetValues<ItemType>().Length];
//
//             // Note: you could load these from JSON, ScriptableObjects, etc.
//             Add(new() { Id = ItemType.Rock,  Stackable = true,  MaxStack = 99, Weight = 1 });
//             Add(new() { Id = ItemType.Wood,  Stackable = true,  MaxStack = 99, Weight = 0.5f });
//             Add(new() { Id = ItemType.Axe,   Stackable = false, Actions = ToolAction.Chop, MaxDurability = 100, Weight = 2 });
//             Add(new() { Id = ItemType.Pickaxe, Stackable = false, Actions = ToolAction.Mine, MaxDurability = 120, Weight = 3 });
//             // …
//         }
//
//         static void Add(ItemDef def) => _defs[(int)def.Id] = def;
//
//         [MethodImpl(MethodImplOptions.AggressiveInlining)]
//         public static ref readonly ItemDef Def(ItemType id) => ref _defs[(int)id];
//     }
// }