# diff.md

## 2026-04-12 变更记录
- 日期/时间：2026-04-12
- 涉及文件：`README.md`、`.gitignore`、`project_structure.md`、`doc/design_document.md`、`doc/level_sketches.md`、`notes.txt`、`assets/README.md`
- 核心 Diff 摘要：
  - 新增 `README.md` 与 `.gitignore`，补充项目架构、运行说明和资源忽略规则。
  - 调整项目结构与设计文档，明确《我的世界》《动物森友会》《崩坏：星穹铁道 4.0》三重参考。
  - 添加 `assets/README.md`，说明用户提供图像文件应存放位置。
  - 更新 `implementation_plan.md` 以反映当前设计方向。
- 修改意图：对齐用户最新游戏概念，并确保项目规划、文档与资源目录一致。

## 2026-04-12 追加修改
- 日期/时间：2026-04-12
- 涉及文件：`README.md`、`doc/design_document.md`、`doc/level_sketches.md`、`implementation_plan.md`、`notes.txt`、`index.html`
- 核心 Diff 摘要：
  - 细化世界场景为“木屋 + 森林 + 河流 + 空地游戏机”，规划更紧凑的小地图。
  - 明确主角拾取确认后启动像素小游戏，并退出回到主世界。
  - 将“每 10 分钟对标复核”写入实施计划和必要文档。
  - 更新概念演示页面内容，以符合最新场景与交互要求。
- 修改意图：让概念与用户要求完全对齐，减少设计粗糙度。

## 2026-04-12 对标设计与画面更新
- 日期/时间：2026-04-12
- 涉及文件：`README.md`、`project_structure.md`、`doc/design_document.md`、`doc/level_sketches.md`、`implementation_plan.md`、`notes.txt`、`index.html`、`style.css`、`app.js`
- 核心 Diff 摘要：
  - 将目标明确为 `Unity Windows x64` 与 `Unity macOS Universal` 两个桌面原生发布目标，加上网页概念交互演示。
  - 补充参考研究与取舍，明确《动物森友会》《我的世界》《崩坏：星穹铁道 4.0》和短流程探索游戏的可落地借鉴点。
  - 补齐主世界玩家行为路径、小游戏规则、UI 反馈、数据结构、游戏内接口、本地存档设计。
  - 增加三轮对标审核和每 10 分钟 review 标准。
  - 将网页概念页改为可交互地图：木屋、森林、河流、桥、空地游戏机、像素小游戏、情绪切换和流程推进。
- 修改意图：把“不要机械拆任务，要补全缺失设计并能落地”的要求转化为可实现的 0-1 项目设计和可视化概念页。

## 2026-04-12 文档归档与审核报告更新
- 日期/时间：2026-04-12
- 涉及文件：`README.md`、`implementation_plan.md`、`project_structure.md`、`notes.txt`、`index.html`、`style.css`、`doc/design_document.md`、`doc/interaction_technical_spec.md`、`doc/visual_alignment_standard.md`、`PROJECT_AUDIT_REPORT.md`
- 核心 Diff 摘要：
  - 将 README 调整为只讲游戏本身，不再展示对标分析、工程审计和文件索引。
  - 新增 `doc/visual_alignment_standard.md`，记录画面对齐方法、80 分通过标准、三角色 review 和 v1 自评分。
  - 新增 `doc/interaction_technical_spec.md`，记录玩家行为路径、状态变化、反馈、接口和 Unity 拆分建议。
  - 新增根目录 `PROJECT_AUDIT_REPORT.md`，按系统认知层、结构控制层、工程执行层和 1-15 项顺序输出项目审核报告。
  - 清理 `style.css`，移除旧样式叠加，保留单一概念演示样式。
  - 更新 `implementation_plan.md`，明确先推送 `main`，再切换并推送 `v1`，推送后等待用户确认再开始实现。
- 修改意图：把设计、画面、玩法和技术标准体系化沉淀，确保后续实现前有可审核、可评分、可执行的依据。

## 2026-04-12 玩家行为与家园建设更新
- 日期/时间：2026-04-12
- 涉及文件：`README.md`、`index.html`、`app.js`、`style.css`、`doc/design_document.md`、`doc/interaction_technical_spec.md`、`doc/level_sketches.md`、`doc/visual_alignment_standard.md`、`implementation_plan.md`、`notes.txt`
- 核心 Diff 摘要：
  - 将玩家行为从任务目标引导改为“发现信息 + 建设需求 + 自由探索”的表达。
  - 增加清晨、午后、夜晚三段可调时间，当前只影响画面氛围和提示，不引入复杂产出概率。
  - 补齐资源清单：木材、石子、花种、河贝、表情碎片、旧卡带、星屑灯芯、贴纸。
  - 补齐建设清单：木桥、花圃、木栅栏、工作台、河岸灯、贴纸墙、游戏机底座、林间路牌。
  - 增加 AI 生成素材和开源素材的审核规则：必须记录提示词或来源、许可证、用途，未授权素材不得入库。
- 修改意图：把用户提出的自由探索、家园建设、固定资源领域、可调时间和素材 review 要求转化为可实现的设计与技术规范。

## 2026-04-12 星绪森林命名与工程骨架更新
- 日期/时间：2026-04-12
- 涉及文件：`README.md`、`index.html`、`PROJECT_AUDIT_REPORT.md`、`implementation_plan.md`、`project_structure.md`、`.gitignore`、`assets/`、`game-data/`、`unity/`、`doc/README.md`
- 核心 Diff 摘要：
  - 游戏名称统一修改为《星绪森林》。
  - 项目定位明确为“不做大地图，但实现完整玩法并可扩展”。
  - 新增 Unity 工程骨架目录，包含场景、脚本、资源、ScriptableObject 配置入口。
  - 新增 `game-data/`，提前规划资源、建设配方和世界节点数据。
  - 新增素材投放目录、提示词、来源记录和开源素材候选来源文件。
  - 新增本地 `doc/unity_newbie_guide.md` 和 `doc/asset_pipeline.md`，并通过 `.gitignore` 设置 `doc/` 大多数详细文档不上传 git，仅保留 `doc/README.md` 索引。
- 修改意图：为后续正式实现打好工程结构、素材流程、Unity 新手协作流程和版本控制边界。

## 2026-04-12 新手运行调试指南更新
- 日期/时间：2026-04-12
- 涉及文件：`RUN_DEBUG_GUIDE.md`、`project_structure.md`
- 核心 Diff 摘要：
  - 新增根目录 `RUN_DEBUG_GUIDE.md`，说明网页演示查看、Unity 安装、免费使用资格核对、Unity 工程配合、2K/1080p 画质、4K 素材源文件、调试步骤和故障排查。
  - 将该指南加入项目结构说明。
- 修改意图：让没有游戏开发经验的个人开发者也能方便运行、查看、调试和逐步调整项目。

## 2026-04-12 v2 架构设计更新
- 日期/时间：2026-04-12
- 涉及文件：`PROJECT_ARCHITECTURE_UPDATE.md`、`README.md`、`project_structure.md`、`implementation_plan.md`、`notes.txt`、`doc/design_document.md`、`doc/interaction_technical_spec.md`、`unity/README.md`、`game-data/resources.json`、`game-data/build-recipes.json`、`game-data/world-nodes.json`、`game-data/item-catalog.json`、`game-data/exchange-recipes.json`、`game-data/blueprint-unlocks.json`
- 核心 Diff 摘要：
  - 新增 `PROJECT_ARCHITECTURE_UPDATE.md`，把“不做大地图”修正为不做多个主世界场景地图，而是在一个单主地图内做持续建设和摆放深度。
  - 补齐资源分布：木材、石子、花种、河贝和鱼来自森林、河岸或河水；表情碎片、旧卡带、星屑灯芯和贴纸主要通过木屋前木牌用物资兑换。
  - 增加木牌兑换、图纸赠送、建设 3 个物品后开启受控自建建筑物的交互路径、状态变化、反馈和接口调用链。
  - 增加物品栏规则：格子数 = 物品种类 + 3，v2 当前 9 类物品对应 12 格，每类物品上限 9999。
  - 新增 `game-data/item-catalog.json`、`game-data/exchange-recipes.json` 和 `game-data/blueprint-unlocks.json`，让后续 Unity 实现可直接映射 ScriptableObject 或 JSON 配置。
- 修改意图：把用户补充的资源、兑换、图纸、自建、物品栏和单主地图系统深度要求，整理为可执行的 PRD、交互规格、数据模型和初步工程架构。

## 2026-04-13 执行顺序与对标审核更新
- 日期/时间：2026-04-13
- 涉及文件：`implementation_plan.md`、`doc/visual_alignment_standard.md`、`PROJECT_ARCHITECTURE_UPDATE.md`、`notes.txt`
- 核心 Diff 摘要：
  - 将网页概念演示调整为最后阶段，在 Unity 核心系统、资源设计和画面方向稳定后再更新。
  - 将“三次对标审核通过标准”细化为 PRD/系分/测分、画面/UI/体验、实现/架构/自动测试三层。
  - 明确每 10 分钟 review 必须按动森、我的世界、星穹铁道的方法做偏差检查：动森看家园生活和温和引导，我的世界看材料到建设反馈，星穹铁道看电子入口、远景光幕和进入后规则变化。
  - 强化要求：用户点到为止的设计、环节和测试要求必须由实现者补齐交互、状态、接口、数据结构、失败分支和验收标准。
- 修改意图：防止实现只停留在机械拆分，确保后续 Unity 实现、UI、最终画面效果和体验都按对标质量门禁推进。

## 2026-04-13 Unity 核心服务续传
- 日期/时间：2026-04-13
- 涉及文件：`unity/Assets/Scripts/StarryForest.asmdef`、`unity/Assets/Tests/EditMode/StarryForest.Tests.EditMode.asmdef`、`unity/Assets/Scripts/World/Gathering/`、`unity/Assets/Tests/EditMode/WorldResourceTests.cs`、`notes.txt`
- 核心 Diff 摘要：
  - 新增运行时代码 asmdef 与 EditMode 测试 asmdef，让 Unity Test Runner 能发现 `CoreServicesTests` 和新增测试。
  - 确认 Unity 6000.4.2f1 下 BatchMode 测试命令去掉 `-quit` 后能生成 `unity/Logs/editmode-test-results.xml`。
  - 新增 `GatherService`，覆盖森林树枝、木屋旁落枝、花种、河岸石子、河贝和浅水河贝等基础材料采集点。
  - 新增 `FishingService`，覆盖河水鱼影和浅水鱼影钓鱼点。
  - 新增 4 个 EditMode 测试，覆盖采集成功、采集未知节点失败、钓鱼成功、非钓鱼节点失败；当前 EditMode 测试 8/8 Passed。
- 修改意图：补齐阶段 3 单主地图资源节点进入场景前的纯逻辑层，使“森林/河岸/河水 -> 基础材料 -> 木牌兑换/建设”的因果链可测试。

## 2026-04-13 体验节奏口径修正
- 日期/时间：2026-04-13
- 涉及文件：`implementation_plan.md`、`PROJECT_ARCHITECTURE_UPDATE.md`、`notes.txt`
- 核心 Diff 摘要：
  - 将“5 分钟闭环”从强制目标改为低压力体验校验，不作为任务限制或固定推进节奏。
  - 明确短流程探索游戏只借鉴目标可见、失败低惩罚和非强制引导方法。
  - 将游戏机入口调整为“发现后的菜单入口”，玩家可自主选择进入像素小游戏。
- 修改意图：避免把自由探索家园冒险做成限时任务链，保持动森式温和引导和玩家自主节奏。

## 2026-04-13 游戏机菜单入口逻辑
- 日期/时间：2026-04-13
- 涉及文件：`unity/Assets/Scripts/Core/PlayerState.cs`、`unity/Assets/Scripts/Core/GameConstants.cs`、`unity/Assets/Scripts/MiniGame/`、`unity/Assets/Tests/EditMode/MiniGameServiceTests.cs`、`notes.txt`
- 核心 Diff 摘要：
  - 为 `PlayerState` 增加 `KnownSystems`、`UnlockedMiniGames` 和 `ActiveMiniGameId`，用于表达发现入口、菜单可用和当前小游戏状态。
  - 新增 `MiniGameService`，支持发现游戏机菜单入口、开始小游戏、完成小游戏并回收贴纸墙奖励。
  - 新增 `MiniGameResult`，记录小游戏 ID、是否成功和贴纸收集数量。
  - 新增 4 个 EditMode 测试，覆盖发现菜单入口、未发现入口不能开始、缺少旧卡带不能开始、成功完成后贴纸墙更新。
  - 当前 EditMode 测试 12/12 Passed。
- 修改意图：把游戏机从固定路线任务节点调整为“发现后菜单入口”，保持玩家自主选择，同时保留旧卡带和贴纸墙的状态闭环。

## 2026-04-13 WorldNodeService 节点路由
- 日期/时间：2026-04-13
- 涉及文件：`unity/Assets/Scripts/World/Nodes/WorldNodeService.cs`、`unity/Assets/Tests/EditMode/WorldNodeServiceTests.cs`、`notes.txt`
- 核心 Diff 摘要：
  - 新增 `WorldNodeService.Interact(state, nodeId)`，将世界节点交互路由到采集、钓鱼或游戏机菜单发现。
  - 新增 4 个 EditMode 测试，覆盖资源节点、钓鱼节点、游戏机发现和未知节点失败分支。
  - 当前 EditMode 测试 16/16 Passed。
- 修改意图：为后续 `WorldHub` 单主地图的 MonoBehaviour 节点接入提供稳定服务层，避免把业务状态写死在场景对象里。

## 2026-04-13 SaveService 与 TimeService
- 日期/时间：2026-04-13
- 涉及文件：`unity/Assets/Scripts/Save/`、`unity/Assets/Scripts/World/TimeService.cs`、`unity/Assets/Tests/EditMode/SaveServiceTests.cs`、`unity/Assets/Tests/EditMode/TimeServiceTests.cs`、`notes.txt`
- 核心 Diff 摘要：
  - 新增 `SaveService` 与 `SaveLoadResult`，使用显式 DTO 保存 `PlayerState` 的物品、图纸、已放置建设物、已发现系统、已解锁小游戏、当前小游戏、建设计数、自建解锁、已完成小游戏和贴纸墙数量。
  - 新增本地 JSON round-trip 测试和缺失存档失败测试。
  - 新增 `TimeService`，支持清晨、午后、夜晚三段切换和氛围提示，不影响库存或资源产出。
  - 新增 2 个时间状态测试；当前 EditMode 测试 22/22 Passed。
- 修改意图：补齐阶段 2/3 的存档落点和时间状态接口，让后续 UI 与场景节点接入时有可测服务，而不是只靠临时场景对象保存状态。

## 2026-04-13 Unity EditMode 自动测试脚本
- 日期/时间：2026-04-13
- 涉及文件：`tools/run-unity-editmode-tests.ps1`、`notes.txt`
- 核心 Diff 摘要：
  - 新增 PowerShell 自动测试脚本，默认使用 `D:\unity\Editor\Unity.exe` 和仓库内 `unity/` 工程。
  - 脚本运行 `-batchmode -runTests -testPlatform editmode`，不附加 `-quit`，因为 Unity Test Runner 会在完成后自行退出。
  - 脚本等待 `unity/Logs/editmode-test-results.xml` 写入并解析 XML，通过失败数决定退出码。
  - 已验证脚本输出 `Unity EditMode tests: 22/22 passed, 0 failed.`。
- 修改意图：把已确认可用的测试命令固化为自动化入口，减少后续阶段误用命令导致“退出码 0 但没有测试 XML”的风险。

## 2026-04-13 GameState 与 WorldNodeInteractor
- 日期/时间：2026-04-13
- 涉及文件：`unity/Assets/Scripts/Core/GameState.cs`、`unity/Assets/Scripts/World/Nodes/WorldNodeInteractor.cs`、`unity/Assets/Tests/EditMode/GameStateTests.cs`、`unity/Assets/Tests/EditMode/WorldNodeInteractorTests.cs`、`notes.txt`
- 核心 Diff 摘要：
  - 新增 `GameState`，统一持有 `PlayerState` 以及库存、木牌、图纸、建设、自建、采集、钓鱼、小游戏、世界节点、时间和存档服务。
  - 新增 `WorldNodeInteractor` MonoBehaviour，场景节点只配置 `nodeId`、显示名和提示语，再把交互转发给 `GameState`。
  - 新增 4 个 EditMode 测试，覆盖 `GameState` 世界节点路由、小游戏菜单入口闭环、未配置节点失败和已配置节点成功转发。
  - 自动脚本验证 EditMode 测试 26/26 Passed。
- 修改意图：为阶段 3 `WorldHub` 单主地图节点接入准备薄组件层，保持业务逻辑集中在可测试服务中。

## 2026-04-13 木牌入口与初始图纸
- 日期/时间：2026-04-13
- 涉及文件：`unity/Assets/Scripts/Signboard/SignboardService.cs`、`unity/Assets/Scripts/Core/GameConstants.cs`、`unity/Assets/Scripts/Core/GameState.cs`、`unity/Assets/Scripts/World/Nodes/WorldNodeService.cs`、`unity/Assets/Tests/EditMode/CoreServicesTests.cs`、`unity/Assets/Tests/EditMode/WorldNodeServiceTests.cs`、`notes.txt`
- 核心 Diff 摘要：
  - 新增 `GameConstants.SignboardSystemId`。
  - 新增 `SignboardService.Open`，打开木牌时写入 `KnownSystems` 并赠送木桥、花圃、林间路牌初始图纸。
  - `GameState` 复用同一个 `BlueprintService` 初始化木牌与建设服务，避免图纸解锁状态分裂。
  - `WorldNodeService` 接入 `home-signboard` 节点路由。
  - 新增 2 个 EditMode 测试，覆盖木牌打开和世界节点路由；自动脚本验证 EditMode 28/28 Passed。
- 修改意图：把木屋前木牌落地为温和系统入口和图纸赠送点，不做任务列表，同时让后续场景节点可以直接触发。

## 2026-04-13 建设放置失败分支
- 日期/时间：2026-04-13
- 涉及文件：`unity/Assets/Scripts/Building/BuildService.cs`、`unity/Assets/Tests/EditMode/CoreServicesTests.cs`、`notes.txt`
- 核心 Diff 摘要：
  - 新增 `BuildService.CanPlace`，统一检查负数网格、图纸是否解锁、建设配方是否存在、目标格是否占用、材料是否足够。
  - `BuildService.Place` 改为先检查再扣材料，失败分支不消耗物资、不增加 `BuiltCount`。
  - 新增 2 个 EditMode 测试，覆盖占用格拒绝和负数网格拒绝。
  - 自动脚本验证 EditMode 30/30 Passed。
- 修改意图：补齐单主地图摆放的最低安全规则，避免建设只成为菜单数值变化，保证放置反馈和状态写入可控。

## 2026-04-13 自建建筑受控放置
- 日期/时间：2026-04-13
- 涉及文件：`unity/Assets/Scripts/Building/CustomBuildService.cs`、`unity/Assets/Scripts/Core/GameState.cs`、`unity/Assets/Tests/EditMode/CoreServicesTests.cs`、`notes.txt`
- 核心 Diff 摘要：
  - `CustomBuildService` 改为依赖 `InventoryService`，创建自建建筑时按材料主题和尺寸消耗资源。
  - 新增解锁、空配置、负数网格、占位和材料不足检查；失败时不扣材料、不写入建设物。
  - 成功时写入结构化 `CustomBuildingData` 并增加 `BuiltCount`。
  - 新增 2 个 EditMode 测试，覆盖主题材料消耗和占位失败不扣材料。
  - 自动脚本验证 EditMode 32/32 Passed。
- 修改意图：让“受控自建建筑物”符合单主地图摆放、材料因果链和存档结构要求，而不是无成本临时装饰。

## 2026-04-13 图纸进度赠送
- 日期/时间：2026-04-13
- 涉及文件：`unity/Assets/Scripts/Building/BlueprintService.cs`、`unity/Assets/Scripts/Building/BuildService.cs`、`unity/Assets/Scripts/MiniGame/MiniGameService.cs`、`unity/Assets/Scripts/Core/GameState.cs`、`unity/Assets/Tests/EditMode/CoreServicesTests.cs`、`unity/Assets/Tests/EditMode/MiniGameServiceTests.cs`、`notes.txt`
- 核心 Diff 摘要：
  - `BlueprintService` 新增修桥后图纸赠送和小游戏完成后图纸赠送。
  - `BuildService` 放置木桥后解锁木栅栏与河岸灯图纸。
  - `MiniGameService` 成功完成小游戏后解锁贴纸墙与游戏机底座图纸。
  - 新增/更新 EditMode 测试覆盖图纸赠送；自动脚本验证 EditMode 33/33 Passed。
- 修改意图：让图纸机制从初始赠送扩展到进度触发，支撑温和引导和家园持续建设，不使用任务列表推进。

## 2026-04-13 木牌兑换可用性查询
- 日期/时间：2026-04-13
- 涉及文件：`unity/Assets/Scripts/Signboard/SignboardService.cs`、`unity/Assets/Tests/EditMode/CoreServicesTests.cs`、`notes.txt`
- 核心 Diff 摘要：
  - 新增 `SignboardService.GetExchangeRecipes`，为后续木牌 UI 提供兑换项列表。
  - 新增 `SignboardService.CanExchange`，根据库存判断兑换项是否可用。
  - 新增 EditMode 测试覆盖材料不足和材料满足时的可兑换状态；自动脚本验证 EditMode 34/34 Passed。
- 修改意图：让木牌 UI 后续只呈现生活化“可换/材料不足”状态，不复制业务规则，不退化成任务清单或后台表格。

## 2026-04-13 WorldHub 单主地图占位场景
- 日期/时间：2026-04-13
- 涉及文件：`unity/Assets/Editor/WorldHubSceneBuilder.cs`、`unity/Assets/Scenes/WorldHub.unity`、`unity/Assets/Tests/EditMode/WorldHubSceneTests.cs`、`notes.txt`
- 核心 Diff 摘要：
  - 新增 `WorldHubSceneBuilder`，可重复生成 `Assets/Scenes/WorldHub.unity`。
  - 场景包含木屋、木屋前木牌、森林树群、森林树枝、森林花种、河流、河岸石子、河贝、鱼影、半损坏木桥、空地游戏机、不可到达远景光幕、主相机和方向光。
  - 关键交互对象挂载 `WorldNodeInteractor`，nodeId 与服务层保持一致。
  - 新增 2 个 EditMode 场景结构测试，覆盖核心地标和交互节点配置；自动脚本验证 EditMode 36/36 Passed。
- 修改意图：进入阶段 3 主世界单地图可玩骨架，用占位素材先验证空间关系、节点配置和状态接入，不提前追求最终美术，不更新网页。

## 2026-04-13 主角移动骨架
- 日期/时间：2026-04-13
- 涉及文件：`unity/Assets/Scripts/Player/PlayerController.cs`、`unity/Assets/Editor/WorldHubSceneBuilder.cs`、`unity/Assets/Scenes/WorldHub.unity`、`unity/Assets/Tests/EditMode/PlayerControllerTests.cs`、`unity/Assets/Tests/EditMode/WorldHubSceneTests.cs`、`notes.txt`
- 核心 Diff 摘要：
  - 新增 `PlayerController`，支持按输入向量在 XZ 平面移动，并钳制输入强度。
  - `WorldHubSceneBuilder` 在木屋附近生成 Player 占位体并挂载 `PlayerController`。
  - 更新 `WorldHub` 场景结构测试，确认 Player 和组件存在。
  - 新增 2 个 EditMode 移动测试；自动脚本验证 EditMode 38/38 Passed。
- 修改意图：补齐阶段 3 主世界可玩骨架的最小主角控制基础，暂不接复杂输入系统或最终动画。

## 2026-04-13 靠近交互提示
- 日期/时间：2026-04-13
- 涉及文件：`unity/Assets/Scripts/World/Nodes/WorldNodeInteractor.cs`、`unity/Assets/Tests/EditMode/WorldNodeInteractorTests.cs`、`notes.txt`
- 核心 Diff 摘要：
  - `WorldNodeInteractor` 新增交互半径、范围判断和提示获取。
  - 新增 EditMode 测试，覆盖靠近时返回提示、远离时不显示提示。
  - 自动脚本验证 EditMode 39/39 Passed。
- 修改意图：为阶段 3 的靠近提示和温和引导打基础，让节点提示是场景生活短句，不是任务列表。

## 2026-04-13 建设节点接入 WorldHub
- 日期/时间：2026-04-13
- 涉及文件：`unity/Assets/Scripts/World/Nodes/WorldNodeService.cs`、`unity/Assets/Editor/WorldHubSceneBuilder.cs`、`unity/Assets/Scenes/WorldHub.unity`、`unity/Assets/Tests/EditMode/WorldNodeServiceTests.cs`、`unity/Assets/Tests/EditMode/WorldHubSceneTests.cs`、`notes.txt`
- 核心 Diff 摘要：
  - `WorldNodeService` 接入建设节点：`river-bridge`、`flower-bed-slot`、`forest-sign-slot`、`craft-bench-slot`、`river-lamp-slot`、`arcade-base-slot`。
  - `WorldHubSceneBuilder` 把半损坏木桥改为可交互建设节点，并新增花圃、林间路牌、河岸灯和游戏机底座空位。
  - 更新场景结构测试和世界节点测试；自动脚本验证 EditMode 40/40 Passed。
- 修改意图：让阶段 4 的建设闭环开始进入单主地图节点层，保持“材料 -> 图纸 -> 放置”的因果链，不做任务列表。

## 2026-04-13 木牌菜单快照
- 日期/时间：2026-04-13
- 涉及文件：`unity/Assets/Scripts/Signboard/SignboardMenuSnapshot.cs`、`unity/Assets/Scripts/Signboard/SignboardService.cs`、`unity/Assets/Tests/EditMode/CoreServicesTests.cs`、`notes.txt`
- 核心 Diff 摘要：
  - 新增 `SignboardMenuSnapshot` 和 `ExchangeRecipeAvailability`，表达木牌 UI 所需的小镇记录、兑换项可用性、已解锁图纸和自建入口状态。
  - `SignboardService.GetMenuSnapshot` 统一生成快照，避免后续 UI 复制配方和状态判断。
  - 新增 EditMode 测试覆盖建设数量、自建解锁、图纸和兑换可用性；自动脚本验证 EditMode 41/41 Passed。
- 修改意图：为木牌生活化 UI 提供稳定数据接口，让它像小镇工具和记录，而不是任务清单或后台表格。

## 2026-04-13 阶段 4 提交与推送状态
- 日期/时间：2026-04-13
- 涉及文件：`notes.txt`
- 核心 Diff 摘要：
  - 阶段 4 本地分支 `stage-4-build-signboard-loop` 已提交 `2dc792c Connect build nodes and signboard menu`。
  - 提交前自动脚本验证 EditMode 41/41 Passed。
  - 推送远端连续两次失败，错误为 SSH `kex_exchange_identification: read: Software caused connection abort`。
- 修改意图：记录阶段提交已完成但远端推送受网络/SSH 连接阻塞，便于后续恢复时继续。

## 2026-04-13 续传执行口径记录
- 日期/时间：2026-04-13
- 涉及文件：`notes.txt`、`diff.md`
- 核心 Diff 摘要：
  - 记录用户确认的续传方式：休息期间继续推进 Unity 主线；最终阶段按 `implementation_plan.md` 的计划最终阶段理解。
  - 记录中间阶段节奏：实现、最小测试、review 后再进入提交处理。
  - 记录上下文或额度边界处理：先写入当前阶段、变更文件、测试命令、测试结果和下一步；后续恢复先读取根目录必读文档。
- 修改意图：保证长时间续传时不会丢失项目口径、测试状态和下一步任务。
