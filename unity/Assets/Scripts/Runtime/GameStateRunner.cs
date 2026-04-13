using System.Collections.Generic;
using System.Linq;
using StarryForest.Core;
using StarryForest.Inventory;
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
    public enum SystemMenuPanel
    {
        Root,
        Save,
        Settings,
        ExitConfirm
    }

    public sealed class GameStateRunner : MonoBehaviour
    {
        private const float MiniGameMoveSpeed = 4.6f;
        private const float AutoArchiveIntervalSeconds = 600f;
        private const float InteractionFeedbackDurationSeconds = 2.2f;
        private const string ItemGuideSystemPrefix = "item-guide-";
        private static readonly ItemId[] EquipmentRingItems =
        {
            ItemId.Axe,
            ItemId.OldCartridge,
            ItemId.Sticker
        };

        private static GameStateRunner instance;

        private readonly List<WorldNodeInteractor> worldNodes = new List<WorldNodeInteractor>();
        private readonly List<GameObject> miniGameStickers = new List<GameObject>();
        private readonly Dictionary<WorldNodeInteractor, float> resourceRespawnTimers = new Dictionary<WorldNodeInteractor, float>();
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
        private bool feedbackAudioEnabled = true;
        private float autoArchiveTimer;
        private float interactionFeedbackTimer;

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
        public SystemMenuPanel ActiveSystemMenuPanel { get; private set; }
        public int InventoryCategoryIndex { get; private set; }
        public int SystemMenuIndex { get; private set; }
        public int SaveMenuSlotIndex { get; private set; }
        public int SettingsMenuIndex { get; private set; }
        public int ExitConfirmIndex { get; private set; }
        public bool FeedbackAudioEnabled => feedbackAudioEnabled;
        public string InteractionFeedbackTitle { get; private set; }
        public string InteractionFeedbackDetail { get; private set; }
        public bool ShowInteractionFeedback => interactionFeedbackTimer > 0f;
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
            MarkKnownItemGuidesFromInventory();
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

            UpdateInteractionFeedback();
            UpdateResourceRespawns();

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
            resourceRespawnTimers.Clear();
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
                MarkKnownItemGuidesFromInventory();
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
                if (TryGetGuideItemForNode(CurrentNode.NodeId, out ItemId guideItem)
                    && ShouldShowFirstItemGuide(guideItem))
                {
                    CurrentPrompt = $"{CurrentNode.DisplayLabel}：第一次发现{GetItemDisplayName(guideItem)}，按 E 或鼠标左键收进背包。之后同类物品只保留短提示";
                    return;
                }

                if (IsBuildNodeCompleted(CurrentNode.NodeId))
                {
                    CurrentPrompt = $"{CurrentNode.DisplayLabel}：{GetCompletedBuildMessage(CurrentNode.NodeId)}";
                    return;
                }

                CurrentPrompt = $"{CurrentNode.DisplayLabel}：{GetPromptForNode(CurrentNode)}，按 E 或鼠标左键";
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
                if (WasPressed(keyboard.qKey))
                {
                    CloseContextPanel("木牌记录已关闭。");
                    return;
                }

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

            if (ShowArcadeMenu && WasPressed(keyboard.qKey))
            {
                CloseContextPanel("游戏机菜单已关闭。");
                return;
            }

            if (WasPressed(keyboard.eKey) || WasMousePrimaryPressed())
            {
                InteractWithNearestNode();
            }
        }

        private void InteractWithNearestNode()
        {
            if (CurrentNode == null)
            {
                SetMessage("附近没有可互动的东西。");
                ShowInteractionFeedbackPanel("没有可互动目标", "再靠近一点，等待提示出现后再点击或按 E。", false);
                return;
            }

            if (IsBuildNodeCompleted(CurrentNode.NodeId))
            {
                SetMessage(GetCompletedBuildMessage(CurrentNode.NodeId));
                ShowInteractionFeedbackPanel(CurrentNode.DisplayLabel, GetCompletedBuildMessage(CurrentNode.NodeId), true);
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

            ShowInteractionProgressPanel(CurrentNode.DisplayLabel, GetInteractionStartMessage(CurrentNode.NodeId));
            OperationResult result = CurrentNode.Interact(GameState);
            SetResultAndAutosave(result, false);
            ShowInteractionFeedbackForResult(CurrentNode, result);
            if (result.Success)
            {
                ScheduleResourceRespawn(CurrentNode);
            }

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

            IReadOnlyList<ItemId> equipmentItems = GetVisibleEquipmentItems();
            for (int index = 0; index < equipKeys.Length && index < equipmentItems.Count; index++)
            {
                if (!WasPressed(equipKeys[index]))
                {
                    continue;
                }

                EquipFromInventory(equipmentItems[index]);
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

        private void CloseContextPanel(string message)
        {
            ShowSignboard = false;
            ShowArcadeMenu = false;
            SetMessage(message);
            PlayFeedback(menuClip);
        }

        private void UpdateMiniGame(Keyboard keyboard)
        {
            if (miniGamePlayer != null)
            {
                Vector2 input = ReadPlanarInput(keyboard);
                miniGamePlayer.position += new Vector3(input.x, input.y, 0f) * MiniGameMoveSpeed * Time.deltaTime;
            }

            CurrentPrompt = "收齐 3 个贴纸后，靠近出口按 E 回到木屋。";
            if (WasPressed(keyboard.eKey) || WasMousePrimaryPressed())
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
                if (ActiveSystemMenuPanel == SystemMenuPanel.Root)
                {
                    ShowSystemMenu = false;
                }
                else
                {
                    ActiveSystemMenuPanel = SystemMenuPanel.Root;
                }

                PlayFeedback(menuClip);
                return;
            }

            ShowSignboard = false;
            ShowArcadeMenu = false;
            ShowInventory = false;
            ShowEquipmentWheel = false;
            ShowSystemMenu = true;
            ActiveSystemMenuPanel = SystemMenuPanel.Root;
            SystemMenuIndex = 0;
            SaveMenuSlotIndex = 0;
            SettingsMenuIndex = 0;
            ExitConfirmIndex = 0;
            PlayFeedback(menuClip);
        }

        private void UpdateSystemMenuShortcuts(Keyboard keyboard)
        {
            switch (ActiveSystemMenuPanel)
            {
                case SystemMenuPanel.Save:
                    UpdateSaveMenuShortcuts(keyboard);
                    break;
                case SystemMenuPanel.Settings:
                    UpdateSettingsMenuShortcuts(keyboard);
                    break;
                case SystemMenuPanel.ExitConfirm:
                    UpdateExitConfirmShortcuts(keyboard);
                    break;
                default:
                    UpdateRootSystemMenuShortcuts(keyboard);
                    break;
            }
        }

        private void UpdateRootSystemMenuShortcuts(Keyboard keyboard)
        {
            const int itemCount = 3;
            if (WasPressed(keyboard.upArrowKey))
            {
                SystemMenuIndex = (SystemMenuIndex - 1 + itemCount) % itemCount;
                PlayFeedback(menuClip);
                return;
            }

            if (WasPressed(keyboard.downArrowKey))
            {
                SystemMenuIndex = (SystemMenuIndex + 1) % itemCount;
                PlayFeedback(menuClip);
                return;
            }

            if (WasPressed(keyboard.enterKey) || WasMousePrimaryPressed())
            {
                switch (SystemMenuIndex)
                {
                    case 0:
                        ActiveSystemMenuPanel = SystemMenuPanel.Save;
                        SaveMenuSlotIndex = 0;
                        break;
                    case 1:
                        ActiveSystemMenuPanel = SystemMenuPanel.Settings;
                        SettingsMenuIndex = 0;
                        break;
                    default:
                        ActiveSystemMenuPanel = SystemMenuPanel.ExitConfirm;
                        ExitConfirmIndex = 0;
                        break;
                }

                PlayFeedback(menuClip);
            }
        }

        private void UpdateSaveMenuShortcuts(Keyboard keyboard)
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
                ActiveSystemMenuPanel = SystemMenuPanel.Root;
                PlayFeedback(menuClip);
            }
        }

        private void UpdateSettingsMenuShortcuts(Keyboard keyboard)
        {
            const int itemCount = 2;
            if (WasPressed(keyboard.upArrowKey))
            {
                SettingsMenuIndex = (SettingsMenuIndex - 1 + itemCount) % itemCount;
                PlayFeedback(menuClip);
                return;
            }

            if (WasPressed(keyboard.downArrowKey))
            {
                SettingsMenuIndex = (SettingsMenuIndex + 1) % itemCount;
                PlayFeedback(menuClip);
                return;
            }

            if (WasPressed(keyboard.enterKey) || WasMousePrimaryPressed())
            {
                if (SettingsMenuIndex == 0)
                {
                    feedbackAudioEnabled = !feedbackAudioEnabled;
                    SetMessage(feedbackAudioEnabled ? "反馈音效已开启。" : "反馈音效已关闭。");
                    PlayFeedback(menuClip);
                    return;
                }

                ActiveSystemMenuPanel = SystemMenuPanel.Root;
                PlayFeedback(menuClip);
            }
        }

        private void UpdateExitConfirmShortcuts(Keyboard keyboard)
        {
            const int itemCount = 2;
            if (WasPressed(keyboard.leftArrowKey) || WasPressed(keyboard.upArrowKey))
            {
                ExitConfirmIndex = (ExitConfirmIndex - 1 + itemCount) % itemCount;
                PlayFeedback(menuClip);
                return;
            }

            if (WasPressed(keyboard.rightArrowKey) || WasPressed(keyboard.downArrowKey))
            {
                ExitConfirmIndex = (ExitConfirmIndex + 1) % itemCount;
                PlayFeedback(menuClip);
                return;
            }

            if (WasPressed(keyboard.enterKey) || WasMousePrimaryPressed())
            {
                if (ExitConfirmIndex == 0)
                {
                    ActiveSystemMenuPanel = SystemMenuPanel.Root;
                    PlayFeedback(menuClip);
                    return;
                }

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
            MarkKnownItemGuidesFromInventory();
            SetResult(OperationResult.Ok(loadResult.Message));
            RefreshWorldViews();
            UpdateNearestNode();
        }

        private void DeleteSelectedSlot(SaveSlotSnapshot selectedSlot)
        {
            SetResult(GameState.SaveSlots.Delete(selectedSlot.SlotId));
        }

        private void SetResultAndAutosave(OperationResult result, bool playAudio = true)
        {
            SetResult(result, playAudio);
            if (result.Success)
            {
                AutosaveProgress();
            }
        }

        private void SetResult(OperationResult result, bool playAudio = true)
        {
            SetMessage(result.Message);
            if (playAudio)
            {
                PlayFeedback(result.Success ? successClip : failClip);
            }

            if (result.Success)
            {
                MarkKnownItemGuidesFromInventory();
            }
        }

        private void ShowInteractionFeedbackForResult(WorldNodeInteractor node, OperationResult result)
        {
            if (node == null)
            {
                ShowInteractionFeedbackPanel(result.Success ? "完成" : "还差一步", result.Message, result.Success);
                return;
            }

            string title = result.Success ? GetInteractionSuccessTitle(node.NodeId, result.Message) : GetInteractionFailureTitle(node.NodeId);
            ShowInteractionFeedbackPanel(title, result.Message, result.Success);
        }

        private void ShowInteractionFeedbackPanel(string title, string detail, bool success)
        {
            InteractionFeedbackTitle = title;
            InteractionFeedbackDetail = detail;
            interactionFeedbackTimer = InteractionFeedbackDurationSeconds;
            PlayFeedback(success ? successClip : failClip);
        }

        private void ShowInteractionProgressPanel(string title, string detail)
        {
            InteractionFeedbackTitle = title;
            InteractionFeedbackDetail = detail;
            interactionFeedbackTimer = InteractionFeedbackDurationSeconds;
            PlayFeedback(menuClip);
        }

        private void UpdateInteractionFeedback()
        {
            if (interactionFeedbackTimer <= 0f)
            {
                return;
            }

            interactionFeedbackTimer = Mathf.Max(0f, interactionFeedbackTimer - Time.deltaTime);
        }

        private void ScheduleResourceRespawn(WorldNodeInteractor node)
        {
            if (node == null || !TryGetResourceRespawnSeconds(node.NodeId, out float respawnSeconds))
            {
                return;
            }

            resourceRespawnTimers[node] = respawnSeconds;
            node.gameObject.SetActive(false);
        }

        private void UpdateResourceRespawns()
        {
            if (resourceRespawnTimers.Count == 0)
            {
                return;
            }

            foreach (KeyValuePair<WorldNodeInteractor, float> entry in resourceRespawnTimers.ToList())
            {
                WorldNodeInteractor node = entry.Key;
                if (node == null)
                {
                    resourceRespawnTimers.Remove(node);
                    continue;
                }

                float remainingSeconds = entry.Value - Time.deltaTime;
                if (remainingSeconds <= 0f)
                {
                    node.gameObject.SetActive(true);
                    resourceRespawnTimers.Remove(node);
                }
                else
                {
                    resourceRespawnTimers[node] = remainingSeconds;
                }
            }
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
            if (feedbackAudioEnabled && audioSource != null && clip != null)
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

        private static string GetInteractionStartMessage(string nodeId)
        {
            if (nodeId == "river-bridge")
            {
                return "开始检查桥桩和木板。";
            }

            if (nodeId == "flower-bed-slot" || nodeId == "forest-flower-bed-slot")
            {
                return "开始整理这块地。";
            }

            if (nodeId == "home-signboard")
            {
                return "打开木牌记录。";
            }

            if (nodeId == "clearing-arcade")
            {
                return "游戏机屏幕亮起来了。";
            }

            return "开始互动。";
        }

        private static string GetInteractionSuccessTitle(string nodeId, string message)
        {
            if (nodeId == "river-bridge")
            {
                if (!string.IsNullOrEmpty(message) && message.Contains("发现了断桥"))
                {
                    return "发现断桥";
                }

                return "桥梁修复完成";
            }

            if (nodeId == "flower-bed-slot" || nodeId == "forest-flower-bed-slot")
            {
                return "地块状态更新";
            }

            if (IsResourceNode(nodeId))
            {
                return "发现物品";
            }

            return "互动完成";
        }

        private static string GetInteractionFailureTitle(string nodeId)
        {
            if (nodeId == "river-bridge")
            {
                return "桥梁还不能修复";
            }

            if (nodeId == "flower-bed-slot" || nodeId == "forest-flower-bed-slot")
            {
                return "地块还不能建设";
            }

            return "还差一步";
        }

        public bool CanEquip(ItemId itemId)
        {
            return System.Array.IndexOf(EquipmentRingItems, itemId) >= 0;
        }

        public IReadOnlyList<ItemId> GetVisibleEquipmentItems()
        {
            List<ItemId> visibleItems = new List<ItemId>();
            if (State == null || GameState == null)
            {
                return visibleItems;
            }

            foreach (ItemId itemId in EquipmentRingItems)
            {
                if (GameState.Inventory.GetCount(State, itemId) > 0)
                {
                    visibleItems.Add(itemId);
                }
            }

            return visibleItems;
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

        private bool ShouldShowFirstItemGuide(ItemId itemId)
        {
            if (State == null || GameState == null)
            {
                return false;
            }

            if (State.KnownSystems.Contains(GetItemGuideSystemId(itemId)))
            {
                return false;
            }

            if (GameState.Inventory.GetCount(State, itemId) > 0)
            {
                State.KnownSystems.Add(GetItemGuideSystemId(itemId));
                return false;
            }

            return true;
        }

        private void MarkKnownItemGuidesFromInventory()
        {
            if (State == null || GameState == null)
            {
                return;
            }

            foreach (ItemId itemId in ItemCatalog.Items)
            {
                if (GameState.Inventory.GetCount(State, itemId) > 0)
                {
                    State.KnownSystems.Add(GetItemGuideSystemId(itemId));
                }
            }
        }

        public static bool TryGetGuideItemForNode(string nodeId, out ItemId itemId)
        {
            switch (nodeId)
            {
                case "forest-branch":
                case "home-fallen-branch":
                    itemId = ItemId.Wood;
                    return true;
                case "forest-flower-seed":
                case "home-grass-flower-seed":
                    itemId = ItemId.FlowerSeed;
                    return true;
                case "river-stone":
                    itemId = ItemId.Stone;
                    return true;
                case "river-shell":
                case "shallow-river-shell":
                    itemId = ItemId.RiverShell;
                    return true;
                case "river-fish":
                case "shallow-fish":
                    itemId = ItemId.Fish;
                    return true;
                default:
                    itemId = default;
                    return false;
            }
        }

        public static bool TryGetResourceRespawnSeconds(string nodeId, out float seconds)
        {
            if (!TryGetGuideItemForNode(nodeId, out ItemId itemId))
            {
                seconds = 0f;
                return false;
            }

            seconds = GetResourceRespawnSeconds(itemId);
            return true;
        }

        private static float GetResourceRespawnSeconds(ItemId itemId)
        {
            return itemId switch
            {
                ItemId.Fish => 30f,
                ItemId.Wood => 45f,
                ItemId.FlowerSeed => 60f,
                ItemId.RiverShell => 75f,
                ItemId.Stone => 90f,
                _ => 60f
            };
        }

        private static string GetItemGuideSystemId(ItemId itemId)
        {
            return $"{ItemGuideSystemPrefix}{itemId}";
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

        private static bool WasMousePrimaryPressed()
        {
            return Mouse.current?.leftButton.wasPressedThisFrame == true;
        }
    }
}
