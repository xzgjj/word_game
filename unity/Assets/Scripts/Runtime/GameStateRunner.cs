using System.Collections.Generic;
using System.Linq;
using StarryForest.Core;
using StarryForest.MiniGame;
using StarryForest.Player;
using StarryForest.Save;
using StarryForest.Signboard;
using StarryForest.World.Nodes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

namespace StarryForest.Runtime
{
    public sealed class GameStateRunner : MonoBehaviour
    {
        private const float MiniGameMoveSpeed = 4.6f;
        private const float AutoArchiveIntervalSeconds = 600f;
        private static readonly ItemId[] EquipmentRingItems =
        {
            ItemId.Axe,
            ItemId.OldCartridge,
            ItemId.Sticker
        };

        private static GameStateRunner instance;

        private readonly List<WorldNodeInteractor> worldNodes = new List<WorldNodeInteractor>();
        private readonly List<GameObject> miniGameStickers = new List<GameObject>();
        private BuildPlacementView[] buildPlacementViews = new BuildPlacementView[0];
        private ClearingPlotView[] clearingPlotViews = new ClearingPlotView[0];
        private StickerWallView stickerWallView;
        private ArcadeMachineView arcadeMachineView;
        private PlayerController playerController;
        private Transform miniGamePlayer;
        private Transform exitDoor;
        private AudioSource audioSource;
        private AudioClip successClip;
        private AudioClip failClip;
        private AudioClip menuClip;
        private bool firstResourceGuideCompleted;
        private float autoArchiveTimer;

        public GameState GameState { get; private set; }
        public PlayerState State => GameState?.Player;
        public WorldNodeInteractor CurrentNode { get; private set; }
        public string CurrentPrompt { get; private set; }
        public string LastMessage { get; private set; }
        public bool ShowSignboard { get; private set; }
        public bool ShowArcadeMenu { get; private set; }
        public bool ShowInventory { get; private set; }
        public bool ShowEquipmentWheel { get; private set; }
        public bool ShowSystemMenu { get; private set; }
        public int InventoryCategoryIndex { get; private set; }
        public int SaveMenuSlotIndex { get; private set; }
        public IReadOnlyList<ItemId> EquipmentItems => EquipmentRingItems;
        public bool IsMiniGameScene => SceneManager.GetActiveScene().name == "MiniGame01";
        public bool CanStartMiniGame => State != null
            && State.KnownSystems.Contains(GameConstants.ArcadeSystemId)
            && GameState.Inventory.GetCount(State, ItemId.OldCartridge) > 0;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
            GameState = new GameState();
            HudView hudView = GetComponent<HudView>() ?? gameObject.AddComponent<HudView>();
            hudView.Configure(this);
            ConfigureAudioFeedback();
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private void Start()
        {
            ResolveSceneObjects();
            RefreshWorldViews();
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                SceneManager.sceneLoaded -= HandleSceneLoaded;
                instance = null;
            }
        }

        private void Update()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard == null || State == null)
            {
                return;
            }

            if (WasPressed(keyboard.escapeKey))
            {
                ToggleSystemMenu();
            }

            if (ShowSystemMenu)
            {
                UpdateSystemMenuShortcuts(keyboard);
                return;
            }

            if (IsMiniGameScene)
            {
                UpdateAutoArchive();
                UpdateMiniGame(keyboard);
                return;
            }

            UpdateAutoArchive();
            UpdateWorldMovement(keyboard);
            UpdateNearestNode();
            UpdateWorldShortcuts(keyboard);
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            ResolveSceneObjects();
            RefreshWorldViews();
        }

        private void ResolveSceneObjects()
        {
            worldNodes.Clear();
            miniGameStickers.Clear();
            playerController = GameObject.Find("Player")?.GetComponent<PlayerController>();
            miniGamePlayer = GameObject.Find("PixelPlayer")?.transform;
            exitDoor = GameObject.Find("ExitDoor")?.transform;
            if (SceneManager.GetActiveScene().name == "WorldHub")
            {
                worldNodes.AddRange(FindObjectsByType<WorldNodeInteractor>(FindObjectsInactive.Exclude));
                buildPlacementViews = FindObjectsByType<BuildPlacementView>(FindObjectsInactive.Exclude);
                clearingPlotViews = FindObjectsByType<ClearingPlotView>(FindObjectsInactive.Exclude);
                stickerWallView = FindAnyObjectByType<StickerWallView>();
                arcadeMachineView = FindAnyObjectByType<ArcadeMachineView>();
            }
            else
            {
                EnsureMiniGameStateForDirectPlay();
                buildPlacementViews = new BuildPlacementView[0];
                clearingPlotViews = new ClearingPlotView[0];
                stickerWallView = null;
                arcadeMachineView = null;
                for (int index = 1; index <= GameConstants.MiniGameStickerTarget; index++)
                {
                    GameObject sticker = GameObject.Find($"Sticker_{index}");
                    if (sticker != null)
                    {
                        miniGameStickers.Add(sticker);
                    }
                }
            }
        }

        private void EnsureMiniGameStateForDirectPlay()
        {
            if (State.ActiveMiniGameId != null)
            {
                return;
            }

            State.KnownSystems.Add(GameConstants.ArcadeSystemId);
            State.UnlockedMiniGames.Add(GameConstants.FirstMiniGameId);
            if (GameState.Inventory.GetCount(State, ItemId.OldCartridge) <= 0)
            {
                GameState.Inventory.Add(State, ItemId.OldCartridge, 1);
            }

            GameState.StartMiniGame(GameConstants.FirstMiniGameId);
        }

        private void UpdateWorldMovement(Keyboard keyboard)
        {
            if (playerController == null)
            {
                return;
            }

            Vector2 input = ReadPlanarInput(keyboard, !ShowInventory);
            playerController.Move(input, Time.deltaTime);
        }

        private void UpdateNearestNode()
        {
            CurrentNode = null;
            CurrentPrompt = string.Empty;
            if (playerController == null)
            {
                return;
            }

            Vector3 playerPosition = playerController.transform.position;
            float nearestDistance = float.MaxValue;
            foreach (WorldNodeInteractor node in worldNodes)
            {
                if (node == null || !node.IsInRange(playerPosition))
                {
                    continue;
                }

                float distance = Vector3.Distance(playerPosition, node.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    CurrentNode = node;
                }
            }

            if (CurrentNode != null)
            {
                if (!firstResourceGuideCompleted && IsResourceNode(CurrentNode.NodeId))
                {
                    CurrentPrompt = $"{CurrentNode.DisplayLabel}：按 E 收进背包。之后同类资源不再弹新手说明";
                    return;
                }

                if (IsBuildNodeCompleted(CurrentNode.NodeId))
                {
                    CurrentPrompt = $"{CurrentNode.DisplayLabel}：{GetCompletedBuildMessage(CurrentNode.NodeId)}";
                    return;
                }

                CurrentPrompt = $"{CurrentNode.DisplayLabel}：{GetPromptForNode(CurrentNode)}，按 E";
            }
        }

        private string GetPromptForNode(WorldNodeInteractor node)
        {
            if (IsBuildNodeCompleted(node.NodeId))
            {
                return GetCompletedBuildMessage(node.NodeId);
            }

            if (node.NodeId == "flower-bed-slot" || node.NodeId == "forest-flower-bed-slot")
            {
                int stage = WorldNodeService.GetWorldNodeStage(State, node.NodeId);
                return stage switch
                {
                    0 => "先清理落枝，会得到木材",
                    1 => "继续清理碎石，会得到石子",
                    2 => "最后清理杂草，会得到花种",
                    _ => "可以摆放花圃"
                };
            }

            if (node.NodeId == "forest-branch" && State.EquippedItemId == ItemId.Axe.ToString())
            {
                return "用斧头整理树枝，获得木材";
            }

            return node.Prompt;
        }

        private void UpdateWorldShortcuts(Keyboard keyboard)
        {
            if (WasPressed(keyboard.tKey))
            {
                SetResultAndAutosave(GameState.Time.CycleNext(State));
            }

            if (WasPressed(keyboard.iKey))
            {
                ShowInventory = !ShowInventory;
                PlayFeedback(menuClip);
            }

            if (WasPressed(keyboard.tabKey))
            {
                ShowEquipmentWheel = !ShowEquipmentWheel;
                PlayFeedback(menuClip);
            }

            if (ShowEquipmentWheel)
            {
                TryEquipByShortcut(keyboard);
                return;
            }

            if (WasPressed(keyboard.f5Key))
            {
                SetResultAndAutosave(GameState.Archives.CreateArchive(State, "manual", "手动档案"));
            }

            if (ShowInventory && WasPressed(keyboard.leftArrowKey))
            {
                MoveInventoryCategory(-1);
                return;
            }

            if (ShowInventory && WasPressed(keyboard.rightArrowKey))
            {
                MoveInventoryCategory(1);
                return;
            }

            if (ShowSignboard)
            {
                if (keyboard.leftShiftKey.isPressed || keyboard.rightShiftKey.isPressed)
                {
                    TryCommerceByShortcut(keyboard);
                }
                else
                {
                    TryExchangeByShortcut(keyboard);
                    TryCommerceByShortcut(keyboard);
                }
            }

            if (ShowArcadeMenu && WasPressed(keyboard.enterKey))
            {
                StartMiniGameFromArcade();
            }

            if (WasPressed(keyboard.eKey))
            {
                InteractWithNearestNode();
            }
        }

        private void InteractWithNearestNode()
        {
            if (CurrentNode == null)
            {
                SetMessage("附近没有可互动的东西。");
                return;
            }

            if (IsBuildNodeCompleted(CurrentNode.NodeId))
            {
                SetMessage(GetCompletedBuildMessage(CurrentNode.NodeId));
                RefreshWorldViews();
                UpdateNearestNode();
                return;
            }

            if (CurrentNode.NodeId == "home-signboard")
            {
                ShowSignboard = true;
                PlayFeedback(menuClip);
            }

            if (CurrentNode.NodeId == "clearing-arcade")
            {
                ShowArcadeMenu = true;
                PlayFeedback(menuClip);
            }

            OperationResult result = CurrentNode.Interact(GameState);
            if (result.Success && IsResourceNode(CurrentNode.NodeId))
            {
                firstResourceGuideCompleted = true;
            }

            SetResultAndAutosave(result);
            RefreshWorldViews();
            UpdateNearestNode();
        }

        private void TryExchangeByShortcut(Keyboard keyboard)
        {
            KeyControl[] keys = { keyboard.digit1Key, keyboard.digit2Key, keyboard.digit3Key, keyboard.digit4Key, keyboard.digit5Key };
            List<ExchangeRecipeAvailability> recipes = GameState.Signboard.GetMenuSnapshot(State).ExchangeRecipes.ToList();
            for (int index = 0; index < keys.Length && index < recipes.Count; index++)
            {
                if (WasPressed(keys[index]))
                {
                    SetResultAndAutosave(GameState.Signboard.Exchange(State, recipes[index].Recipe.Id));
                    RefreshWorldViews();
                    return;
                }
            }
        }

        private void TryCommerceByShortcut(Keyboard keyboard)
        {
            bool shiftPressed = keyboard.leftShiftKey.isPressed || keyboard.rightShiftKey.isPressed;
            KeyControl[] sellKeys = { keyboard.digit1Key, keyboard.digit2Key, keyboard.digit3Key, keyboard.digit4Key };
            string[] sellIds = { "sell-wood", "sell-stone", "sell-river-shell", "sell-fish" };
            if (shiftPressed)
            {
                for (int index = 0; index < sellKeys.Length; index++)
                {
                    if (WasPressed(sellKeys[index]))
                    {
                        SetResultAndAutosave(GameState.Signboard.Sell(State, sellIds[index]));
                        RefreshWorldViews();
                        return;
                    }
                }
            }

            KeyControl[] buyKeys = { keyboard.digit6Key, keyboard.digit7Key, keyboard.digit8Key, keyboard.digit9Key };
            string[] buyIds = { "buy-wood", "buy-stone", "buy-flower-seed", "buy-axe" };
            for (int index = 0; index < buyKeys.Length; index++)
            {
                if (WasPressed(buyKeys[index]))
                {
                    SetResultAndAutosave(GameState.Signboard.Buy(State, buyIds[index]));
                    RefreshWorldViews();
                    return;
                }
            }
        }

        private void TryEquipByShortcut(Keyboard keyboard)
        {
            if (WasPressed(keyboard.backspaceKey))
            {
                State.EquippedItemId = null;
                ShowEquipmentWheel = false;
                SetResultAndAutosave(OperationResult.Ok("已收起装备。"));
                return;
            }

            KeyControl[] equipKeys =
            {
                keyboard.digit1Key,
                keyboard.digit2Key,
                keyboard.digit3Key,
                keyboard.digit4Key,
                keyboard.digit5Key,
                keyboard.digit6Key,
                keyboard.digit7Key,
                keyboard.digit8Key,
                keyboard.digit9Key
            };

            for (int index = 0; index < equipKeys.Length && index < EquipmentRingItems.Length; index++)
            {
                if (!WasPressed(equipKeys[index]))
                {
                    continue;
                }

                EquipFromInventory(EquipmentRingItems[index]);
                return;
            }
        }

        private void EquipFromInventory(ItemId itemId)
        {
            if (!CanEquip(itemId))
            {
                SetResult(OperationResult.Fail("这个物品还不能装备。"));
                return;
            }

            if (GameState.Inventory.GetCount(State, itemId) <= 0)
            {
                SetResult(OperationResult.Fail($"{GetItemDisplayName(itemId)}还没有获得。先收集或在木牌购买。"));
                return;
            }

            State.EquippedItemId = itemId.ToString();
            ShowEquipmentWheel = false;
            SetResultAndAutosave(OperationResult.Ok($"已装备{GetItemDisplayName(itemId)}。"));
        }

        private void StartMiniGameFromArcade()
        {
            OperationResult result = GameState.StartMiniGame(GameConstants.FirstMiniGameId);
            SetResultAndAutosave(result);
            if (result.Success)
            {
                ShowArcadeMenu = false;
                ShowSignboard = false;
                SceneManager.LoadScene("MiniGame01", LoadSceneMode.Single);
            }
        }

        private void UpdateMiniGame(Keyboard keyboard)
        {
            if (miniGamePlayer != null)
            {
                Vector2 input = ReadPlanarInput(keyboard);
                miniGamePlayer.position += new Vector3(input.x, input.y, 0f) * MiniGameMoveSpeed * Time.deltaTime;
            }

            CurrentPrompt = "收齐 3 个贴纸后，靠近出口按 E 回到木屋。";
            if (WasPressed(keyboard.eKey))
            {
                TryMiniGameInteraction();
            }
        }

        private void TryMiniGameInteraction()
        {
            if (miniGamePlayer == null)
            {
                return;
            }

            foreach (GameObject sticker in miniGameStickers)
            {
                if (sticker == null || !sticker.activeSelf)
                {
                    continue;
                }

                if (Vector3.Distance(miniGamePlayer.position, sticker.transform.position) <= 0.7f)
                {
                    OperationResult result = GameState.MiniGames.CollectSticker(State);
                    SetResultAndAutosave(result);
                    if (result.Success)
                    {
                        sticker.SetActive(false);
                    }

                    return;
                }
            }

            if (exitDoor != null && Vector3.Distance(miniGamePlayer.position, exitDoor.position) <= 0.9f)
            {
                if (!GameState.MiniGames.CanExit(State))
                {
                    SetMessage("贴纸还没有收齐，出口还在等光。");
                    return;
                }

                OperationResult result = GameState.FinishMiniGame(new MiniGameResult(GameConstants.FirstMiniGameId, true, GameConstants.MiniGameStickerTarget));
                SetResultAndAutosave(result);
                if (result.Success)
                {
                    SceneManager.LoadScene("WorldHub", LoadSceneMode.Single);
                }
            }
        }

        private void RefreshWorldViews()
        {
            foreach (BuildPlacementView view in buildPlacementViews)
            {
                view.Refresh(State);
            }

            foreach (ClearingPlotView view in clearingPlotViews)
            {
                view.Refresh(State);
            }

            bool bridgeBuilt = buildPlacementViews.Any(view => view.BlueprintId == BlueprintId.Bridge && view.IsBuilt(State));
            playerController?.SetBridgeRepaired(bridgeBuilt);
            stickerWallView?.Refresh(State);
            arcadeMachineView?.Refresh(State, GameState.Inventory);
        }

        private void ToggleSystemMenu()
        {
            if (ShowSystemMenu)
            {
                ShowSystemMenu = false;
                PlayFeedback(menuClip);
                return;
            }

            ShowSignboard = false;
            ShowArcadeMenu = false;
            ShowInventory = false;
            ShowEquipmentWheel = false;
            ShowSystemMenu = true;
            SaveMenuSlotIndex = 0;
            PlayFeedback(menuClip);
        }

        private void UpdateSystemMenuShortcuts(Keyboard keyboard)
        {
            IReadOnlyList<SaveSlotSnapshot> slots = GameState.SaveSlots.GetSlots();
            if (slots.Count == 0)
            {
                return;
            }

            SaveMenuSlotIndex = Mathf.Clamp(SaveMenuSlotIndex, 0, slots.Count - 1);
            if (WasPressed(keyboard.upArrowKey))
            {
                SaveMenuSlotIndex = (SaveMenuSlotIndex - 1 + slots.Count) % slots.Count;
                PlayFeedback(menuClip);
                return;
            }

            if (WasPressed(keyboard.downArrowKey))
            {
                SaveMenuSlotIndex = (SaveMenuSlotIndex + 1) % slots.Count;
                PlayFeedback(menuClip);
                return;
            }

            SaveSlotSnapshot selectedSlot = slots[SaveMenuSlotIndex];
            if (WasPressed(keyboard.sKey))
            {
                SaveSelectedManualSlot(selectedSlot);
                return;
            }

            if (WasPressed(keyboard.lKey))
            {
                LoadSelectedSlot(selectedSlot);
                return;
            }

            if (WasPressed(keyboard.deleteKey) || WasPressed(keyboard.backspaceKey))
            {
                DeleteSelectedSlot(selectedSlot);
                return;
            }

            if (WasPressed(keyboard.qKey))
            {
                OperationResult saveResult = GameState.SaveSlots.SaveAuto(State);
                SetResult(saveResult.Success ? OperationResult.Ok("已保存并退出。") : saveResult);
                if (saveResult.Success)
                {
                    Application.Quit();
                }
            }
        }

        private void SaveSelectedManualSlot(SaveSlotSnapshot selectedSlot)
        {
            if (selectedSlot.IsAuto)
            {
                SetResult(OperationResult.Fail("自动保存由系统维护，请选择手动存档槽。"));
                return;
            }

            if (!SaveSlotService.TryGetManualSlot(selectedSlot.SlotId, out int manualSlot))
            {
                SetResult(OperationResult.Fail("存档槽不存在。"));
                return;
            }

            SetResult(GameState.SaveSlots.SaveManual(State, manualSlot));
        }

        private void LoadSelectedSlot(SaveSlotSnapshot selectedSlot)
        {
            SaveLoadResult loadResult = GameState.SaveSlots.Load(selectedSlot.SlotId);
            if (!loadResult.Success)
            {
                SetResult(OperationResult.Fail(loadResult.Message));
                return;
            }

            GameState.ReplacePlayer(loadResult.State);
            ShowSystemMenu = false;
            autoArchiveTimer = 0f;
            SetResult(OperationResult.Ok(loadResult.Message));
            RefreshWorldViews();
            UpdateNearestNode();
        }

        private void DeleteSelectedSlot(SaveSlotSnapshot selectedSlot)
        {
            SetResult(GameState.SaveSlots.Delete(selectedSlot.SlotId));
        }

        private void SetResultAndAutosave(OperationResult result)
        {
            SetResult(result);
            if (result.Success)
            {
                AutosaveProgress();
            }
        }

        private void SetResult(OperationResult result)
        {
            SetMessage(result.Message);
            PlayFeedback(result.Success ? successClip : failClip);
        }

        private void AutosaveProgress()
        {
            OperationResult saveResult = GameState.SaveSlots.SaveAuto(State);
            if (!saveResult.Success)
            {
                SetMessage($"{LastMessage}（自动保存失败：{saveResult.Message}）");
            }
        }

        private void SetMessage(string message)
        {
            LastMessage = message;
        }

        private void UpdateAutoArchive()
        {
            autoArchiveTimer += Time.deltaTime;
            if (autoArchiveTimer < AutoArchiveIntervalSeconds)
            {
                return;
            }

            autoArchiveTimer = 0f;
            GameState.Archives.CreateArchive(State, "auto", "10 分钟自动档案");
            AutosaveProgress();
        }

        private void MoveInventoryCategory(int direction)
        {
            InventoryCategoryIndex = (InventoryCategoryIndex + direction + HudView.InventoryCategoryCount) % HudView.InventoryCategoryCount;
            PlayFeedback(menuClip);
        }

        private void ConfigureAudioFeedback()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.volume = 0.18f;
            successClip = CreateToneClip("Feedback_Success", 740f, 0.08f, 0.13f);
            failClip = CreateToneClip("Feedback_Fail", 220f, 0.12f, 0.1f);
            menuClip = CreateToneClip("Feedback_Menu", 520f, 0.06f, 0.09f);
        }

        private void PlayFeedback(AudioClip clip)
        {
            if (audioSource != null && clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
        }

        private static AudioClip CreateToneClip(string name, float frequency, float durationSeconds, float amplitude)
        {
            const int sampleRate = 44100;
            int sampleCount = Mathf.Max(1, Mathf.RoundToInt(sampleRate * durationSeconds));
            float[] samples = new float[sampleCount];
            for (int index = 0; index < sampleCount; index++)
            {
                float t = index / (float)sampleRate;
                float fade = 1f - (index / (float)sampleCount);
                samples[index] = Mathf.Sin(2f * Mathf.PI * frequency * t) * amplitude * fade;
            }

            AudioClip clip = AudioClip.Create(name, sampleCount, 1, sampleRate, false);
            clip.SetData(samples, 0);
            return clip;
        }

        private static bool IsResourceNode(string nodeId)
        {
            return nodeId == "forest-branch"
                || nodeId == "home-fallen-branch"
                || nodeId == "forest-flower-seed"
                || nodeId == "home-grass-flower-seed"
                || nodeId == "river-stone"
                || nodeId == "river-shell"
                || nodeId == "shallow-river-shell"
                || nodeId == "river-fish"
                || nodeId == "shallow-fish";
        }

        public bool CanEquip(ItemId itemId)
        {
            return System.Array.IndexOf(EquipmentRingItems, itemId) >= 0;
        }

        public bool IsEquipped(ItemId itemId)
        {
            return State != null && State.EquippedItemId == itemId.ToString();
        }

        private bool IsBuildNodeCompleted(string nodeId)
        {
            return !string.IsNullOrEmpty(nodeId)
                && buildPlacementViews.Any(view => view != null && view.NodeId == nodeId && view.IsBuilt(State));
        }

        private static string GetCompletedBuildMessage(string nodeId)
        {
            return nodeId switch
            {
                "river-bridge" => "木桥已经修好，可以从桥面过河",
                "flower-bed-slot" => "花圃已经建好，地块状态已保存",
                "forest-flower-bed-slot" => "森林花圃已经建好，地块状态已保存",
                _ => "这里已经完成建设"
            };
        }

        private static string GetItemDisplayName(ItemId itemId)
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

        private static Vector2 ReadPlanarInput(Keyboard keyboard, bool includeArrowKeys = true)
        {
            Vector2 input = Vector2.zero;
            if (keyboard.aKey.isPressed || (includeArrowKeys && keyboard.leftArrowKey.isPressed))
            {
                input.x -= 1f;
            }

            if (keyboard.dKey.isPressed || (includeArrowKeys && keyboard.rightArrowKey.isPressed))
            {
                input.x += 1f;
            }

            if (keyboard.sKey.isPressed || (includeArrowKeys && keyboard.downArrowKey.isPressed))
            {
                input.y -= 1f;
            }

            if (keyboard.wKey.isPressed || (includeArrowKeys && keyboard.upArrowKey.isPressed))
            {
                input.y += 1f;
            }

            return Vector2.ClampMagnitude(input, 1f);
        }

        private static bool WasPressed(ButtonControl control)
        {
            return control != null && control.wasPressedThisFrame;
        }
    }
}
