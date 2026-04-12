using NUnit.Framework;
using StarryForest.Core;
using StarryForest.Inventory;
using StarryForest.World.Nodes;
using UnityEngine;

namespace StarryForest.Tests.EditMode
{
    public sealed class WorldNodeInteractorTests
    {
        [Test]
        public void InteractorFailsWhenNodeIdIsMissing()
        {
            GameObject gameObject = new GameObject("test-node");
            WorldNodeInteractor interactor = gameObject.AddComponent<WorldNodeInteractor>();

            OperationResult result = interactor.Interact(new GameState());

            Object.DestroyImmediate(gameObject);
            Assert.IsFalse(result.Success);
        }

        [Test]
        public void InteractorRoutesConfiguredNodeToGameState()
        {
            GameObject gameObject = new GameObject("test-node");
            WorldNodeInteractor interactor = gameObject.AddComponent<WorldNodeInteractor>();
            interactor.Configure("river-shell", "河贝", "捡起河贝");
            GameState gameState = new GameState();

            OperationResult result = interactor.Interact(gameState);

            Object.DestroyImmediate(gameObject);
            Assert.IsTrue(result.Success, result.Message);
            Assert.AreEqual(1, gameState.Inventory.GetCount(gameState.Player, ItemId.RiverShell));
        }

        [Test]
        public void InteractorReturnsPromptOnlyInRange()
        {
            GameObject gameObject = new GameObject("test-node");
            WorldNodeInteractor interactor = gameObject.AddComponent<WorldNodeInteractor>();
            interactor.Configure("forest-branch", "森林树枝", "拾取木材");

            string nearPrompt = interactor.GetPrompt(new Vector3(0.5f, 0f, 0f));
            string farPrompt = interactor.GetPrompt(new Vector3(5f, 0f, 0f));

            Object.DestroyImmediate(gameObject);
            Assert.AreEqual("拾取木材", nearPrompt);
            Assert.AreEqual(string.Empty, farPrompt);
        }
    }
}
