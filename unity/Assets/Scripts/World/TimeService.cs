using StarryForest.Core;

namespace StarryForest.World
{
    public sealed class TimeService
    {
        public OperationResult SetTime(PlayerState state, TimeOfDay timeOfDay)
        {
            state.TimeOfDay = timeOfDay;
            return OperationResult.Ok(GetLightingHint(timeOfDay));
        }

        public OperationResult CycleNext(PlayerState state)
        {
            TimeOfDay next = state.TimeOfDay switch
            {
                TimeOfDay.Morning => TimeOfDay.Noon,
                TimeOfDay.Noon => TimeOfDay.Night,
                _ => TimeOfDay.Morning
            };

            return SetTime(state, next);
        }

        public string GetLightingHint(TimeOfDay timeOfDay)
        {
            return timeOfDay switch
            {
                TimeOfDay.Morning => "清晨光线柔和，木屋前的提示更清楚。",
                TimeOfDay.Noon => "午后视野明亮，森林和河岸材料更容易辨认。",
                TimeOfDay.Night => "夜晚光幕和游戏机更显眼，河岸灯提示增强。",
                _ => "时间状态已更新。"
            };
        }
    }
}
