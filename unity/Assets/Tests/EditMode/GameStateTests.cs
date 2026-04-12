using NUnit.Framework;
using StarryForest.Core;
using StarryForest.Inventory;
using StarryForest.MiniGame;

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
            OperationResult finishResult = gameState.FinishMiniGame(new MiniGameResult(GameConstants.FirstMiniGameId, true, 3));

            Assert.IsTrue(startResult.Success, startResult.Message);
            Assert.IsTrue(finishResult.Success, finishResult.Message);
            Assert.AreEqual(1, gameState.Player.StickerWallCount);
            Assert.AreEqual(3, gameState.Inventory.GetCount(gameState.Player, ItemId.Sticker));
        }
    }
}
