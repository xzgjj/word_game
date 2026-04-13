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
            return IsInsideRect(position, -0.95f, 2.24f, -5.75f, -2.65f)
                || IsInsideRect(position, -0.5f, 2.38f, -2.7f, 0.8f)
                || IsInsideRect(position, -1.2f, 2.1f, 0.7f, 4.6f);
        }

        private static bool CrossesBlockedRiver(Vector3 from, Vector3 to, bool allowBridgeCrossing)
        {
            if (IsOnBridge(to, allowBridgeCrossing))
            {
                return false;
            }

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
                && position.x >= -1.05f
                && position.x <= 2.45f
                && position.z >= -3.75f
                && position.z <= -2.55f;
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
            if (TryProjectOutOfRect(position, -0.95f, 2.24f, -5.75f, -2.65f, out projected)
                || TryProjectOutOfRect(position, -0.5f, 2.38f, -2.7f, 0.8f, out projected)
                || TryProjectOutOfRect(position, -1.2f, 2.1f, 0.7f, 4.6f, out projected))
            {
                return projected;
            }

            return projected;
        }

        private static bool TryProjectOutOfRect(Vector3 position, float minX, float maxX, float minZ, float maxZ, out Vector3 projected)
        {
            projected = position;
            if (!IsInsideRect(position, minX, maxX, minZ, maxZ))
            {
                return false;
            }

            float distanceToLeft = Mathf.Abs(position.x - minX);
            float distanceToRight = Mathf.Abs(position.x - maxX);
            projected.x = distanceToLeft <= distanceToRight ? minX : maxX;
            return true;
        }
    }
}
