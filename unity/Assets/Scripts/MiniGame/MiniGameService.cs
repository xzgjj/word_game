using StarryForest.Core;
using StarryForest.Building;
using StarryForest.Inventory;

namespace StarryForest.MiniGame
{
    public sealed class MiniGameService
    {
        private readonly InventoryService inventoryService;
        private readonly BlueprintService blueprintService;

        public MiniGameService(InventoryService inventoryService)
            : this(inventoryService, new BlueprintService())
        {
        }

        public MiniGameService(InventoryService inventoryService, BlueprintService blueprintService)
        {
            this.inventoryService = inventoryService;
            this.blueprintService = blueprintService;
        }

        public OperationResult DiscoverArcade(PlayerState state)
        {
            state.KnownSystems.Add(GameConstants.ArcadeSystemId);
            state.UnlockedMiniGames.Add(GameConstants.FirstMiniGameId);
            return OperationResult.Ok("游戏机菜单入口已发现。");
        }

        public bool IsArcadeMenuAvailable(PlayerState state)
        {
            return state.KnownSystems.Contains(GameConstants.ArcadeSystemId);
        }

        public OperationResult Start(PlayerState state, string miniGameId)
        {
            if (state.ActiveMiniGameId != null)
            {
                return OperationResult.Fail("已经在小游戏中。");
            }

            if (!IsArcadeMenuAvailable(state))
            {
                return OperationResult.Fail("还没有发现游戏机入口。");
            }

            if (!state.UnlockedMiniGames.Contains(miniGameId))
            {
                return OperationResult.Fail("小游戏尚未解锁。");
            }

            if (inventoryService.GetCount(state, ItemId.OldCartridge) <= 0)
            {
                return OperationResult.Fail("需要先准备旧卡带。");
            }

            state.ActiveMiniGameId = miniGameId;
            state.ActiveMiniGameStickerCount = 0;
            return OperationResult.Ok("进入像素小游戏。");
        }

        public OperationResult CollectSticker(PlayerState state)
        {
            if (state.ActiveMiniGameId == null)
            {
                return OperationResult.Fail("当前不在小游戏中。");
            }

            if (state.ActiveMiniGameStickerCount >= GameConstants.MiniGameStickerTarget)
            {
                return OperationResult.Ok("贴纸已经收齐。");
            }

            state.ActiveMiniGameStickerCount += 1;
            return OperationResult.Ok("贴纸已收集。");
        }

        public bool CanExit(PlayerState state)
        {
            return state.ActiveMiniGameId != null
                && state.ActiveMiniGameStickerCount >= GameConstants.MiniGameStickerTarget;
        }

        public OperationResult Finish(PlayerState state, MiniGameResult result)
        {
            if (result.StickersCollected < 0)
            {
                return OperationResult.Fail("贴纸数量不能小于 0。");
            }

            if (state.ActiveMiniGameId != result.MiniGameId)
            {
                return OperationResult.Fail("小游戏状态不匹配。");
            }

            if (result.Success && result.StickersCollected < GameConstants.MiniGameStickerTarget)
            {
                return OperationResult.Fail("贴纸还没有收齐。");
            }

            state.ActiveMiniGameId = null;
            state.ActiveMiniGameStickerCount = 0;
            if (!result.Success)
            {
                return OperationResult.Ok("已返回主世界，未结算贴纸。");
            }

            state.CompletedMiniGames.Add(result.MiniGameId);
            state.StickerWallCount += 1;
            blueprintService.GrantMiniGameCompletionBlueprints(state);
            if (result.StickersCollected > 0)
            {
                inventoryService.Add(state, ItemId.Sticker, result.StickersCollected);
            }

            return OperationResult.Ok("贴纸已带回木屋。");
        }
    }
}
