using StarryForest.Core;

namespace StarryForest.World.Gathering
{
    public sealed class GatherNodeDefinition
    {
        public GatherNodeDefinition(string nodeId, ItemId itemId, int amount, string sourceLabel)
        {
            NodeId = nodeId;
            ItemId = itemId;
            Amount = amount;
            SourceLabel = sourceLabel;
        }

        public string NodeId { get; }
        public ItemId ItemId { get; }
        public int Amount { get; }
        public string SourceLabel { get; }
    }
}
