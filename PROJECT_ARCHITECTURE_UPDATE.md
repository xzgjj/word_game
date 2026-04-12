# 星绪森林初步项目架构更新

版本：v2 设计优化稿。
日期：2026-04-12。

## 一、核心口径修正

《星绪森林》不做大地图，意思不是少内容、少系统、少可玩性，而是不做多个主世界场景地图、不做跨区域大世界扩张。v1 主世界应是一个持续演化的单地图：木屋前、森林、河岸、水域、空地游戏机和远景光幕都在同一张地图内，玩家可以在这张地图里采集、兑换、建设、摆放和逐步解锁自建建筑物。

正确目标是：单地图有足够密度，系统可扩展，玩家路径自然，不靠任务列表驱动。

## 二、参考分析与转化

参考对象不是用来复制内容，而是提取可落地方法。

| 参考 | 可借鉴方法 | 在本项目中的转化 |
| --- | --- | --- |
| 动物森友会 | 单岛生活、资源每日感、家具摆放、轻社交式提示 | 单主地图持续建设，木屋前木牌承担温和引导和兑换 |
| 我的世界 | 材料驱动建造、资源可理解、放置反馈强 | 木材、石子、花种、河贝、鱼成为基础材料，建设物由配方驱动 |
| 星露谷物语 | 小范围空间长期经营、时间氛围、资源转化 | 清晨/午后/夜晚影响视觉和提示，不做复杂产出概率 |
| Dragon Quest Builders | 图纸驱动建造、先给目标再允许自由建造 | 木牌赠送建设物图纸，建设 3 个物品后开启自建功能 |
| 短流程探索游戏 | 目标可见、路线短、失败低惩罚、非强制引导 | 游戏机和贴纸墙作为可见闭环，像素小游戏失败不扣材料；不把限时闭环当作强制节奏 |
| 崩坏：星穹铁道 | 电子入口、远景光幕、进入后规则变化 | 空地游戏机和不可达现代远景只做氛围与模式切换，不引入战斗系统 |

## 三、资源分布

资源必须长在玩家能理解的位置，避免凭空奖励。

| 资源 | 来源位置 | 获取方式 | 主要用途 | 可见反馈 |
| --- | --- | --- | --- | --- |
| 木材 `wood` | 森林树枝、木屋旁落枝 | 靠近拾取 | 木桥、栅栏、工作台、基础建筑 | 木片飞入物品栏 |
| 石子 `stone` | 河岸、浅滩边缘 | 靠近拾取 | 花圃、底座、河岸灯 | 石子弹跳和轻微水波 |
| 花种 `flowerSeed` | 森林边缘、木屋前草丛 | 靠近拾取 | 花圃、庭院装饰 | 小花粒子进入物品栏 |
| 河贝 `riverShell` | 河岸和浅水区 | 靠近拾取 | 风铃、装饰、木牌兑换 | 河岸闪光 |
| 鱼 `fish` | 河水 | 简化钓鱼或点击水面互动 | 木牌兑换、装饰水箱、后续料理 | 水花和鱼影 |
| 表情碎片 `emotionShard` | 木屋前木牌兑换 | 消耗基础物资兑换 | 情绪外观、路牌提示 | 情绪栏闪光 |
| 旧卡带 `oldCartridge` | 木屋前木牌兑换或首次赠送 | 消耗基础物资兑换 | 解锁游戏机小游戏 | 游戏机屏幕亮起 |
| 星屑灯芯 `starCore` | 木屋前木牌兑换，夜晚优惠提示 | 消耗基础物资兑换 | 河岸灯、游戏机底座 | 夜晚发光 |
| 贴纸 `sticker` | 像素小游戏奖励，也可木牌少量兑换 | 完成小游戏或兑换 | 贴纸墙、记录册 | 贴纸飞回 HUD |

资源分层：
- 基础材料：木材、石子、花种、河贝、鱼。
- 转化材料：表情碎片、旧卡带、星屑灯芯、贴纸。
- 解锁内容：建设图纸、自建建筑功能、小游戏入口。

## 四、木牌兑换系统

木牌放在木屋前，是主世界的温和系统入口。它不表现为强任务列表，而是像村口公告牌：玩家靠近后看到“今日可换”“图纸赠送”“小镇记录”。

木牌职责：
- 展示基础材料可以换什么。
- 赠送早期建设物图纸。
- 兑换表情碎片、旧卡带、星屑灯芯和贴纸。
- 记录已建设物数量。
- 在建设 3 个物品后解锁“自建建筑物”功能。

首版兑换建议：

| 兑换项 | 消耗 | 获得 | 解锁条件 |
| --- | --- | --- | --- |
| 表情碎片 | 木材 1 + 花种 1 | 表情碎片 1 | 初始 |
| 旧卡带 | 河贝 1 + 鱼 1 | 旧卡带 1 | 首次接近游戏机后 |
| 星屑灯芯 | 石子 2 + 河贝 1 | 星屑灯芯 1 | 夜晚首次查看木牌后 |
| 贴纸 | 鱼 2 + 表情碎片 1 | 贴纸 1 | 完成首个小游戏后 |

图纸赠送建议：
- 初始赠送：木桥图纸、花圃图纸、林间路牌图纸。
- 修桥后赠送：木栅栏图纸、河岸灯图纸。
- 首次完成小游戏后赠送：贴纸墙升级图纸、游戏机底座图纸。
- 建设 3 个任意图纸物品后：解锁自建建筑物功能。

## 五、自建建筑物功能

自建不是完整自由建造，也不是体素沙盒。它是“受控自定义建筑物”：

1. 玩家通过木牌打开自建界面。
2. 选择基础外形：小木屋、摊位、灯架、牌子、花架。
3. 选择材料主题：木材、石子、花种、河贝、星屑。
4. 选择占地尺寸：1x1 或 2x2，首版不做更大尺寸。
5. 系统计算消耗材料并生成一个 `customBuilding` 存档记录。
6. 玩家在单地图可放置区域选择位置。
7. 放置成功后地图刷新，建筑物计入建设数量和小镇记录。

首版约束：
- 自建建筑物只改变外观、灯光或轻提示，不产生复杂生产链。
- 自建建筑物必须遵守可放置区域、碰撞、上限和存档规则。
- 自建功能只有在 `builtCount >= 3` 后开启。

## 六、物品栏设计

物品栏格子数必须等于“物品种类 + 3”。当前 v2 初始物品种类为 9 类：

1. 木材 `wood`
2. 石子 `stone`
3. 花种 `flowerSeed`
4. 河贝 `riverShell`
5. 鱼 `fish`
6. 表情碎片 `emotionShard`
7. 旧卡带 `oldCartridge`
8. 星屑灯芯 `starCore`
9. 贴纸 `sticker`

因此首版物品栏格子数为 `9 + 3 = 12`。

每类物品堆叠上限：`9999`。

规则：
- 物品栏按物品种类固定槽位，不做随机排序。
- 图纸默认不占物品栏，放入 `unlockedBlueprints`。如果后续把图纸设计成可交易道具，则图纸也必须计入物品种类，并重新计算格子数。
- 当新增物品时，系统从 `ItemCatalog` 计算 `inventorySlotCount = inventoryItemCount + 3`。
- 显示层必须展示当前数量、上限和是否可用于兑换或建设。

## 七、玩家行为路径

### 首次自由探索路径

1. 玩家出生在木屋前。
   - 状态：`positionId = home-yard`，`knownSystems.signboard = false`。
   - 反馈：看到木屋、木牌、森林、河岸和远处游戏机光点。

2. 玩家靠近木牌。
   - 状态：`knownSystems.signboard = true`。
   - 反馈：木牌提示“森林和河边的东西可以换成小镇物件”，赠送木桥、花圃、林间路牌图纸。

3. 玩家进入森林和河岸采集。
   - 状态：基础材料增加：木材、花种、石子、河贝；河水可钓鱼。
   - 反馈：材料进入物品栏；物品栏 12 格显示数量；木牌兑换项从灰色变成可兑换。

4. 玩家回到木牌兑换。
   - 状态：消耗基础材料，获得表情碎片或旧卡带。
   - 反馈：兑换项显示“已获得”；若拿到旧卡带，游戏机入口发光增强。

5. 玩家修桥或放置初始建设物。
   - 状态：`builtCount += 1`，对应建筑物写入 `placedBuildings`。
   - 反馈：地图上出现建筑物，木牌记录更新。

6. 玩家累计建设 3 个物品。
   - 状态：`customBuildUnlocked = true`。
   - 反馈：木牌新增“自建建筑物”入口。

7. 玩家发现游戏机菜单入口，并自主选择是否进入小游戏。
   - 状态：`currentMode = pixelMiniGame`，若需要旧卡带则消耗或标记已使用。
   - 反馈：主角像素化，贴纸目标显示 0/3。

8. 玩家完成小游戏返回木屋。
   - 状态：`completedMiniGames` 增加，`stickerWallCount += 1`。
   - 反馈：游戏机点亮，贴纸墙新增贴纸，木牌解锁新图纸。

### 重复游玩路径

玩家每天或每轮进入同一张地图，优先做三类事情：采集基础材料、通过木牌兑换成长材料、建设或自建新物件。系统不需要新增大地图，也可以通过单图摆放、图纸和装饰深度形成长期目标。

## 八、状态与数据结构

```ts
type ItemId =
    | "wood"
    | "stone"
    | "flowerSeed"
    | "riverShell"
    | "fish"
    | "emotionShard"
    | "oldCartridge"
    | "starCore"
    | "sticker";

type BlueprintId =
    | "bridge"
    | "flowerBed"
    | "forestSign"
    | "woodFence"
    | "riverLamp"
    | "stickerWallUpgrade"
    | "arcadeBase"
    | "customBuilding";

type TimeOfDay = "morning" | "noon" | "night";
type EmotionMode = "joy" | "calm" | "hype";

interface ItemStack {
    itemId: ItemId;
    count: number;
    maxStack: 9999;
}

interface InventoryState {
    slotCount: number;
    items: Record<ItemId, number>;
}

interface ExchangeRecipe {
    id: string;
    outputItemId: ItemId;
    outputCount: number;
    cost: Partial<Record<ItemId, number>>;
    unlockCondition?: string;
}

interface PlayerState {
    positionId: string;
    emotion: EmotionMode;
    timeOfDay: TimeOfDay;
    inventory: InventoryState;
    unlockedBlueprints: BlueprintId[];
    placedBuildings: PlacedBuilding[];
    builtCount: number;
    customBuildUnlocked: boolean;
    completedMiniGames: string[];
    stickerWallCount: number;
}

interface PlacedBuilding {
    id: string;
    blueprintId: BlueprintId;
    position: { x: number; y: number };
    rotation: 0 | 90 | 180 | 270;
    variant?: string;
    customData?: CustomBuildingData;
}

interface CustomBuildingData {
    shape: "tinyCabin" | "stall" | "lampFrame" | "sign" | "flowerRack";
    theme: "wood" | "stone" | "flower" | "riverShell" | "star";
    size: "1x1" | "2x2";
}
```

## 九、接口与调用链

核心服务：

- `InventoryService.getSlotCount(itemCatalog)`：返回物品种类 + 3。
- `InventoryService.add(itemId, count)`：增加物品，单项上限 9999。
- `InventoryService.spend(cost)`：检查并扣除材料。
- `GatherService.collect(nodeId)`：采集森林、河岸、水域资源。
- `FishingService.catchFish(waterNodeId)`：简化钓鱼并产出鱼。
- `SignboardService.open()`：打开木牌界面并刷新可兑换项。
- `SignboardService.exchange(recipeId)`：材料兑换表情碎片、旧卡带、星屑灯芯或贴纸。
- `BlueprintService.grant(triggerId)`：根据进度赠送图纸。
- `BuildService.canPlace(blueprintId, position)`：检查材料、图纸、可放置区域和碰撞。
- `BuildService.place(blueprintId, position)`：消耗材料，生成建筑物，增加 `builtCount`。
- `CustomBuildService.unlockIfReady()`：当 `builtCount >= 3` 时开启自建。
- `CustomBuildService.create(config, position)`：生成受控自建建筑。
- `MiniGameService.start(miniGameId)`：进入游戏机小游戏。
- `MiniGameService.finish(result)`：结算贴纸和图纸赠送。
- `SaveService.save(slotId)` / `SaveService.load(slotId)`：本地 JSON 存档。

调用链示例：采集到兑换

```text
PlayerController.interact(node)
-> GatherService.collect(nodeId)
-> InventoryService.add(itemId, count)
-> UIService.refreshInventory()
-> SignboardService.refreshAvailability()
```

调用链示例：木牌兑换

```text
PlayerController.interact(signboard)
-> SignboardService.open()
-> UI 选择 exchangeRecipe
-> InventoryService.spend(cost)
-> InventoryService.add(outputItemId, outputCount)
-> BlueprintService.grant(optionalTrigger)
-> SaveService.save(auto)
```

调用链示例：建设 3 个物品后开启自建

```text
BuildService.place(blueprintId, position)
-> PlayerState.placedBuildings.add(building)
-> PlayerState.builtCount += 1
-> CustomBuildService.unlockIfReady()
-> SignboardService.refreshMenu()
-> UIService.showUnlock("自建建筑物已开启")
```

## 十、存档与数据库设计

MVP 不接服务器数据库，桌面版使用本地 JSON，网页演示使用 `localStorage`。但数据结构按将来可迁移数据库的方式设计。

本地存档建议：

```json
{
    "schemaVersion": 2,
    "player": {
        "positionId": "home-yard",
        "emotion": "joy",
        "timeOfDay": "morning",
        "inventory": {
            "slotCount": 12,
            "items": {
                "wood": 0,
                "stone": 0,
                "flowerSeed": 0,
                "riverShell": 0,
                "fish": 0,
                "emotionShard": 0,
                "oldCartridge": 0,
                "starCore": 0,
                "sticker": 0
            }
        },
        "unlockedBlueprints": ["bridge", "flowerBed", "forestSign"],
        "placedBuildings": [],
        "builtCount": 0,
        "customBuildUnlocked": false,
        "completedMiniGames": [],
        "stickerWallCount": 0
    }
}
```

未来如果接入数据库，可拆为：

- `player_save`：存档主记录、版本、时间、玩家位置。
- `inventory_item`：玩家物品数量。
- `unlocked_blueprint`：已解锁图纸。
- `placed_building`：地图摆放建筑物。
- `mini_game_progress`：小游戏完成记录。

当前不实现数据库，只保留迁移方向。

## 十一、Unity 初步架构

```text
Assets/
  Scripts/
    Core/GameState.cs
    Core/GameBootstrap.cs
    Inventory/ItemCatalog.cs
    Inventory/InventoryService.cs
    World/GatherNode.cs
    World/FishingNode.cs
    World/SignboardNode.cs
    Building/BlueprintCatalog.cs
    Building/BuildService.cs
    Building/PlacementGrid.cs
    Building/CustomBuildService.cs
    MiniGame/MiniGameStateMachine.cs
    Save/SaveService.cs
    UI/InventoryPanel.cs
    UI/SignboardPanel.cs
    UI/BuildPanel.cs
```

场景策略：
- `WorldHub` 是唯一主世界地图，允许持续摆放元素。
- 像素小游戏可以做成同工程内单独场景或同场景 overlay，但它不是第二张主世界地图。
- 所有主世界建设物回写 `placedBuildings`，不能只存在于场景临时对象。

## 十二、落地 Review

补全责任：用户给出的设计、环节、PRD、系分和测分只要存在点到为止的地方，就必须由实现者补齐交互、状态、接口、数据结构、失败分支、测试点和验收标准。不得只复述需求或停在机械拆分。

资深游戏设计者：
- 现在的资源链条更完整：基础材料来自森林和河水，成长材料来自木牌兑换，图纸来自木牌赠送，建设 3 个物品后开启自建。
- 风险是木牌 UI 变成任务面板。解决方式是木牌只展示“可换”和“已发现”，不显示长任务清单。
- 对标动森：检查木屋、木牌、小镇记录和建设是否形成家园生活动机，而不是任务面板。
- 对标我的世界：检查材料来源、材料用途和建设落位是否有清楚因果反馈。
- 对标星穹铁道：检查游戏机、旧卡带、远景光幕和像素模式是否表达规则切换，但不引入战斗/抽卡/大剧情。

资深玩家：
- 玩家能理解“我采集这些东西是为了换东西和建设家园”。
- 首次体验目标应是：玩家能自然理解采集、兑换、建设、游戏机菜单入口和贴纸墙展示之间的关系；不以 5 分钟闭环作为强制任务节奏。
- 风险是鱼、河贝、星屑灯芯过早出现造成认知压力。解决方式是按靠近区域和时间逐步显露。
- UI 体验标准：物品栏、木牌兑换、图纸和建设界面必须像生活工具，不能像后台表格。

资深架构师：
- 需要集中 `PlayerState`，特别是物品栏、图纸、摆放建筑物和建设计数。
- 物品栏格子数必须由 `ItemCatalog` 计算，不能手写死 12。
- 自建建筑必须作为结构化数据保存，不能只保存 Prefab 名称。
- MVP 不接数据库，但存档 JSON 要加 `schemaVersion`，为后续迁移留空间。
- 测试标准：每完成一个功能，至少要有最小测试或手动验证记录；阶段完成后跑 Unity BatchMode 自动测试和构建验证。

通过条件：
- 单主地图不扩成多场景大地图。
- 资源、兑换、图纸、建设、自建、小游戏奖励都有状态字段。
- 物品栏满足 `物品种类 + 3`，每类上限 9999。
- 建设 3 个物品后能触发自建解锁。
- 任意一次交互都能说清楚：玩家做了什么、状态怎么变、屏幕显示什么反馈。
- 每 10 分钟 review 都按动森、我的世界、星穹铁道三类方法做一次偏差检查；发现偏离就先修设计或实现，不继续堆功能。
- 网页概念演示必须放在 Unity 核心系统和画面方向稳定之后更新，网页不能反向主导 Unity 实现。
