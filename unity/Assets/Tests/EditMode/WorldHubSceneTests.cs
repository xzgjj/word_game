using NUnit.Framework;
using StarryForest.Player;
using StarryForest.World.Nodes;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace StarryForest.Tests.EditMode
{
    public sealed class WorldHubSceneTests
    {
        private const string ScenePath = "Assets/Scenes/WorldHub.unity";

        [Test]
        public void WorldHubSceneContainsCoreLandmarks()
        {
            EditorSceneManager.OpenScene(ScenePath);

            Assert.IsNotNull(GameObject.Find("HomeCabin"));
            Assert.IsNotNull(GameObject.Find("Player"));
            Assert.IsNotNull(GameObject.Find("HomeSignboard"));
            Assert.IsNotNull(GameObject.Find("River"));
            Assert.IsNotNull(GameObject.Find("DamagedBridge"));
            Assert.IsNotNull(GameObject.Find("ArcadeMachine"));
            Assert.IsNotNull(GameObject.Find("DistantLightScreen_Unreachable"));
        }

        [Test]
        public void WorldHubSceneConfiguresInteractiveNodes()
        {
            EditorSceneManager.OpenScene(ScenePath);

            AssertNode("HomeSignboard", "home-signboard");
            AssertNode("ForestBranchNode", "forest-branch");
            AssertNode("ForestFlowerSeedNode", "forest-flower-seed");
            AssertNode("RiverStoneNode", "river-stone");
            AssertNode("RiverShellNode", "river-shell");
            AssertNode("RiverFishNode", "river-fish");
            AssertNode("ArcadeMachine", "clearing-arcade");
            Assert.IsNotNull(GameObject.Find("Player").GetComponent<PlayerController>());
        }

        private static void AssertNode(string objectName, string nodeId)
        {
            GameObject gameObject = GameObject.Find(objectName);
            Assert.IsNotNull(gameObject, objectName);

            WorldNodeInteractor interactor = gameObject.GetComponent<WorldNodeInteractor>();
            Assert.IsNotNull(interactor, objectName);
            Assert.AreEqual(nodeId, interactor.NodeId);
        }
    }
}
