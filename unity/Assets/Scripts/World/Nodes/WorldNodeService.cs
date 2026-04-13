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
        private const string BridgeRepairGuideId = "bridge-repair-guide";

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
                if (nodeId == "river-bridge" && !state.KnownSystems.Contains(BridgeRepairGuideId))
                {
                    return DiscoverBridgeRepairGuide(state);
                }

                if (IsClearableFlowerBedSlot(nodeId) && GetWorldNodeStage(state, nodeId) < 3)
                {
                    return ClearFlowerBedSlot(state, nodeId);
                }

                OperationResult preflightResult = GetBuildPreflightResult(state, nodeId, blueprintId);
                if (!preflightResult.Success)
                {
                    return preflightResult;
                }

                OperationResult buildResult = buildService.Place(state, blueprintId, gridX, gridY);
                if (!buildResult.Success)
                {
                    return GetBuildFailureResult(nodeId, buildResult);
                }

                return GetBuildSuccessResult(nodeId, buildResult);
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

        private OperationResult GetBuildPreflightResult(PlayerState state, string nodeId, BlueprintId blueprintId)
        {
            if (!state.UnlockedBlueprints.Contains(blueprintId))
            {
                if (nodeId == "river-bridge")
                {
                    state.UnlockedBlueprints.Add(BlueprintId.Bridge);
                    return OperationResult.Ok();
                }

                return OperationResult.Fail("图纸还没记录在木牌里。先打开木牌记录。");
            }

            if (inventoryService == null)
            {
                return OperationResult.Ok();
            }

            if (nodeId == "river-bridge" && inventoryService.GetCount(state, ItemId.Wood) < 1)
            {
                return OperationResult.Fail("修桥需要木材 1。先领取今日赠礼、出售物品买木材，或清理落枝。");
            }

            if (IsClearableFlowerBedSlot(nodeId) && inventoryService.GetCount(state, ItemId.FlowerSeed) < 2)
            {
                return OperationResult.Fail("花圃需要花种 2 和石子 1。先清理地块或在木牌购买花种。");
            }

            return OperationResult.Ok();
        }

        private OperationResult DiscoverBridgeRepairGuide(PlayerState state)
        {
            if (inventoryService == null)
            {
                return OperationResult.Fail("修桥引导未接入物品栏。");
            }

            state.KnownSystems.Add(BridgeRepairGuideId);
            state.UnlockedBlueprints.Add(BlueprintId.Bridge);
            int woodNeeded = 1 - inventoryService.GetCount(state, ItemId.Wood);
            if (woodNeeded > 0)
            {
                OperationResult addResult = inventoryService.Add(state, ItemId.Wood, woodNeeded);
                if (!addResult.Success)
                {
                    return addResult;
                }
            }

            return OperationResult.Ok(woodNeeded > 0
                ? "你发现了断桥。木屋旁的备用木板已放入背包，再点击断桥就能修复。"
                : "你发现了断桥。背包里的木材足够修复，再点击断桥就能开始。");
        }

        private static OperationResult GetBuildFailureResult(string nodeId, OperationResult buildResult)
        {
            if (nodeId == "river-bridge" && buildResult.Message == "材料不足。")
            {
                return OperationResult.Fail("修桥材料不足：需要木材 1。");
            }

            if (IsClearableFlowerBedSlot(nodeId) && buildResult.Message == "材料不足。")
            {
                return OperationResult.Fail("花圃材料不足：需要花种 2 和石子 1。");
            }

            return buildResult;
        }

        private static OperationResult GetBuildSuccessResult(string nodeId, OperationResult buildResult)
        {
            return nodeId switch
            {
                "river-bridge" => OperationResult.Ok("木桥修好了。现在可以从桥面过河。"),
                "flower-bed-slot" => OperationResult.Ok("花圃建好了。地块已经变成花园。"),
                "forest-flower-bed-slot" => OperationResult.Ok("森林花圃建好了。这里多了一片花园。"),
                _ => buildResult
            };
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
