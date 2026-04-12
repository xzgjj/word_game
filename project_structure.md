# 项目结构与技术选型建议

## 当前根目录
- `CLAUDE.md`：项目准则与约束说明。
- `AGENT_EXECUTION_PROTOCOL.md`：AI 代理执行规范与确认流程。
- `implementation_plan.md`：项目阶段规划、review 规则与提交计划。
- `diff.md`：关键修改记录。
- `notes.txt`：项目笔记与复核记录。
- `PROJECT_AUDIT_REPORT.md`：项目完成与审核报告。
- `PROJECT_ARCHITECTURE_UPDATE.md`：v2 架构更新，覆盖单主地图、资源、木牌、图纸、自建、物品栏、接口和存档设计。
- `README.md`：项目概念、运行说明与文件索引。
- `.gitignore`：忽略生成文件和运行时输出，不能忽略规范与执行文档。
- `index.html`：网页概念演示入口。
- `style.css`：概念页视觉样式。
- `app.js`：概念页交互逻辑。
- `assets/`：保存图像、像素素材与用户提供资源。
- `game-data/`：保存可转为 Unity ScriptableObject 的资源、物品、兑换、图纸、建设和世界节点配置。
- `doc/`：设计文档目录。

## `doc/` 目录
`doc/` 目录用于保存本地详细设计资料。按照当前 git 策略，大多数详细文档不上传远端，只上传 `doc/README.md` 作为索引。

本地详细文档包括：
- `design_document.md`：游戏整体设计规划，覆盖玩法、交互、UI、接口、数据结构和验收标准。
- `level_sketches.md`：核心玩法与关卡草图，覆盖地图、节点、动线、触发条件、反馈和数据变化。
- `interaction_technical_spec.md`：玩家行为路径、状态变化、接口和 Unity 拆分。
- `visual_alignment_standard.md`：画面对齐方法、80 分通过标准和三轮 review。
- `unity_newbie_guide.md`：Unity 新手注册、安装、授权和协作指南。
- `asset_pipeline.md`：素材生成、开源素材和许可证记录流程。
- `RUN_DEBUG_GUIDE.md`：新手运行、查看、调试、画质和素材导入指南。

## 后续 Unity 工程建议
后续实现时建议新增 `unity/` 目录，但本轮只做规划与网页概念演示，不提前创建空工程。

```text
unity/
  Assets/
    Scenes/
      WorldHub.unity
      MiniGame01.unity
    Scripts/
      Core/GameState.cs
      Inventory/InventoryService.cs
      Player/PlayerController.cs
      Player/EmotionController.cs
      World/WorldNodeInteractor.cs
      World/GatherNode.cs
      World/FishingNode.cs
      World/SignboardNode.cs
      Building/BlueprintCatalog.cs
      Building/BuildService.cs
      Building/PlacementGrid.cs
      Building/CustomBuildService.cs
      MiniGame/MiniGameStateMachine.cs
      Save/SaveService.cs
    Art/Emoji/
    Art/World/
    Art/Pixel/
    UI/HudController.cs
```

## `game-data/` 目录
- `item-catalog.json`：物品种类、格子数公式和 9999 上限。
- `resources.json`：资源类型和来源领域。
- `exchange-recipes.json`：木屋前木牌兑换配方。
- `blueprint-unlocks.json`：图纸赠送与自建解锁触发。
- `build-recipes.json`：建设配方。
- `world-nodes.json`：主世界交互节点。

## 架构说明
- 主世界：单主地图，木屋、森林、河岸、河水、空地游戏机和远景光幕都在同一张地图内，并允许持续摆放建设元素。
- 玩家：动态表情包主角，主世界圆形动态表情，像素小游戏切换为方块表情。
- 交互：资源拾取、河水钓鱼、木牌兑换、图纸赠送、修桥、建设、自建建筑物、确认进入游戏机、小游戏完成返回。
- 状态：`PlayerState` 管理位置、情绪、物品栏、图纸、摆放建筑物、建设数量、已解锁小游戏和已完成小游戏。
- 存档：桌面版本地 JSON，网页演示 `localStorage`。

## 技术选型
- 网页概念演示：`HTML + CSS + JavaScript`，无构建步骤。
- 桌面原生：`Unity` 单工程，多平台打包 `Windows x64` 和 `macOS Universal`。
- 不新增第二引擎，避免在 0-1 阶段引入重复工程成本。

## 初始开发建议
1. 先让网页概念页把画面和交互闭环讲清楚。
2. 再创建 Unity 工程，按 `WorldHub` 与 `MiniGame01` 两个场景搭骨架。
3. 所有资源命名先服务 MVP，不提前建立大规模资产体系。
4. 每 10 分钟对照 `implementation_plan.md` 执行玩法、画面、实现复核。
