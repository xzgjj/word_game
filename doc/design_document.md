# 游戏设计规划文档



## 一、项目定位
- 项目名称：星绪森林。
- 类型：现代情绪冒险 + 小地图家园探索 + 轻量建设 + 关卡式轻松平台。
- 目标产出：`Unity Windows x64` 桌面原生版、`Unity macOS Universal` 桌面原生版、`HTML + CSS + JavaScript` 网页概念演示。
- MVP 范围：不做多个主世界场景地图，但要在一张类似动森小岛的单主地图内形成完整玩法闭环。第一版只做木屋、旁友森林、河流、半损坏木桥、空地游戏机和一个像素小游戏，同时允许在同一张地图内持续摆放不同建设元素。
- 核心闭环：木屋周围自由查看 -> 在森林和河水区域收集基础材料 -> 木屋前木牌兑换成长物资与图纸 -> 修桥或布置家园 -> 建设 3 个物品后开启自建建筑物 -> 自主进入游戏机 -> 像素小游戏 -> 返回主世界 -> 贴纸墙更新。
- 行为原则：适当引导的自由探索为主，不做任务列表，不用强制任务引导玩家收集；通过建设物、场景缺口、材料提示和可见反馈让玩家自然理解收集理由。



## 二、参考研究与取舍
- 《动物森友会：新视野》：提取小岛生活、家园起点、户外布置、桥和坡道式连接、温和互动。落地为木屋安全区、短距离散步动线、清楚的节点提示。来源：https://animalcrossing.nintendo.com/new-horizons/create/
- 《我的世界》：提取木屋、自然资源、采集、可读边界和微建造。落地为森林拾取木材、河边修桥、方块化资源轮廓。来源：https://www.minecraft.net/en-us/about-minecraft
- 《崩坏：星穹铁道 4.0》：提取现代光幕、电子屏、入口后规则变化。落地为不可到达远景、游戏机屏幕、像素模式转场。来源：https://blog.playstation.com/2026/02/06/honkai-star-rail-version-4-0-no-aha-at-full-moon-will-go-live-on-february-13/
- 短流程探索游戏：提取目标可见、路径短、失败不惩罚。落地为 3 分钟内完成的贴纸小游戏。来源：https://ashorthike.com/
- 素材获取：优先 AI 生成自有素材或使用明确 CC0/开源许可素材，候选来源包含 Kenney、OpenGameArt、Poly Haven、ambientCG；所有素材必须记录来源、许可证和用途，未经确认不得直接提交第三方美术资源。



## 三、核心体验目标
- 30 秒内理解：玩家能看到木屋、森林、河流、空地游戏机，知道游戏机是目标。
- 90 秒内完成第一轮主世界探索：拾取至少一个资源，修桥或抵达空地。
- 3 分钟内完成第一个像素小游戏：收集 3 个贴纸，使用一次情绪能力，到达出口。
- 回到主世界时有变化：游戏机点亮，木屋贴纸墙新增贴纸，玩家知道闭环完成。



## 四、场景设计



### 主世界小地图
- 木屋：出生点、贴纸墙、邮箱、长椅，表达安全区。
- 旁友森林：8 到 12 棵树，树下有木材和表情碎片，形成温和遮挡。
- 河流：斜向穿过地图，河面有反光；第一版只通过一座半损坏木桥解决。
- 空地：河对岸圆形草地，中心放置游戏机状互动装置，有屏幕、摇杆、拾取光圈。
- 远景：地图边缘的现代光幕、远轨道和淡色灯牌，只服务氛围，不增加探索面积。
- 时间：玩家可切换清晨、午后、夜晚三种时段；时段影响光照、可见提示和氛围，不改变资源产出概率，避免系统复杂化。



### 资源与建设清单

| 资源 | 来源领域 | 主要用途 | 反馈 |
| --- | --- | --- | --- |
| 木材 | 旁友森林、木屋旁落枝 | 修木桥、木栅栏、工作台、基础建筑 | 木片飞入物品栏 |
| 石子 | 河岸、浅滩边缘 | 花圃、底座、河岸灯 | 轻微水波与拾取声 |
| 花种 | 森林边缘、木屋前草丛 | 花圃、庭院装饰 | 地面出现小花标记 |
| 河贝 | 河岸和浅水区 | 风铃、摆件、木牌兑换 | 河岸闪光 |
| 鱼 | 河水 | 木牌兑换、装饰水箱、后续料理 | 水花和鱼影 |
| 表情碎片 | 木屋前木牌兑换 | 情绪外观、林间路牌提示 | 情绪按钮轻闪 |
| 旧卡带 | 木屋前木牌兑换或首次赠送 | 进入像素小游戏 | 游戏机屏幕点亮 |
| 星屑灯芯 | 木屋前木牌兑换，夜晚提示增强 | 河岸灯、游戏机底座灯带 | 夜晚发光 |
| 贴纸 | 像素小游戏奖励，后续可木牌少量兑换 | 贴纸墙、表情图鉴 | 贴纸飞回 HUD |

| 建设物 | 材料 | 行为价值 | 状态变化 |
| --- | --- | --- | --- |
| 木桥 | 木材 1 | 连通河岸与空地 | `bridge.repaired = true` |
| 花圃 | 花种 2、石子 1 | 装饰木屋周围 | `homeGarden.level += 1` |
| 木栅栏 | 木材 2 | 标记庭院边界 | `yard.fencePlaced = true` |
| 工作台 | 木材 2、石子 1 | 打开建设清单 | `craftBench.unlocked = true` |
| 河岸灯 | 木材 1、星屑灯芯 1 | 夜晚引导和氛围 | `riverLamp.enabled = true` |
| 贴纸墙 | 贴纸 3 | 展示小游戏成果 | `stickerWall.count += 1` |
| 游戏机底座 | 石子 2、星屑灯芯 1 | 强化入口识别 | `arcadeBase.built = true` |
| 林间路牌 | 木材 1、表情碎片 1 | 轻提示，不做任务箭头 | `forestSign.placed = true` |

木屋前新增 `木牌` 作为温和系统入口。木牌不做强任务列表，而是提供材料兑换、图纸赠送、小镇记录和自建建筑物解锁。首版基础材料来自森林和河水区域；表情碎片、旧卡带、星屑灯芯和少量贴纸主要通过木牌用物资兑换。建设 3 个任意图纸物品后，`customBuildUnlocked = true`，允许玩家创建受控自建建筑物。

物品栏规则：当前 v2 物品种类为木材、石子、花种、河贝、鱼、表情碎片、旧卡带、星屑灯芯、贴纸，共 9 类；物品栏格子数必须为 `物品种类 + 3 = 12`。每类物品放置上限为 `9999`。图纸默认进入 `unlockedBlueprints`，不占物品栏；如果后续把图纸设计为可交易道具，则必须计入物品种类并重新计算格子数。



### 像素小游戏
- 场景：一屏横向小关，像素主角、网格平台、贴纸、出口门。
- 目标：收集 3 个贴纸后到达出口。
- 情绪能力：欢笑短冲刺，安静短漂浮，激动高跳触发按钮。
- 失败处理：掉落后回到最近平台，不扣资源，不强制重开。



## 五、玩家行为路径



1. 玩家在木屋门口出生，HUD 显示“发现：木屋前有一块发光木牌”，不显示任务列表。
2. 玩家查看木牌，获得初始图纸，例如木桥、花圃、林间路牌；木牌说明森林和河水里的材料可以兑换成长物资。
3. 玩家进入森林和河岸，拾取木材、石子、花种、河贝，并可以在河水中获得鱼；基础材料飞入物品栏。
4. 玩家回到木牌，使用基础材料兑换表情碎片、旧卡带、星屑灯芯或少量贴纸；兑换项根据材料是否满足显示可用状态。
5. 玩家到河边，如果木材不足，桥节点显示“缺少 1 个木材”；如果木材足够，提示可修桥。
6. 玩家修桥或放置其他图纸建设物后，`builtCount += 1`；累计建设 3 个物品后，木牌开启“自建建筑物”入口。
7. 玩家可以继续在单主地图内布置木屋周围，也可以走到空地游戏机；如果已获得旧卡带，靠近游戏机后出现“插入旧卡带，进入像素模式？”。
8. 玩家确认后转场，HUD 切到像素布局，主角由圆形动态表情变为方块像素表情。
9. 玩家在小游戏中收集 3 个贴纸并到达出口。
10. 游戏返回空地，游戏机点亮；玩家回到木屋，贴纸墙新增贴纸，木牌可赠送下一批图纸。



## 六、UI 与反馈
- 主世界 HUD：当前位置、目标、木材/贴纸/碎片计数、当前可用按键。
- 情绪栏：欢笑、安静、激动三枚按钮，选中态只改变边框和提示，保持布局稳定。
- 交互提示：角色或节点附近显示短句，例如“拾取木材”“修复木桥”“进入像素模式”。
- 完成反馈：贴纸飞入 HUD、游戏机灯带点亮、木屋贴纸墙新增图标。



### 大世界能力反馈



- 欢笑：大世界中短距离移动更轻快，靠近可拾取物时拾取反馈更明显；像素模式中作为冲刺。
- 安静：大世界中增强隐藏拾取点、河岸闪光和夜晚灯芯提示；像素模式中作为漂浮。
- 激动：大世界中建设确认反馈更强，例如桥板弹起、木栅栏落位；像素模式中作为高跳。



### 动森式过桥搭建提示



- 玩家第一次到河边时，只看到“木桥缺少木材 1”，不弹强制任务。
- 玩家物品栏有木材时，桥节点高亮并出现“修一下木桥”的生活化提示。
- 玩家确认后，木桥从半损坏状态切换为可通行状态，空地游戏机的光线增强。
- 如果玩家不修桥，可以回木屋整理建设清单或继续在森林采集，不阻断自由探索感。



## 七、数据结构与接口



```ts
type EmotionMode = "joy" | "calm" | "hype";

interface PlayerState {
    positionId: string;
    emotion: EmotionMode;
    timeOfDay: "morning" | "noon" | "night";
    inventory: InventoryState;
    unlockedBlueprints: BlueprintId[];
    placedBuildings: PlacedBuilding[];
    builtCount: number;
    customBuildUnlocked: boolean;
    unlockedMiniGames: string[];
    completedMiniGames: string[];
    stickerWallCount: number;
}

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

interface InventoryState {
    slotCount: number; // item type count + 3, v2 = 12
    items: Record<ItemId, number>; // each item max = 9999
}

type BlueprintId =
    | "bridge"
    | "flowerBed"
    | "forestSign"
    | "woodFence"
    | "riverLamp"
    | "stickerWallUpgrade"
    | "arcadeBase"
    | "customBuilding";

interface PlacedBuilding {
    id: string;
    blueprintId: BlueprintId;
    position: { x: number; y: number };
    rotation: 0 | 90 | 180 | 270;
    customData?: CustomBuildingData;
}

interface CustomBuildingData {
    shape: "tinyCabin" | "stall" | "lampFrame" | "sign" | "flowerRack";
    theme: "wood" | "stone" | "flower" | "riverShell" | "star";
    size: "1x1" | "2x2";
}

interface WorldNode {
    id: string;
    type: "resource" | "bridge" | "arcade" | "home";
    label: string;
    requiredItem?: string;
    completed?: boolean;
}

interface MiniGameResult {
    miniGameId: string;
    success: boolean;
    stickersCollected: number;
    exitReason: "completed" | "manual_exit" | "failed_retry";
}
```

- `WorldService.getNodeState(nodeId)`：读取节点是否可交互。
- `WorldService.interact(nodeId, playerState)`：执行拾取、修桥、进入游戏机等行为。
- `InventoryService.getSlotCount(itemCatalog)`：按“物品种类 + 3”计算物品栏格子数。
- `InventoryService.add(itemId, count)` / `InventoryService.spend(cost)`：处理 9999 上限、材料增加和材料消耗。
- `GatherService.collect(nodeId)` / `FishingService.catchFish(waterNodeId)`：处理森林、河岸和河水资源。
- `SignboardService.exchange(recipeId)`：通过木屋前木牌兑换表情碎片、旧卡带、星屑灯芯或贴纸。
- `BlueprintService.grant(triggerId)`：根据初始、修桥、小游戏完成等触发点赠送图纸。
- `BuildService.canPlace(recipeId, targetSlot, playerState)` / `BuildService.place(recipeId, targetSlot, playerState)`：检查材料、图纸和碰撞，并写入 `placedBuildings`。
- `CustomBuildService.unlockIfReady(playerState)` / `CustomBuildService.create(config, targetSlot)`：建设 3 个物品后开启受控自建建筑物。
- `MiniGameService.start(miniGameId, playerState)`：记录入口位置并切换像素小游戏。
- `MiniGameService.finish(result)`：结算贴纸并返回主世界。
- `SaveService.save(slotId, playerState)` / `SaveService.load(slotId)`：本地存档读写。



## 八、技术架构
- 桌面原生版本 1：`Unity Windows x64`，优先实现第三视角小地图、节点交互、场景切换、本地存档。
- 桌面原生版本 2：`Unity macOS Universal`，同一 Unity 工程多平台打包，保持功能一致。
- 网页概念演示：用于确认画面构图、交互路径和状态变化，不替代最终桌面版。
- 存档：MVP 不接入服务器数据库；Unity 使用本地 JSON，网页演示使用 `localStorage`。
- 主世界场景策略：`WorldHub` 是唯一主世界地图，允许持续摆放元素；像素小游戏可以作为独立小游戏场景或同场景 overlay，但不视为第二张主世界地图。
- 架构更新基线：资源分布、木牌兑换、图纸、自建建筑、物品栏和接口调用链详见根目录 `PROJECT_ARCHITECTURE_UPDATE.md`。



## 九、三轮对标审核
- 玩法对标：是否保留轻松探索、资源拾取、微建造、入口确认、小游戏返回。
- 画面对标：是否能从画面上分辨木屋、森林、河流、空地、游戏机、远景光幕和像素模式。
- 实现对标：每个交互是否有触发条件、反馈、数据变化和失败处理。
- 详细评分标准：见 `doc/visual_alignment_standard.md`，80 分以上才允许进入实现阶段。
- 详细行为路径与技术拆分：见 `doc/interaction_technical_spec.md`。



## 十、每 10 分钟开发复核
- 复核当前实现是否仍服务“小地图闭环”，是否超范围做大地图或复杂系统。
- 复核依据：本文件、`doc/level_sketches.md`、`implementation_plan.md`。
- 记录方式：关键复核写入 `notes.txt` 或 `diff.md`。
- 停止条件：若出现技术栈变更、删除文件、规范文件被忽略、交互闭环被破坏，立即暂停并报告。
