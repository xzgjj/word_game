using System;
using System.Collections.Generic;
using System.IO;
using StarryForest.Core;
using UnityEngine;

namespace StarryForest.Save
{
    public sealed class SaveSlotService
    {
        public const string AutoSlotId = "auto";
        public const int ManualSlotCount = 3;

        private readonly SaveService saveService;
        private readonly string rootDirectory;

        public SaveSlotService(SaveService saveService, string rootDirectory = null)
        {
            this.saveService = saveService;
            this.rootDirectory = string.IsNullOrWhiteSpace(rootDirectory)
                ? Path.Combine(Application.persistentDataPath, "Saves")
                : rootDirectory;
        }

        public IReadOnlyList<SaveSlotSnapshot> GetSlots()
        {
            List<SaveSlotSnapshot> slots = new List<SaveSlotSnapshot>
            {
                CreateSnapshot(AutoSlotId, "自动保存", true)
            };

            for (int slot = 1; slot <= ManualSlotCount; slot++)
            {
                slots.Add(CreateSnapshot(GetManualSlotId(slot), $"手动存档 {slot}", false));
            }

            return slots;
        }

        public OperationResult SaveAuto(PlayerState state)
        {
            return saveService.Save(state, GetSlotPath(AutoSlotId));
        }

        public OperationResult SaveManual(PlayerState state, int slot)
        {
            if (!IsValidManualSlot(slot))
            {
                return OperationResult.Fail("手动存档槽不存在。");
            }

            return saveService.Save(state, GetSlotPath(GetManualSlotId(slot)));
        }

        public SaveLoadResult Load(string slotId)
        {
            return saveService.Load(GetSlotPath(slotId));
        }

        public OperationResult DeleteManual(int slot)
        {
            if (!IsValidManualSlot(slot))
            {
                return OperationResult.Fail("手动存档槽不存在。");
            }

            string path = GetSlotPath(GetManualSlotId(slot));
            if (!File.Exists(path))
            {
                return OperationResult.Fail("存档不存在。");
            }

            File.Delete(path);
            return OperationResult.Ok("手动存档已删除。");
        }

        public OperationResult Delete(string slotId)
        {
            if (slotId == AutoSlotId)
            {
                return OperationResult.Fail("自动保存不可删除。");
            }

            if (!TryGetManualSlot(slotId, out int manualSlot))
            {
                return OperationResult.Fail("存档槽不存在。");
            }

            return DeleteManual(manualSlot);
        }

        public static string GetManualSlotId(int slot)
        {
            return $"manual-{slot}";
        }

        public static bool TryGetManualSlot(string slotId, out int slot)
        {
            slot = 0;
            if (string.IsNullOrEmpty(slotId) || !slotId.StartsWith("manual-", StringComparison.Ordinal))
            {
                return false;
            }

            return int.TryParse(slotId.Substring("manual-".Length), out slot) && IsValidManualSlot(slot);
        }

        public static bool IsValidManualSlot(int slot)
        {
            return slot >= 1 && slot <= ManualSlotCount;
        }

        private SaveSlotSnapshot CreateSnapshot(string slotId, string label, bool isAuto)
        {
            string path = GetSlotPath(slotId);
            bool exists = File.Exists(path);
            DateTime? lastWriteTime = exists ? File.GetLastWriteTime(path) : (DateTime?)null;
            return new SaveSlotSnapshot(slotId, label, isAuto, exists, lastWriteTime);
        }

        private string GetSlotPath(string slotId)
        {
            return Path.Combine(rootDirectory, $"{slotId}.json");
        }
    }

    public sealed class SaveSlotSnapshot
    {
        public SaveSlotSnapshot(string slotId, string label, bool isAuto, bool exists, DateTime? lastWriteTime)
        {
            SlotId = slotId;
            Label = label;
            IsAuto = isAuto;
            Exists = exists;
            LastWriteTime = lastWriteTime;
        }

        public string SlotId { get; }
        public string Label { get; }
        public bool IsAuto { get; }
        public bool Exists { get; }
        public DateTime? LastWriteTime { get; }
    }
}
