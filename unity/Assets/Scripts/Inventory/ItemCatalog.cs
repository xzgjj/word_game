using System;
using System.Collections.Generic;
using StarryForest.Core;

namespace StarryForest.Inventory
{
    public static class ItemCatalog
    {
        private static readonly ItemId[] AllItems =
        {
            ItemId.Wood,
            ItemId.Stone,
            ItemId.FlowerSeed,
            ItemId.RiverShell,
            ItemId.Fish,
            ItemId.EmotionShard,
            ItemId.StarCoin,
            ItemId.OldCartridge,
            ItemId.StarCore,
            ItemId.Sticker,
            ItemId.Axe
        };

        public static IReadOnlyList<ItemId> Items => AllItems;

        public static int InventorySlotCount => AllItems.Length + GameConstants.ExtraInventorySlots;

        public static bool Contains(ItemId itemId)
        {
            return Array.IndexOf(AllItems, itemId) >= 0;
        }
    }
}
