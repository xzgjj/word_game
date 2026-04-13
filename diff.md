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

## 2026-04-13 小游戏贴纸闭环与 MiniGame01 场景
- 日期/时间：2026-04-13
- 涉及文件：`unity/Assets/Scripts/Core/GameConstants.cs`、`unity/Assets/Scripts/Core/PlayerState.cs`、`unity/Assets/Scripts/MiniGame/MiniGameService.cs`、`unity/Assets/Scripts/Save/SaveService.cs`、`unity/Assets/Editor/WorldHubSceneBuilder.cs`、`unity/Assets/Scenes/MiniGame01.unity`、`unity/Assets/Tests/EditMode/MiniGameServiceTests.cs`、`unity/Assets/Tests/EditMode/MiniGameSceneTests.cs`、`unity/Assets/Tests/EditMode/SaveServiceTests.cs`、`unity/Assets/Tests/EditMode/GameStateTests.cs`、`notes.txt`
- 核心 Diff 摘要：
  - 新增 `GameConstants.MiniGameStickerTarget = 3` 和 `PlayerState.ActiveMiniGameStickerCount`。
  - `MiniGameService` 新增贴纸收集、出口条件判断、成功前贴纸目标校验；开始小游戏时重置计数，失败/手动返回不结算贴纸。
  - `SaveService` 同步保存/读取当前小游戏贴纸计数。
  - `WorldHubSceneBuilder` 新增 `BuildMiniGame01Scene`，生成像素玩家、3 个贴纸、出口门和平台的占位小游戏场景。
  - 新增/更新 EditMode 测试；自动脚本验证 EditMode 44/44 Passed。
- 修改意图：把“旧卡带/游戏机菜单 -> 像素小游戏 -> 收集贴纸 -> 回木屋贴纸墙”的核心奖励闭环落到可测试状态机和占位场景。

## 2026-04-13 阶段 5 提交与推送状态
- 日期/时间：2026-04-13
- 涉及文件：`notes.txt`
- 核心 Diff 摘要：
  - 阶段 5 本地分支 `stage-5-minigame-reward-loop` 已提交 `cf609c1 Add mini game sticker loop`。
  - 提交前自动脚本验证 EditMode 44/44 Passed。
  - 推送远端失败，错误为 SSH `Connection closed by 198.18.0.84 port 22`。
- 修改意图：记录阶段 5 本地完成但远端推送受网络/SSH 连接阻塞，后续需重试阶段 4 和阶段 5 分支推送。

## 2026-04-13 续传执行口径记录
- 日期/时间：2026-04-13
- 涉及文件：`notes.txt`、`diff.md`
- 核心 Diff 摘要：
  - 记录用户确认的续传方式：休息期间继续推进 Unity 主线；最终阶段按 `implementation_plan.md` 的计划最终阶段理解。
  - 记录中间阶段节奏：实现、最小测试、review 后再进入提交处理。
  - 记录上下文或额度边界处理：先写入当前阶段、变更文件、测试命令、测试结果和下一步；后续恢复先读取根目录必读文档。
- 修改意图：保证长时间续传时不会丢失项目口径、测试状态和下一步任务。

## 2026-04-13 阶段 6 自动构建与质量验证
- 日期/时间：2026-04-13
- 涉及文件：`tools/build-unity-windows.ps1`、`tools/run-unity-playmode-tests.ps1`、`tools/run-windows-build-smoke.ps1`、`tools/run-stage6-validation.ps1`、`unity/Assets/Editor/BuildAutomation.cs`、`unity/Assets/Tests/PlayMode/`、`unity/Assets/Settings/`、`unity/ProjectSettings/ProjectSettings.asset`、`unity/unity.slnx`、`notes.txt`
- 核心 Diff 摘要：
  - 新增 `BuildAutomation.BuildWindows`，通过 Unity `BuildPipeline` 构建 `WorldHub` 与 `MiniGame01` 到 `Builds/Windows/StarryForest.exe`，并在构建失败时抛出异常。
  - 新增 PlayMode 冒烟测试，运行态加载 `WorldHub` 和 `MiniGame01`，确认主角、木牌、游戏机、远景光幕、贴纸和出口门存在。
  - 新增 PlayMode 测试脚本、Windows 构建脚本、Windows 构建产物启动冒烟脚本和统一阶段 6 验证脚本。
  - 修正构建脚本竞态：启动前删除旧日志，避免读取上一轮成功标记；以本轮日志成功标记和构建产物存在作为通过条件。
  - 保留 Unity 6/URP 导入补全的渲染与 Standalone batching 配置；Unity Services 开启副作用已改回关闭。
  - 统一验证已通过：EditMode 44/44 Passed，PlayMode 2/2 Passed，Windows x64 构建生成，构建产物启动冒烟通过。
- 修改意图：完成阶段 6 自动测试、构建和产物可启动验证，让后续提交前有单一稳定质量门禁入口，同时保持网页概念演示最后更新。

## 2026-04-13 阶段 7 网页最终概念对齐
- 日期/时间：2026-04-13
- 涉及文件：`index.html`、`style.css`、`app.js`、`assets/reference/visual_reference_sources.md`、`notes.txt`
- 核心 Diff 摘要：
  - 在 Unity 阶段 6 验证通过后更新网页概念演示，保持“网页最后、跟随 Unity”的顺序。
  - 将网页从旧的木材/贴纸简化演示改为同步 Unity 当前核心系统：12 格物品栏、9 类物品、木牌兑换、初始图纸、修桥后图纸、小游戏后图纸、建设 3 个物品后自建解锁。
  - 将游戏机表达为发现后的菜单入口，旧卡带满足后玩家自主进入像素小游戏，不做强制 5 分钟流程。
  - 像素小游戏保留 3 个贴纸、出口点亮、回主世界后贴纸墙 +1 和后续图纸写入木牌。
  - 记录参考来源：动森取个人岛屿家园与 DIY 资源链，Minecraft 取资源到建设反馈，星穹铁道取星际电子入口和进入后规则变化；未复制画面或素材。
  - Chrome 验证：无控制台错误；脚本闭环结果为建设 3/3、自建已开启、贴纸墙 1、游戏机菜单可自主进入；Lighthouse snapshot Accessibility 83、Best Practices 100。
- 修改意图：让网页最终概念演示跟随 Unity 已跑通系统表达同一套核心闭环，同时保留对标方法而不复制参考游戏画面。

## 2026-04-13 阶段 8 Unity 视觉可玩版规划
- 日期/时间：2026-04-13
- 涉及文件：`doc/unity_visual_playable_guide.md`、`doc/README.md`、`implementation_plan.md`、`notes.txt`、`diff.md`
- 核心 Diff 摘要：
  - 新增 Unity 视觉可玩版执行手册，面向没做过游戏的新手和后续 Codex 实现。
  - 文档细化 Unity Hub 打开项目、查看 Project/Hierarchy/Scene/Game/Inspector/Console、找到场景和资产、运行 Play、使用自动验证脚本的步骤。
  - 文档补齐 Prefab/Materials 建议目录、主世界视觉升级、生活工具 UI、建设反馈、自建建筑、游戏机入口和像素小游戏的实现顺序。
  - 文档加入 AI 生成素材提示词模板，包含项目要求关键词，并要求所有素材进入工程前记录许可证和用途。
  - `implementation_plan.md` 新增阶段 8：Unity 视觉可玩版，明确任务清单、验收标准和 80 分审核要求。
- 修改意图：让下一阶段能直接从文档进入 Unity 实现和验证，而不是停在抽象建议；同时保证对标只取方法、不复制参考游戏画面。

## 2026-04-13 阶段 8 主角与美术方向确认
- 日期/时间：2026-04-13
- 涉及文件：`assets/reference/protagonist/aethel_ocean_oracle_protagonist_concept.png`、`assets/licenses/asset_sources.md`、`assets/reference/visual_reference_sources.md`、`doc/visual_playable_art_direction.md`、`doc/unity_visual_playable_guide.md`、`doc/README.md`、`implementation_plan.md`、`notes.txt`
- 核心 Diff 摘要：
  - 将用户提供的主角概念图复制到素材参考目录，记录 SHA256、用途和发布前授权复核要求。
  - 新增视觉可玩版美术方向文档，确认主角必须保留长黑发、紫黑服装、羽饰/贝壳点缀、漂浮灵鱼和安静/欢笑/激动三表情。
  - 将动森与星露谷的生活节奏、家园资源循环方法纳入 Unity 视觉升级参考，但明确不复制画面、角色、UI 或素材。
  - 补齐主世界单地图构图、主角功能映射、资产清单、AI 生成提示词、阶段 8A/8B/8C 实现路线和 100 分验收表。
  - 更新阶段 8 计划和 Unity 指导文档，使后续实现可直接按主角图、资产清单和验收标准推进。
- 修改意图：把用户“主角用这一个，其余由实现者补全”的要求转化为可执行的美术、资产、UI 和 Unity 实现标准。

## 2026-04-13 阶段 8A Unity 视觉可玩版实现
- 日期/时间：2026-04-13
- 涉及文件：`unity/Assets/Editor/WorldHubSceneBuilder.cs`、`unity/Assets/Scenes/WorldHub.unity`、`unity/Assets/Scenes/MiniGame01.unity`、`unity/Assets/Scripts/Runtime/`、`unity/Assets/Scripts/StarryForest.asmdef`、`unity/Assets/Tests/EditMode/WorldHubSceneTests.cs`、`unity/Assets/Tests/EditMode/MiniGameSceneTests.cs`、`unity/Assets/Tests/PlayMode/SceneSmokeTests.cs`、`doc/unity_visual_playable_guide.md`、`implementation_plan.md`、`notes.txt`
- 核心 Diff 摘要：
  - 新增运行态视觉可玩层：`GameStateRunner`、`HudView`、`BuildPlacementView`、`StickerWallView`、`ArcadeMachineView`。
  - 操作方式落地为 `WASD/方向键` 移动、`E` 互动、木牌 `1-4` 兑换、游戏机菜单 `Enter` 进入小游戏、小游戏 `E` 收集贴纸和回家。
  - `WorldHubSceneBuilder` 升级主世界：主角代理体包含黑发、紫黑服装、羽饰/贝壳点缀和灵鱼；木屋、木牌、森林、河流、桥、游戏机、远景光幕、贴纸墙和建设空位可读性增强。
  - 建设成功后通过 `BuildPlacementView` 显示已建设对象，贴纸墙通过 `StickerWallView` 显示小游戏回收成果，游戏机屏幕通过 `ArcadeMachineView` 根据菜单发现/旧卡带状态发光。
  - `MiniGame01` 升级为可操作的像素贴纸场景，包含 Aethel 像素代理、灵鱼、3 个贴纸和出口门。
  - 补充 EditMode/PlayMode 场景结构测试，覆盖运行态组件和主角/小游戏视觉代理对象。
- 验证状态：
  - Unity 批处理场景生成成功，`stage8-build-worldhub.log` 与 `stage8-build-minigame.log` 显示脚本编译和退出成功，无 C# 编译错误。
  - `git diff --check` 已通过；Unity 生成场景 YAML 的行尾空格已清理，仅剩仓库 LF/CRLF 提示。
  - 自动 EditMode 测试当前被已打开的 Unity Editor 项目锁阻塞：`XingxuForestUnity - SampleScene - Windows, Mac, Linux - Unity 6.4` 正在运行。关闭该 Unity 编辑器窗口后需重跑 `tools/run-stage6-validation.ps1`。
- 修改意图：先把视觉可玩版本做到能打开、能操作、能完成核心闭环，再继续替换更高精资产。

## 2026-04-13 阶段 8B 视觉对标与二级验收补全
- 日期/时间：2026-04-13
- 涉及文件：`assets/reference/visual-benchmark/`、`assets/licenses/asset_sources.md`、`assets/reference/visual_reference_sources.md`、`doc/visual_benchmark_review_stage8.md`、`doc/visual_playable_art_direction.md`、`doc/unity_visual_playable_guide.md`、`doc/README.md`、`PROJECT_DIRECTOR_REVIEW_AND_OPTIMIZATION.md`、`implementation_plan.md`、`notes.txt`
- 核心 Diff 摘要：
  - 归档用户补充的 3 张视觉对标图：Minecraft 森林/河流构图、动森家/桥/河构图、动森店前空地/角色构图；记录来源、用途和 SHA256。
  - 新增阶段 8B 视觉对标文档，明确当前阶段 8A 只是功能演示，不是画面完成版。
  - 将动森、Minecraft、星露谷、星穹铁道的可借鉴方法拆成主角、森林、河流、木屋、游戏机和 UI 的可执行标准。
  - 细化交互路径：生活采集、兑换建设、游戏机回家展示三条路径，每一步写明状态变化和可见反馈。
  - 新增二级画面验收和二级交互验收，各 100 分；低于 80 分不能扩展玩法，先修 `WorldHub` 画面。
- 修改意图：回应用户指出的森林、河流和人物辨识度差距，把抽象“对标参考图”转换为可实现、可 review、可验收的 Unity 场景标准。

## 2026-04-13 暂停前续传检查点
- 日期/时间：2026-04-13
- 当前分支：`stage-8-unity-visual-playable-plan`
- 当前状态：
  - 用户要求暂停，记录必要信息，后续恢复继续。
  - Unity 进程已确认无残留。
  - 中断前 `tools\run-unity-editmode-tests.ps1` 实际生成了 `unity/Logs/editmode-test-results.xml`，结果为 EditMode `46/46 Passed, 0 Failed`。
  - PlayMode、Windows 构建和启动冒烟尚未在 8B 后继续运行。
  - `WorldHubSceneBuilder` 已做 8B 代码层调整：主角更有轮廓、森林减少为边缘树群、河流分段并加湿土岸线、木屋增加门/花环/暖光。
  - `WorldHub.unity` 已通过 `stage8b-build-worldhub.log` 重新生成成功；`MiniGame01.unity` 为阶段 8A 生成版本。
- 新增用户参考图记录：
  - `animal_crossing_forest_garden_layout_reference.jpg`：森林花园布局、花丛、树木尺度和装饰旗帜。
  - `refined_character_silhouette_reference.jpg`：人物精细度、服装层次和形象强化目标。
  - `animal_crossing_dialog_interaction_reference.webp`：生活化对话/互动气泡参考。
  - `animal_crossing_terrain_plot_reference.png`：非矩形地形、边界、地块与可建设区域参考。
  - `animal_crossing_inventory_reference.jpg`：背包圆角容器、图标网格、操作提示参考。
- 恢复后下一步：
  - 先读取 `AGENT_EXECUTION_PROTOCOL.md`、`CLAUDE.md`、`implementation_plan.md`、`notes.txt`、`diff.md`、`doc/visual_benchmark_review_stage8.md`。
  - 继续把新增图 1-5 纳入 `doc/visual_benchmark_review_stage8.md`：森林花园布局、人物形象精细化、动作/互动反馈、背包打开才显示、任务/记录系统口径。
  - 调研并记录动森动作、背包、地形边界、河流阻挡和简单环境反馈方法；必要时用 Chrome/网页来源辅助。
  - 实现玩家不能越界、未修桥不能过河、地图非纯长方形边界、背包默认隐藏按键打开、对话/互动气泡、动作反馈最小实现。
  - 跑 `tools\run-stage6-validation.ps1`，再做 8B 二级验收 review。

## 2026-04-13 新对话续传提示词
- 日期/时间：2026-04-13
- 当前分支：`stage-8-unity-visual-playable-plan`
- 当前要求：
  - 继续《星绪森林》Unity 视觉可玩版阶段 8B。
  - Unity 是主实现，网页演示不要更新。
  - 目标不是复制参考图，而是按动森、Minecraft、星露谷、星穹铁道的方法对标：家园生活、森林花园层次、材料到建设反馈、家门口劳动循环、电子入口规则切换。
  - 用户已指定主角图为主角方向；后续要提升为“动森式强轮廓 + 更高精细度”的角色代理和 HUD 表情。
  - 用户补充的参考图已归档到 `assets/reference/visual-benchmark/`，包括森林花园、精细角色、动森对话、地形地块、背包 UI 等。
  - 当前 8A 已能操作：WASD/方向键移动、E 互动、木牌 1-4 兑换、游戏机 Enter 进入小游戏、小游戏 E 收贴纸/回家；EditMode 46/46 Passed。
  - 当前 8B 已开始：`WorldHubSceneBuilder` 已改主角轮廓、森林树群、分段河道、湿土岸线、木屋细节，并已重新生成 `WorldHub.unity`。
  - 还没完成：玩家越界限制、未修桥不能过河、非纯长方形地图边界、背包默认隐藏按键打开、动森式对话/互动气泡、动作反馈、PlayMode/Windows 构建/启动冒烟验证。
- 新对话提示词：

```text
你是 Codex，继续接手项目《星绪森林》。项目路径：D:\app_project\game\xiangsu。

先读取并遵守：
1. AGENT_EXECUTION_PROTOCOL.md
2. CLAUDE.md
3. implementation_plan.md
4. notes.txt
5. diff.md
6. doc/visual_playable_art_direction.md
7. doc/visual_benchmark_review_stage8.md
8. doc/unity_visual_playable_guide.md

当前分支：stage-8-unity-visual-playable-plan。不要提交，不要推送，除非用户明确同意。

当前目标：继续 Unity 视觉可玩版阶段 8B。Unity 桌面原生版优先，网页概念演示不要更新。目标是让 WorldHub 不只是能操作，而是按用户给的动森/Minecraft/星露谷/星穹铁道参考方法提升到可 review 的视觉水平。

已完成状态：
- 阶段 8A 已实现 GameStateRunner、HudView、BuildPlacementView、StickerWallView、ArcadeMachineView。
- WorldHub 已能 WASD/方向键移动、E 互动、T 时间提示、木牌 1-4 兑换、游戏机 Enter 进入 MiniGame01、小游戏 E 收贴纸/回家。
- 主角方向已确定，参考图已复制到 assets/reference/protagonist/aethel_ocean_oracle_protagonist_concept.png。
- 用户补充参考图已复制到 assets/reference/visual-benchmark/，用途包括森林花园布局、精细角色轮廓、动森对话互动、非矩形地形地块、动森背包 UI。
- 中断前 EditMode 测试结果为 46/46 Passed，0 Failed。
- 8B 代码已开始调整 WorldHubSceneBuilder：主角更有轮廓、森林减少为边缘树群、河流分段并加湿土岸线、木屋增加门/花环/暖光；WorldHub.unity 已重新生成成功。

必须继续完成：
1. 继续补充 doc/visual_benchmark_review_stage8.md，把新增图 1-5 的设计标准写清楚：森林花园、人物形象、对话互动、背包、任务/记录系统。
2. 研究并落实动森式动作/交互反馈、背包打开方式、简单环境反馈；只学习方法，不复制素材或 UI。
3. Unity 实现：
   - 玩家不能移动到地图外。
   - 地图边界不能是完整长方形，要做简单非矩形岛屿/围栏/自然边界。
   - 未修桥前不能过河，修桥后可以通过桥过河。
   - 背包默认隐藏，按键打开，样式参考动森大圆角容器和图标网格。
   - 木牌/记录系统像生活记录，不像任务列表。
   - 增加最小动作/互动反馈：移动、采集、建设、打开菜单、小游戏收贴纸、回家展示。
4. 跑 tools\run-stage6-validation.ps1，修到 EditMode、PlayMode、Windows 构建和启动冒烟通过。
5. 做二级 review：画面验收和交互验收均按 doc/visual_benchmark_review_stage8.md 80 分以上标准；不足先修，不新增玩法。

注意：
- 不要更新网页演示。
- 不要删除用户素材、配置、锁文件或项目文件。
- 不要改变技术栈，不要引入战斗、抽卡、联网、账号、多主世界地图。
- 所有最终提交/推送等待用户确认。
```

## 2026-04-13 清理后建设花圃补充
- 日期/时间：2026-04-13
- 涉及文件：`doc/visual_benchmark_review_stage8.md`、`implementation_plan.md`、`diff.md`、`notes.txt`
- 核心设计：
  - 新增阶段 8B 必要行为：玩家先清理木屋前或森林边缘的落枝、杂草、碎石，获得基础材料，再把清理出的空地变成花圃建设空位。
  - 这条链路吸收动森的资源采集、DIY/建设和岛屿布置方法，星露谷的“过度生长土地 -> 清理 -> 农场/花园空间”方法，Minecraft 的材料到建设反馈方法。
  - 清理前不能直接建设花圃；清理后显示空地/花圃槽；建设仍调用 `BuildService.Place(FlowerBed)`；失败不扣材料。
  - 状态需要可持久化，建议新增 `ClearedWorldNodes` 或等价结构并同步 `SaveService`，避免清理只存在于场景临时对象。
- 新对话提示词补充：

```text
补充阶段 8B 要求：实现“清理后建设花圃”链路。玩家在木屋前或森林边缘看到被落枝、杂草、碎石占住的花园地块；靠近按 E 分阶段清理，分别获得 wood/stone/flowerSeed，HUD 和场景都要有反馈；清理完成后该点变成 FlowerBedSlot，之后才能调用 BuildService.Place(FlowerBed) 建设花圃。清理前不能直接建设，失败不扣材料，清理状态必须持久化到 PlayerState/SaveService。参考动森的资源采集、DIY/建设和岛屿布置，星露谷的清理过度生长土地并建设农场空间，Minecraft 的材料到建设反馈；只学习方法，不复制素材或 UI。
```

## 2026-04-13 暂停续传记录
- 日期/时间：2026-04-13
- 涉及文件：`notes.txt`、`diff.md`
- 当前状态：
  - 用户要求暂停，等额度重置后继续。
  - 当前无 Unity 进程残留。
  - `WorldHubSceneBuilder.cs` 已继续按阶段 8B 标准修改森林、河流、主角和木屋锚点，并已重新生成过 `WorldHub.unity`。
  - `tools/run-unity-editmode-tests.ps1` 启动后被用户中断，EditMode 结果未知。
- 恢复后第一步：
  - 读取 `AGENT_EXECUTION_PROTOCOL.md`、`CLAUDE.md`、`implementation_plan.md`、`notes.txt`、`diff.md`。
  - 确认 Unity 进程为空。
  - 运行 `tools/run-unity-editmode-tests.ps1`，失败则读取 `unity/Logs/editmode-test.log` 修复。
- 用户新增待整合要求：
  - 新增 5 张参考图：`FwiEGF0aAAIT0uG.jpg`、`img_v3_0210n_6dd01027-e1f2-420d-ad10-32b104111fbg.jpg`、`OIP-C.webp`、`PixPin_2026-04-13_10-52-42.png`、`v2-6eacba3eef80a2339fb147bf9269c6be_r.jpg`，本地文件已确认存在但未复制/分析。
  - 需要补充森林花园布局、人物形象、地图边界/岛屿形状、人物动作交互、未搭桥前不可过河、背包默认关闭、任务/记录 UI、环境反馈的设计标准、验收与实现。
- 修改意图：保证暂停后恢复不会丢失当前实现状态、未完成测试和新增设计要求。

## 2026-04-13 阶段 8B Unity 实现续跑
- 日期/时间：2026-04-13
- 涉及文件：
  - `unity/Assets/Scripts/Core/PlayerState.cs`
  - `unity/Assets/Scripts/World/Nodes/WorldNodeService.cs`
  - `unity/Assets/Scripts/Save/SaveService.cs`
  - `unity/Assets/Scripts/Player/PlayerController.cs`
  - `unity/Assets/Scripts/Runtime/GameStateRunner.cs`
  - `unity/Assets/Scripts/Runtime/HudView.cs`
  - `unity/Assets/Scripts/Runtime/BuildPlacementView.cs`
  - `unity/Assets/Scripts/Runtime/ClearingPlotView.cs`
  - `unity/Assets/Editor/WorldHubSceneBuilder.cs`
  - `tools/run-unity-scene-builder.ps1`
  - `tools/run-stage6-validation.ps1`
  - `doc/visual_benchmark_review_stage8.md`
  - `doc/unity_visual_playable_guide.md`
  - `implementation_plan.md`
- 变更摘要：
  - 增加 `PlayerState.WorldNodeStages`，用 `SaveService` 序列化为 `worldNodeStages`，保证清理阶段可持久化。
  - `WorldNodeService` 对 `flower-bed-slot` 与 `forest-flower-bed-slot` 执行三段清理：落枝 -> 木材、碎石 -> 石子、杂草 -> 花种；清理满 3 阶后才走 `BuildService.Place(FlowerBed)`。
  - 新增 `ClearingPlotView`，把清理阶段映射到落枝/碎石/杂草/清理后土地区块的可见切换。
  - `BuildPlacementView` 改为可绑定具体网格，避免两个花圃槽因为同一个 `BlueprintId.FlowerBed` 同时显示已建。
  - `PlayerController` 增加非矩形岛屿边界钳制和河流跨越限制：未修桥不能过河，修桥后只允许桥附近过河。
  - `HudView` 背包默认隐藏，按 `I` 开关；保留三表情 HUD 文案和生活记录口径。
  - `WorldHubSceneBuilder` 改成非单一矩形草地组合，并生成木屋前/森林边缘两个清理后建设花圃点。
  - 新增场景生成脚本并接入 Stage 6 验证，确保跑测试/构建前先重建 Unity 场景。
- 最小测试：
  - `tools\run-unity-scene-builder.ps1`：通过；`scene-builder.log` 无 C# warning/error。
  - `tools\run-unity-editmode-tests.ps1`：48/48 passed，0 failed。
  - `tools\run-unity-playmode-tests.ps1`：2/2 passed，0 failed。
  - `tools\run-stage6-validation.ps1`：通过；场景重建、EditMode 50/50、PlayMode 2/2、Windows x64 构建和启动冒烟均成功。
  - `git diff --check`：通过；仅保留 CRLF 提示。
- 验证脚本修正：
  - 连续 Unity BatchMode 调用会短时间占用项目锁，已在 `run-stage6-validation.ps1` 的场景生成、EditMode、PlayMode、Windows 构建之间加入等待。
  - Unity 批处理会把 `UnityConnectSettings.asset` Services 开关临时改为开启；按离线桌面原型口径已改回关闭，不作为功能变更。
- 约束：
  - 网页概念演示未更新。
  - 未引入战斗、抽卡、联网、账号或多主世界地图。
  - 用户修正 git 口径：规划 plan 最终阶段结束的 git 需要再次同意；其他阶段正常推送。

## 2026-04-13 玩家试玩缺陷与阶段 8C/9 规划补充
- 日期/时间：2026-04-13
- 用户截图：
  - `C:\Users\72440\Desktop\PixPin_2026-04-13_14-29-30.png`
  - `C:\Users\72440\Desktop\PixPin_2026-04-13_14-30-10.png`
  - `C:\Users\72440\Desktop\PixPin_2026-04-13_14-36-15.png`
- 用户指出的问题：
  - 主角可以从上方超出地图。
  - 已修桥后无法稳定通过木桥。
  - 主角可以从两条木条/河岸空隙进入河水。
  - 主角仍是几何代理体，离最终动森水平和 Aethel 角色目标差距大。
  - `implementation_plan.md` 对阶段 8C、阶段 9、调试步骤、玩家试玩反馈回路、快照对标评价体系写得不够细。
- 代码修复：
  - `PlayerController` 从简单坐标钳制改为 `IsWalkable` 判定：陆地区域集合、河道禁入带、修桥后的桥面通行走廊。
  - 新增/更新 `PlayerControllerTests`：上方越界、右侧岛屿上沿、河道缝隙、未修桥禁入、修桥后桥位通过。
  - 当前最小验证：`tools\run-unity-editmode-tests.ps1` 输出 EditMode 53/53 Passed。
- 文档修复：
  - `implementation_plan.md` 新增阶段 8C“视觉/物理一致性与角色升级”和阶段 9“对标快照、试玩回路与发布级视觉打磨”。
  - 规划明确每个设计阶段完成后先让用户试玩，Codex 根据截图/建议修正，再进入下一切片。
  - 规划补充快照评价体系：画面、物理、交互各 100 分；任一维度低于 85 分不新增玩法。

## 2026-04-13 背包分类、桥识别与 README 重写
- 日期/时间：2026-04-13
- 用户新增问题：
  - 桥看得到但不识别，无法稳定显示修复。
  - 不需要常驻“自由探索”提示；开局只在第一次遇到物品时提示一次。
  - 背包必须是分类分栏，左右切换：采集物、游戏货币/交换物、可建造种植物、室内/特殊物。
  - README 需要按标准角度重写游戏简介、体验、技术栈、架构图、主要设计与抽象。
- 当前修复方向：
  - `WorldNodeInteractor.Configure` 支持交互半径，`DamagedBridge` 交互半径提高到覆盖河岸可站区域。
  - `HudView` 背包改为分类页，左右切换；未获得的非基础物品不显示。
  - `GameStateRunner` 去掉开局演示文案，新增首个资源一次性引导、1-5 木牌兑换、轻量程序化音效反馈。
  - `README.md` 按 Unity 主实现重写，补充体验、背包分类、玩家路径、架构图、技术栈和验证命令。

## 2026-04-13 交互设计体系文档
- 日期/时间：2026-04-13
- 涉及文件：
  - `doc/interaction-design-framework.md`
  - `doc/interaction-fallback-spec.md`
  - `doc/visual_benchmark_review_stage8.md`
  - `doc/README.md`
  - `notes.txt`
- 变更摘要：
  - 新增交互设计框架，按对标方法、交互原则、主路径矩阵、背包信息架构、反馈层级和快照评分流程组织。
  - 新增失败分支与边界规范，覆盖物品栏、木牌兑换、建设、清理花圃、移动物理、游戏机、存档和自动验证。
  - 阶段 8 review 文档新增 8C/9 快照量化评分表，画面、物理、交互各 100 分，明确扣分规则。
- 修改意图：把用户“点到为止”的交互、测分和对标要求补成可直接指导实现和 review 的设计体系。
