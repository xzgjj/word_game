using System.Collections.Generic;
using StarryForest.Core;

namespace StarryForest.Signboard
{
    public sealed class ExchangeRecipe
    {
        public ExchangeRecipe(string id, ItemId outputItemId, int outputCount, IReadOnlyDictionary<ItemId, int> cost)
        {
            Id = id;
            OutputItemId = outputItemId;
            OutputCount = outputCount;
            Cost = cost;
        }

        public string Id { get; }
        public ItemId OutputItemId { get; }
        public int OutputCount { get; }
        public IReadOnlyDictionary<ItemId, int> Cost { get; }
    }
}
