using Core.ECS;
using Core.ECS.Core.ECS;
using GamePlay.Tool;

namespace GamePlay.Inventory {
    public struct InventoryComponent : IComponent {
        public Buffer<EntityId> Items; // This is a buffer of EntityIds representing items in the inventoryComponent.
    }

    public static class InventoryExtensions {
        public static int Count(this InventoryComponent inventoryComponent) => inventoryComponent.Items.Count;
        public static EntityId GetItem(this InventoryComponent inventoryComponent, int index) {
            if (index < 0 || index >= inventoryComponent.Items.Count) {
                throw new System.IndexOutOfRangeException("Index out of range for inventoryComponent items.");
            }

            return inventoryComponent.Items[index];
        }

        // public static EntityId GetItem(this InventoryComponent inventoryComponent, int index) {
        //     if (index < 0 || index >= inventoryComponent.Items.Count) {
        //         throw new System.IndexOutOfRangeException("Index out of range for inventoryComponent items.");
        //     }
        //
        //     return inventoryComponent.Items[index];
        // }
        //
        // public static bool HasItem(this InventoryComponent inventoryComponent, int index) => index >= 0 && index < inventoryComponent.Items.Count;
        //
        // public static bool HasItem(this InventoryComponent inventoryComponent, EntityId item) {
        //     return inventoryComponent.Items.Contains(item);
        // }
        //
        // public static bool HasToolFor(this InventoryComponent inventoryComponent, ToolAction action) {
        //     foreach (var item in inventoryComponent.Items) {
        //         if (item.IsTool && item.CanDo(action)) {
        //             return true;
        //         }
        //     }
        //     return false;
        // }

        // public static bool AddItem(this InventoryComponent inventoryComponent, in EntityId item) {
        //     // example rule: don't over-stack
        //     if (item.Count > 1 && TryFindStack(inventoryComponent, item.Type, out int i)) {
        //         ref var stack = ref inventoryComponent.Items[i];
        //         byte space = (byte) (ItemDB.Def(item.Type).MaxStack - stack.Count);
        //         if (space == 0) return false;
        //         byte toMove = (byte) Mathf.Min(space, item.Count);
        //         stack.Count += toMove;
        //         // if item.Count > toMove you’d handle leftovers outside
        //         return true;
        //     }
        //
        //     inventoryComponent.Items.Add(item);
        //     return true;
        // }
        //
        // public static bool RemoveItem(this InventoryComponent inventoryComponent, in Item item) =>
        //     inventoryComponent.Items.RemoveSwapBack(item);
        //
        // public static bool HasToolFor(this InventoryComponent inventoryComponent, ToolAction action) {
        //     foreach (ref var it in inventoryComponent.Items.AsSpan())
        //         if (it.IsTool && it.CanDo(action))
        //             return true;
        //     return false;
        // }
        //
        // public static bool TryFindStack(this InventoryComponent inventoryComponent, ItemType type, out int index) {
        //     for (int i = 0; i < inventoryComponent.Items.Count; i++)
        //         if (inventoryComponent.Items[i].Type == type && inventoryComponent.Items[i].Count < ItemDB.Def(type).MaxStack) {
        //             index = i;
        //             return true;
        //         }
        //
        //     index = -1;
        //     return false;
        // }
    }
}