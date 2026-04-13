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
            Vector3 candidate = proposedPosition;
            if (!CrossesBlockedRiver(currentPosition, candidate, allowBridgeCrossing) && IsWalkable(candidate, allowBridgeCrossing))
            {
                return candidate;
            }

            Vector3 horizontalOnly = new Vector3(candidate.x, currentPosition.y, currentPosition.z);
            if (!CrossesBlockedRiver(currentPosition, horizontalOnly, allowBridgeCrossing) && IsWalkable(horizontalOnly, allowBridgeCrossing))
            {
                return horizontalOnly;
            }

            Vector3 verticalOnly = new Vector3(currentPosition.x, currentPosition.y, candidate.z);
            if (!CrossesBlockedRiver(currentPosition, verticalOnly, allowBridgeCrossing) && IsWalkable(verticalOnly, allowBridgeCrossing))
            {
                return verticalOnly;
            }

            return ProjectOutOfRiver(currentPosition, allowBridgeCrossing);
        }

        private static bool IsWalkable(Vector3 position, bool allowBridgeCrossing)
        {
            if (IsOnBridge(position, allowBridgeCrossing))
            {
                return true;
            }

            if (IsInsideRiver(position))
            {
                return false;
            }

            return IsInsideRect(position, -7.95f, 5.25f, -4.95f, 3.85f)
                || IsInsideRect(position, -8.4f, -4.1f, -4.75f, -0.15f)
                || IsInsideRect(position, -8.3f, -3.0f, 0.12f, 4.28f)
                || IsInsideRect(position, 3.45f, 7.95f, -4.1f, -0.7f);
        }

        private static bool IsInsideRiver(Vector3 position)
        {
            return position.x > -0.58f && position.x < 2.12f && position.z > -5.05f && position.z < 4.25f;
        }

        private static bool CrossesBlockedRiver(Vector3 from, Vector3 to, bool allowBridgeCrossing)
        {
            bool crossesRiverBand = (from.x <= -0.58f && to.x >= 2.12f)
                || (from.x >= 2.12f && to.x <= -0.58f)
                || IsInsideRiver(to);

            if (!crossesRiverBand)
            {
                return false;
            }

            return !IsOnBridge(from, allowBridgeCrossing) || !IsOnBridge(to, allowBridgeCrossing);
        }

        private static bool IsOnBridge(Vector3 position, bool allowBridgeCrossing)
        {
            return allowBridgeCrossing
                && position.x >= -0.75f
                && position.x <= 2.35f
                && position.z >= -4.1f
                && position.z <= -2.25f;
        }

        private static bool IsInsideRect(Vector3 position, float minX, float maxX, float minZ, float maxZ)
        {
            return position.x >= minX && position.x <= maxX && position.z >= minZ && position.z <= maxZ;
        }

        private static Vector3 ProjectOutOfRiver(Vector3 position, bool allowBridgeCrossing)
        {
            if (!IsInsideRiver(position) || IsOnBridge(position, allowBridgeCrossing))
            {
                return position;
            }

            Vector3 projected = position;
            float distanceToLeftBank = Mathf.Abs(position.x - -0.58f);
            float distanceToRightBank = Mathf.Abs(position.x - 2.12f);
            projected.x = distanceToLeftBank <= distanceToRightBank ? -0.58f : 2.12f;
            return projected;
        }
    }
}
