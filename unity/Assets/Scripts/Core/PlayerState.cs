using System.Collections.Generic;

namespace StarryForest.Core
{
    public sealed class PlayerState
    {
        public PlayerState(int inventorySlotCount)
        {
            PositionId = "home-yard";
            Emotion = EmotionMode.Joy;
            TimeOfDay = TimeOfDay.Morning;
            InventorySlotCount = inventorySlotCount;
            Items = new Dictionary<ItemId, int>();
            UnlockedBlueprints = new HashSet<BlueprintId>();
            PlacedBuildings = new List<PlacedBuilding>();
            KnownSystems = new HashSet<string>();
            UnlockedMiniGames = new HashSet<string>();
            CompletedMiniGames = new HashSet<string>();
        }

        public string PositionId { get; set; }
        public EmotionMode Emotion { get; set; }
        public TimeOfDay TimeOfDay { get; set; }
        public int InventorySlotCount { get; set; }
        public Dictionary<ItemId, int> Items { get; }
        public HashSet<BlueprintId> UnlockedBlueprints { get; }
        public List<PlacedBuilding> PlacedBuildings { get; }
        public HashSet<string> KnownSystems { get; }
        public HashSet<string> UnlockedMiniGames { get; }
        public string ActiveMiniGameId { get; set; }
        public int BuiltCount { get; set; }
        public bool CustomBuildUnlocked { get; set; }
        public HashSet<string> CompletedMiniGames { get; }
        public int StickerWallCount { get; set; }
    }
}
