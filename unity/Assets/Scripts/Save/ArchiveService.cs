using System;
using StarryForest.Core;

namespace StarryForest.Save
{
    public sealed class ArchiveService
    {
        public const int MaxArchiveRecords = 20;

        public OperationResult CreateArchive(PlayerState state, string source, string label)
        {
            int discoveredItemTypes = 0;
            foreach (int count in state.Items.Values)
            {
                if (count > 0)
                {
                    discoveredItemTypes += 1;
                }
            }

            DateTime createdAtUtc = DateTime.UtcNow;
            string id = $"{source}-{createdAtUtc.Ticks}-{state.ArchiveRecords.Count + 1}";
            state.ArchiveRecords.Add(new ArchiveRecord(
                id,
                source,
                label,
                createdAtUtc.ToString("o"),
                state.BuiltCount,
                state.StickerWallCount,
                discoveredItemTypes));

            while (state.ArchiveRecords.Count > MaxArchiveRecords)
            {
                state.ArchiveRecords.RemoveAt(0);
            }

            return OperationResult.Ok(source == "auto" ? "自动档案已记录。" : "档案已保存。");
        }
    }
}
