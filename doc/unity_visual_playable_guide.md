# 星绪森林 Unity 视觉可玩版指导

## 目标

这份文档用于下一阶段 `stage-8-unity-visual-playable`。目标不是重做系统，而是把已经通过自动测试和构建验证的 Unity 原型，提升成“能打开、能跑、能截图、能看懂闭环”的视觉可玩版。

当前 Unity 已经具备：

- `WorldHub.unity` 单主地图骨架。
- `MiniGame01.unity` 像素小游戏骨架。
- `InventoryService`、`SignboardService`、`BuildService`、`CustomBuildService`、`MiniGameService`、`SaveService`。
- EditMode、PlayMode、Windows 构建和构建产物启动冒烟脚本。

下一阶段不能扩成多个大地图，也不能把网页演示反向搬进 Unity。Unity 是主实现，网页只是展示镜像。

## 视觉对标原则

只学习方法，不复制参考游戏画面、角色、UI、贴图、布局或素材。

用户补充参考图的原始目录是 `C:\Users\72440\Desktop\小游戏`。实现和验收时优先看仓库归档副本 `assets/reference/visual-benchmark/` 与 `assets/reference/protagonist/`；如果仓库归档缺失或需要核对细节，再回到桌面目录对照。

| 参考 | 取法 | 星绪森林落地 |
| --- | --- | --- |
| 动物森友会 | 家园生活动机、低压力引导、岛上摆放、公告牌/生活工具感 | 木屋是家，木牌像小镇记录，建设物围绕木屋和河岸布置 |
| 星露谷物语 | 家门口资源劳动、农场/小镇生活节奏、低压力日常循环 | 从木屋到森林/河岸的短距离采集，资源回到家园建设，不做任务强制路线 |
| 我的世界 | 材料来源清楚、材料到建设的因果链、放置反馈明确 | 森林/河岸/河水给基础材料，木牌换成长物资，建设成功后场景对象可见变化 |
| 崩坏：星穹铁道 | 现代电子入口、远景光幕、进入后规则变化 | 空地游戏机和不可达远景光幕有发光科技感，进入小游戏后从主世界采集规则切到像素贴纸规则 |

不能做：

- 不做战斗、抽卡、大剧情压过家园核心。
- 不做多个主世界地图。
- 不做完整体素沙盒。
- 不做强制 5 分钟任务链。
- 不把木牌 UI 做成任务列表或后台表格。

## 主角形象已确认

用户已指定主角形象，作为阶段 8 视觉基准：

```text
assets/reference/protagonist/aethel_ocean_oracle_protagonist_concept.png
SHA256: C587856FF8FAC00E315F47CE9FC41FDCE0641983DD132C20BBA715AB639D3FB6
```

必须保留的角色元素：

- 长黑发、紫黑服装、羽饰/贝壳/珊瑚感点缀。
- 漂浮灵鱼或水光小精灵。
- 安静、欢笑、激动三种表情。
- 与河岸、星屑灯芯、动态表情系统相关的视觉记忆点。

需要调整以融入游戏：

- 弱化战斗感和过度华丽细节，避免压过温和家园冒险。
- 控制紫色占比，主世界不能被紫蓝色调统治。
- 阶段 8 先做低成本可替换实现：世界中用低模代理体或竖版 Sprite/Card，UI 中使用三表情头像；暂不新增角色动作技术栈。

详细资产清单和验收见：

```text
doc/visual_playable_art_direction.md
```

## 新手怎么打开 Unity 工程

### 方法 A：Unity Hub 打开

1. 打开 Unity Hub。
2. 点击 `Add` 或 `Add project from disk`。
3. 选择项目目录：

```text
D:\app_project\game\xiangsu\unity
```

4. 等 Unity 导入完成。
5. 在 Unity 底部或左侧的 `Project` 面板里打开：

```text
Assets/Scenes/WorldHub.unity
```

6. 点击顶部中间的三角形 `Play`。

### 方法 B：命令行打开

```powershell
D:\unity\Editor\Unity.exe -projectPath D:\app_project\game\xiangsu\unity
```

### 第一次打开要等什么

第一次导入会慢，Unity 可能会编译脚本、导入 URP 设置、生成 Library。等右下角进度完成再点 Play。不要手动删除 `Packages`、`ProjectSettings`、`Assets` 或锁文件。

## 你在 Unity 里要认识的窗口

- `Project`：文件资源浏览器。找脚本、场景、Prefab、材质都在这里。
- `Hierarchy`：当前场景里的对象列表。比如 `Player`、`HomeCabin`、`ArcadeMachine`。
- `Scene`：编辑地图和摆对象的视图。
- `Game`：运行后玩家实际看到的画面。
- `Inspector`：选中对象后看组件和参数。
- `Console`：看错误、警告和日志。红色 error 必须先处理。

常用操作：

- 双击 `WorldHub.unity`：打开主世界。
- 选中 `Hierarchy` 里的 `Player`：右侧 Inspector 看 `PlayerController`。
- 选中 `HomeSignboard`：看 `WorldNodeInteractor` 里的 `nodeId` 是否是 `home-signboard`。
- 点击 `Play`：运行场景。
- 再点一次 `Play`：停止运行。

## 下一阶段文件结构

建议新增或补齐：

```text
unity/Assets/Prefabs/
  World/
    HomeCabin.prefab
    HomeSignboard.prefab
    ForestTree.prefab
    ForestBranchNode.prefab
    ForestFlowerSeedNode.prefab
    RiverStoneNode.prefab
    RiverShellNode.prefab
    RiverFishNode.prefab
    DamagedBridge.prefab
    RepairedBridge.prefab
    ArcadeMachine.prefab
    DistantLightScreen.prefab
  Building/
    FlowerBed.prefab
    ForestSign.prefab
    WoodFence.prefab
    CraftBench.prefab
    RiverLamp.prefab
    StickerWall.prefab
    ArcadeBase.prefab
    CustomBuilding_1x1.prefab
    CustomBuilding_2x2.prefab
  MiniGame/
    PixelPlayer.prefab
    PixelSticker.prefab
    PixelExitDoor.prefab
unity/Assets/Materials/
  World/
  Building/
  UI/
unity/Assets/Scripts/UI/
  HudView.cs
  InventoryView.cs
  SignboardView.cs
  BuildMenuView.cs
  PromptView.cs
unity/Assets/Scripts/Runtime/
  GameStateRunner.cs
  BuildPlacementView.cs
  StickerWallView.cs
  ArcadeMachineView.cs
```

便捷做法：先用 Unity 基础几何体组合 Prefab，不急着找外部素材。比如木屋用方体 + 斜屋顶，树用圆柱 + 球/低模冠，游戏机用方体 + Emission 屏幕。

## 第一轮视觉可玩版要做什么

### 1. 主世界画面升级

保留当前 `WorldHub` 布局，但把纯占位块变成有识别度的低模对象。

必须一眼看出来：

- 左上或左侧是木屋家园。
- 木屋前有木牌。
- 一侧有旁友森林，能看见树和可拾取资源。
- 地图中部或下方有河流、河岸和鱼影。
- 河上有半损坏木桥，修复后变化明显。
- 空地游戏机在另一侧，是现代电子入口。
- 背景有不可到达远景光幕，但不能抢走木屋家园核心。

最低实现：

- 给每个核心对象做独立 Prefab。
- 给每个 Prefab 加明确名称和材质。
- 关键交互对象继续挂 `WorldNodeInteractor`。
- `nodeId` 不能变，必须和服务层一致。

### 1.5. 阶段 8B 对标升级重点

阶段 8A 已经能操作，但画面还没有到用户提供的参考图水平。阶段 8B 先按 `doc/visual_benchmark_review_stage8.md` 修以下内容：

- 森林：主可见树木控制在 5-7 棵，做成“森林边缘 + 可进入缺口”，不做树阵或树墙。
- 河流：从蓝色长条升级为分段河道、浅滩、湿土边、水面高光；石子、河贝、鱼影分区摆放。
- 主角：代理体升级到 2.5D 角色牌或更细低模组合，64px 缩略图仍能看出黑发、紫黑衣、头饰和灵鱼。
- 木屋：必须有屋顶、门、窗、暖光、贴纸墙，成为主世界第一视觉锚点。
- 构图：1080p 截图必须同时读出木屋、木牌、主角、森林、河、桥、游戏机、远景光幕。

### 2. 材质与颜色

首版材质用 URP Lit 即可。

推荐颜色：

| 对象 | 方向 | 说明 |
| --- | --- | --- |
| 地面 | 柔和草绿 | 不要大片深蓝/紫色，不要单一色相 |
| 木屋 | 暖木色 + 深色屋顶 | 让玩家先识别“家” |
| 木牌 | 暖黄色木牌 | 像生活入口，不像后台菜单 |
| 森林 | 多层绿，少量花色 | 让木材/花种来源清楚 |
| 河水 | 蓝绿，轻微透明或高光 | 不做复杂水体 |
| 游戏机 | 深灰机身 + 青绿色 Emission | 体现现代电子入口 |
| 远景光幕 | 低透明青色 Emission | 只做远景提示，不可到达 |
| 像素小游戏 | 深色网格 + 高对比贴纸 | 进入后规则变化明显 |

不要使用 dominant purple/purple-blue gradient、纯米色/棕橙咖啡主题、深蓝单色主题。

### 3. UI 做成生活工具

Unity 首版 UI 不要照搬网页右侧大面板。建议屏幕上只常驻：

- 左上：简短地点/提示。
- 右上：小型 12 格背包按钮或展开面板。
- 靠近木牌：打开木牌面板。
- 靠近建设空位：显示“可放置 / 缺材料 / 图纸未解锁”。

木牌面板只做三块：

```text
今日可换
图纸记录
小镇记录
```

每条兑换显示：

```text
换旧卡带
需要：河贝 1 / 鱼 1
状态：可换 或 材料不足
```

不要写：

```text
任务 1：去收集河贝和鱼
任务 2：兑换旧卡带
```

### 4. 建设反馈

建设成功后必须有场景变化，不能只变数字。

建议机制：

1. 玩家靠近 `flower-bed-slot`。
2. UI 显示 `花圃：花种 2 / 石子 1`。
3. 点击放置。
4. 调用 `BuildService.CanPlace()`。
5. 成功时扣材料，隐藏空位对象，显示 `FlowerBed.prefab`。
6. 失败时显示短提示，不扣材料。

修桥同理：

- 未修：`DamagedBridge.prefab`。
- 成功：切换为 `RepairedBridge.prefab`。
- 修桥后图纸解锁：木栅栏、河岸灯。

### 5. 自建建筑反馈

建设 3 个图纸物品后：

- 木牌小镇记录显示：`自建已开启`。
- 建设 UI 新增：`自建小屋`。
- 首版只支持：
  - `CustomBuilding_1x1`
  - `CustomBuilding_2x2`

自建主题：

| 材料主题 | 外观建议 |
| --- | --- |
| Wood | 木板墙、暖色屋顶 |
| Stone | 石基座、灰色边框 |
| FlowerSeed | 花环、草绿色点缀 |
| RiverShell | 贝壳色墙面、蓝绿屋檐 |
| StarCore | 小发光窗、青色点光 |

不要做完整自由搭积木。要做“受控组合”，保证可存档、可测试、可美术统一。

### 6. 游戏机和像素小游戏

游戏机是菜单入口，不是强制路线。

主世界：

- 玩家发现游戏机后，记录 `KnownSystems`。
- 没有旧卡带：显示“旧卡带未就绪”。
- 有旧卡带：显示“进入像素小游戏”。

进入转场：

- 游戏机屏幕发光增强。
- 主画面短暂变暗 0.3 到 0.5 秒。
- 切换到 `MiniGame01.unity`。

小游戏：

- PixelPlayer。
- 3 个贴纸。
- 出口门。
- 收集 3 个贴纸后出口发光。
- 完成后回到 `WorldHub`，木屋贴纸墙 +1。

失败或退出：

- 不扣材料。
- 不丢旧卡带。
- 不结算贴纸墙。

## Codex 可以直接做的任务清单

建议分 5 个小提交做，每个小功能后跑验证。

### 任务 1：Prefab 和材质目录

Codex 可直接：

- 新建 `Prefabs/` 与 `Materials/` 目录。
- 用编辑器脚本生成基础低模 Prefab。
- 为木屋、木牌、树、桥、游戏机、远景光幕创建统一材质。

验收：

- Project 面板能看到 Prefab。
- 材质命名清楚。
- 没有外部素材许可证问题。

### 任务 2：WorldHubSceneBuilder 升级

Codex 可直接：

- 修改 `WorldHubSceneBuilder` 使用 Prefab 或更精细的几何体。
- 重新生成 `WorldHub.unity`。
- 保持所有 nodeId 不变。

验收：

- `WorldHubSceneTests` 仍通过。
- Scene 视图能明显看出木屋、木牌、森林、河流、桥、游戏机、光幕。

### 任务 3：运行态视图层

Codex 可直接：

- 新增 `GameStateRunner`，运行时创建并持有 `GameState`。
- 新增 `PromptView`，显示短提示。
- 新增 `BuildPlacementView`，按 `BuildService` 成功/失败切换空位和建设物。

验收：

- 点击或靠近节点能看到提示。
- 建设成功后场景对象变化。
- 材料不足不扣材料。

### 任务 4：生活工具 UI

Codex 可直接：

- 新增 `InventoryView`，显示 12 格物品栏。
- 新增 `SignboardView`，显示兑换、图纸、小镇记录。
- 新增 `BuildMenuView`，显示已解锁图纸和材料状态。

验收：

- UI 不是任务列表。
- 木牌打开后能看到初始图纸。
- 可换/材料不足状态来自服务层，不复制规则。

### 任务 5：视觉和构建验证

Codex 可直接：

- 跑 `tools/run-stage6-validation.ps1`。
- 用 Unity 或 Windows 构建产物截图。
- 记录到 `notes.txt` 和 `diff.md`。

验收：

- EditMode 44/44 仍通过。
- PlayMode 2/2 或新增测试通过。
- Windows 构建通过并能启动。
- 1080p 截图能达到 80 分原型标准。

## 你手动检查怎么做

### 打开主世界

1. 打开 Unity Hub。
2. 打开 `D:\app_project\game\xiangsu\unity`。
3. 在 Project 面板打开 `Assets/Scenes/WorldHub.unity`。
4. 看 Hierarchy 是否有：

```text
WorldHub
Player
HomeCabin
HomeSignboard
River
DamagedBridge
ArcadeMachine
DistantLightScreen_Unreachable
```

5. 点 Play。

### 阶段 8A 当前可操作方式

进入 `WorldHub.unity` 后：

```text
WASD / 方向键：移动主角
E：和附近对象互动
T：切换清晨 / 午后 / 夜晚提示
Esc：关闭木牌或游戏机菜单
```

推荐体验路径：

1. 从木屋旁主角位置出发，靠近 `HomeSignboard`，按 `E` 打开木牌，获得初始图纸。
2. 去森林边缘靠近 `ForestBranchNode` 和 `ForestFlowerSeedNode`，按 `E` 反复收集木材和花种。
3. 去河岸靠近 `RiverStoneNode`、`RiverShellNode`、`RiverFishNode`，按 `E` 收集石子、河贝和鱼。
4. 回到木牌，面板打开时按数字键兑换：

```text
1：木材 + 花种 -> 表情碎片
2：河贝 + 鱼 -> 旧卡带
3：石子 + 河贝 -> 星屑灯芯
4：鱼 + 表情碎片 -> 贴纸
```

5. 去 `DamagedBridge`、`FlowerBedSlot`、`ForestSignSlot` 按 `E` 建设；成功后空位会切换成建设物。
6. 建设 3 个图纸物品后，木牌记录显示自建开启。
7. 靠近 `ArcadeMachine` 按 `E` 打开游戏机菜单；有旧卡带后按 `Enter` 进入 `MiniGame01`。
8. 在小游戏中用 `WASD` 移动，靠近 3 个贴纸按 `E` 收集；收齐后靠近 `ExitDoor` 按 `E` 回主世界。
9. 回到主世界后，木屋旁 `StickerWall` 会显示新增贴纸。

当前阶段的主角是低成本视觉代理体：黑发、紫黑服装、羽饰/贝壳点缀和灵鱼已经先落到场景里，用于验证“能融入游戏”的方向；不是最终商用角色模型。

### 看资源

在 Project 面板里找：

```text
Assets/Prefabs
Assets/Materials
Assets/Scripts
Assets/Scenes
```

如果你不知道一个对象从哪里来：

1. 在 Hierarchy 选中它。
2. 看 Inspector 顶部名字。
3. 如果它是 Prefab，Inspector 顶部会有 Prefab 相关按钮或路径。

### 看脚本是否挂对

选中木牌或资源点，看 Inspector 里是否有：

```text
WorldNodeInteractor
Node Id: home-signboard
```

常见 nodeId：

```text
home-signboard
forest-branch
forest-flower-seed
river-stone
river-shell
river-fish
river-bridge
clearing-arcade
flower-bed-slot
forest-sign-slot
river-lamp-slot
arcade-base-slot
```

## 自动验证命令

在 PowerShell 里进入项目根目录：

```powershell
cd D:\app_project\game\xiangsu
```

只跑 EditMode：

```powershell
tools\run-unity-editmode-tests.ps1
```

只跑 PlayMode：

```powershell
tools\run-unity-playmode-tests.ps1
```

构建 Windows：

```powershell
tools\build-unity-windows.ps1
```

完整阶段 6 验证：

```powershell
tools\run-stage6-validation.ps1
```

注意：如果 Unity Editor 已经打开了 `D:\app_project\game\xiangsu\unity`，BatchMode 自动测试会因为项目锁失败。先保存并关闭这个 Unity 编辑器窗口，再运行上述命令。

如果测试失败：

1. 先读 `unity/Logs/editmode-test.log` 或 `unity/Logs/playmode-test.log`。
2. 优先修编译错误。
3. 再修测试失败。
4. 不要先扩大功能。

## AI 生成素材提示词模板

如果需要生成概念图或贴图，提示词必须包含项目关键词：

```text
现代情绪冒险，关卡式轻松平台，桌面原生，网页演示原型，动态表情包。
为原创游戏《星绪森林》生成低多边形温和家园主世界概念图：单主地图，左侧木屋，木屋前木牌，旁友森林，河岸与河水，半损坏木桥，空地游戏机，远处不可到达的青绿色现代光幕。风格参考方法来自家园生活、材料建设反馈、电子入口规则切换，但不复制任何现有游戏画面、角色、UI 或素材。色彩清爽，2K 游戏截图构图，适合 Unity URP 低模实现。
```

单物件提示词：

```text
现代情绪冒险，关卡式轻松平台，桌面原生，网页演示原型，动态表情包。
原创低多边形 Unity URP 游戏资产，木屋前小镇木牌，暖木色，圆角矩形标牌，生活工具感，不像任务列表，不包含文字，不复制现有 IP，适合《星绪森林》单主地图家园冒险。
```

进入工程前必须记录到：

```text
assets/licenses/asset_sources.md
```

## 80 分审核表

| 项目 | 分数 |
| --- | --- |
| 单主地图关系一眼可读：木屋、木牌、森林、河流、桥、游戏机、光幕 | 15 |
| 材料到建设反馈清楚：采集、兑换、建设、失败提示 | 15 |
| UI 像生活工具：背包、木牌、小镇记录，不像任务列表 | 15 |
| 游戏机入口有现代电子感，进入后规则变化明显 | 15 |
| 贴纸墙回收和家园成长可见 | 10 |
| 画面统一：颜色、比例、光照不混乱 | 15 |
| 自动测试、构建、启动验证通过 | 15 |

80 分以上才进入下一轮内容扩展。低于 80 分先修画面和反馈，不新增玩法。

## 下一步推荐执行顺序

1. 建分支：`stage-8-unity-visual-playable`。
2. 生成 Prefab 和材质目录。
3. 升级 `WorldHubSceneBuilder`。
4. 跑 EditMode 和 PlayMode。
5. 加运行态提示与建设可视反馈。
6. 跑完整 `tools/run-stage6-validation.ps1`。
7. 截图做 80 分审核。
8. 记录 `notes.txt` / `diff.md`。
9. 通过后提交推送。
