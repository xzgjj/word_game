using NUnit.Framework;
using StarryForest.Core;
using StarryForest.Inventory;
using StarryForest.World.Gathering;

namespace StarryForest.Tests.EditMode
{
    public sealed class WorldResourceTests
    {
        [Test]
        public void GatherCollectsForestBranchIntoWood()
        {
            PlayerState state = NewState();
            InventoryService inventory = new InventoryService();
            GatherService gather = new GatherService(inventory);

            OperationResult result = gather.Collect(state, "forest-branch");

            Assert.IsTrue(result.Success, result.Message);
            Assert.AreEqual(1, inventory.GetCount(state, ItemId.Wood));
        }

        [Test]
        public void GatherRejectsUnknownNodeWithoutAddingItems()
        {
            PlayerState state = NewState();
            InventoryService inventory = new InventoryService();
            GatherService gather = new GatherService(inventory);

            OperationResult result = gather.Collect(state, "unknown-node");

            Assert.IsFalse(result.Success);
            Assert.AreEqual(0, inventory.GetCount(state, ItemId.Wood));
            Assert.AreEqual(0, inventory.GetCount(state, ItemId.Stone));
            Assert.AreEqual(0, inventory.GetCount(state, ItemId.FlowerSeed));
            Assert.AreEqual(0, inventory.GetCount(state, ItemId.RiverShell));
        }

        [Test]
        public void FishingCollectsRiverFishIntoFish()
        {
            PlayerState state = NewState();
            InventoryService inventory = new InventoryService();
            FishingService fishing = new FishingService(inventory);

            OperationResult result = fishing.CatchFish(state, "river-fish");

            Assert.IsTrue(result.Success, result.Message);
            Assert.AreEqual(1, inventory.GetCount(state, ItemId.Fish));
        }

        [Test]
        public void FishingRejectsNonFishingNode()
        {
            PlayerState state = NewState();
            InventoryService inventory = new InventoryService();
            FishingService fishing = new FishingService(inventory);

            OperationResult result = fishing.CatchFish(state, "forest-branch");

            Assert.IsFalse(result.Success);
            Assert.AreEqual(0, inventory.GetCount(state, ItemId.Fish));
        }

        private static PlayerState NewState()
        {
            return new PlayerState(ItemCatalog.InventorySlotCount);
        }
    }
}
