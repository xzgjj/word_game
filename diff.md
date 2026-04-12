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
