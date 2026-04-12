using StarryForest.Core;
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

        public WorldNodeService(
            GatherService gatherService,
            FishingService fishingService,
            MiniGameService miniGameService,
            SignboardService signboardService = null)
        {
            this.gatherService = gatherService;
            this.fishingService = fishingService;
            this.miniGameService = miniGameService;
            this.signboardService = signboardService;
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

            return OperationResult.Fail("世界节点不存在或尚未接入。");
        }
    }
}
