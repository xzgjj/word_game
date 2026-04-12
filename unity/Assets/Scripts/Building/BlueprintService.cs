using StarryForest.Core;

namespace StarryForest.Building
{
    public sealed class BlueprintService
    {
        public void GrantInitialBlueprints(PlayerState state)
        {
            state.UnlockedBlueprints.Add(BlueprintId.Bridge);
            state.UnlockedBlueprints.Add(BlueprintId.FlowerBed);
            state.UnlockedBlueprints.Add(BlueprintId.ForestSign);
        }

        public void UnlockCustomBuildingIfReady(PlayerState state)
        {
            if (state.BuiltCount >= GameConstants.CustomBuildUnlockCount)
            {
                state.CustomBuildUnlocked = true;
                state.UnlockedBlueprints.Add(BlueprintId.CustomBuilding);
            }
        }

        public void GrantBridgeRepairBlueprints(PlayerState state)
        {
            state.UnlockedBlueprints.Add(BlueprintId.WoodFence);
            state.UnlockedBlueprints.Add(BlueprintId.RiverLamp);
        }

        public void GrantMiniGameCompletionBlueprints(PlayerState state)
        {
            state.UnlockedBlueprints.Add(BlueprintId.StickerWall);
            state.UnlockedBlueprints.Add(BlueprintId.ArcadeBase);
        }
    }
}
