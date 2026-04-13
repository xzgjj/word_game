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
        private GUIStyle slotStyle;
        private GUIStyle slotHighlightStyle;
        private Texture2D panelTexture;
        private Texture2D slotTexture;
        private Texture2D slotHighlightTexture;

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
            DrawPromptBubble();
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

            if (runner.ShowInteractionFeedback)
            {
                DrawInteractionFeedback();
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
            Rect rect = new Rect(16, 16, 520, 118);
            GUI.Box(rect, GUIContent.none, panelStyle);
            GUILayout.BeginArea(new Rect(rect.x + 12, rect.y + 10, rect.width - 24, rect.height - 20));
            GUILayout.Label("星绪森林", titleStyle);
            GUILayout.Label(runner.IsMiniGameScene
                ? "WASD 移动，E 收集贴纸或从出口回家。"
                : "WASD 移动，E 互动，I 背包，Tab 装备，Esc 系统菜单。靠近木牌后按数字键买卖兑换。", bodyStyle);
            if (!string.IsNullOrEmpty(runner.LastMessage))
            {
                GUILayout.Label(runner.LastMessage, bodyStyle);
            }
            GUILayout.EndArea();
        }

        private void DrawPromptBubble()
        {
            if (string.IsNullOrEmpty(runner.CurrentPrompt))
            {
                return;
            }

            Rect rect = new Rect((Screen.width - 360) * 0.5f, Screen.height - 206, 360, 62);
            GUI.Box(rect, GUIContent.none, panelStyle);
            GUI.Box(new Rect(rect.x + 14, rect.y + 14, 42, 34), "E", slotHighlightStyle);
            GUI.Label(new Rect(rect.x + 66, rect.y + 13, rect.width - 82, 36), runner.CurrentPrompt, bodyStyle);
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
            Rect rect = new Rect(Screen.width - 438, 108, 422, 292);
            GUI.Box(rect, GUIContent.none, panelStyle);
            int categoryIndex = Mathf.Clamp(runner.InventoryCategoryIndex, 0, InventoryCategoryNames.Length - 1);
            GUI.Label(new Rect(rect.x + 16, rect.y + 12, rect.width - 32, 24), $"{ItemCatalog.InventorySlotCount} 格背包  {InventoryCategoryNames[categoryIndex]}", titleStyle);
            GUI.Label(new Rect(rect.x + 16, rect.y + 38, rect.width - 32, 24), "← / → 切换分类，Tab 更换装备", hintStyle);

            List<ItemId> visibleItems = GetVisibleInventoryItems(runner.State, categoryIndex);
            if (visibleItems.Count == 0)
            {
                GUI.Label(new Rect(rect.x + 18, rect.y + 80, rect.width - 36, 24), "这一栏还没有发现物品。", bodyStyle);
            }

            const int columns = 4;
            const float slotWidth = 92f;
            const float slotHeight = 48f;
            Vector2 origin = new Vector2(rect.x + 16, rect.y + 72);
            int slotIndex = 0;
            foreach (ItemId itemId in visibleItems)
            {
                runner.State.Items.TryGetValue(itemId, out int count);
                string equipmentHint = runner.IsEquipped(itemId)
                    ? "已装备"
                    : runner.CanEquip(itemId) ? "  可装备" : string.Empty;
                Rect slotRect = new Rect(
                    origin.x + (slotIndex % columns) * (slotWidth + 6f),
                    origin.y + (slotIndex / columns) * (slotHeight + 8f),
                    slotWidth,
                    slotHeight);
                DrawInventorySlot(slotRect, GetItemName(itemId), count.ToString(), equipmentHint, runner.IsEquipped(itemId));
                slotIndex += 1;
            }

            int reservedSlots = Mathf.Max(0, ItemCatalog.InventorySlotCount - visibleItems.Count);
            int reserveDrawCount = Mathf.Min(3, reservedSlots);
            for (int index = 0; index < reserveDrawCount; index++)
            {
                Rect slotRect = new Rect(
                    origin.x + (slotIndex % columns) * (slotWidth + 6f),
                    origin.y + (slotIndex / columns) * (slotHeight + 8f),
                    slotWidth,
                    slotHeight);
                GUI.Box(slotRect, "预留记录格", slotStyle);
                slotIndex += 1;
            }

            int discoveredCount = GetVisibleInventoryItems(runner.State).Count;
            int hiddenCount = ItemCatalog.Items.Count - discoveredCount;
            GUI.Label(new Rect(rect.x + 16, rect.y + rect.height - 34, rect.width - 32, 24), $"已发现 {discoveredCount}/{ItemCatalog.Items.Count} 类  隐藏 {hiddenCount} 类  预留记录格 3", hintStyle);
        }

        private void DrawSignboard()
        {
            SignboardMenuSnapshot snapshot = runner.GameState.Signboard.GetMenuSnapshot(runner.State);
            Rect rect = new Rect(16, Screen.height - 320, 620, 300);
            GUI.Box(rect, GUIContent.none, panelStyle);
            GUI.Label(new Rect(rect.x + 16, rect.y + 12, rect.width - 32, 24), "木屋前木牌", titleStyle);
            GUI.Label(new Rect(rect.x + 16, rect.y + 38, rect.width - 32, 22), "今日可换  按 1-5 兑换，6-9 购买，Shift+1-4 出售，Q 关闭。", hintStyle);

            int shortcut = 1;
            Rect exchangeRect = new Rect(rect.x + 16, rect.y + 70, 286, 124);
            Rect buyRect = new Rect(rect.x + 318, rect.y + 70, 134, 124);
            Rect sellRect = new Rect(rect.x + 466, rect.y + 70, 138, 124);
            GUI.Box(exchangeRect, "兑换", slotStyle);
            GUI.Box(buyRect, "购买", slotStyle);
            GUI.Box(sellRect, "出售", slotStyle);

            float rowY = exchangeRect.y + 28;
            foreach (ExchangeRecipeAvailability availability in snapshot.ExchangeRecipes)
            {
                string unavailableReason = availability.Recipe.IsDailyReward ? "今日已领" : "材料不足";
                GUI.Label(new Rect(exchangeRect.x + 10, rowY, exchangeRect.width - 20, 18), $"{shortcut}. {GetItemName(availability.Recipe.OutputItemId)} x{availability.Recipe.OutputCount}  {FormatCost(availability.Recipe.Cost)}  {(availability.CanExchange ? "可换" : unavailableReason)}", bodyStyle);
                rowY += 20;
                shortcut += 1;
            }

            shortcut = 6;
            rowY = buyRect.y + 28;
            foreach (CommerceOfferAvailability availability in snapshot.BuyOffers)
            {
                GUI.Label(new Rect(buyRect.x + 10, rowY, buyRect.width - 20, 18), $"{shortcut}. {GetItemName(availability.Offer.ItemId)} {(availability.CanUse ? "可买" : "缺币")}", bodyStyle);
                rowY += 20;
                shortcut += 1;
            }

            shortcut = 1;
            rowY = sellRect.y + 28;
            foreach (CommerceOfferAvailability availability in snapshot.SellOffers)
            {
                GUI.Label(new Rect(sellRect.x + 10, rowY, sellRect.width - 20, 18), $"S{shortcut}. {GetItemName(availability.Offer.ItemId)} {(availability.CanUse ? "可卖" : "缺少")}", bodyStyle);
                rowY += 20;
                shortcut += 1;
            }

            Rect recordRect = new Rect(rect.x + 16, rect.y + 208, rect.width - 32, 70);
            GUI.Box(recordRect, GUIContent.none, slotStyle);
            GUI.Label(new Rect(recordRect.x + 12, recordRect.y + 8, recordRect.width - 24, 18), $"图纸记录  {string.Join(" / ", snapshot.UnlockedBlueprints)}", bodyStyle);
            GUI.Label(new Rect(recordRect.x + 12, recordRect.y + 28, recordRect.width - 24, 18), $"小镇记录  建设 {snapshot.BuiltCount}/3，{(snapshot.CustomBuildUnlocked ? "自建已开启" : "自建未开启")}，贴纸墙 {runner.State.StickerWallCount}", bodyStyle);
            GUI.Label(new Rect(recordRect.x + 12, recordRect.y + 48, recordRect.width - 24, 18), $"档案记录  {runner.State.ArchiveRecords.Count}/20，任务记录预留 {runner.State.QuestRecords.Count}", bodyStyle);
        }

        private void DrawEquipmentWheel()
        {
            Rect rect = new Rect((Screen.width - 440) * 0.5f, (Screen.height - 360) * 0.5f, 440, 360);
            GUI.Box(rect, GUIContent.none, panelStyle);
            GUI.Label(new Rect(rect.x + 18, rect.y + 16, rect.width - 36, 24), "装备圆环", titleStyle);
            GUI.Label(new Rect(rect.x + 18, rect.y + 44, rect.width - 36, 24), "Tab 关闭，数字键选择，Backspace 收起装备。", hintStyle);

            IReadOnlyList<ItemId> equipmentItems = runner.GetVisibleEquipmentItems();
            if (equipmentItems.Count == 0)
            {
                GUI.Label(new Rect(rect.x + 32, rect.y + 150, rect.width - 64, 48), "还没有获得可装备物品。先在木牌购买斧头，或继续探索获得新物品。", bodyStyle);
                return;
            }

            Vector2 center = new Vector2(rect.x + rect.width * 0.5f, rect.y + rect.height * 0.54f);
            Rect centerRect = new Rect(center.x - 72f, center.y - 38f, 144f, 76f);
            GUI.Box(centerRect, $"当前\n{GetEquippedItemName(runner.State)}", slotHighlightStyle);

            float radius = 116f;
            for (int index = 0; index < equipmentItems.Count; index++)
            {
                ItemId itemId = equipmentItems[index];
                string status = runner.IsEquipped(itemId) ? "已装备" : "可装备";
                float angle = (-90f + (360f / equipmentItems.Count) * index) * Mathf.Deg2Rad;
                Vector2 position = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
                Rect itemRect = new Rect(position.x - 54f, position.y - 28f, 108f, 56f);
                GUI.Box(itemRect, $"{index + 1}\n{GetItemName(itemId)}\n{status}", runner.IsEquipped(itemId) ? slotHighlightStyle : slotStyle);
            }

            GUI.Label(new Rect(rect.x + 18, rect.y + rect.height - 34, rect.width - 36, 24), "选择后会关闭圆环并写入自动档。", hintStyle);
        }

        private void DrawInventorySlot(Rect slotRect, string itemName, string count, string hint, bool highlighted)
        {
            GUI.Box(slotRect, GUIContent.none, highlighted ? slotHighlightStyle : slotStyle);
            GUI.Label(new Rect(slotRect.x + 8, slotRect.y + 5, slotRect.width - 16, 18), itemName, bodyStyle);
            GUI.Label(new Rect(slotRect.x + 8, slotRect.y + 24, slotRect.width - 16, 18), $"x{count} {hint}", hintStyle);
        }

        private void DrawSystemMenu()
        {
            switch (runner.ActiveSystemMenuPanel)
            {
                case SystemMenuPanel.Save:
                    DrawSaveMenu();
                    break;
                case SystemMenuPanel.Settings:
                    DrawSettingsMenu();
                    break;
                case SystemMenuPanel.ExitConfirm:
                    DrawExitConfirmMenu();
                    break;
                default:
                    DrawSystemRootMenu();
                    break;
            }
        }

        private void DrawSystemRootMenu()
        {
            Rect rect = new Rect((Screen.width - 420) * 0.5f, (Screen.height - 270) * 0.5f, 420, 270);
            GUI.Box(rect, GUIContent.none, panelStyle);
            GUILayout.BeginArea(new Rect(rect.x + 18, rect.y + 16, rect.width - 36, rect.height - 32));
            GUILayout.Label("系统菜单", titleStyle);
            GUILayout.Label("↑ / ↓ 选择，Enter 或鼠标左键确认，Esc 返回。", hintStyle);
            GUILayout.Space(12);
            DrawMenuRow(0, runner.SystemMenuIndex, "存档");
            DrawMenuRow(1, runner.SystemMenuIndex, "游戏设置");
            DrawMenuRow(2, runner.SystemMenuIndex, "退出游戏");
            GUILayout.EndArea();
        }

        private void DrawSaveMenu()
        {
            IReadOnlyList<SaveSlotSnapshot> slots = runner.GameState.SaveSlots.GetSlots();
            Rect rect = new Rect((Screen.width - 520) * 0.5f, (Screen.height - 340) * 0.5f, 520, 340);
            GUI.Box(rect, GUIContent.none, panelStyle);
            GUILayout.BeginArea(new Rect(rect.x + 18, rect.y + 16, rect.width - 36, rect.height - 32));
            GUILayout.Label("存档", titleStyle);
            GUILayout.Label("↑ / ↓ 选择，S 保存手动档，L 读取，Delete 删除手动档，Q 返回上一级。", hintStyle);
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

        private void DrawSettingsMenu()
        {
            Rect rect = new Rect((Screen.width - 460) * 0.5f, (Screen.height - 260) * 0.5f, 460, 260);
            GUI.Box(rect, GUIContent.none, panelStyle);
            GUILayout.BeginArea(new Rect(rect.x + 18, rect.y + 16, rect.width - 36, rect.height - 32));
            GUILayout.Label("游戏设置", titleStyle);
            GUILayout.Label("↑ / ↓ 选择，Enter 或鼠标左键调整。", hintStyle);
            GUILayout.Space(12);
            DrawMenuRow(0, runner.SettingsMenuIndex, $"反馈音效：{(runner.FeedbackAudioEnabled ? "开" : "关")}");
            DrawMenuRow(1, runner.SettingsMenuIndex, "返回");
            GUILayout.EndArea();
        }

        private void DrawExitConfirmMenu()
        {
            Rect rect = new Rect((Screen.width - 460) * 0.5f, (Screen.height - 230) * 0.5f, 460, 230);
            GUI.Box(rect, GUIContent.none, panelStyle);
            GUILayout.BeginArea(new Rect(rect.x + 18, rect.y + 16, rect.width - 36, rect.height - 32));
            GUILayout.Label("退出游戏", titleStyle);
            GUILayout.Label("退出前会写入自动存档。确定要退出吗？", bodyStyle);
            GUILayout.Space(12);
            DrawMenuRow(0, runner.ExitConfirmIndex, "取消，回到系统菜单");
            DrawMenuRow(1, runner.ExitConfirmIndex, "保存并退出");
            GUILayout.EndArea();
        }

        private void DrawInteractionFeedback()
        {
            Rect rect = new Rect((Screen.width - 360) * 0.5f, Screen.height - 118, 360, 86);
            GUI.Box(rect, GUIContent.none, panelStyle);
            GUILayout.BeginArea(new Rect(rect.x + 14, rect.y + 10, rect.width - 28, rect.height - 20));
            GUILayout.Label(runner.InteractionFeedbackTitle, titleStyle);
            GUILayout.Label(runner.InteractionFeedbackDetail, bodyStyle);
            GUILayout.EndArea();
        }

        private void DrawMenuRow(int index, int selectedIndex, string label)
        {
            GUILayout.Label($"{(index == selectedIndex ? "> " : "  ")}{label}", bodyStyle);
        }

        private void DrawArcadeMenu()
        {
            Rect rect = new Rect(Screen.width - 336, Screen.height - 170, 320, 150);
            GUI.Box(rect, GUIContent.none, panelStyle);
            GUILayout.BeginArea(new Rect(rect.x + 12, rect.y + 10, rect.width - 24, rect.height - 20));
            GUILayout.Label("空地游戏机", titleStyle);
            GUILayout.Label("游戏机菜单已发现。", bodyStyle);
            GUILayout.Label(runner.CanStartMiniGame ? "按 Enter 进入像素小游戏。" : "旧卡带未就绪：在木牌用河贝 + 鱼兑换。", hintStyle);
            GUILayout.Label("Q 关闭菜单，Esc 打开系统菜单。", bodyStyle);
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

            panelTexture = Resources.Load<Texture2D>("KenneyUI/button_rectangle_depth_flat")
                ?? CreateSolidTexture(new Color(0.12f, 0.16f, 0.15f, 0.92f));
            slotTexture = Resources.Load<Texture2D>("KenneyUI/button_square_depth_flat")
                ?? CreateSolidTexture(new Color(0.2f, 0.25f, 0.22f, 0.94f));
            slotHighlightTexture = Resources.Load<Texture2D>("KenneyUI/button_round_depth_flat")
                ?? CreateSolidTexture(new Color(0.34f, 0.43f, 0.34f, 0.96f));

            panelStyle = new GUIStyle(GUI.skin.box)
            {
                normal = { background = panelTexture },
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
            slotStyle = new GUIStyle(GUI.skin.box)
            {
                alignment = TextAnchor.MiddleCenter,
                wordWrap = true,
                normal = { background = slotTexture, textColor = Color.white },
                padding = new RectOffset(6, 6, 4, 4)
            };
            slotHighlightStyle = new GUIStyle(slotStyle)
            {
                normal = { background = slotHighlightTexture, textColor = Color.white }
            };
        }

        private static Texture2D CreateSolidTexture(Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
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
