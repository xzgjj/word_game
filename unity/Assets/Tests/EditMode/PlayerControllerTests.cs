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

            Assert.AreEqual(-0.6f, gameObject.transform.position.x, 0.0001f);
            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void MoveProjectsPlayerOutOfRiverGap()
        {
            GameObject gameObject = new GameObject("player");
            PlayerController player = gameObject.AddComponent<PlayerController>();
            player.MoveSpeed = 4f;
            gameObject.transform.position = new Vector3(-0.9f, 0f, 1.3f);

            player.Move(new Vector2(0f, 1f), 0.1f);

            Assert.AreEqual(-1.2f, gameObject.transform.position.x, 0.0001f);
            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void MoveRejectsNorthernRiverBendGap()
        {
            GameObject gameObject = new GameObject("player");
            PlayerController player = gameObject.AddComponent<PlayerController>();
            player.MoveSpeed = 4f;
            gameObject.transform.position = new Vector3(-1.25f, 0f, 1.35f);

            player.Move(new Vector2(1f, 0f), 0.2f);

            Assert.LessOrEqual(gameObject.transform.position.x, -1.2f);
            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void MoveAllowsBridgeCrossingAfterBridgeIsRepaired()
        {
            GameObject gameObject = new GameObject("player");
            PlayerController player = gameObject.AddComponent<PlayerController>();
            player.MoveSpeed = 4f;
            player.SetBridgeRepaired(true);
            gameObject.transform.position = new Vector3(-0.95f, 0f, -3.2f);

            player.Move(new Vector2(1f, 0f), 0.6f);

            Assert.Greater(gameObject.transform.position.x, 1f);
            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void MoveClampsRightIslandTopBoundary()
        {
            GameObject gameObject = new GameObject("player");
            PlayerController player = gameObject.AddComponent<PlayerController>();
            player.MoveSpeed = 4f;
            gameObject.transform.position = new Vector3(6f, 0f, -0.8f);

            player.Move(new Vector2(0f, 1f), 1f);

            Assert.LessOrEqual(gameObject.transform.position.z, -0.7f);
            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void MoveRejectsTopAreaOutsideIsland()
        {
            GameObject gameObject = new GameObject("player");
            PlayerController player = gameObject.AddComponent<PlayerController>();
            player.MoveSpeed = 4f;
            gameObject.transform.position = new Vector3(-2f, 0f, 3.8f);

            player.Move(new Vector2(0f, 1f), 0.5f);

            Assert.AreEqual(3.8f, gameObject.transform.position.z, 0.0001f);
            Object.DestroyImmediate(gameObject);
        }
    }
}
