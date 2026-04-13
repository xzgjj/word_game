using System.Linq;
using StarryForest.Core;
using StarryForest.World.Nodes;
using UnityEngine;

namespace StarryForest.Runtime
{
    public sealed class ClearingPlotView : MonoBehaviour
    {
        [SerializeField] private string nodeId;
        [SerializeField] private BlueprintId blueprintId;
        [SerializeField] private int gridX;
        [SerializeField] private int gridY;
        [SerializeField] private GameObject branchLayer;
        [SerializeField] private GameObject stoneLayer;
        [SerializeField] private GameObject weedLayer;
        [SerializeField] private GameObject clearedSlotLayer;

        public string NodeId => nodeId;

        public void Configure(
            string newNodeId,
            BlueprintId newBlueprintId,
            int newGridX,
            int newGridY,
            GameObject newBranchLayer,
            GameObject newStoneLayer,
            GameObject newWeedLayer,
            GameObject newClearedSlotLayer)
        {
            nodeId = newNodeId;
            blueprintId = newBlueprintId;
            gridX = newGridX;
            gridY = newGridY;
            branchLayer = newBranchLayer;
            stoneLayer = newStoneLayer;
            weedLayer = newWeedLayer;
            clearedSlotLayer = newClearedSlotLayer;
            Refresh(null);
        }

        public void Refresh(PlayerState state)
        {
            int stage = state == null ? 0 : WorldNodeService.GetWorldNodeStage(state, nodeId);
            bool isBuilt = state != null && state.PlacedBuildings.Any(building =>
                building.BlueprintId == blueprintId && building.GridX == gridX && building.GridY == gridY);

            SetActive(branchLayer, stage < 1 && !isBuilt);
            SetActive(stoneLayer, stage < 2 && !isBuilt);
            SetActive(weedLayer, stage < 3 && !isBuilt);
            SetActive(clearedSlotLayer, stage >= 3 && !isBuilt);
        }

        private static void SetActive(GameObject gameObject, bool active)
        {
            if (gameObject != null)
            {
                gameObject.SetActive(active);
            }
        }
    }
}
