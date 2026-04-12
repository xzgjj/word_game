using System.Collections.Generic;
using StarryForest.Core;

namespace StarryForest.Building
{
    public sealed class BuildRecipe
    {
        public BuildRecipe(BlueprintId blueprintId, IReadOnlyDictionary<ItemId, int> cost)
        {
            BlueprintId = blueprintId;
            Cost = cost;
        }

        public BlueprintId BlueprintId { get; }
        public IReadOnlyDictionary<ItemId, int> Cost { get; }
    }
}
