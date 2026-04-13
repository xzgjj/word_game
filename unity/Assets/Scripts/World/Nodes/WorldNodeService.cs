using StarryForest.Core;
using StarryForest.Building;
using StarryForest.Inventory;
using StarryForest.MiniGame;
using StarryForest.Signboard;
using StarryForest.World.Gathering;

namespace StarryForest.World.Nodes
{
    public sealed class WorldNodeService
    {
        private const string ArcadeNodeId = "clearing-arcade";
        private const string SignboardNodeId = "home-signboard";

        private readonly GatherService gatherService;
        private readonly FishingService fishingService;
        private readonly MiniGameService miniGameService;
        private readonly InventoryService inventoryService;
        private readonly SignboardService signboardService;
        private readonly BuildService buildService;

        public WorldNodeService(
            GatherService gatherService,
            FishingService fishingService,
            MiniGameService miniGameService,
            InventoryService inventoryService = null,
            SignboardService signboardService = null,
            BuildService buildService = null)
        {
            this.gatherService = gatherService;
            this.fishingService = fishingService;
            this.miniGameService = miniGameService;
            this.inventoryService = inventoryService;
            this.signboardService = signboardService;
            this.buildService = buildService;
        }

        public OperationResult Interact(PlayerState state, string nodeId)
        {
            if (gatherService.HasNode(nodeId))
            {
                return gatherService.Collect(state, nodeId);
            }

            if (fishingService.HasNode(nodeId))
            {
                return fishingService.CatchFish(state, nodeId);
            }

            if (nodeId == ArcadeNodeId)
            {
                return miniGameService.DiscoverArcade(state);
            }

            if (nodeId == SignboardNodeId && signboardService != null)
            {
                return signboardService.Open(state);
            }

            if (buildService != null && TryGetBuildNode(nodeId, out BlueprintId blueprintId, out int gridX, out int gridY))
            {
                if (IsClearableFlowerBedSlot(nodeId) && GetWorldNodeStage(state, nodeId) < 3)
                {
                    return ClearFlowerBedSlot(state, nodeId);
                }

                return buildService.Place(state, blueprintId, gridX, gridY);
            }

            return OperationResult.Fail("世界节点不存在或尚未接入。");
        }

        public static int GetWorldNodeStage(PlayerState state, string nodeId)
        {
            return state.WorldNodeStages.TryGetValue(nodeId, out int stage) ? stage : 0;
        }

        private OperationResult ClearFlowerBedSlot(PlayerState state, string nodeId)
        {
            if (inventoryService == null)
            {
                return OperationResult.Fail("清理服务未接入。");
            }

            int stage = GetWorldNodeStage(state, nodeId);
            ItemId reward = stage switch
            {
                0 => ItemId.Wood,
                1 => ItemId.Stone,
                _ => ItemId.FlowerSeed
            };

            OperationResult addResult = inventoryService.Add(state, reward, 1);
            if (!addResult.Success)
            {
                return addResult;
            }

            state.WorldNodeStages[nodeId] = stage + 1;
            return OperationResult.Ok((stage + 1) switch
            {
                1 => "落枝清掉了，木材已进入背包。",
                2 => "碎石清掉了，石子已进入背包。",
                _ => "杂草清掉了，这片地变成可建设花圃的空位。"
            });
        }

        private static bool IsClearableFlowerBedSlot(string nodeId)
        {
            return nodeId == "flower-bed-slot" || nodeId == "forest-flower-bed-slot";
        }

        private static bool TryGetBuildNode(string nodeId, out BlueprintId blueprintId, out int gridX, out int gridY)
        {
            switch (nodeId)
            {
                case "river-bridge":
                    blueprintId = BlueprintId.Bridge;
                    gridX = 8;
                    gridY = 2;
                    return true;
                case "flower-bed-slot":
                    blueprintId = BlueprintId.FlowerBed;
                    gridX = 3;
                    gridY = 1;
                    return true;
                case "forest-flower-bed-slot":
                    blueprintId = BlueprintId.FlowerBed;
                    gridX = 2;
                    gridY = 3;
                    return true;
                case "forest-sign-slot":
                    blueprintId = BlueprintId.ForestSign;
                    gridX = 2;
                    gridY = 4;
                    return true;
                case "craft-bench-slot":
                    blueprintId = BlueprintId.CraftBench;
                    gridX = 4;
                    gridY = 1;
                    return true;
                case "river-lamp-slot":
                    blueprintId = BlueprintId.RiverLamp;
                    gridX = 7;
                    gridY = 4;
                    return true;
                case "arcade-base-slot":
                    blueprintId = BlueprintId.ArcadeBase;
                    gridX = 10;
                    gridY = 2;
                    return true;
                default:
                    blueprintId = default;
                    gridX = 0;
                    gridY = 0;
                    return false;
            }
        }
    }
}
