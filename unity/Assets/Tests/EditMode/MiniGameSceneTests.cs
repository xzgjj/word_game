using NUnit.Framework;
using StarryForest.Runtime;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace StarryForest.Tests.EditMode
{
    public sealed class MiniGameSceneTests
    {
        private const string ScenePath = "Assets/Scenes/MiniGame01.unity";

        [Test]
        public void MiniGameSceneContainsStickerGoalAndExit()
        {
            EditorSceneManager.OpenScene(ScenePath);

            Assert.IsNotNull(GameObject.Find("PixelPlayer"));
            Assert.IsNotNull(GameObject.Find("Sticker_1"));
            Assert.IsNotNull(GameObject.Find("Sticker_2"));
            Assert.IsNotNull(GameObject.Find("Sticker_3"));
            Assert.IsNotNull(GameObject.Find("ExitDoor"));
        }

        [Test]
        public void MiniGameSceneContainsRuntimeAndAethelPixelProxy()
        {
            EditorSceneManager.OpenScene(ScenePath);

            Assert.IsNotNull(GameObject.Find("GameStateRunner").GetComponent<GameStateRunner>());
            Assert.IsNotNull(GameObject.Find("PixelPlayer_Hair"));
            Assert.IsNotNull(GameObject.Find("PixelSpiritFish"));
        }
    }
}
