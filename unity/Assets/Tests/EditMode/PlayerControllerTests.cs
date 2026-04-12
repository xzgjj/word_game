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

            player.Move(new Vector2(1f, 0f), 0.5f);

            Assert.AreEqual(1f, gameObject.transform.position.x, 0.0001f);
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

            player.Move(new Vector2(2f, 0f), 1f);

            Assert.AreEqual(1f, gameObject.transform.position.x, 0.0001f);
            Object.DestroyImmediate(gameObject);
        }
    }
}
