using System.Collections;
using NUnit.Framework;
using StarryForest.Player;
using StarryForest.Runtime;
using StarryForest.World.Nodes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace StarryForest.Tests.PlayMode
{
    public sealed class SceneSmokeTests
    {
        [UnityTest]
        public IEnumerator WorldHubLoadsWithPlayerAndCoreInteractions()
        {
            yield return LoadScene("Assets/Scenes/WorldHub.unity");

            GameObject player = GameObject.Find("Player");
            Assert.IsNotNull(player);
            Assert.IsNotNull(player.GetComponent<PlayerController>());

            AssertNode("HomeSignboard", "home-signboard");
            AssertNode("ArcadeMachine", "clearing-arcade");
            Assert.IsNotNull(GameObject.Find("GameStateRunner")?.GetComponent<GameStateRunner>());
            Assert.IsNotNull(GameObject.Find("StickerWall")?.GetComponent<StickerWallView>());
            Assert.IsNotNull(GameObject.Find("DistantLightScreen_Unreachable"));
        }

        [UnityTest]
        public IEnumerator MiniGameSceneLoadsWithStickerGoalAndExit()
        {
            yield return LoadScene("Assets/Scenes/MiniGame01.unity");

            Assert.IsNotNull(GameObject.Find("PixelPlayer"));
            Assert.IsNotNull(GameObject.Find("Sticker_1"));
            Assert.IsNotNull(GameObject.Find("Sticker_2"));
            Assert.IsNotNull(GameObject.Find("Sticker_3"));
            Assert.IsNotNull(GameObject.Find("ExitDoor"));
        }

        private static IEnumerator LoadScene(string scenePath)
        {
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            yield return null;

            Scene scene = SceneManager.GetActiveScene();
            Assert.AreEqual(sceneName, scene.name);
            Assert.IsTrue(scene.isLoaded, scenePath);
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
