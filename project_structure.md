# 项目结构与技术选型建议

## 当前根目录
- `CLAUDE.md`：项目准则与约束说明。
- `AGENT_EXECUTION_PROTOCOL.md`：AI 代理执行规范与确认流程。
- `implementation_plan.md`：项目阶段规划、review 规则与提交计划。
- `diff.md`：关键修改记录。
- `notes.txt`：项目笔记与复核记录。
- `README.md`：项目概念、运行说明与文件索引。
- `.gitignore`：忽略生成文件和运行时输出，不能忽略规范与执行文档。
- `index.html`：网页概念演示入口。
- `style.css`：概念页视觉样式。
- `app.js`：概念页交互逻辑。
- `assets/`：保存图像、像素素材与用户提供资源。
- `doc/`：设计文档目录。

## `doc/` 目录
- `design_document.md`：游戏整体设计规划，覆盖玩法、交互、UI、接口、数据结构和验收标准。
- `level_sketches.md`：核心玩法与关卡草图，覆盖地图、节点、动线、触发条件、反馈和数据变化。

## 后续 Unity 工程建议
后续实现时建议新增 `unity/` 目录，但本轮只做规划与网页概念演示，不提前创建空工程。

```text
unity/
  Assets/
    Scenes/
      WorldHub.unity
      MiniGame01.unity
    Scripts/
      Player/PlayerController.cs
      Player/EmotionController.cs
      World/WorldNodeInteractor.cs
      World/BridgeRepairNode.cs
      World/ArcadeEntranceNode.cs
      MiniGame/MiniGameStateMachine.cs
      Save/SaveService.cs
    Art/Emoji/
    Art/World/
    Art/Pixel/
    UI/HudController.cs
```

## 架构说明
- 主世界：小地图，木屋、森林、河流、空地游戏机和远景光幕。
- 玩家：动态表情包主角，主世界圆形动态表情，像素小游戏切换为方块表情。
- 交互：资源拾取、修桥、确认进入游戏机、小游戏完成返回。
- 状态：`PlayerState` 管理位置、情绪、背包、已解锁小游戏和已完成小游戏。
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
