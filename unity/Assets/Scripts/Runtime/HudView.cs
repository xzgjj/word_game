using System.Collections.Generic;
using StarryForest.Core;
using StarryForest.Inventory;
using StarryForest.Save;
using StarryForest.Signboard;
using UnityEngine;

namespace StarryForest.Runtime
{
    public sealed class HudView : MonoBehaviour
    {
        private static readonly string[] InventoryCategoryNames =
        {
            "采集",
            "交换/货币",
            "建造种植",
            "装备/室内"
        };

        private static readonly ItemId[][] InventoryCategoryItems =
        {
            new[] { ItemId.Wood, ItemId.Stone, ItemId.RiverShell, ItemId.Fish },
            new[] { ItemId.StarCoin, ItemId.EmotionShard, ItemId.StarCore, ItemId.Sticker },
            new[] { ItemId.FlowerSeed, ItemId.Wood, ItemId.Stone },
            new[] { ItemId.Axe, ItemId.OldCartridge, ItemId.Sticker }
        };

        [SerializeField] private GameStateRunner runner;
        private GUIStyle panelStyle;
        private GUIStyle titleStyle;
        private GUIStyle bodyStyle;
        private GUIStyle hintStyle;

        public static int InventoryCategoryCount => InventoryCategoryNames.Length;

        public void Configure(GameStateRunner newRunner)
        {
            runner = newRunner;
        }

        private void Awake()
        {
            if (runner == null)
            {
                runner = GetComponent<GameStateRunner>();
            }
        }

        private void OnGUI()
        {
            if (runner == null || runner.State == null)
            {
                return;
            }

            EnsureStyles();
            DrawTopHint();
            DrawEmotionPortraits();
            if (runner.ShowInventory)
            {
                DrawInventory();
            }

            if (runner.ShowSignboard)
            {
                DrawSignboard();
            }

            if (runner.ShowEquipmentWheel)
            {
                DrawEquipmentWheel();
            }

            if (runner.ShowSystemMenu)
            {
                DrawSystemMenu();
            }

            if (runner.ShowArcadeMenu)
            {
                DrawArcadeMenu();
            }

            if (runner.IsMiniGameScene)
            {
                DrawMiniGamePanel();
            }
        }

        private void DrawTopHint()
        {
            Rect rect = new Rect(16, 16, 520, 130);
            GUI.Box(rect, GUIContent.none, panelStyle);
            GUILayout.BeginArea(new Rect(rect.x + 12, rect.y + 10, rect.width - 24, rect.height - 20));
            GUILayout.Label("星绪森林 视觉可玩版", titleStyle);
            GUILayout.Label(runner.IsMiniGameScene
                ? "WASD 移动，E 收集贴纸或从出口回家。"
                : "WASD 移动，E 互动，I 背包，Tab 装备圆环，Esc 存档菜单。靠近木牌后按 1-5 兑换。", bodyStyle);
            if (!string.IsNullOrEmpty(runner.CurrentPrompt))
            {
                GUILayout.Label(runner.CurrentPrompt, hintStyle);
            }
            if (!string.IsNullOrEmpty(runner.LastMessage))
            {
                GUILayout.Label(runner.LastMessage, bodyStyle);
            }
            GUILayout.EndArea();
        }

        private void DrawEmotionPortraits()
        {
            Rect rect = new Rect(Screen.width - 316, 16, 300, 82);
            GUI.Box(rect, GUIContent.none, panelStyle);
            GUILayout.BeginArea(new Rect(rect.x + 12, rect.y + 8, rect.width - 24, rect.height - 16));
            GUILayout.Label("表情", titleStyle);
            GUILayout.Label("安静  长发微垂  |  欢笑  亮眼笑脸  |  激动  星光眼", bodyStyle);
            GUILayout.EndArea();
        }

        private void DrawInventory()
        {
            Rect rect = new Rect(Screen.width - 316, 108, 300, 220);
            GUI.Box(rect, GUIContent.none, panelStyle);
            GUILayout.BeginArea(new Rect(rect.x + 12, rect.y + 10, rect.width - 24, rect.height - 20));
            int categoryIndex = Mathf.Clamp(runner.InventoryCategoryIndex, 0, InventoryCategoryNames.Length - 1);
            GUILayout.Label($"{ItemCatalog.InventorySlotCount} 格背包  {InventoryCategoryNames[categoryIndex]}", titleStyle);
            GUILayout.Label("← / → 切换分类，Tab 从装备/室内物品里更换装备", hintStyle);
            List<ItemId> visibleItems = GetVisibleInventoryItems(runner.State, categoryIndex);
            if (visibleItems.Count == 0)
            {
                GUILayout.Label("这一栏还没有发现物品。", bodyStyle);
            }

            foreach (ItemId itemId in visibleItems)
            {
                runner.State.Items.TryGetValue(itemId, out int count);
                string equipmentHint = runner.IsEquipped(itemId)
                    ? "  已装备"
                    : runner.CanEquip(itemId) ? "  可装备" : string.Empty;
                GUILayout.Label($"{GetItemName(itemId)}  {count}{equipmentHint}", bodyStyle);
            }
            int discoveredCount = GetVisibleInventoryItems(runner.State).Count;
            int hiddenCount = ItemCatalog.Items.Count - discoveredCount;
            GUILayout.Label($"已发现 {discoveredCount}/{ItemCatalog.Items.Count} 类  隐藏 {hiddenCount} 类  预留记录格 3", hintStyle);
            GUILayout.EndArea();
        }

        private void DrawSignboard()
        {
            SignboardMenuSnapshot snapshot = runner.GameState.Signboard.GetMenuSnapshot(runner.State);
            Rect rect = new Rect(16, Screen.height - 320, 620, 300);
            GUI.Box(rect, GUIContent.none, panelStyle);
            GUILayout.BeginArea(new Rect(rect.x + 12, rect.y + 10, rect.width - 24, rect.height - 20));
            GUILayout.Label("木屋前木牌", titleStyle);
            GUILayout.Label("今日可换  按 1-5 兑换，Esc 关闭", hintStyle);

            int shortcut = 1;
            foreach (ExchangeRecipeAvailability availability in snapshot.ExchangeRecipes)
            {
                string unavailableReason = availability.Recipe.IsDailyReward ? "今日已领" : "材料不足";
                GUILayout.Label($"{shortcut}. {GetItemName(availability.Recipe.OutputItemId)} x{availability.Recipe.OutputCount}  需要 {FormatCost(availability.Recipe.Cost)}  {(availability.CanExchange ? "可换" : unavailableReason)}", bodyStyle);
                shortcut += 1;
            }

            GUILayout.Space(8);
            GUILayout.Label("星币购买  6 木材 / 7 石子 / 8 花种 / 9 斧头", hintStyle);
            foreach (CommerceOfferAvailability availability in snapshot.BuyOffers)
            {
                GUILayout.Label($"{GetItemName(availability.Offer.ItemId)} x{availability.Offer.ItemCount}  {availability.Offer.CoinCount} 星币  {(availability.CanUse ? "可买" : "星币不足")}", bodyStyle);
            }

            GUILayout.Label("出售收集物  Shift+1 木材 / Shift+2 石子 / Shift+3 河贝 / Shift+4 鱼", hintStyle);
            foreach (CommerceOfferAvailability availability in snapshot.SellOffers)
            {
                GUILayout.Label($"{GetItemName(availability.Offer.ItemId)} x{availability.Offer.ItemCount}  换 {availability.Offer.CoinCount} 星币  {(availability.CanUse ? "可卖" : "数量不足")}", bodyStyle);
            }

            GUILayout.Space(8);
            GUILayout.Label($"图纸记录  {string.Join(" / ", snapshot.UnlockedBlueprints)}", bodyStyle);
            GUILayout.Label($"小镇记录  建设 {snapshot.BuiltCount}/3，{(snapshot.CustomBuildUnlocked ? "自建已开启" : "自建未开启")}，贴纸墙 {runner.State.StickerWallCount}", bodyStyle);
            GUILayout.Label($"档案记录  {runner.State.ArchiveRecords.Count}/20，任务记录预留 {runner.State.QuestRecords.Count}", bodyStyle);
            GUILayout.EndArea();
        }

        private void DrawEquipmentWheel()
        {
            Rect rect = new Rect((Screen.width - 360) * 0.5f, (Screen.height - 280) * 0.5f, 360, 280);
            GUI.Box(rect, GUIContent.none, panelStyle);
            GUILayout.BeginArea(new Rect(rect.x + 16, rect.y + 14, rect.width - 32, rect.height - 28));
            GUILayout.Label("装备圆环", titleStyle);
            GUILayout.Label("Tab 关闭，数字键选择，Backspace 收起装备。物品来自背包装备/室内分类。", hintStyle);
            GUILayout.Space(10);
            IReadOnlyList<ItemId> equipmentItems = runner.EquipmentItems;
            for (int index = 0; index < equipmentItems.Count; index++)
            {
                ItemId itemId = equipmentItems[index];
                bool ownsItem = runner.GameState.Inventory.GetCount(runner.State, itemId) > 0;
                string status = runner.IsEquipped(itemId) ? "已装备" : ownsItem ? "可装备" : "未获得";
                GUILayout.Label($"{index + 1}. {GetItemName(itemId)}  {status}", bodyStyle);
            }

            GUILayout.Label($"当前装备：{GetEquippedItemName(runner.State)}", hintStyle);
            GUILayout.EndArea();
        }

        private void DrawSystemMenu()
        {
            IReadOnlyList<SaveSlotSnapshot> slots = runner.GameState.SaveSlots.GetSlots();
            Rect rect = new Rect((Screen.width - 520) * 0.5f, (Screen.height - 340) * 0.5f, 520, 340);
            GUI.Box(rect, GUIContent.none, panelStyle);
            GUILayout.BeginArea(new Rect(rect.x + 18, rect.y + 16, rect.width - 36, rect.height - 32));
            GUILayout.Label("系统菜单", titleStyle);
            GUILayout.Label("↑ / ↓ 选择存档，S 保存手动档，L 读取，Delete 删除手动档，Q 保存并退出，Esc 返回。", hintStyle);
            GUILayout.Space(10);

            for (int index = 0; index < slots.Count; index++)
            {
                SaveSlotSnapshot slot = slots[index];
                string cursor = index == runner.SaveMenuSlotIndex ? "> " : "  ";
                string state = slot.Exists && slot.LastWriteTime.HasValue
                    ? slot.LastWriteTime.Value.ToString("yyyy-MM-dd HH:mm")
                    : "空";
                string lockHint = slot.IsAuto ? "  自动档不可删除" : string.Empty;
                GUILayout.Label($"{cursor}{slot.Label}  {state}{lockHint}", bodyStyle);
            }

            GUILayout.EndArea();
        }

        private void DrawArcadeMenu()
        {
            Rect rect = new Rect(Screen.width - 336, Screen.height - 170, 320, 150);
            GUI.Box(rect, GUIContent.none, panelStyle);
            GUILayout.BeginArea(new Rect(rect.x + 12, rect.y + 10, rect.width - 24, rect.height - 20));
            GUILayout.Label("空地游戏机", titleStyle);
            GUILayout.Label("游戏机菜单已发现。", bodyStyle);
            GUILayout.Label(runner.CanStartMiniGame ? "按 Enter 进入像素小游戏。" : "旧卡带未就绪：在木牌用河贝 + 鱼兑换。", hintStyle);
            GUILayout.Label("Esc 关闭菜单。", bodyStyle);
            GUILayout.EndArea();
        }

        private void DrawMiniGamePanel()
        {
            Rect rect = new Rect(Screen.width - 316, Screen.height - 150, 300, 130);
            GUI.Box(rect, GUIContent.none, panelStyle);
            GUILayout.BeginArea(new Rect(rect.x + 12, rect.y + 10, rect.width - 24, rect.height - 20));
            GUILayout.Label("像素贴纸小游戏", titleStyle);
            GUILayout.Label($"贴纸 {runner.State.ActiveMiniGameStickerCount}/{GameConstants.MiniGameStickerTarget}", bodyStyle);
            GUILayout.Label(runner.State.ActiveMiniGameStickerCount >= GameConstants.MiniGameStickerTarget
                ? "出口已点亮，靠近出口按 E 回家。"
                : "收齐 3 个贴纸后出口会点亮。", hintStyle);
            GUILayout.EndArea();
        }

        private void EnsureStyles()
        {
            if (panelStyle != null)
            {
                return;
            }

            panelStyle = new GUIStyle(GUI.skin.box)
            {
                normal = { background = Texture2D.grayTexture },
                padding = new RectOffset(12, 12, 12, 12)
            };
            titleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 18,
                fontStyle = FontStyle.Bold,
                normal = { textColor = new Color(0.95f, 1f, 0.92f) }
            };
            bodyStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 14,
                wordWrap = true,
                normal = { textColor = Color.white }
            };
            hintStyle = new GUIStyle(bodyStyle)
            {
                normal = { textColor = new Color(0.75f, 1f, 0.9f) }
            };
        }

        private static string FormatCost(IReadOnlyDictionary<ItemId, int> cost)
        {
            if (cost.Count == 0)
            {
                return "今日赠礼";
            }

            List<string> parts = new List<string>();
            foreach (KeyValuePair<ItemId, int> entry in cost)
            {
                parts.Add($"{GetItemName(entry.Key)} {entry.Value}");
            }

            return string.Join(" / ", parts);
        }

        private static List<ItemId> GetVisibleInventoryItems(PlayerState state, int categoryIndex = -1)
        {
            List<ItemId> visibleItems = new List<ItemId>
            {
                ItemId.Wood,
                ItemId.Stone,
                ItemId.FlowerSeed
            };

            IReadOnlyList<ItemId> sourceItems = categoryIndex >= 0 ? InventoryCategoryItems[categoryIndex] : ItemCatalog.Items;
            foreach (ItemId itemId in sourceItems)
            {
                if (visibleItems.Contains(itemId))
                {
                    continue;
                }

                if (state.Items.TryGetValue(itemId, out int count) && count > 0)
                {
                    visibleItems.Add(itemId);
                }
            }

            if (categoryIndex >= 0)
            {
                visibleItems.RemoveAll(itemId => System.Array.IndexOf(InventoryCategoryItems[categoryIndex], itemId) < 0);
            }

            return visibleItems;
        }

        private static string GetItemName(ItemId itemId)
        {
            return itemId switch
            {
                ItemId.Wood => "木材",
                ItemId.Stone => "石子",
                ItemId.FlowerSeed => "花种",
                ItemId.RiverShell => "河贝",
                ItemId.Fish => "鱼",
                ItemId.EmotionShard => "表情碎片",
                ItemId.StarCoin => "星币",
                ItemId.OldCartridge => "旧卡带",
                ItemId.StarCore => "星屑灯芯",
                ItemId.Sticker => "贴纸",
                ItemId.Axe => "斧头",
                _ => itemId.ToString()
            };
        }

        private static string GetEquippedItemName(PlayerState state)
        {
            if (string.IsNullOrEmpty(state.EquippedItemId) || !System.Enum.TryParse(state.EquippedItemId, out ItemId itemId))
            {
                return "无";
            }

            return GetItemName(itemId);
        }
    }
}
