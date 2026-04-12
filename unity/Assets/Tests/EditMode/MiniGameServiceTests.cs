using NUnit.Framework;
using StarryForest.Core;
using StarryForest.Inventory;
using StarryForest.MiniGame;

namespace StarryForest.Tests.EditMode
{
    public sealed class MiniGameServiceTests
    {
        [Test]
        public void DiscoverArcadeMakesMenuEntryAvailable()
        {
            PlayerState state = NewState();
            MiniGameService miniGame = new MiniGameService(new InventoryService());

            OperationResult result = miniGame.DiscoverArcade(state);

            Assert.IsTrue(result.Success, result.Message);
            Assert.IsTrue(miniGame.IsArcadeMenuAvailable(state));
            Assert.IsTrue(state.UnlockedMiniGames.Contains(GameConstants.FirstMiniGameId));
        }

        [Test]
        public void StartFailsBeforeArcadeIsDiscovered()
        {
            PlayerState state = NewState();
            MiniGameService miniGame = new MiniGameService(new InventoryService());

            OperationResult result = miniGame.Start(state, GameConstants.FirstMiniGameId);

            Assert.IsFalse(result.Success);
            Assert.IsNull(state.ActiveMiniGameId);
        }

        [Test]
        public void StartRequiresOldCartridgeAfterDiscovery()
        {
            PlayerState state = NewState();
            MiniGameService miniGame = new MiniGameService(new InventoryService());
            miniGame.DiscoverArcade(state);

            OperationResult result = miniGame.Start(state, GameConstants.FirstMiniGameId);

            Assert.IsFalse(result.Success);
            Assert.IsNull(state.ActiveMiniGameId);
        }

        [Test]
        public void FinishSuccessfulMiniGameUpdatesStickerWall()
        {
            PlayerState state = NewState();
            InventoryService inventory = new InventoryService();
            MiniGameService miniGame = new MiniGameService(inventory);
            miniGame.DiscoverArcade(state);
            inventory.Add(state, ItemId.OldCartridge, 1);
            miniGame.Start(state, GameConstants.FirstMiniGameId);
            miniGame.CollectSticker(state);
            miniGame.CollectSticker(state);
            miniGame.CollectSticker(state);

            OperationResult result = miniGame.Finish(state, new MiniGameResult(GameConstants.FirstMiniGameId, true, 3));

            Assert.IsTrue(result.Success, result.Message);
            Assert.IsNull(state.ActiveMiniGameId);
            Assert.IsTrue(state.CompletedMiniGames.Contains(GameConstants.FirstMiniGameId));
            Assert.AreEqual(1, state.StickerWallCount);
            Assert.AreEqual(3, inventory.GetCount(state, ItemId.Sticker));
            Assert.IsTrue(state.UnlockedBlueprints.Contains(BlueprintId.StickerWall));
            Assert.IsTrue(state.UnlockedBlueprints.Contains(BlueprintId.ArcadeBase));
        }

        [Test]
        public void CollectStickerTracksExitReadiness()
        {
            PlayerState state = NewState();
            InventoryService inventory = new InventoryService();
            MiniGameService miniGame = new MiniGameService(inventory);
            miniGame.DiscoverArcade(state);
            inventory.Add(state, ItemId.OldCartridge, 1);
            miniGame.Start(state, GameConstants.FirstMiniGameId);

            miniGame.CollectSticker(state);
            miniGame.CollectSticker(state);

            Assert.IsFalse(miniGame.CanExit(state));

            miniGame.CollectSticker(state);

            Assert.IsTrue(miniGame.CanExit(state));
            Assert.AreEqual(GameConstants.MiniGameStickerTarget, state.ActiveMiniGameStickerCount);
        }

        [Test]
        public void FinishRejectsSuccessBeforeStickerTarget()
        {
            PlayerState state = NewState();
            InventoryService inventory = new InventoryService();
            MiniGameService miniGame = new MiniGameService(inventory);
            miniGame.DiscoverArcade(state);
            inventory.Add(state, ItemId.OldCartridge, 1);
            miniGame.Start(state, GameConstants.FirstMiniGameId);
            miniGame.CollectSticker(state);

            OperationResult result = miniGame.Finish(state, new MiniGameResult(GameConstants.FirstMiniGameId, true, 1));

            Assert.IsFalse(result.Success);
            Assert.AreEqual(GameConstants.FirstMiniGameId, state.ActiveMiniGameId);
        }

        [Test]
        public void StartRejectsSecondMiniGameWhileActive()
        {
            PlayerState state = NewState();
            InventoryService inventory = new InventoryService();
            MiniGameService miniGame = new MiniGameService(inventory);
            miniGame.DiscoverArcade(state);
            inventory.Add(state, ItemId.OldCartridge, 1);
            miniGame.Start(state, GameConstants.FirstMiniGameId);

            OperationResult result = miniGame.Start(state, GameConstants.FirstMiniGameId);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(GameConstants.FirstMiniGameId, state.ActiveMiniGameId);
        }

        [Test]
        public void FinishRejectsNegativeStickerCount()
        {
            PlayerState state = NewState();
            InventoryService inventory = new InventoryService();
            MiniGameService miniGame = new MiniGameService(inventory);
            miniGame.DiscoverArcade(state);
            inventory.Add(state, ItemId.OldCartridge, 1);
            miniGame.Start(state, GameConstants.FirstMiniGameId);

            OperationResult result = miniGame.Finish(state, new MiniGameResult(GameConstants.FirstMiniGameId, true, -1));

            Assert.IsFalse(result.Success);
            Assert.AreEqual(GameConstants.FirstMiniGameId, state.ActiveMiniGameId);
            Assert.AreEqual(0, state.StickerWallCount);
        }

        private static PlayerState NewState()
        {
            return new PlayerState(ItemCatalog.InventorySlotCount);
        }
    }
}
