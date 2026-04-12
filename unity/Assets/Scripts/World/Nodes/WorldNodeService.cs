using StarryForest.Core;
using StarryForest.Building;
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
        private readonly SignboardService signboardService;
        private readonly BuildService buildService;

        public WorldNodeService(
            GatherService gatherService,
            FishingService fishingService,
            MiniGameService miniGameService,
            SignboardService signboardService = null,
            BuildService buildService = null)
        {
            this.gatherService = gatherService;
            this.fishingService = fishingService;
            this.miniGameService = miniGameService;
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
                return buildService.Place(state, blueprintId, gridX, gridY);
            }

            return OperationResult.Fail("世界节点不存在或尚未接入。");
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
