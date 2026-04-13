using StarryForest.Core;
using StarryForest.Inventory;
using UnityEngine;

namespace StarryForest.Runtime
{
    public sealed class ArcadeMachineView : MonoBehaviour
    {
        [SerializeField] private Renderer screenRenderer;

        public Renderer ScreenRenderer => screenRenderer;

        public void Configure(Renderer newScreenRenderer)
        {
            screenRenderer = newScreenRenderer;
        }

        public void Refresh(PlayerState state, InventoryService inventory)
        {
            if (screenRenderer == null)
            {
                return;
            }

            bool discovered = state != null && state.KnownSystems.Contains(GameConstants.ArcadeSystemId);
            bool ready = discovered && inventory != null && inventory.GetCount(state, ItemId.OldCartridge) > 0;
            Color color = ready
                ? new Color(0.32f, 1f, 0.86f)
                : discovered
                    ? new Color(0.18f, 0.72f, 0.86f)
                    : new Color(0.07f, 0.28f, 0.34f);

            Material material = screenRenderer.material;
            material.color = color;
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", color * (ready ? 2.2f : 1.2f));
        }
    }
}
