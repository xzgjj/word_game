namespace StarryForest.Core
{
    public sealed class PlacedBuilding
    {
        public PlacedBuilding(string id, BlueprintId blueprintId, int gridX, int gridY, int rotation)
        {
            Id = id;
            BlueprintId = blueprintId;
            GridX = gridX;
            GridY = gridY;
            Rotation = rotation;
        }

        public string Id { get; }
        public BlueprintId BlueprintId { get; }
        public int GridX { get; }
        public int GridY { get; }
        public int Rotation { get; }
        public CustomBuildingData CustomData { get; set; }
    }
}
