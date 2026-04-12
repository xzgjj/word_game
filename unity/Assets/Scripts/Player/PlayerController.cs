using UnityEngine;

namespace StarryForest.Player
{
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 4f;

        public float MoveSpeed
        {
            get => moveSpeed;
            set => moveSpeed = Mathf.Max(0f, value);
        }

        public void Move(Vector2 input, float deltaTime)
        {
            Vector2 clampedInput = Vector2.ClampMagnitude(input, 1f);
            Vector3 movement = new Vector3(clampedInput.x, 0f, clampedInput.y) * moveSpeed * Mathf.Max(0f, deltaTime);
            transform.position += movement;
        }
    }
}
