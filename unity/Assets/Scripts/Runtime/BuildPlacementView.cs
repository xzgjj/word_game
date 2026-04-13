using System.Linq;
using StarryForest.Core;
using UnityEngine;

namespace StarryForest.Runtime
{
    public sealed class BuildPlacementView : MonoBehaviour
    {
        [SerializeField] private string nodeId;
        [SerializeField] private BlueprintId blueprintId;
        [SerializeField] private bool useSpecificGrid;
        [SerializeField] private int gridX;
        [SerializeField] private int gridY;
        [SerializeField] private GameObject builtObject;
        private Renderer[] placeholderRenderers;

        public string NodeId => nodeId;
        public BlueprintId BlueprintId => blueprintId;
        public GameObject BuiltObject => builtObject;

        public void Configure(string newNodeId, BlueprintId newBlueprintId, GameObject newBuiltObject)
        {
            nodeId = newNodeId;
            blueprintId = newBlueprintId;
            builtObject = newBuiltObject;
            CachePlaceholderRenderers();
            Refresh(null);
        }

        public void Configure(string newNodeId, BlueprintId newBlueprintId, int newGridX, int newGridY, GameObject newBuiltObject)
        {
            nodeId = newNodeId;
            blueprintId = newBlueprintId;
            gridX = newGridX;
            gridY = newGridY;
            useSpecificGrid = true;
            builtObject = newBuiltObject;
            CachePlaceholderRenderers();
            Refresh(null);
        }

        public void Refresh(PlayerState state)
        {
            bool isBuilt = state != null && state.PlacedBuildings.Any(IsMatchingBuilding);
            if (builtObject != null)
            {
                builtObject.SetActive(isBuilt);
            }

            if (placeholderRenderers == null)
            {
                CachePlaceholderRenderers();
            }

            foreach (Renderer renderer in placeholderRenderers)
            {
                if (renderer != null)
                {
                    renderer.enabled = !isBuilt;
                }
            }
        }

        public bool IsBuilt(PlayerState state)
        {
            return state != null && state.PlacedBuildings.Any(IsMatchingBuilding);
        }

        private void Awake()
        {
            CachePlaceholderRenderers();
        }

        private void CachePlaceholderRenderers()
        {
            placeholderRenderers = GetComponentsInChildren<Renderer>(true)
                .Where(renderer => builtObject == null || !renderer.transform.IsChildOf(builtObject.transform))
                .ToArray();
        }

        private bool IsMatchingBuilding(PlacedBuilding building)
        {
            return building.BlueprintId == blueprintId
                && (!useSpecificGrid || (building.GridX == gridX && building.GridY == gridY));
        }
    }
}
