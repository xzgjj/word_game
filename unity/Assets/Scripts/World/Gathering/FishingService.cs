using System.Collections.Generic;
using StarryForest.Core;
using StarryForest.Inventory;

namespace StarryForest.World.Gathering
{
    public sealed class FishingService
    {
        private readonly InventoryService inventoryService;
        private readonly HashSet<string> fishingNodes;

        public FishingService(InventoryService inventoryService)
        {
            this.inventoryService = inventoryService;
            fishingNodes = new HashSet<string>
            {
                "river-fish",
                "shallow-fish"
            };
        }

        public bool HasNode(string nodeId)
        {
            return fishingNodes.Contains(nodeId);
        }

        public OperationResult CatchFish(PlayerState state, string nodeId)
        {
            if (!fishingNodes.Contains(nodeId))
            {
                return OperationResult.Fail("钓鱼点不存在。");
            }

            OperationResult addResult = inventoryService.Add(state, ItemId.Fish, 1);
            if (!addResult.Success)
            {
                return addResult;
            }

            return OperationResult.Ok("鱼已收进物品栏。");
        }
    }
}
