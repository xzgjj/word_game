using NUnit.Framework;
using StarryForest.Core;
using StarryForest.Inventory;
using StarryForest.MiniGame;
using StarryForest.Runtime;

namespace StarryForest.Tests.EditMode
{
    public sealed class GameStateTests
    {
        [Test]
        public void GameStateRoutesWorldNodeInteraction()
        {
            GameState gameState = new GameState();

            OperationResult result = gameState.InteractWithWorldNode("forest-branch");

            Assert.IsTrue(result.Success, result.Message);
            Assert.AreEqual(1, gameState.Inventory.GetCount(gameState.Player, ItemId.Wood));
        }

        [Test]
        public void GameStateStartsAndFinishesMiniGameThroughMenuState()
        {
            GameState gameState = new GameState();
            gameState.InteractWithWorldNode("clearing-arcade");
            gameState.Inventory.Add(gameState.Player, ItemId.OldCartridge, 1);

            OperationResult startResult = gameState.StartMiniGame(GameConstants.FirstMiniGameId);
            gameState.MiniGames.CollectSticker(gameState.Player);
            gameState.MiniGames.CollectSticker(gameState.Player);
            gameState.MiniGames.CollectSticker(gameState.Player);
            OperationResult finishResult = gameState.FinishMiniGame(new MiniGameResult(GameConstants.FirstMiniGameId, true, 3));

            Assert.IsTrue(startResult.Success, startResult.Message);
            Assert.IsTrue(finishResult.Success, finishResult.Message);
            Assert.AreEqual(1, gameState.Player.StickerWallCount);
            Assert.AreEqual(3, gameState.Inventory.GetCount(gameState.Player, ItemId.Sticker));
        }

        [Test]
        public void ItemGuideMapsWorldNodesByItemType()
        {
            Assert.IsTrue(GameStateRunner.TryGetGuideItemForNode("forest-branch", out ItemId wood));
            Assert.IsTrue(GameStateRunner.TryGetGuideItemForNode("home-fallen-branch", out ItemId homeWood));
            Assert.IsTrue(GameStateRunner.TryGetGuideItemForNode("river-shell", out ItemId shell));
            Assert.IsFalse(GameStateRunner.TryGetGuideItemForNode("home-signboard", out _));
            Assert.AreEqual(ItemId.Wood, wood);
            Assert.AreEqual(ItemId.Wood, homeWood);
            Assert.AreEqual(ItemId.RiverShell, shell);
        }

        [Test]
        public void ResourceRespawnPolicyIsFixedByItemType()
        {
            Assert.IsTrue(GameStateRunner.TryGetResourceRespawnSeconds("forest-branch", out float branchSeconds));
            Assert.IsTrue(GameStateRunner.TryGetResourceRespawnSeconds("home-fallen-branch", out float homeBranchSeconds));
            Assert.IsTrue(GameStateRunner.TryGetResourceRespawnSeconds("river-fish", out float fishSeconds));
            Assert.IsTrue(GameStateRunner.TryGetResourceRespawnSeconds("river-stone", out float stoneSeconds));
            Assert.IsFalse(GameStateRunner.TryGetResourceRespawnSeconds("home-signboard", out _));
            Assert.AreEqual(branchSeconds, homeBranchSeconds);
            Assert.AreEqual(45f, branchSeconds);
            Assert.AreEqual(30f, fishSeconds);
            Assert.AreEqual(90f, stoneSeconds);
        }
    }
}
