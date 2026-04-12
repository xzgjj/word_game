using System.Collections.Generic;
using StarryForest.Building;
using StarryForest.Core;
using StarryForest.Inventory;

namespace StarryForest.Signboard
{
    public sealed class SignboardService
    {
        private readonly InventoryService inventoryService;
        private readonly BlueprintService blueprintService;
        private readonly Dictionary<string, ExchangeRecipe> exchangeRecipes;

        public SignboardService(InventoryService inventoryService)
            : this(inventoryService, new BlueprintService())
        {
        }

        public SignboardService(InventoryService inventoryService, BlueprintService blueprintService)
        {
            this.inventoryService = inventoryService;
            this.blueprintService = blueprintService;
            exchangeRecipes = new Dictionary<string, ExchangeRecipe>
            {
                {
                    "exchange-emotion-shard",
                    new ExchangeRecipe(
                        "exchange-emotion-shard",
                        ItemId.EmotionShard,
                        1,
                        new Dictionary<ItemId, int>
                        {
                            { ItemId.Wood, 1 },
                            { ItemId.FlowerSeed, 1 }
                        })
                },
                {
                    "exchange-old-cartridge",
                    new ExchangeRecipe(
                        "exchange-old-cartridge",
                        ItemId.OldCartridge,
                        1,
                        new Dictionary<ItemId, int>
                        {
                            { ItemId.RiverShell, 1 },
                            { ItemId.Fish, 1 }
                        })
                },
                {
                    "exchange-star-core",
                    new ExchangeRecipe(
                        "exchange-star-core",
                        ItemId.StarCore,
                        1,
                        new Dictionary<ItemId, int>
                        {
                            { ItemId.Stone, 2 },
                            { ItemId.RiverShell, 1 }
                        })
                },
                {
                    "exchange-sticker",
                    new ExchangeRecipe(
                        "exchange-sticker",
                        ItemId.Sticker,
                        1,
                        new Dictionary<ItemId, int>
                        {
                            { ItemId.Fish, 2 },
                            { ItemId.EmotionShard, 1 }
                        })
                }
            };
        }

        public OperationResult Open(PlayerState state)
        {
            state.KnownSystems.Add(GameConstants.SignboardSystemId);
            blueprintService.GrantInitialBlueprints(state);
            return OperationResult.Ok("木牌记录已打开。");
        }

        public IReadOnlyCollection<ExchangeRecipe> GetExchangeRecipes()
        {
            return exchangeRecipes.Values;
        }

        public bool CanExchange(PlayerState state, string recipeId)
        {
            return exchangeRecipes.TryGetValue(recipeId, out ExchangeRecipe recipe)
                && inventoryService.CanAfford(state, recipe.Cost);
        }

        public OperationResult Exchange(PlayerState state, string recipeId)
        {
            if (!exchangeRecipes.TryGetValue(recipeId, out ExchangeRecipe recipe))
            {
                return OperationResult.Fail("兑换配方不存在。");
            }

            OperationResult spendResult = inventoryService.Spend(state, recipe.Cost);
            if (!spendResult.Success)
            {
                return spendResult;
            }

            return inventoryService.Add(state, recipe.OutputItemId, recipe.OutputCount);
        }
    }
}
