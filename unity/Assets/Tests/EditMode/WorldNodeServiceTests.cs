using NUnit.Framework;
using StarryForest.Core;
using StarryForest.Building;
using StarryForest.Inventory;
using StarryForest.MiniGame;
using StarryForest.Signboard;
using StarryForest.World.Gathering;
using StarryForest.World.Nodes;

namespace StarryForest.Tests.EditMode
{
    public sealed class WorldNodeServiceTests
    {
        [Test]
        public void InteractRoutesGatherNodeToInventory()
        {
            PlayerState state = NewState();
            InventoryService inventory = new InventoryService();
            WorldNodeService worldNodes = NewWorldNodeService(inventory);

            OperationResult result = worldNodes.Interact(state, "river-stone");

            Assert.IsTrue(result.Success, result.Message);
            Assert.AreEqual(1, inventory.GetCount(state, ItemId.Stone));
        }

        [Test]
        public void InteractRoutesFishingNodeToInventory()
        {
            PlayerState state = NewState();
            InventoryService inventory = new InventoryService();
            WorldNodeService worldNodes = NewWorldNodeService(inventory);

            OperationResult result = worldNodes.Interact(state, "river-fish");

            Assert.IsTrue(result.Success, result.Message);
            Assert.AreEqual(1, inventory.GetCount(state, ItemId.Fish));
        }

        [Test]
        public void InteractWithArcadeDiscoversMenuEntry()
        {
            PlayerState state = NewState();
            InventoryService inventory = new InventoryService();
            WorldNodeService worldNodes = NewWorldNodeService(inventory);

            OperationResult result = worldNodes.Interact(state, "clearing-arcade");

            Assert.IsTrue(result.Success, result.Message);
            Assert.IsTrue(state.KnownSystems.Contains(GameConstants.ArcadeSystemId));
            Assert.IsTrue(state.UnlockedMiniGames.Contains(GameConstants.FirstMiniGameId));
        }

        [Test]
        public void InteractWithSignboardDiscoversSystemAndGrantsBlueprints()
        {
            PlayerState state = NewState();
            InventoryService inventory = new InventoryService();
            WorldNodeService worldNodes = NewWorldNodeService(inventory);

            OperationResult result = worldNodes.Interact(state, "home-signboard");

            Assert.IsTrue(result.Success, result.Message);
            Assert.IsTrue(state.KnownSystems.Contains(GameConstants.SignboardSystemId));
            Assert.IsTrue(state.UnlockedBlueprints.Contains(BlueprintId.Bridge));
        }

        [Test]
        public void InteractRejectsUnknownNode()
        {
            PlayerState state = NewState();
            InventoryService inventory = new InventoryService();
            WorldNodeService worldNodes = NewWorldNodeService(inventory);

            OperationResult result = worldNodes.Interact(state, "unknown-node");

            Assert.IsFalse(result.Success);
        }

        [Test]
        public void InteractWithBridgeNodePlacesBridge()
        {
            PlayerState state = NewState();
            InventoryService inventory = new InventoryService();
            BlueprintService blueprints = new BlueprintService();
            WorldNodeService worldNodes = NewWorldNodeService(inventory, blueprints);
            blueprints.GrantInitialBlueprints(state);
            inventory.Add(state, ItemId.Wood, 1);

            OperationResult discovery = worldNodes.Interact(state, "river-bridge");
            OperationResult result = worldNodes.Interact(state, "river-bridge");

            Assert.IsTrue(discovery.Success, discovery.Message);
            Assert.IsTrue(result.Success, result.Message);
            Assert.AreEqual("木桥修好了。现在可以从桥面过河。", result.Message);
            Assert.AreEqual(1, state.BuiltCount);
            Assert.AreEqual(BlueprintId.Bridge, state.PlacedBuildings[0].BlueprintId);
        }

        [Test]
        public void InteractWithBridgeFirstDiscoveryGrantsRepairWoodAndThenBuilds()
        {
            PlayerState state = NewState();
            InventoryService inventory = new InventoryService();
            BlueprintService blueprints = new BlueprintService();
            WorldNodeService worldNodes = NewWorldNodeService(inventory, blueprints);

            OperationResult discovery = worldNodes.Interact(state, "river-bridge");

            Assert.IsTrue(discovery.Success, discovery.Message);
            StringAssert.Contains("备用木板", discovery.Message);
            Assert.IsTrue(state.UnlockedBlueprints.Contains(BlueprintId.Bridge));
            Assert.AreEqual(1, inventory.GetCount(state, ItemId.Wood));

            OperationResult repair = worldNodes.Interact(state, "river-bridge");

            Assert.IsTrue(repair.Success, repair.Message);
            Assert.AreEqual("木桥修好了。现在可以从桥面过河。", repair.Message);
            Assert.AreEqual(1, state.BuiltCount);
            Assert.AreEqual(0, inventory.GetCount(state, ItemId.Wood));
        }

        [Test]
        public void InteractWithBridgeAfterGuideReportsMissingWoodIfMaterialWasUsed()
        {
            PlayerState state = NewState();
            InventoryService inventory = new InventoryService();
            BlueprintService blueprints = new BlueprintService();
            WorldNodeService worldNodes = NewWorldNodeService(inventory, blueprints);

            OperationResult discovery = worldNodes.Interact(state, "river-bridge");
            OperationResult spent = inventory.Spend(state, new System.Collections.Generic.Dictionary<ItemId, int>
            {
                { ItemId.Wood, 1 }
            });
            OperationResult repair = worldNodes.Interact(state, "river-bridge");

            Assert.IsTrue(discovery.Success, discovery.Message);
            Assert.IsTrue(spent.Success, spent.Message);
            Assert.IsFalse(repair.Success);
            Assert.AreEqual("修桥需要木材 1。先领取今日赠礼、出售物品买木材，或清理落枝。", repair.Message);
            Assert.AreEqual(0, state.BuiltCount);
        }

        [Test]
        public void FlowerBedSlotRequiresThreeClearingStepsBeforeBuilding()
        {
            PlayerState state = NewState();
            InventoryService inventory = new InventoryService();
            BlueprintService blueprints = new BlueprintService();
            WorldNodeService worldNodes = NewWorldNodeService(inventory, blueprints);
            blueprints.GrantInitialBlueprints(state);

            OperationResult first = worldNodes.Interact(state, "flower-bed-slot");
            OperationResult second = worldNodes.Interact(state, "flower-bed-slot");
            OperationResult third = worldNodes.Interact(state, "flower-bed-slot");

            Assert.IsTrue(first.Success, first.Message);
            Assert.IsTrue(second.Success, second.Message);
            Assert.IsTrue(third.Success, third.Message);
            Assert.AreEqual(3, WorldNodeService.GetWorldNodeStage(state, "flower-bed-slot"));
            Assert.AreEqual(1, inventory.GetCount(state, ItemId.Wood));
            Assert.AreEqual(1, inventory.GetCount(state, ItemId.Stone));
            Assert.AreEqual(1, inventory.GetCount(state, ItemId.FlowerSeed));
            Assert.AreEqual(0, state.BuiltCount);

            OperationResult buildAttempt = worldNodes.Interact(state, "flower-bed-slot");

            Assert.IsFalse(buildAttempt.Success);
            Assert.AreEqual(0, state.BuiltCount);
            Assert.AreEqual(1, inventory.GetCount(state, ItemId.Stone));
            Assert.AreEqual(1, inventory.GetCount(state, ItemId.FlowerSeed));
        }

        [Test]
        public void ClearedFlowerBedSlotBuildsAfterMaterialsAreAvailable()
        {
            PlayerState state = NewState();
            InventoryService inventory = new InventoryService();
            BlueprintService blueprints = new BlueprintService();
            WorldNodeService worldNodes = NewWorldNodeService(inventory, blueprints);
            blueprints.GrantInitialBlueprints(state);

            worldNodes.Interact(state, "forest-flower-bed-slot");
            worldNodes.Interact(state, "forest-flower-bed-slot");
            worldNodes.Interact(state, "forest-flower-bed-slot");
            inventory.Add(state, ItemId.FlowerSeed, 1);

            OperationResult result = worldNodes.Interact(state, "forest-flower-bed-slot");

            Assert.IsTrue(result.Success, result.Message);
            Assert.AreEqual(1, state.BuiltCount);
            Assert.AreEqual(BlueprintId.FlowerBed, state.PlacedBuildings[0].BlueprintId);
            Assert.AreEqual(2, state.PlacedBuildings[0].GridX);
            Assert.AreEqual(3, state.PlacedBuildings[0].GridY);
        }

        private static WorldNodeService NewWorldNodeService(InventoryService inventory, BlueprintService blueprints = null)
        {
            BlueprintService blueprintService = blueprints ?? new BlueprintService();
            return new WorldNodeService(
                new GatherService(inventory),
                new FishingService(inventory),
                new MiniGameService(inventory),
                inventory,
                new SignboardService(inventory, blueprintService),
                new BuildService(inventory, blueprintService));
        }

        private static PlayerState NewState()
        {
            return new PlayerState(ItemCatalog.InventorySlotCount);
        }
    }
}
