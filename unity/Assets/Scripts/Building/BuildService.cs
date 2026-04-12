using System.Collections.Generic;
using StarryForest.Core;
using StarryForest.Inventory;

namespace StarryForest.Building
{
    public sealed class BuildService
    {
        private readonly InventoryService inventoryService;
        private readonly BlueprintService blueprintService;
        private readonly Dictionary<BlueprintId, BuildRecipe> buildRecipes;

        public BuildService(InventoryService inventoryService, BlueprintService blueprintService)
        {
            this.inventoryService = inventoryService;
            this.blueprintService = blueprintService;
            buildRecipes = new Dictionary<BlueprintId, BuildRecipe>
            {
                { BlueprintId.Bridge, new BuildRecipe(BlueprintId.Bridge, new Dictionary<ItemId, int> { { ItemId.Wood, 1 } }) },
                { BlueprintId.FlowerBed, new BuildRecipe(BlueprintId.FlowerBed, new Dictionary<ItemId, int> { { ItemId.FlowerSeed, 2 }, { ItemId.Stone, 1 } }) },
                { BlueprintId.ForestSign, new BuildRecipe(BlueprintId.ForestSign, new Dictionary<ItemId, int> { { ItemId.Wood, 1 }, { ItemId.EmotionShard, 1 } }) },
                { BlueprintId.WoodFence, new BuildRecipe(BlueprintId.WoodFence, new Dictionary<ItemId, int> { { ItemId.Wood, 2 } }) },
                { BlueprintId.RiverLamp, new BuildRecipe(BlueprintId.RiverLamp, new Dictionary<ItemId, int> { { ItemId.Wood, 1 }, { ItemId.StarCore, 1 } }) },
                { BlueprintId.ArcadeBase, new BuildRecipe(BlueprintId.ArcadeBase, new Dictionary<ItemId, int> { { ItemId.Stone, 2 }, { ItemId.StarCore, 1 } }) },
                { BlueprintId.CraftBench, new BuildRecipe(BlueprintId.CraftBench, new Dictionary<ItemId, int> { { ItemId.Wood, 2 }, { ItemId.Stone, 1 } }) },
                { BlueprintId.CustomBuilding, new BuildRecipe(BlueprintId.CustomBuilding, new Dictionary<ItemId, int> { { ItemId.Wood, 2 }, { ItemId.Stone, 1 } }) }
            };
        }

        public OperationResult Place(PlayerState state, BlueprintId blueprintId, int gridX, int gridY)
        {
            OperationResult canPlaceResult = CanPlace(state, blueprintId, gridX, gridY);
            if (!canPlaceResult.Success)
            {
                return canPlaceResult;
            }

            BuildRecipe recipe = buildRecipes[blueprintId];
            OperationResult spendResult = inventoryService.Spend(state, recipe.Cost);
            if (!spendResult.Success)
            {
                return spendResult;
            }

            string buildingId = $"{blueprintId}-{state.PlacedBuildings.Count + 1}";
            state.PlacedBuildings.Add(new PlacedBuilding(buildingId, blueprintId, gridX, gridY, 0));
            state.BuiltCount += 1;
            if (blueprintId == BlueprintId.Bridge)
            {
                blueprintService.GrantBridgeRepairBlueprints(state);
            }

            blueprintService.UnlockCustomBuildingIfReady(state);
            return OperationResult.Ok("建设物已放置。");
        }

        public OperationResult CanPlace(PlayerState state, BlueprintId blueprintId, int gridX, int gridY)
        {
            if (gridX < 0 || gridY < 0)
            {
                return OperationResult.Fail("建设位置不在可放置区域。");
            }

            if (!state.UnlockedBlueprints.Contains(blueprintId))
            {
                return OperationResult.Fail("图纸未解锁。");
            }

            if (!buildRecipes.TryGetValue(blueprintId, out BuildRecipe recipe))
            {
                return OperationResult.Fail("建设配方不存在。");
            }

            foreach (PlacedBuilding placedBuilding in state.PlacedBuildings)
            {
                if (placedBuilding.GridX == gridX && placedBuilding.GridY == gridY)
                {
                    return OperationResult.Fail("该位置已有建设物。");
                }
            }

            if (!inventoryService.CanAfford(state, recipe.Cost))
            {
                return OperationResult.Fail("材料不足。");
            }

            return OperationResult.Ok("可以放置。");
        }
    }
}
