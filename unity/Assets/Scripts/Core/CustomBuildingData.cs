namespace StarryForest.Core
{
    public enum CustomBuildingShape
    {
        TinyCabin,
        Stall,
        LampFrame,
        Sign,
        FlowerRack
    }

    public enum CustomBuildingTheme
    {
        Wood,
        Stone,
        Flower,
        RiverShell,
        Star
    }

    public enum CustomBuildingSize
    {
        OneByOne,
        TwoByTwo
    }

    public sealed class CustomBuildingData
    {
        public CustomBuildingData(CustomBuildingShape shape, CustomBuildingTheme theme, CustomBuildingSize size)
        {
            Shape = shape;
            Theme = theme;
            Size = size;
        }

        public CustomBuildingShape Shape { get; }
        public CustomBuildingTheme Theme { get; }
        public CustomBuildingSize Size { get; }
    }
}
