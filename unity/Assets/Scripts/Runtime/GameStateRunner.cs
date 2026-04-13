using System.Collections.Generic;
using System.Linq;
using StarryForest.Core;
using StarryForest.MiniGame;
using StarryForest.Player;
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

        public GameState GameState { get; private set; }
        public PlayerState State => GameState?.Player;
        public WorldNodeInteractor CurrentNode { get; private set; }
        public string CurrentPrompt { get; private set; }
        public string LastMessage { get; private set; }
        public bool ShowSignboard { get; private set; }
        public bool ShowArcadeMenu { get; private set; }
        public bool ShowInventory { get; private set; }
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
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private void Start()
        {
            ResolveSceneObjects();
            RefreshWorldViews();
            SetMessage("从木屋出发。先靠近木牌打开图纸，再去森林和河岸收材料。");
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
                ShowSignboard = false;
                ShowArcadeMenu = false;
            }

            if (IsMiniGameScene)
            {
                UpdateMiniGame(keyboard);
                return;
            }

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

            Vector2 input = ReadPlanarInput(keyboard);
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
                CurrentPrompt = $"{CurrentNode.DisplayLabel}：{GetPromptForNode(CurrentNode)}，按 E";
            }
        }

        private string GetPromptForNode(WorldNodeInteractor node)
        {
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

            return node.Prompt;
        }

        private void UpdateWorldShortcuts(Keyboard keyboard)
        {
            if (WasPressed(keyboard.tKey))
            {
                SetResult(GameState.Time.CycleNext(State));
            }

            if (WasPressed(keyboard.iKey))
            {
                ShowInventory = !ShowInventory;
            }

            if (ShowSignboard)
            {
                TryExchangeByShortcut(keyboard);
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

            if (CurrentNode.NodeId == "home-signboard")
            {
                ShowSignboard = true;
            }

            if (CurrentNode.NodeId == "clearing-arcade")
            {
                ShowArcadeMenu = true;
            }

            OperationResult result = CurrentNode.Interact(GameState);
            SetResult(result);
            RefreshWorldViews();
        }

        private void TryExchangeByShortcut(Keyboard keyboard)
        {
            KeyControl[] keys = { keyboard.digit1Key, keyboard.digit2Key, keyboard.digit3Key, keyboard.digit4Key };
            List<ExchangeRecipeAvailability> recipes = GameState.Signboard.GetMenuSnapshot(State).ExchangeRecipes.ToList();
            for (int index = 0; index < keys.Length && index < recipes.Count; index++)
            {
                if (WasPressed(keys[index]))
                {
                    SetResult(GameState.Signboard.Exchange(State, recipes[index].Recipe.Id));
                    RefreshWorldViews();
                    return;
                }
            }
        }

        private void StartMiniGameFromArcade()
        {
            OperationResult result = GameState.StartMiniGame(GameConstants.FirstMiniGameId);
            SetResult(result);
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
                    SetResult(GameState.MiniGames.CollectSticker(State));
                    sticker.SetActive(false);
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
                SetResult(result);
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

        private void SetResult(OperationResult result)
        {
            SetMessage(result.Message);
        }

        private void SetMessage(string message)
        {
            LastMessage = message;
        }

        private static Vector2 ReadPlanarInput(Keyboard keyboard)
        {
            Vector2 input = Vector2.zero;
            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
            {
                input.x -= 1f;
            }

            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
            {
                input.x += 1f;
            }

            if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
            {
                input.y -= 1f;
            }

            if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
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
