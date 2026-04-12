# 表情探索：情绪小镇

## 项目简介
一个 0-1 阶段的现代情绪冒险小游戏。玩家扮演动态表情包主角，从木屋出发，穿过旁友森林和河流，在空地拾取游戏机卡带，进入像素小游戏，完成后回到主世界并更新木屋贴纸墙。

## 当前目标
- 桌面原生版本 1：`Unity Windows x64`。
- 桌面原生版本 2：`Unity macOS Universal`。
- 网页概念演示：`HTML + CSS + JavaScript`，用于确认画面构图和交互闭环。

## MVP 玩法闭环
1. 木屋门口出生，看到目标提示。
2. 森林拾取木材和表情碎片。
3. 河边修复木桥并过河。
4. 空地确认拾取游戏机卡带。
5. 切换到像素小游戏，收集 3 个贴纸。
6. 返回主世界，游戏机点亮，木屋贴纸墙新增贴纸。

## 视觉对标
- 《动物森友会：新视野》：小岛生活、家园起点、户外布置、桥和坡道式连接。
- 《我的世界》：木屋、自然资源、方块化边界、采集和微建造。
- 《崩坏：星穹铁道 4.0》：远景光幕、现代电子屏、入口后形态与规则变化。
- 短流程探索游戏：目标可见、路径短、失败不惩罚、完成后有明确反馈。

## 技术选型
- 网页演示：直接打开 `index.html`。
- 桌面原生：后续使用 Unity 单工程多平台打包，目标为 Windows 和 macOS。
- 本地存档：桌面版使用本地 JSON，网页演示使用 `localStorage`。

## 项目文件说明
- `CLAUDE.md`：项目准则与约束说明。
- `AGENT_EXECUTION_PROTOCOL.md`：AI 执行规范与确认流程。
- `implementation_plan.md`：阶段规划、review 规则与交付计划。
- `diff.md`：修改记录。
- `notes.txt`：项目关键笔记与复核记录。
- `project_structure.md`：架构与目录建议。
- `doc/design_document.md`：完整游戏设计规划。
- `doc/level_sketches.md`：核心玩法与关卡草图。
- `index.html`、`style.css`、`app.js`：网页概念交互演示。
- `assets/`：图像、像素素材和用户提供资源。

## 运行说明
当前阶段直接打开 `index.html` 即可查看网页概念演示，不需要安装依赖或启动服务器。

## 注意事项
- `.gitignore` 不应包含 `CLAUDE.md`、`AGENT_EXECUTION_PROTOCOL.md`、`implementation_plan.md`、`diff.md`、`notes.txt` 等规范和执行文档。
- 不允许删除项目文件、配置文件、依赖锁定文件、数据库文件或用户上传资源。
- 后续实现每 10 分钟按 `implementation_plan.md` 复核玩法、画面和实现偏差。
