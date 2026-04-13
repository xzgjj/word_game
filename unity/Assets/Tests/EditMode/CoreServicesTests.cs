using System.Collections.Generic;
using NUnit.Framework;
using StarryForest.Building;
using StarryForest.Core;
using StarryForest.Inventory;
using StarryForest.Save;
using StarryForest.Signboard;

namespace StarryForest.Tests.EditMode
{
    public sealed class CoreServicesTests
    {
        [Test]
        public void InventorySlotCountUsesItemTypesPlusThree()
        {
            Assert.AreEqual(14, ItemCatalog.InventorySlotCount);
        }

        [Test]
        public void InventoryAddClampsAtMaxStack()
        {
            PlayerState state = NewState();
            InventoryService inventory = new InventoryService();

            inventory.Add(state, ItemId.Wood, GameConstants.MaxItemStack + 10);

            Assert.AreEqual(GameConstants.MaxItemStack, inventory.GetCount(state, ItemId.Wood));
        }

        [Test]
        public void SignboardExchangeSpendsBaseMaterialsAndAddsConvertedItem()
        {
            PlayerState state = NewState();
            InventoryService inventory = new InventoryService();
            SignboardService signboard = new SignboardService(inventory);
            inventory.Add(state, ItemId.Wood, 1);
            inventory.Add(state, ItemId.FlowerSeed, 1);

            OperationResult result = signboard.Exchange(state, "exchange-emotion-shard");

            Assert.IsTrue(result.Success, result.Message);
            Assert.AreEqual(0, inventory.GetCount(state, ItemId.Wood));
            Assert.AreEqual(0, inventory.GetCount(state, ItemId.FlowerSeed));
            Assert.AreEqual(1, inventory.GetCount(state, ItemId.EmotionShard));
        }

        [Test]
        public void SignboardDailyRewardAddsStarterMaterialOnce()
        {
            PlayerState state = NewState();
            InventoryService inventory = new InventoryService();
            SignboardService signboard = new SignboardService(inventory);

            OperationResult first = signboard.Exchange(state, "daily-wood-gift");
            OperationResult second = signboard.Exchange(state, "daily-wood-gift");

            Assert.IsTrue(first.Success, first.Message);
            Assert.IsFalse(second.Success);
            Assert.AreEqual(1, inventory.GetCount(state, ItemId.Wood));
            Assert.IsTrue(state.SignboardDailyRewardClaimed);
        }

        [Test]
        public void SignboardOpenDiscoversSystemAndGrantsInitialBlueprints()
        {
            PlayerState state = NewState();
            SignboardService signboard = new SignboardService(new InventoryService(), new BlueprintService());

            OperationResult result = signboard.Open(state);

            Assert.IsTrue(result.Success, result.Message);
            Assert.IsTrue(state.KnownSystems.Contains(GameConstants.SignboardSystemId));
            Assert.IsTrue(state.UnlockedBlueprints.Contains(BlueprintId.Bridge));
            Assert.IsTrue(state.UnlockedBlueprints.Contains(BlueprintId.FlowerBed));
            Assert.IsTrue(state.UnlockedBlueprints.Contains(BlueprintId.ForestSign));
        }

        [Test]
        public void SignboardCanExchangeReflectsMaterialAvailability()
        {
            PlayerState state = NewState();
            InventoryService inventory = new InventoryService();
            SignboardService signboard = new SignboardService(inventory);

            Assert.IsFalse(signboard.CanExchange(state, "exchange-emotion-shard"));

            inventory.Add(state, ItemId.Wood, 1);
            inventory.Add(state, ItemId.FlowerSeed, 1);

            Assert.IsTrue(signboard.CanExchange(state, "exchange-emotion-shard"));
            Assert.IsTrue(signboard.CanExchange(state, "daily-wood-gift"));
            signboard.Exchange(state, "daily-wood-gift");
            Assert.IsFalse(signboard.CanExchange(state, "daily-wood-gift"));
        }

        [Test]
        public void SignboardMenuSnapshotReportsTownRecordAndRecipeAvailability()
        {
            PlayerState state = NewState();
            state.BuiltCount = 3;
            state.CustomBuildUnlocked = true;
            state.UnlockedBlueprints.Add(BlueprintId.CustomBuilding);
            InventoryService inventory = new InventoryService();
            inventory.Add(state, ItemId.Wood, 1);
            inventory.Add(state, ItemId.FlowerSeed, 1);
            SignboardService signboard = new SignboardService(inventory);

            SignboardMenuSnapshot snapshot = signboard.GetMenuSnapshot(state);

            Assert.AreEqual(3, snapshot.BuiltCount);
            Assert.IsTrue(snapshot.CustomBuildUnlocked);
            CollectionAssert.Contains(snapshot.UnlockedBlueprints, BlueprintId.CustomBuilding);
            Assert.IsTrue(snapshot.ExchangeRecipes.Count > 0);
            Assert.IsTrue(snapshot.ExchangeRecipes[0].CanExchange);
            Assert.IsTrue(snapshot.BuyOffers.Count > 0);
            Assert.IsTrue(snapshot.SellOffers.Count > 0);
        }

        [Test]
        public void SignboardSellCollectiblesForCoinsAndBuyAxe()
        {
            PlayerState state = NewState();
            InventoryService inventory = new InventoryService();
            SignboardService signboard = new SignboardService(inventory);
            inventory.Add(state, ItemId.Fish, 2);

            OperationResult sell = signboard.Sell(state, "sell-fish");
            OperationResult buyWithoutEnoughCoins = signboard.Buy(state, "buy-axe");

            Assert.IsTrue(sell.Success, sell.Message);
            Assert.AreEqual(1, inventory.GetCount(state, ItemId.Fish));
            Assert.AreEqual(3, inventory.GetCount(state, ItemId.StarCoin));
            Assert.IsFalse(buyWithoutEnoughCoins.Success);
            inventory.Add(state, ItemId.StarCoin, 2);
            OperationResult buy = signboard.Buy(state, "buy-axe");

            Assert.IsTrue(buy.Success, buy.Message);
            Assert.AreEqual(1, inventory.GetCount(state, ItemId.Axe));
            Assert.AreEqual(0, inventory.GetCount(state, ItemId.StarCoin));
        }

        [Test]
        public void ArchiveServiceKeepsOnlyTwentyRecords()
        {
            PlayerState state = NewState();
            ArchiveService archiveService = new ArchiveService();

            for (int index = 0; index < 25; index++)
            {
                archiveService.CreateArchive(state, "auto", $"自动档案 {index}");
            }

            Assert.AreEqual(20, state.ArchiveRecords.Count);
            Assert.AreEqual("自动档案 5", state.ArchiveRecords[0].Label);
        }

        [Test]
        public void PlacingThreeBuildingsUnlocksCustomBuilding()
        {
            PlayerState state = NewState();
            InventoryService inventory = new InventoryService();
            BlueprintService blueprints = new BlueprintService();
            BuildService build = new BuildService(inventory, blueprints);
            blueprints.GrantInitialBlueprints(state);
            inventory.Add(state, ItemId.Wood, 2);
            inventory.Add(state, ItemId.FlowerSeed, 2);
            inventory.Add(state, ItemId.Stone, 1);
            inventory.Add(state, ItemId.EmotionShard, 1);

            List<OperationResult> results = new List<OperationResult>
            {
                build.Place(state, BlueprintId.Bridge, 1, 0),
                build.Place(state, BlueprintId.FlowerBed, 0, 1),
                build.Place(state, BlueprintId.ForestSign, 2, 0)
            };

            Assert.IsTrue(results.TrueForAll(result => result.Success));
            Assert.AreEqual(3, state.BuiltCount);
            Assert.IsTrue(state.CustomBuildUnlocked);
            Assert.IsTrue(state.UnlockedBlueprints.Contains(BlueprintId.CustomBuilding));
        }

        [Test]
        public void PlacingBridgeGrantsBridgeRepairBlueprints()
        {
            PlayerState state = NewState();
            InventoryService inventory = new InventoryService();
            BlueprintService blueprints = new BlueprintService();
            BuildService build = new BuildService(inventory, blueprints);
            blueprints.GrantInitialBlueprints(state);
            inventory.Add(state, ItemId.Wood, 1);

            OperationResult result = build.Place(state, BlueprintId.Bridge, 1, 0);

            Assert.IsTrue(result.Success, result.Message);
            Assert.IsTrue(state.UnlockedBlueprints.Contains(BlueprintId.WoodFence));
            Assert.IsTrue(state.UnlockedBlueprints.Contains(BlueprintId.RiverLamp));
        }

        [Test]
        public void BuildServiceRejectsOccupiedGridWithoutSpendingMaterials()
        {
            PlayerState state = NewState();
            InventoryService inventory = new InventoryService();
            BlueprintService blueprints = new BlueprintService();
            BuildService build = new BuildService(inventory, blueprints);
            blueprints.GrantInitialBlueprints(state);
            inventory.Add(state, ItemId.Wood, 2);

            OperationResult first = build.Place(state, BlueprintId.Bridge, 1, 0);
            OperationResult second = build.Place(state, BlueprintId.ForestSign, 1, 0);

            Assert.IsTrue(first.Success, first.Message);
            Assert.IsFalse(second.Success);
            Assert.AreEqual(1, state.BuiltCount);
            Assert.AreEqual(1, inventory.GetCount(state, ItemId.Wood));
        }

        [Test]
        public void BuildServiceRejectsNegativeGrid()
        {
            PlayerState state = NewState();
            InventoryService inventory = new InventoryService();
            BlueprintService blueprints = new BlueprintService();
            BuildService build = new BuildService(inventory, blueprints);
            blueprints.GrantInitialBlueprints(state);
            inventory.Add(state, ItemId.Wood, 1);

            OperationResult result = build.Place(state, BlueprintId.Bridge, -1, 0);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(0, state.BuiltCount);
            Assert.AreEqual(1, inventory.GetCount(state, ItemId.Wood));
        }

        [Test]
        public void CustomBuildConsumesThemeMaterialAndStoresCustomData()
        {
            PlayerState state = NewState();
            state.CustomBuildUnlocked = true;
            InventoryService inventory = new InventoryService();
            CustomBuildService customBuild = new CustomBuildService(inventory);
            inventory.Add(state, ItemId.StarCore, 2);
            CustomBuildingData customData = new CustomBuildingData(
                CustomBuildingShape.LampFrame,
                CustomBuildingTheme.Star,
                CustomBuildingSize.TwoByTwo);

            OperationResult result = customBuild.Create(state, customData, 3, 3);

            Assert.IsTrue(result.Success, result.Message);
            Assert.AreEqual(0, inventory.GetCount(state, ItemId.StarCore));
            Assert.AreEqual(1, state.BuiltCount);
            Assert.AreEqual(CustomBuildingTheme.Star, state.PlacedBuildings[0].CustomData.Theme);
        }

        [Test]
        public void CustomBuildRejectsOccupiedGridWithoutSpendingMaterials()
        {
            PlayerState state = NewState();
            state.CustomBuildUnlocked = true;
            state.PlacedBuildings.Add(new PlacedBuilding("Bridge-1", BlueprintId.Bridge, 1, 1, 0));
            InventoryService inventory = new InventoryService();
            CustomBuildService customBuild = new CustomBuildService(inventory);
            inventory.Add(state, ItemId.Wood, 1);
            CustomBuildingData customData = new CustomBuildingData(
                CustomBuildingShape.TinyCabin,
                CustomBuildingTheme.Wood,
                CustomBuildingSize.OneByOne);

            OperationResult result = customBuild.Create(state, customData, 1, 1);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(1, inventory.GetCount(state, ItemId.Wood));
        }

        private static PlayerState NewState()
        {
            return new PlayerState(ItemCatalog.InventorySlotCount);
        }
    }
}
