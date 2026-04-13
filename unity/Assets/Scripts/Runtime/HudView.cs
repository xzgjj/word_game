using System.Collections.Generic;
using StarryForest.Core;
using StarryForest.Inventory;
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
            "室内/特殊"
        };

        private static readonly ItemId[][] InventoryCategoryItems =
        {
            new[] { ItemId.Wood, ItemId.Stone, ItemId.RiverShell, ItemId.Fish },
            new[] { ItemId.EmotionShard, ItemId.StarCore, ItemId.Sticker },
            new[] { ItemId.FlowerSeed, ItemId.Wood, ItemId.Stone },
            new[] { ItemId.OldCartridge, ItemId.Sticker }
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
                : "WASD 移动，E 互动，I 打开/隐藏背包，T 切换时间。靠近木牌后按 1-5 兑换。", bodyStyle);
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
            GUILayout.Label($"12 格背包  {InventoryCategoryNames[categoryIndex]}", titleStyle);
            GUILayout.Label("← / → 切换分类", hintStyle);
            List<ItemId> visibleItems = GetVisibleInventoryItems(runner.State, categoryIndex);
            if (visibleItems.Count == 0)
            {
                GUILayout.Label("这一栏还没有发现物品。", bodyStyle);
            }

            foreach (ItemId itemId in visibleItems)
            {
                runner.State.Items.TryGetValue(itemId, out int count);
                GUILayout.Label($"{GetItemName(itemId)}  {count}", bodyStyle);
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
            GUILayout.Label($"图纸记录  {string.Join(" / ", snapshot.UnlockedBlueprints)}", bodyStyle);
            GUILayout.Label($"小镇记录  建设 {snapshot.BuiltCount}/3，{(snapshot.CustomBuildUnlocked ? "自建已开启" : "自建未开启")}，贴纸墙 {runner.State.StickerWallCount}", bodyStyle);
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
                ItemId.OldCartridge => "旧卡带",
                ItemId.StarCore => "星屑灯芯",
                ItemId.Sticker => "贴纸",
                _ => itemId.ToString()
            };
        }
    }
}
