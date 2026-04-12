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

            OperationResult result = worldNodes.Interact(state, "river-bridge");

            Assert.IsTrue(result.Success, result.Message);
            Assert.AreEqual(1, state.BuiltCount);
            Assert.AreEqual(BlueprintId.Bridge, state.PlacedBuildings[0].BlueprintId);
        }

        private static WorldNodeService NewWorldNodeService(InventoryService inventory, BlueprintService blueprints = null)
        {
            BlueprintService blueprintService = blueprints ?? new BlueprintService();
            return new WorldNodeService(
                new GatherService(inventory),
                new FishingService(inventory),
                new MiniGameService(inventory),
                new SignboardService(inventory, blueprintService),
                new BuildService(inventory, blueprintService));
        }

        private static PlayerState NewState()
        {
            return new PlayerState(ItemCatalog.InventorySlotCount);
        }
    }
}
