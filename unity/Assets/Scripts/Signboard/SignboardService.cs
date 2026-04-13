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
        private readonly Dictionary<string, CommerceOffer> buyOffers;
        private readonly Dictionary<string, CommerceOffer> sellOffers;

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
                    "daily-wood-gift",
                    new ExchangeRecipe(
                        "daily-wood-gift",
                        ItemId.Wood,
                        1,
                        new Dictionary<ItemId, int>(),
                        true)
                },
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
            buyOffers = new Dictionary<string, CommerceOffer>
            {
                { "buy-wood", new CommerceOffer("buy-wood", ItemId.Wood, 1, 2) },
                { "buy-stone", new CommerceOffer("buy-stone", ItemId.Stone, 1, 2) },
                { "buy-flower-seed", new CommerceOffer("buy-flower-seed", ItemId.FlowerSeed, 1, 2) },
                { "buy-axe", new CommerceOffer("buy-axe", ItemId.Axe, 1, 5) }
            };
            sellOffers = new Dictionary<string, CommerceOffer>
            {
                { "sell-wood", new CommerceOffer("sell-wood", ItemId.Wood, 1, 1) },
                { "sell-stone", new CommerceOffer("sell-stone", ItemId.Stone, 1, 1) },
                { "sell-river-shell", new CommerceOffer("sell-river-shell", ItemId.RiverShell, 1, 2) },
                { "sell-fish", new CommerceOffer("sell-fish", ItemId.Fish, 1, 3) }
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
                && (!recipe.IsDailyReward || !state.SignboardDailyRewardClaimed)
                && inventoryService.CanAfford(state, recipe.Cost);
        }

        public SignboardMenuSnapshot GetMenuSnapshot(PlayerState state)
        {
            List<ExchangeRecipeAvailability> recipes = new List<ExchangeRecipeAvailability>();
            foreach (ExchangeRecipe recipe in exchangeRecipes.Values)
            {
                recipes.Add(new ExchangeRecipeAvailability(recipe, CanExchange(state, recipe.Id)));
            }

            List<CommerceOfferAvailability> buySnapshot = new List<CommerceOfferAvailability>();
            foreach (CommerceOffer offer in buyOffers.Values)
            {
                buySnapshot.Add(new CommerceOfferAvailability(offer, CanBuy(state, offer.Id)));
            }

            List<CommerceOfferAvailability> sellSnapshot = new List<CommerceOfferAvailability>();
            foreach (CommerceOffer offer in sellOffers.Values)
            {
                sellSnapshot.Add(new CommerceOfferAvailability(offer, CanSell(state, offer.Id)));
            }

            return new SignboardMenuSnapshot(
                state.BuiltCount,
                state.CustomBuildUnlocked,
                recipes,
                buySnapshot,
                sellSnapshot,
                new List<BlueprintId>(state.UnlockedBlueprints));
        }

        public OperationResult Exchange(PlayerState state, string recipeId)
        {
            if (!exchangeRecipes.TryGetValue(recipeId, out ExchangeRecipe recipe))
            {
                return OperationResult.Fail("兑换配方不存在。");
            }

            if (recipe.IsDailyReward)
            {
                if (state.SignboardDailyRewardClaimed)
                {
                    return OperationResult.Fail("今日赠礼已经领取。");
                }

                OperationResult addRewardResult = inventoryService.Add(state, recipe.OutputItemId, recipe.OutputCount);
                if (addRewardResult.Success)
                {
                    state.SignboardDailyRewardClaimed = true;
                }

                return addRewardResult.Success ? OperationResult.Ok("今日赠礼已放入背包。") : addRewardResult;
            }

            OperationResult spendResult = inventoryService.Spend(state, recipe.Cost);
            if (!spendResult.Success)
            {
                return spendResult;
            }

            return inventoryService.Add(state, recipe.OutputItemId, recipe.OutputCount);
        }

        public bool CanBuy(PlayerState state, string offerId)
        {
            return buyOffers.TryGetValue(offerId, out CommerceOffer offer)
                && inventoryService.GetCount(state, ItemId.StarCoin) >= offer.CoinCount;
        }

        public OperationResult Buy(PlayerState state, string offerId)
        {
            if (!buyOffers.TryGetValue(offerId, out CommerceOffer offer))
            {
                return OperationResult.Fail("购买条目不存在。");
            }

            OperationResult spendResult = inventoryService.Spend(state, new Dictionary<ItemId, int> { { ItemId.StarCoin, offer.CoinCount } });
            if (!spendResult.Success)
            {
                return spendResult;
            }

            return inventoryService.Add(state, offer.ItemId, offer.ItemCount);
        }

        public bool CanSell(PlayerState state, string offerId)
        {
            return sellOffers.TryGetValue(offerId, out CommerceOffer offer)
                && inventoryService.GetCount(state, offer.ItemId) >= offer.ItemCount;
        }

        public OperationResult Sell(PlayerState state, string offerId)
        {
            if (!sellOffers.TryGetValue(offerId, out CommerceOffer offer))
            {
                return OperationResult.Fail("出售条目不存在。");
            }

            OperationResult spendResult = inventoryService.Spend(state, new Dictionary<ItemId, int> { { offer.ItemId, offer.ItemCount } });
            if (!spendResult.Success)
            {
                return spendResult;
            }

            return inventoryService.Add(state, ItemId.StarCoin, offer.CoinCount);
        }
    }
}
