namespace StarryForest.Core
{
    public sealed class ArchiveRecord
    {
        public ArchiveRecord(string id, string source, string label, string createdAtUtc, int builtCount, int stickerWallCount, int discoveredItemTypes)
        {
            Id = id;
            Source = source;
            Label = label;
            CreatedAtUtc = createdAtUtc;
            BuiltCount = builtCount;
            StickerWallCount = stickerWallCount;
            DiscoveredItemTypes = discoveredItemTypes;
        }

        public string Id { get; }
        public string Source { get; }
        public string Label { get; }
        public string CreatedAtUtc { get; }
        public int BuiltCount { get; }
        public int StickerWallCount { get; }
        public int DiscoveredItemTypes { get; }
    }
}
