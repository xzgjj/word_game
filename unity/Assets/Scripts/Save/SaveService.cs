using System;
using System.Collections.Generic;
using System.IO;
using StarryForest.Core;
using StarryForest.Inventory;
using UnityEngine;

namespace StarryForest.Save
{
    public sealed class SaveService
    {
        public OperationResult Save(PlayerState state, string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return OperationResult.Fail("存档路径不能为空。");
            }

            string directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string json = JsonUtility.ToJson(PlayerSaveData.FromState(state), true);
            File.WriteAllText(filePath, json);
            return OperationResult.Ok("存档已保存。");
        }

        public SaveLoadResult Load(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                return SaveLoadResult.Fail("存档不存在。");
            }

            PlayerSaveData data = JsonUtility.FromJson<PlayerSaveData>(File.ReadAllText(filePath));
            if (data == null || data.schemaVersion <= 0)
            {
                return SaveLoadResult.Fail("存档格式无效。");
            }

            return SaveLoadResult.Ok(data.ToState(), "存档已读取。");
        }
    }

    [Serializable]
    public sealed class PlayerSaveData
    {
        public int schemaVersion;
        public string positionId;
        public string emotion;
        public string timeOfDay;
        public int inventorySlotCount;
        public List<ItemCountData> items = new List<ItemCountData>();
        public List<string> unlockedBlueprints = new List<string>();
        public List<PlacedBuildingData> placedBuildings = new List<PlacedBuildingData>();
        public List<string> knownSystems = new List<string>();
        public List<string> unlockedMiniGames = new List<string>();
        public string activeMiniGameId;
        public int activeMiniGameStickerCount;
        public int builtCount;
        public bool customBuildUnlocked;
        public List<string> completedMiniGames = new List<string>();
        public int stickerWallCount;

        public static PlayerSaveData FromState(PlayerState state)
        {
            PlayerSaveData data = new PlayerSaveData
            {
                schemaVersion = 2,
                positionId = state.PositionId,
                emotion = state.Emotion.ToString(),
                timeOfDay = state.TimeOfDay.ToString(),
                inventorySlotCount = state.InventorySlotCount,
                activeMiniGameId = state.ActiveMiniGameId,
                activeMiniGameStickerCount = state.ActiveMiniGameStickerCount,
                builtCount = state.BuiltCount,
                customBuildUnlocked = state.CustomBuildUnlocked,
                stickerWallCount = state.StickerWallCount
            };

            foreach (KeyValuePair<ItemId, int> item in state.Items)
            {
                data.items.Add(new ItemCountData { itemId = item.Key.ToString(), count = item.Value });
            }

            foreach (BlueprintId blueprintId in state.UnlockedBlueprints)
            {
                data.unlockedBlueprints.Add(blueprintId.ToString());
            }

            foreach (PlacedBuilding building in state.PlacedBuildings)
            {
                data.placedBuildings.Add(PlacedBuildingData.FromPlacedBuilding(building));
            }

            data.knownSystems.AddRange(state.KnownSystems);
            data.unlockedMiniGames.AddRange(state.UnlockedMiniGames);
            data.completedMiniGames.AddRange(state.CompletedMiniGames);

            return data;
        }

        public PlayerState ToState()
        {
            PlayerState state = new PlayerState(inventorySlotCount > 0 ? inventorySlotCount : ItemCatalog.InventorySlotCount)
            {
                PositionId = string.IsNullOrEmpty(positionId) ? "home-yard" : positionId,
                Emotion = ParseEnum(emotion, EmotionMode.Joy),
                TimeOfDay = ParseEnum(timeOfDay, TimeOfDay.Morning),
                ActiveMiniGameId = string.IsNullOrEmpty(activeMiniGameId) ? null : activeMiniGameId,
                ActiveMiniGameStickerCount = activeMiniGameStickerCount,
                BuiltCount = builtCount,
                CustomBuildUnlocked = customBuildUnlocked,
                StickerWallCount = stickerWallCount
            };

            foreach (ItemCountData item in items)
            {
                if (Enum.TryParse(item.itemId, out ItemId itemId))
                {
                    state.Items[itemId] = item.count;
                }
            }

            foreach (string blueprint in unlockedBlueprints)
            {
                if (Enum.TryParse(blueprint, out BlueprintId blueprintId))
                {
                    state.UnlockedBlueprints.Add(blueprintId);
                }
            }

            foreach (PlacedBuildingData building in placedBuildings)
            {
                state.PlacedBuildings.Add(building.ToPlacedBuilding());
            }

            AddStrings(state.KnownSystems, knownSystems);
            AddStrings(state.UnlockedMiniGames, unlockedMiniGames);
            AddStrings(state.CompletedMiniGames, completedMiniGames);

            return state;
        }

        private static TEnum ParseEnum<TEnum>(string value, TEnum fallback) where TEnum : struct
        {
            return Enum.TryParse(value, out TEnum parsed) ? parsed : fallback;
        }

        private static void AddStrings(HashSet<string> target, List<string> source)
        {
            foreach (string value in source)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    target.Add(value);
                }
            }
        }
    }

    [Serializable]
    public sealed class ItemCountData
    {
        public string itemId;
        public int count;
    }

    [Serializable]
    public sealed class PlacedBuildingData
    {
        public string id;
        public string blueprintId;
        public int gridX;
        public int gridY;
        public int rotation;
        public CustomBuildingSaveData customData;

        public static PlacedBuildingData FromPlacedBuilding(PlacedBuilding building)
        {
            PlacedBuildingData data = new PlacedBuildingData
            {
                id = building.Id,
                blueprintId = building.BlueprintId.ToString(),
                gridX = building.GridX,
                gridY = building.GridY,
                rotation = building.Rotation
            };

            if (building.CustomData != null)
            {
                data.customData = CustomBuildingSaveData.FromCustomData(building.CustomData);
            }

            return data;
        }

        public PlacedBuilding ToPlacedBuilding()
        {
            BlueprintId parsedBlueprintId = Enum.TryParse(blueprintId, out BlueprintId value) ? value : BlueprintId.Bridge;
            PlacedBuilding building = new PlacedBuilding(id, parsedBlueprintId, gridX, gridY, rotation);
            if (customData != null)
            {
                building.CustomData = customData.ToCustomData();
            }

            return building;
        }
    }

    [Serializable]
    public sealed class CustomBuildingSaveData
    {
        public string shape;
        public string theme;
        public string size;

        public static CustomBuildingSaveData FromCustomData(CustomBuildingData customData)
        {
            return new CustomBuildingSaveData
            {
                shape = customData.Shape.ToString(),
                theme = customData.Theme.ToString(),
                size = customData.Size.ToString()
            };
        }

        public CustomBuildingData ToCustomData()
        {
            CustomBuildingShape parsedShape = Enum.TryParse(shape, out CustomBuildingShape shapeValue) ? shapeValue : CustomBuildingShape.TinyCabin;
            CustomBuildingTheme parsedTheme = Enum.TryParse(theme, out CustomBuildingTheme themeValue) ? themeValue : CustomBuildingTheme.Wood;
            CustomBuildingSize parsedSize = Enum.TryParse(size, out CustomBuildingSize sizeValue) ? sizeValue : CustomBuildingSize.OneByOne;
            return new CustomBuildingData(parsedShape, parsedTheme, parsedSize);
        }
    }
}
