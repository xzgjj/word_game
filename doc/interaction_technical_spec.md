# 交互与技术细节规格

## 一、适用版本

适用版本：v1 规划版、网页概念演示、后续 Unity Windows x64 和 Unity macOS Universal 桌面原生版。

v2 口径修正：不做大地图指不做多个主世界场景地图。`WorldHub` 是唯一主世界地图，但它可以像动森小岛一样持续摆放不同建设元素。基础材料来自森林、河岸和河水；表情碎片、旧卡带、星屑灯芯、贴纸主要通过木屋前木牌用物资兑换；木牌赠送建设物图纸；建设 3 个物品后开启受控自建建筑物。

## 二、玩家行为路径

1. 玩家进入游戏，出生在木屋门口。
   - 状态变化：`positionId = home-yard`，`emotion = joy`。
   - 可见反馈：HUD 显示当前位置、发现信息和资源计数；主角为欢笑表情；不显示任务列表；木屋前木牌有轻微发光。

2. 玩家查看木屋前木牌。
   - 状态变化：`knownSystems.signboard = true`，初始赠送 `unlockedBlueprints += bridge, flowerBed, forestSign`。
   - 可见反馈：木牌显示“可兑换”和“图纸赠送”，不显示强制任务列表；物品栏显示初始格子数。

3. 玩家移动到旁友森林、河岸和河水边，靠近资源点。
   - 状态变化：资源节点进入可交互状态。
   - 可见反馈：树枝、石子、花种、河贝和鱼影出现轻提示，提示“拾取木材”“捡起河贝”“钓一下鱼”。

4. 玩家确认拾取。
   - 状态变化：基础材料增加，例如 `inventory.items.wood += 1`、`inventory.items.riverShell += 1`、`inventory.items.fish += 1`，单项不超过 9999。
   - 可见反馈：资源飞入物品栏，木牌兑换项和建设图纸状态刷新。

5. 玩家回到木牌兑换成长物资。
   - 状态变化：若材料满足，扣除基础材料并增加 `emotionShard`、`oldCartridge`、`starCore` 或 `sticker`。
   - 可见反馈：兑换项从灰色变成可点；兑换后显示“已获得”；如果获得旧卡带，空地游戏机光效增强。

6. 玩家前往河边木桥。
   - 状态变化：如果 `wood < 1`，桥节点返回不可修复；如果 `wood >= 1`，桥节点可修复。
   - 可见反馈：木材不足时提示回森林；木材足够时桥板点亮。

7. 玩家确认修桥或放置其他图纸建设物。
   - 状态变化：`placedBuildings += building`，`builtCount += 1`；如果累计建设 3 个物品，`customBuildUnlocked = true`。
   - 可见反馈：桥板补齐或建筑物落位；木牌记录更新；达到 3 个建设物时木牌新增“自建建筑物”入口。

8. 玩家调节时段。
   - 状态变化：`timeOfDay = morning | noon | night`。
   - 可见反馈：光照色调变化；夜晚时游戏机和河岸灯更明显；资源数量不因时段变化，避免复杂化。

9. 玩家抵达空地游戏机并确认使用旧卡带。
   - 状态变化：`currentScene = mini-game-01`，`inPixelMode = true`。
   - 可见反馈：主角方块化，HUD 目标改为收集 3 个贴纸。

10. 玩家在像素小游戏中收集贴纸。
   - 状态变化：每个贴纸从未收集变为已收集；`stickers += 1`。
   - 可见反馈：贴纸变淡，计数更新；贴纸达到 3 个后出口发光。

11. 玩家点击出口。
   - 状态变化：`completedMiniGames += arcade-01`，`inPixelMode = false`。
   - 可见反馈：返回主世界，目标改为回木屋查看贴纸墙。

12. 玩家回到木屋。
    - 状态变化：`positionId = home-yard`。
    - 可见反馈：提示第一轮闭环完成，贴纸墙新增贴纸，木牌可赠送下一批图纸。

## 二点一、大世界自由探索路径

大世界探索不使用任务列表，而使用“场景缺口 + 建设需求 + 材料反馈”引导。

1. 木屋周围
   - 行为：玩家查看贴纸墙、花圃空位、工作台空位、栅栏边界。
   - 状态：`homeArea.inspected = true`。
   - 反馈：建设空位显示材料短句，例如“花圃：花种 2 / 石子 1”。

2. 旁友森林
   - 行为：玩家拾取树枝、花种，观察林间路牌空位。
   - 状态：`inventory.items.wood += 1`，`inventory.items.flowerSeed += 1`。
   - 反馈：材料进入物品栏，木桥、花圃和路牌图纸状态刷新。

3. 河岸浅滩
   - 行为：玩家观察桥梁缺口、拾取石子或河贝，点击河水获得鱼。
   - 状态：`riverBank.visited = true`，`inventory.items.stone`、`inventory.items.riverShell`、`inventory.items.fish` 按拾取更新。
   - 反馈：桥显示缺少材料；石子、河贝和鱼影用闪光或水波提示，避免任务箭头。

4. 半损坏木桥
   - 行为：玩家靠近并确认修复。
   - 状态：若 `wood >= 1`，设置 `bridge.repaired = true`；否则只显示缺口。
   - 反馈：木板补齐，角色可通过，空地游戏机的光变得更明显。

5. 空地游戏机
   - 行为：玩家选择是否进入像素模式。
   - 状态：`inPixelMode = true`。
   - 反馈：游戏机屏幕点亮，主角变为像素形态。

6. 回到家园
   - 行为：玩家把小游戏贴纸带回木屋。
   - 状态：`stickerWall.count += 1`。
   - 反馈：贴纸墙新增图案，小镇记录点亮。

7. 木屋前木牌
   - 行为：玩家查看兑换、领取图纸、检查建设数量。
   - 状态：`knownSystems.signboard = true`；兑换成功时扣除基础物资并增加成长物资；图纸进入 `unlockedBlueprints`。
   - 反馈：可兑换项根据材料变亮；赠送图纸以短动画进入图纸册；建设 3 个物品后出现“自建建筑物”入口。

8. 自建建筑物
   - 行为：玩家在木牌选择外形、材料主题和 1x1/2x2 尺寸，并在地图摆放。
   - 状态：`customBuildUnlocked = true` 时可用；成功后写入 `placedBuildings`。
   - 反馈：放置预览显示可放/不可放；确认后建筑物落位并保存。

## 二点二、建设对象实现细节

| 对象 | 玩家行为 | 状态变化 | 可见反馈 | 实现建议 |
| --- | --- | --- | --- | --- |
| 木桥 | 靠近缺口并确认修复 | `bridge.repaired = true` | 桥板补齐，空地可达 | `BridgeRepairNode` |
| 花圃 | 在木屋旁选择空位放置 | `homeGarden.level += 1` | 小花出现，庭院更完整 | `BuildSlot + BuildRecipe` |
| 木栅栏 | 沿木屋边界放置 | `yard.fencePlaced = true` | 家园边界更清晰 | 网格槽位 |
| 工作台 | 放在木屋旁 | `craftBench.unlocked = true` | 建设清单展开 | UI 面板入口 |
| 河岸灯 | 放在河岸 | `riverLamp.enabled = true` | 夜晚发光 | 时间系统联动 |
| 贴纸墙 | 回家贴上贴纸 | `stickerWall.count += 1` | 贴纸图案新增 | 奖励展示 |
| 林间路牌 | 放在森林入口 | `forestSign.placed = true` | 轻提示方向 | 非任务式引导 |
| 游戏机底座 | 装饰空地入口 | `arcadeBase.built = true` | 游戏机更醒目 | 入口强化 |

## 三、核心数据结构

```ts
type EmotionMode = "joy" | "calm" | "hype";

type PlayerState = {
    positionId: string;
    emotion: EmotionMode;
    timeOfDay: "morning" | "noon" | "night";
    inventory: InventoryState;
    bridgeRepaired: boolean;
    homeGardenLevel: number;
    stickerWallCount: number;
    unlockedBlueprints: BlueprintId[];
    placedBuildings: PlacedBuilding[];
    builtCount: number;
    customBuildUnlocked: boolean;
    completedMiniGames: string[];
};

type ItemId = "wood" | "stone" | "flowerSeed" | "riverShell" | "fish" | "emotionShard" | "oldCartridge" | "starCore" | "sticker";

type InventoryState = {
    slotCount: number; // item type count + 3, v2 = 12
    items: Record<ItemId, number>; // each item max = 9999
};

type BlueprintId = "bridge" | "flowerBed" | "forestSign" | "woodFence" | "riverLamp" | "stickerWallUpgrade" | "arcadeBase" | "customBuilding";

type PlacedBuilding = {
    id: string;
    blueprintId: BlueprintId;
    position: { x: number; y: number };
    rotation: 0 | 90 | 180 | 270;
    customData?: CustomBuildingData;
};

type CustomBuildingData = {
    shape: "tinyCabin" | "stall" | "lampFrame" | "sign" | "flowerRack";
    theme: "wood" | "stone" | "flower" | "riverShell" | "star";
    size: "1x1" | "2x2";
};
```

## 三、资源与建设数据

```ts
type ResourceId = ItemId;

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
- `InventoryService.getSlotCount(itemCatalog)`：按“物品种类 + 3”计算物品栏格子数。
- `InventoryService.add(itemId, count, playerState)`：增加物品，单项上限 9999。
- `InventoryService.spend(cost, playerState)`：检查并扣除兑换或建设材料。
- `GatherService.collect(nodeId, playerState)`：处理森林、河岸和浅水资源。
- `FishingService.catchFish(waterNodeId, playerState)`：处理河水中的鱼。
- `SignboardService.open(playerState)`：打开木屋前木牌界面，刷新兑换和图纸。
- `SignboardService.exchange(recipeId, playerState)`：消耗基础材料，获得表情碎片、旧卡带、星屑灯芯或贴纸。
- `BlueprintService.grant(triggerId, playerState)`：按初始、修桥、小游戏完成等触发点赠送图纸。
- `BuildService.canBuild(recipeId, playerState)`：检查资源是否满足建设物需求。
- `BuildService.place(recipeId, targetSlot, playerState)`：消耗材料并更新单主地图中的摆放建筑，写入 `placedBuildings` 并增加 `builtCount`。
- `CustomBuildService.unlockIfReady(playerState)`：当 `builtCount >= 3` 时开启自建建筑物。
- `CustomBuildService.create(config, targetSlot, playerState)`：创建受控自建建筑物并保存结构化数据。
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
- `InventoryService.cs`：物品栏格子数、9999 上限、材料增减。
- `GatherNode.cs` / `FishingNode.cs`：森林、河岸、河水资源交互。
- `SignboardNode.cs` / `SignboardPanel.cs`：木牌兑换、图纸赠送、自建入口。
- `BlueprintCatalog.cs`：建设图纸数据。
- `PlacementGrid.cs`：单主地图可放置区域、碰撞和旋转。
- `CustomBuildService.cs`：建设 3 个物品后的受控自建建筑物。
- `WorldNodeInteractor.cs`：节点靠近、提示、确认、状态写入。
- `MiniGameStateMachine.cs`：进入、收集、出口、返回。
- `SaveService.cs`：本地 JSON 存档。

## 六、网页演示边界

网页演示不追求最终画面精度，只承担三件事：

1. 证明场景关系成立：木屋、森林、河流、桥、空地游戏机的位置和目标路径清楚。
2. 证明玩法闭环成立：拾取、修桥、进入、收集、返回均能演示。
3. 证明桌面版元素一致：网页里的节点和状态命名能映射到 Unity 场景与脚本。
