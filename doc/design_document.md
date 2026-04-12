# 游戏设计规划文档



## 一、项目定位
- 项目名称：表情探索：情绪小镇。
- 类型：现代情绪冒险 + 关卡式轻松平台 + 小地图探索。
- 目标产出：`Unity Windows x64` 桌面原生版、`Unity macOS Universal` 桌面原生版、`HTML + CSS + JavaScript` 网页概念演示。
- MVP 范围：不做大地图，只做木屋、旁友森林、河流、半损坏木桥、空地游戏机和一个像素小游戏。
- 核心闭环：木屋出发 -> 森林拾取 -> 修桥过河 -> 确认游戏机 -> 像素小游戏 -> 返回主世界 -> 贴纸墙更新。



## 二、参考研究与取舍
- 《动物森友会：新视野》：提取小岛生活、家园起点、户外布置、桥和坡道式连接、温和互动。落地为木屋安全区、短距离散步动线、清楚的节点提示。来源：https://animalcrossing.nintendo.com/new-horizons/create/
- 《我的世界》：提取木屋、自然资源、采集、可读边界和微建造。落地为森林拾取木材、河边修桥、方块化资源轮廓。来源：https://www.minecraft.net/en-us/about-minecraft
- 《崩坏：星穹铁道 4.0》：提取现代光幕、电子屏、入口后规则变化。落地为不可到达远景、游戏机屏幕、像素模式转场。来源：https://blog.playstation.com/2026/02/06/honkai-star-rail-version-4-0-no-aha-at-full-moon-will-go-live-on-february-13/
- 短流程探索游戏：提取目标可见、路径短、失败不惩罚。落地为 3 分钟内完成的贴纸小游戏。来源：https://ashorthike.com/



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



### 像素小游戏
- 场景：一屏横向小关，像素主角、网格平台、贴纸、出口门。
- 目标：收集 3 个贴纸后到达出口。
- 情绪能力：欢笑短冲刺，安静短漂浮，激动高跳触发按钮。
- 失败处理：掉落后回到最近平台，不扣资源，不强制重开。



## 五、玩家行为路径

1. 玩家在木屋门口出生，HUD 显示“去空地看看发光的游戏机”。
2. 玩家进入森林，拾取木材和表情碎片，资源飞入 HUD。
3. 玩家到河边，如果木材不足提示回森林；如果木材足够，确认修桥。
4. 玩家过桥到空地，靠近游戏机，弹出“拾起游戏卡带，进入像素模式？”。
5. 玩家确认后转场，HUD 切到像素布局，主角由圆形动态表情变为方块像素表情。
6. 玩家在小游戏中收集 3 个贴纸并到达出口。
7. 游戏返回空地，游戏机点亮；玩家回到木屋，贴纸墙新增贴纸。



## 六、UI 与反馈
- 主世界 HUD：当前位置、目标、木材/贴纸/碎片计数、当前可用按键。
- 情绪栏：欢笑、安静、激动三枚按钮，选中态只改变边框和提示，保持布局稳定。
- 交互提示：角色或节点附近显示短句，例如“拾取木材”“修复木桥”“进入像素模式”。
- 完成反馈：贴纸飞入 HUD、游戏机灯带点亮、木屋贴纸墙新增图标。



## 七、数据结构与接口



```ts
type EmotionMode = "joy" | "calm" | "hype";

interface PlayerState {
    positionId: string;
    emotion: EmotionMode;
    inventory: { wood: number; stickers: number; shards: number };
    unlockedMiniGames: string[];
    completedMiniGames: string[];
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
- `MiniGameService.start(miniGameId, playerState)`：记录入口位置并切换像素小游戏。
- `MiniGameService.finish(result)`：结算贴纸并返回主世界。
- `SaveService.save(slotId, playerState)` / `SaveService.load(slotId)`：本地存档读写。



## 八、技术架构
- 桌面原生版本 1：`Unity Windows x64`，优先实现第三视角小地图、节点交互、场景切换、本地存档。
- 桌面原生版本 2：`Unity macOS Universal`，同一 Unity 工程多平台打包，保持功能一致。
- 网页概念演示：用于确认画面构图、交互路径和状态变化，不替代最终桌面版。
- 存档：MVP 不接入服务器数据库；Unity 使用本地 JSON，网页演示使用 `localStorage`。



## 九、三轮对标审核
- 玩法对标：是否保留轻松探索、资源拾取、微建造、入口确认、小游戏返回。
- 画面对标：是否能从画面上分辨木屋、森林、河流、空地、游戏机、远景光幕和像素模式。
- 实现对标：每个交互是否有触发条件、反馈、数据变化和失败处理。



## 十、每 10 分钟开发复核
- 复核当前实现是否仍服务“小地图闭环”，是否超范围做大地图或复杂系统。
- 复核依据：本文件、`doc/level_sketches.md`、`implementation_plan.md`。
- 记录方式：关键复核写入 `notes.txt` 或 `diff.md`。
- 停止条件：若出现技术栈变更、删除文件、规范文件被忽略、交互闭环被破坏，立即暂停并报告。
