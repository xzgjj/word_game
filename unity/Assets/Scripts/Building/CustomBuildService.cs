using StarryForest.Core;
using StarryForest.Inventory;

namespace StarryForest.Building
{
    public sealed class CustomBuildService
    {
        private readonly InventoryService inventoryService;

        public CustomBuildService(InventoryService inventoryService)
        {
            this.inventoryService = inventoryService;
        }

        public OperationResult Create(PlayerState state, CustomBuildingData customData, int gridX, int gridY)
        {
            if (!state.CustomBuildUnlocked)
            {
                return OperationResult.Fail("自建建筑物尚未开启。");
            }

            if (customData == null)
            {
                return OperationResult.Fail("自建建筑物配置不能为空。");
            }

            if (gridX < 0 || gridY < 0)
            {
                return OperationResult.Fail("建设位置不在可放置区域。");
            }

            foreach (PlacedBuilding existingBuilding in state.PlacedBuildings)
            {
                if (existingBuilding.GridX == gridX && existingBuilding.GridY == gridY)
                {
                    return OperationResult.Fail("该位置已有建设物。");
                }
            }

            ItemId materialItemId = GetThemeMaterial(customData.Theme);
            int materialCost = customData.Size == CustomBuildingSize.TwoByTwo ? 2 : 1;
            OperationResult spendResult = inventoryService.Spend(
                state,
                new System.Collections.Generic.Dictionary<ItemId, int> { { materialItemId, materialCost } });
            if (!spendResult.Success)
            {
                return spendResult;
            }

            string buildingId = $"CustomBuilding-{state.PlacedBuildings.Count + 1}";
            PlacedBuilding placedBuilding = new PlacedBuilding(buildingId, BlueprintId.CustomBuilding, gridX, gridY, 0)
            {
                CustomData = customData
            };
            state.PlacedBuildings.Add(placedBuilding);
            state.BuiltCount += 1;
            return OperationResult.Ok("自建建筑物已放置。");
        }

        private static ItemId GetThemeMaterial(CustomBuildingTheme theme)
        {
            return theme switch
            {
                CustomBuildingTheme.Stone => ItemId.Stone,
                CustomBuildingTheme.Flower => ItemId.FlowerSeed,
                CustomBuildingTheme.RiverShell => ItemId.RiverShell,
                CustomBuildingTheme.Star => ItemId.StarCore,
                _ => ItemId.Wood
            };
        }
    }
}
