
using Core.ECS;

namespace GamePlay.Inventory {
    public struct ItemTag : IComponent { }
}

// using System;
// using System.Runtime.CompilerServices;
// using GamePlay.InventoryComponent;
// using GamePlay.InventoryComponent.GamePlay.InventoryComponent;
// using UnityEngine;
//
// public struct Item
// {
//     // ───── raw data ─────
//     public ItemType Type;  // 2 bytes
//     public byte     Count; // 1–99 for stackables, always 1 for tools
//     public byte     Durability; // 0–100, only relevant for tools
//
//     public readonly bool IsTool
//     {
//         [MethodImpl(MethodImplOptions.AggressiveInlining)]
//         get => ItemDB.Def(Type).IsTool;
//     }
//
//     public readonly bool CanDo(ToolAction action) => (ItemDB.Def(Type).Actions & action) != 0;
//
//     public readonly float Weight
//     {
//         [MethodImpl(MethodImplOptions.AggressiveInlining)]
//         get => ItemDB.Def(Type).Weight * Mathf.MaxVelocity(1, Count);
//     }
// }