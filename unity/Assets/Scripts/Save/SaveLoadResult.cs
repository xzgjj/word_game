using StarryForest.Core;

namespace StarryForest.Save
{
    public sealed class SaveLoadResult
    {
        private SaveLoadResult(bool success, string message, PlayerState state)
        {
            Success = success;
            Message = message;
            State = state;
        }

        public bool Success { get; }
        public string Message { get; }
        public PlayerState State { get; }

        public static SaveLoadResult Ok(PlayerState state, string message = "")
        {
            return new SaveLoadResult(true, message, state);
        }

        public static SaveLoadResult Fail(string message)
        {
            return new SaveLoadResult(false, message, null);
        }
    }
}
