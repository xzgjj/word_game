# 交互与技术细节规格

## 一、适用版本

适用版本：v1 规划版、网页概念演示、后续 Unity Windows x64 和 Unity macOS Universal 桌面原生版。

## 二、玩家行为路径

1. 玩家进入游戏，出生在木屋门口。
   - 状态变化：`positionId = home-yard`，`emotion = joy`。
   - 可见反馈：HUD 显示当前位置、发现信息和资源计数；主角为欢笑表情；不显示任务列表。

2. 玩家查看木屋周围。
   - 状态变化：`homeArea.inspected = true`。
   - 可见反馈：贴纸墙、花圃空位、工作台空位出现轻提示；建设清单显示材料需求。

3. 玩家移动到旁友森林，靠近资源点。
   - 状态变化：资源节点进入可交互状态。
   - 可见反馈：树枝和碎片轻微浮动，提示“拾取木材”。

4. 玩家确认拾取。
   - 状态变化：`inventory.wood += 1`，`inventory.shards += 1`。
   - 可见反馈：资源飞入 HUD，建设清单中木桥、栅栏或路牌的可建设状态更新。

5. 玩家前往河边木桥。
   - 状态变化：如果 `wood < 1`，桥节点返回不可修复；如果 `wood >= 1`，桥节点可修复。
   - 可见反馈：木材不足时提示回森林；木材足够时桥板点亮。

6. 玩家确认修桥。
   - 状态变化：`bridge.repaired = true`。
   - 可见反馈：桥出现高亮描边，目标改为前往空地游戏机。

7. 玩家调节时段。
   - 状态变化：`timeOfDay = morning | noon | night`。
   - 可见反馈：光照色调变化；夜晚时游戏机和河岸灯更明显；资源数量不因时段变化，避免复杂化。

8. 玩家抵达空地游戏机并确认拾取卡带。
   - 状态变化：`currentScene = mini-game-01`，`inPixelMode = true`。
   - 可见反馈：主角方块化，HUD 目标改为收集 3 个贴纸。

9. 玩家在像素小游戏中收集贴纸。
   - 状态变化：每个贴纸从未收集变为已收集；`stickers += 1`。
   - 可见反馈：贴纸变淡，计数更新；贴纸达到 3 个后出口发光。

10. 玩家点击出口。
   - 状态变化：`completedMiniGames += arcade-01`，`inPixelMode = false`。
   - 可见反馈：返回主世界，目标改为回木屋查看贴纸墙。

11. 玩家回到木屋。
   - 状态变化：`positionId = home-yard`。
   - 可见反馈：提示第一轮闭环完成，贴纸墙新增贴纸。

## 三、核心数据结构

```ts
type EmotionMode = "joy" | "calm" | "hype";

type PlayerState = {
    positionId: string;
    emotion: EmotionMode;
    timeOfDay: "morning" | "noon" | "night";
    inventory: { wood: number; stickers: number; shards: number };
    completedMiniGames: string[];
};
```

## 三、资源与建设数据

```ts
type ResourceId = "wood" | "stone" | "flowerSeed" | "shell" | "shard" | "cartridge" | "starCore" | "sticker";

type BuildRecipe = {
    id: string;
    label: string;
    cost: Partial<Record<ResourceId, number>>;
    unlocks: string[];
};
```

建设配方必须优先服务场景行为，不做纯数值堆叠。v1 推荐配方：木桥、花圃、木栅栏、工作台、河岸灯、贴纸墙、游戏机底座、林间路牌。

## 四、核心接口

- `WorldService.interact(nodeId, playerState)`：处理资源、桥、游戏机、木屋节点。
- `BuildService.canBuild(recipeId, playerState)`：检查资源是否满足建设物需求。
- `BuildService.place(recipeId, targetSlot, playerState)`：消耗材料并更新木屋周围或河岸状态。
- `TimeService.setTime(timeOfDay, playerState)`：切换清晨、午后、夜晚的视觉状态。
- `MiniGameService.start(miniGameId, playerState)`：进入像素模式并记录入口状态。
- `MiniGameService.collect(stickerId, playerState)`：收集贴纸并更新出口条件。
- `MiniGameService.finish(result, playerState)`：返回主世界并写入奖励。
- `SaveService.save(slotId, playerState)`：本地保存进度。
- `SaveService.load(slotId)`：读取本地进度。

## 五、Unity 拆分建议

- `WorldHub.unity`：木屋、森林、河流、桥、空地游戏机、远景光幕。
- `MiniGame01.unity`：像素平台、贴纸、出口、情绪能力验证点。
- `PlayerController.cs`：移动、面向、交互触发。
- `EmotionController.cs`：欢笑、安静、激动三种能力。
- `WorldNodeInteractor.cs`：节点靠近、提示、确认、状态写入。
- `MiniGameStateMachine.cs`：进入、收集、出口、返回。
- `SaveService.cs`：本地 JSON 存档。

## 六、网页演示边界

网页演示不追求最终画面精度，只承担三件事：

1. 证明场景关系成立：木屋、森林、河流、桥、空地游戏机的位置和目标路径清楚。
2. 证明玩法闭环成立：拾取、修桥、进入、收集、返回均能演示。
3. 证明桌面版元素一致：网页里的节点和状态命名能映射到 Unity 场景与脚本。
