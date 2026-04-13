using StarryForest.Core;
using UnityEngine;

namespace StarryForest.World.Nodes
{
    public sealed class WorldNodeInteractor : MonoBehaviour
    {
        [SerializeField] private string nodeId;
        [SerializeField] private string displayLabel;
        [SerializeField] private string prompt;
        [SerializeField] private float interactionRadius = 1.5f;

        public string NodeId => nodeId;
        public string DisplayLabel => displayLabel;
        public string Prompt => prompt;
        public float InteractionRadius => GetEffectiveInteractionRadius();

        private void Awake()
        {
            if (nodeId == "river-bridge" && interactionRadius < 2.6f)
            {
                interactionRadius = 2.6f;
            }
        }

        public void Configure(string newNodeId, string newDisplayLabel, string newPrompt, float newInteractionRadius = 1.5f)
        {
            nodeId = newNodeId;
            displayLabel = newDisplayLabel;
            prompt = newPrompt;
            interactionRadius = newInteractionRadius;
        }

        public bool IsInRange(Vector3 playerPosition)
        {
            return isActiveAndEnabled
                && gameObject.activeInHierarchy
                && Vector3.Distance(transform.position, playerPosition) <= InteractionRadius;
        }

        public string GetPrompt(Vector3 playerPosition)
        {
            return IsInRange(playerPosition) ? prompt : string.Empty;
        }

        public OperationResult Interact(GameState gameState)
        {
            if (gameState == null)
            {
                return OperationResult.Fail("游戏状态未初始化。");
            }

            if (string.IsNullOrWhiteSpace(nodeId))
            {
                return OperationResult.Fail("世界节点未配置。");
            }

            return gameState.InteractWithWorldNode(nodeId);
        }

        private float GetEffectiveInteractionRadius()
        {
            return nodeId == "river-bridge" && interactionRadius < 2.6f ? 2.6f : interactionRadius;
        }
    }
}
