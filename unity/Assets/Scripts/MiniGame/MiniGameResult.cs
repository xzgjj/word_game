namespace StarryForest.MiniGame
{
    public sealed class MiniGameResult
    {
        public MiniGameResult(string miniGameId, bool success, int stickersCollected)
        {
            MiniGameId = miniGameId;
            Success = success;
            StickersCollected = stickersCollected;
        }

        public string MiniGameId { get; }
        public bool Success { get; }
        public int StickersCollected { get; }
    }
}
