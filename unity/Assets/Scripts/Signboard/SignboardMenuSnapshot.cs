using System.Collections.Generic;
using StarryForest.Core;

namespace StarryForest.Signboard
{
    public sealed class SignboardMenuSnapshot
    {
        public SignboardMenuSnapshot(
            int builtCount,
            bool customBuildUnlocked,
            IReadOnlyList<ExchangeRecipeAvailability> exchangeRecipes,
            IReadOnlyList<BlueprintId> unlockedBlueprints)
        {
            BuiltCount = builtCount;
            CustomBuildUnlocked = customBuildUnlocked;
            ExchangeRecipes = exchangeRecipes;
            UnlockedBlueprints = unlockedBlueprints;
        }

        public int BuiltCount { get; }
        public bool CustomBuildUnlocked { get; }
        public IReadOnlyList<ExchangeRecipeAvailability> ExchangeRecipes { get; }
        public IReadOnlyList<BlueprintId> UnlockedBlueprints { get; }
    }

    public sealed class ExchangeRecipeAvailability
    {
        public ExchangeRecipeAvailability(ExchangeRecipe recipe, bool canExchange)
        {
            Recipe = recipe;
            CanExchange = canExchange;
        }

        public ExchangeRecipe Recipe { get; }
        public bool CanExchange { get; }
    }
}
