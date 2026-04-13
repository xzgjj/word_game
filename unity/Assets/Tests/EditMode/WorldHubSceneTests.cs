using NUnit.Framework;
using StarryForest.Player;
using StarryForest.Runtime;
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
            AssertNode("DamagedBridge", "river-bridge");
            AssertNode("FlowerBedSlot", "flower-bed-slot");
            AssertNode("ForestFlowerBedSlot", "forest-flower-bed-slot");
            AssertNode("ForestSignSlot", "forest-sign-slot");
            AssertNode("RiverLampSlot", "river-lamp-slot");
            AssertNode("ArcadeMachine", "clearing-arcade");
            AssertNode("ArcadeBaseSlot", "arcade-base-slot");
            Assert.IsNotNull(GameObject.Find("Player").GetComponent<PlayerController>());
            WorldNodeInteractor bridge = GameObject.Find("DamagedBridge").GetComponent<WorldNodeInteractor>();
            Assert.GreaterOrEqual(bridge.InteractionRadius, 2.4f);
        }

        [Test]
        public void WorldHubSceneContainsVisualPlayableRuntimeViews()
        {
            EditorSceneManager.OpenScene(ScenePath);

            Assert.IsNotNull(GameObject.Find("GameStateRunner").GetComponent<GameStateRunner>());
            Assert.IsNotNull(GameObject.Find("GameStateRunner").GetComponent<HudView>());
            Assert.IsNotNull(GameObject.Find("DamagedBridge").GetComponent<BuildPlacementView>());
            Assert.IsNotNull(GameObject.Find("FlowerBedSlot").GetComponent<BuildPlacementView>());
            Assert.IsNotNull(GameObject.Find("FlowerBedSlot").GetComponent<ClearingPlotView>());
            Assert.IsNotNull(GameObject.Find("ForestFlowerBedSlot").GetComponent<ClearingPlotView>());
            Assert.IsNotNull(GameObject.Find("StickerWall").GetComponent<StickerWallView>());
            Assert.IsNotNull(GameObject.Find("ArcadeMachine").GetComponent<ArcadeMachineView>());
            Assert.IsNotNull(GameObject.Find("PlayerBody_AethelProxy"));
            Assert.IsNotNull(GameObject.Find("SpiritFish_Quiet"));
            Assert.IsNotNull(GameObject.Find("IslandHomeLobe"));
            Assert.IsNotNull(GameObject.Find("NaturalBoundary_EastFence"));
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
