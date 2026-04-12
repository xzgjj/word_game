using StarryForest.Inventory;
using StarryForest.MiniGame;
using StarryForest.Save;
using StarryForest.World;
using StarryForest.World.Gathering;
using StarryForest.World.Nodes;

namespace StarryForest.Core
{
    public sealed class GameState
    {
        public GameState()
            : this(new PlayerState(ItemCatalog.InventorySlotCount))
        {
        }

        public GameState(PlayerState playerState)
        {
            Player = playerState;
            Inventory = new InventoryService();
            Blueprints = new Building.BlueprintService();
            Signboard = new Signboard.SignboardService(Inventory, Blueprints);
            Builder = new Building.BuildService(Inventory, Blueprints);
            CustomBuilder = new Building.CustomBuildService(Inventory);
            Gather = new GatherService(Inventory);
            Fishing = new FishingService(Inventory);
            MiniGames = new MiniGameService(Inventory, Blueprints);
            WorldNodes = new WorldNodeService(Gather, Fishing, MiniGames, Signboard, Builder);
            Time = new TimeService();
            Save = new SaveService();
        }

        public PlayerState Player { get; private set; }
        public InventoryService Inventory { get; }
        public Building.BlueprintService Blueprints { get; }
        public Signboard.SignboardService Signboard { get; }
        public Building.BuildService Builder { get; }
        public Building.CustomBuildService CustomBuilder { get; }
        public GatherService Gather { get; }
        public FishingService Fishing { get; }
        public MiniGameService MiniGames { get; }
        public WorldNodeService WorldNodes { get; }
        public TimeService Time { get; }
        public SaveService Save { get; }

        public OperationResult InteractWithWorldNode(string nodeId)
        {
            return WorldNodes.Interact(Player, nodeId);
        }

        public OperationResult StartMiniGame(string miniGameId)
        {
            return MiniGames.Start(Player, miniGameId);
        }

        public OperationResult FinishMiniGame(MiniGameResult result)
        {
            return MiniGames.Finish(Player, result);
        }

        public SaveLoadResult LoadFromPath(string filePath)
        {
            SaveLoadResult result = Save.Load(filePath);
            if (result.Success)
            {
                Player = result.State;
            }

            return result;
        }
    }
}
