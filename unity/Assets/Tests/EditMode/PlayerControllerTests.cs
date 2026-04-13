using NUnit.Framework;
using StarryForest.Player;
using UnityEngine;

namespace StarryForest.Tests.EditMode
{
    public sealed class PlayerControllerTests
    {
        [Test]
        public void MoveUpdatesTransformOnGroundPlane()
        {
            GameObject gameObject = new GameObject("player");
            PlayerController player = gameObject.AddComponent<PlayerController>();
            player.MoveSpeed = 2f;
            gameObject.transform.position = new Vector3(-5f, 0f, 0f);

            player.Move(new Vector2(1f, 0f), 0.5f);

            Assert.AreEqual(-4f, gameObject.transform.position.x, 0.0001f);
            Assert.AreEqual(0f, gameObject.transform.position.y, 0.0001f);
            Assert.AreEqual(0f, gameObject.transform.position.z, 0.0001f);
            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void MoveClampsDiagonalInput()
        {
            GameObject gameObject = new GameObject("player");
            PlayerController player = gameObject.AddComponent<PlayerController>();
            player.MoveSpeed = 1f;
            gameObject.transform.position = new Vector3(-5f, 0f, 0f);

            player.Move(new Vector2(2f, 0f), 1f);

            Assert.AreEqual(-4f, gameObject.transform.position.x, 0.0001f);
            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void MoveBlocksRiverBandUntilBridgeIsRepaired()
        {
            GameObject gameObject = new GameObject("player");
            PlayerController player = gameObject.AddComponent<PlayerController>();
            player.MoveSpeed = 4f;
            gameObject.transform.position = new Vector3(-0.6f, 0f, 0f);

            player.Move(new Vector2(1f, 0f), 1f);

            Assert.AreEqual(-0.45f, gameObject.transform.position.x, 0.0001f);
            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void MoveAllowsBridgeCrossingAfterBridgeIsRepaired()
        {
            GameObject gameObject = new GameObject("player");
            PlayerController player = gameObject.AddComponent<PlayerController>();
            player.MoveSpeed = 4f;
            player.SetBridgeRepaired(true);
            gameObject.transform.position = new Vector3(-0.6f, 0f, -3.2f);

            player.Move(new Vector2(1f, 0f), 0.4f);

            Assert.Greater(gameObject.transform.position.x, -0.45f);
            Object.DestroyImmediate(gameObject);
        }
    }
}
