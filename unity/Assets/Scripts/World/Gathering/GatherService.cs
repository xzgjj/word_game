using System.Collections.Generic;
using StarryForest.Core;
using StarryForest.Inventory;

namespace StarryForest.World.Gathering
{
    public sealed class GatherService
    {
        private readonly InventoryService inventoryService;
        private readonly Dictionary<string, GatherNodeDefinition> gatherNodes;

        public GatherService(InventoryService inventoryService)
        {
            this.inventoryService = inventoryService;
            gatherNodes = new Dictionary<string, GatherNodeDefinition>
            {
                { "forest-branch", new GatherNodeDefinition("forest-branch", ItemId.Wood, 1, "旁友森林树枝") },
                { "home-fallen-branch", new GatherNodeDefinition("home-fallen-branch", ItemId.Wood, 1, "木屋旁落枝") },
                { "forest-flower-seed", new GatherNodeDefinition("forest-flower-seed", ItemId.FlowerSeed, 1, "森林边缘花种") },
                { "home-grass-flower-seed", new GatherNodeDefinition("home-grass-flower-seed", ItemId.FlowerSeed, 1, "木屋前草丛") },
                { "river-stone", new GatherNodeDefinition("river-stone", ItemId.Stone, 1, "河岸石子") },
                { "river-shell", new GatherNodeDefinition("river-shell", ItemId.RiverShell, 1, "河岸河贝") },
                { "shallow-river-shell", new GatherNodeDefinition("shallow-river-shell", ItemId.RiverShell, 1, "浅水区河贝") }
            };
        }

        public bool HasNode(string nodeId)
        {
            return gatherNodes.ContainsKey(nodeId);
        }

        public OperationResult Collect(PlayerState state, string nodeId)
        {
            if (!gatherNodes.TryGetValue(nodeId, out GatherNodeDefinition node))
            {
                return OperationResult.Fail("采集点不存在。");
            }

            OperationResult addResult = inventoryService.Add(state, node.ItemId, node.Amount);
            if (!addResult.Success)
            {
                return addResult;
            }

            return OperationResult.Ok($"{node.SourceLabel}已收进物品栏。");
        }
    }
}
