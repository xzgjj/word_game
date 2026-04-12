using System.Collections.Generic;
using StarryForest.Core;

namespace StarryForest.Inventory
{
    public sealed class InventoryService
    {
        public int GetSlotCount()
        {
            return ItemCatalog.InventorySlotCount;
        }

        public int GetCount(PlayerState state, ItemId itemId)
        {
            return state.Items.TryGetValue(itemId, out int count) ? count : 0;
        }

        public OperationResult Add(PlayerState state, ItemId itemId, int amount)
        {
            if (!ItemCatalog.Contains(itemId))
            {
                return OperationResult.Fail("未知物品。");
            }

            if (amount <= 0)
            {
                return OperationResult.Fail("增加数量必须大于 0。");
            }

            int current = GetCount(state, itemId);
            int next = current + amount;
            state.Items[itemId] = next > GameConstants.MaxItemStack ? GameConstants.MaxItemStack : next;
            return OperationResult.Ok("物品已进入物品栏。");
        }

        public bool CanAfford(PlayerState state, IReadOnlyDictionary<ItemId, int> cost)
        {
            foreach (KeyValuePair<ItemId, int> entry in cost)
            {
                if (entry.Value < 0 || GetCount(state, entry.Key) < entry.Value)
                {
                    return false;
                }
            }

            return true;
        }

        public OperationResult Spend(PlayerState state, IReadOnlyDictionary<ItemId, int> cost)
        {
            if (!CanAfford(state, cost))
            {
                return OperationResult.Fail("材料不足。");
            }

            foreach (KeyValuePair<ItemId, int> entry in cost)
            {
                state.Items[entry.Key] = GetCount(state, entry.Key) - entry.Value;
            }

            return OperationResult.Ok("材料已消耗。");
        }
    }
}
