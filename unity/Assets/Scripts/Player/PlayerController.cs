using UnityEngine;

namespace StarryForest.Player
{
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private bool bridgeRepaired;

        public float MoveSpeed
        {
            get => moveSpeed;
            set => moveSpeed = Mathf.Max(0f, value);
        }

        public void Move(Vector2 input, float deltaTime)
        {
            Vector2 clampedInput = Vector2.ClampMagnitude(input, 1f);
            Vector3 movement = new Vector3(clampedInput.x, 0f, clampedInput.y) * moveSpeed * Mathf.Max(0f, deltaTime);
            transform.position = ClampToPlayableIsland(transform.position, transform.position + movement, bridgeRepaired);
        }

        public void SetBridgeRepaired(bool repaired)
        {
            bridgeRepaired = repaired;
        }

        private static Vector3 ClampToPlayableIsland(Vector3 currentPosition, Vector3 proposedPosition, bool allowBridgeCrossing)
        {
            Vector3 clamped = proposedPosition;
            clamped.x = Mathf.Clamp(clamped.x, -8.1f, 7.45f);
            clamped.z = Mathf.Clamp(clamped.z, -5.0f, 4.65f);

            if (clamped.x < -6.7f && clamped.z > 3.45f)
            {
                clamped.z = 3.45f;
            }

            if (clamped.x > 6.35f && clamped.z > 2.95f)
            {
                clamped.z = 2.95f;
            }

            if (clamped.x > 6.9f && clamped.z < -3.65f)
            {
                clamped.z = -3.65f;
            }

            bool currentOnLeftBank = currentPosition.x < -0.45f;
            bool proposedOnRightBank = clamped.x > 2.0f;
            bool currentOnRightBank = currentPosition.x > 2.0f;
            bool proposedOnLeftBank = clamped.x < -0.45f;
            bool crossingRiver = (currentOnLeftBank && proposedOnRightBank) || (currentOnRightBank && proposedOnLeftBank);
            bool proposedInsideRiver = clamped.x > -0.45f && clamped.x < 2.0f;
            bool onBridgeCrossing = allowBridgeCrossing && clamped.z > -3.85f && clamped.z < -2.55f;
            if ((crossingRiver || proposedInsideRiver) && !onBridgeCrossing)
            {
                clamped.x = currentPosition.x <= 0.78f ? -0.45f : 2.0f;
            }

            return clamped;
        }
    }
}
