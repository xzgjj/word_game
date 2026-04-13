using StarryForest.Core;
using UnityEngine;

namespace StarryForest.Runtime
{
    public sealed class StickerWallView : MonoBehaviour
    {
        [SerializeField] private GameObject[] stickerSlots;

        public int StickerSlotCount => stickerSlots?.Length ?? 0;

        public void Configure(GameObject[] newStickerSlots)
        {
            stickerSlots = newStickerSlots;
            Refresh(null);
        }

        public void Refresh(PlayerState state)
        {
            int visibleCount = state == null ? 0 : Mathf.Clamp(state.StickerWallCount, 0, StickerSlotCount);
            for (int index = 0; index < StickerSlotCount; index++)
            {
                if (stickerSlots[index] != null)
                {
                    stickerSlots[index].SetActive(index < visibleCount);
                }
            }
        }
    }
}
