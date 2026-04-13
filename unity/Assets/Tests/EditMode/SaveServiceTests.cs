using System.IO;
using NUnit.Framework;
using StarryForest.Core;
using StarryForest.Inventory;
using StarryForest.Save;

namespace StarryForest.Tests.EditMode
{
    public sealed class SaveServiceTests
    {
        [Test]
        public void SaveAndLoadRoundTripsCorePlayerState()
        {
            string filePath = Path.Combine(Path.GetTempPath(), "starry-forest-save-test.json");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            PlayerState state = new PlayerState(ItemCatalog.InventorySlotCount)
            {
                PositionId = "home-yard",
                Emotion = EmotionMode.Calm,
                TimeOfDay = TimeOfDay.Night,
                BuiltCount = 3,
                CustomBuildUnlocked = true,
                StickerWallCount = 1,
                SignboardDailyRewardClaimed = true,
                ActiveMiniGameId = GameConstants.FirstMiniGameId,
                ActiveMiniGameStickerCount = 2
            };
            state.Items[ItemId.Wood] = 5;
            state.Items[ItemId.Sticker] = 3;
            state.UnlockedBlueprints.Add(BlueprintId.CustomBuilding);
            state.WorldNodeStages["flower-bed-slot"] = 3;
            state.PlacedBuildings.Add(new PlacedBuilding("CustomBuilding-1", BlueprintId.CustomBuilding, 2, 4, 90)
            {
                CustomData = new CustomBuildingData(CustomBuildingShape.TinyCabin, CustomBuildingTheme.Star, CustomBuildingSize.TwoByTwo)
            });
            state.KnownSystems.Add(GameConstants.ArcadeSystemId);
            state.UnlockedMiniGames.Add(GameConstants.FirstMiniGameId);
            state.CompletedMiniGames.Add(GameConstants.FirstMiniGameId);

            SaveService saveService = new SaveService();
            OperationResult saveResult = saveService.Save(state, filePath);
            SaveLoadResult loadResult = saveService.Load(filePath);

            Assert.IsTrue(saveResult.Success, saveResult.Message);
            Assert.IsTrue(loadResult.Success, loadResult.Message);
            Assert.AreEqual(ItemCatalog.InventorySlotCount, loadResult.State.InventorySlotCount);
            Assert.AreEqual(EmotionMode.Calm, loadResult.State.Emotion);
            Assert.AreEqual(TimeOfDay.Night, loadResult.State.TimeOfDay);
            Assert.AreEqual(2, loadResult.State.ActiveMiniGameStickerCount);
            Assert.AreEqual(5, loadResult.State.Items[ItemId.Wood]);
            Assert.AreEqual(3, loadResult.State.Items[ItemId.Sticker]);
            Assert.IsTrue(loadResult.State.UnlockedBlueprints.Contains(BlueprintId.CustomBuilding));
            Assert.AreEqual(3, loadResult.State.WorldNodeStages["flower-bed-slot"]);
            Assert.AreEqual(1, loadResult.State.PlacedBuildings.Count);
            Assert.AreEqual(CustomBuildingTheme.Star, loadResult.State.PlacedBuildings[0].CustomData.Theme);
            Assert.IsTrue(loadResult.State.KnownSystems.Contains(GameConstants.ArcadeSystemId));
            Assert.IsTrue(loadResult.State.CompletedMiniGames.Contains(GameConstants.FirstMiniGameId));
            Assert.IsTrue(loadResult.State.SignboardDailyRewardClaimed);

            File.Delete(filePath);
        }

        [Test]
        public void LoadMissingSaveFails()
        {
            SaveService saveService = new SaveService();

            SaveLoadResult result = saveService.Load(Path.Combine(Path.GetTempPath(), "missing-starry-forest-save.json"));

            Assert.IsFalse(result.Success);
            Assert.IsNull(result.State);
        }
    }
}
