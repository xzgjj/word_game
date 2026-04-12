# 星绪森林新手运行与调试指南

## 目标

这份文档面向第一次做游戏的个人开发者。目标是让你能低门槛查看网页演示、准备 Unity 工程、运行调试游戏，并按 2K 默认画面、1080p 可选画面、4K 素材源文件的标准管理质量。

## 一、最快查看网页演示

1. 打开项目目录。
2. 双击 `index.html`。
3. 在页面中点击：
   - 旁友森林：拾取木材和表情碎片。
   - 空地游戏机：第一次点击修桥，第二次点击进入像素模式。
   - 贴纸：收集 3 个。
   - 出口：返回主世界。

当前网页演示不需要安装依赖、不需要登录、不需要启动服务器。

## 二、Unity 安装与免费使用核对

官方入口：

- Unity Hub：https://unity.com/en/unity-hub
- Unity Personal：https://unity.com/products/unity-personal
- Unity 安装说明：https://docs.unity.com/hub/add-editor

当前官方说明中，Unity Personal 通常适用于最近 12 个月收入和融资低于 200,000 美元的个人或小组织。最终以 Unity 官网、Unity Hub 和你安装时看到的条款为准。

低门槛建议：

1. 注册 Unity ID。
2. 安装 Unity Hub。
3. 通过 Unity Hub 安装 Unity LTS 版本。
4. 勾选 Windows Build Support。
5. 如果你要打包 macOS，最好在 macOS 设备上安装对应构建支持。
6. 暂时不要接入 Unity 服务、账号系统、联网 SDK 或复杂插件。

## 三、项目怎么和 Unity 配合

当前 `unity/` 是骨架目录。正式实现时用 Unity Hub 在 `unity/` 目录创建工程，并保持以下结构：

```text
unity/
  Assets/
    Scenes/WorldHub.unity
    Scenes/MiniGame01.unity
    Scripts/Core/
    Scripts/Player/
    Scripts/World/
    Scripts/Building/
    Scripts/MiniGame/
    Scripts/Save/
    Scripts/UI/
    ScriptableObjects/Resources/
    ScriptableObjects/BuildRecipes/
    ScriptableObjects/WorldNodes/
```

数据先参考 `game-data/`：

- `resources.json`：资源类型。
- `build-recipes.json`：建设配方。
- `world-nodes.json`：主世界交互节点。

## 四、推荐画质目标

- 默认画面：2K，建议 2560x1440。
- 可选低档：1080p，建议 1920x1080。
- 素材源文件：优先 4K 或更高；如果达不到 4K，需要做清晰度审核和后期优化。
- 性能目标：个人小项目优先稳定，先保证 1080p 流畅，再提升 2K。

Unity 设置建议：

- 提供画质选项：1080p、2K。
- 提供窗口模式和全屏模式。
- 使用 `Screen.SetResolution` 做分辨率切换。
- 限制同屏粒子、实时灯光和后处理数量。
- 首版不使用高性能要求的体积雾、大量动态阴影或复杂水体。

## 五、素材生成和导入流程

1. 先看 `assets/reference/visual_reference_sources.md`，明确只参考方法，不复制素材。
2. 再看 `assets/prompts/google-image-prompts.md`，用提示词生成候选图。
3. 优先使用 GPT 图片生成能力做自有素材候选。
4. 如果效果不好，再使用 Google 图片生成。
5. 开源素材只从候选来源下载，逐项记录许可证。
6. 原始素材放 `assets/source/` 或 `assets/generated/`。
7. 处理后的 Unity 可用素材放 `assets/processed/`。
8. 任何素材进入工程前，都写入 `assets/licenses/asset_sources.md`。

4K 素材审核：

- 角色和主要场景概念图：尽量 4K。
- UI 图标和贴纸：可以矢量或高分辨率 PNG。
- 如果只有低清素材：先用作占位，不进入最终资产；需要重新生成或高清化。

## 六、运行调试清单

网页阶段：

- 打开 `index.html`。
- 检查控制台是否有 error/warn。
- 点击完整闭环：森林 -> 游戏机 -> 游戏机 -> 3 个贴纸 -> 出口。
- 检查标题是否为《星绪森林》。

Unity 阶段：

- 打开 `WorldHub` 场景。
- 点击 Play。
- 检查主角能移动。
- 检查森林资源能拾取。
- 检查木桥缺材料提示和修复反馈。
- 检查时间切换是否改变光照。
- 检查游戏机能进入 `MiniGame01`。
- 检查贴纸收集和返回主世界。
- 检查本地存档是否能保存贴纸墙状态。

## 七、出问题时怎么查

- 页面打不开：直接用浏览器打开 `index.html`，不要先装依赖。
- 点击没反应：检查浏览器控制台，确认 `app.js` 是否加载。
- Unity 打不开：先确认 Unity Hub 已登录并安装 LTS 编辑器。
- 打包失败：先只打 Windows x64；macOS 等主流程稳定后再处理。
- 画面模糊：确认素材源文件分辨率、Unity 导入压缩、目标分辨率设置。
- 性能低：先关后处理、减少动态灯光、降低阴影和粒子。

## 八、实现前确认

开始写 Unity 代码前，先确认三件事：

1. `README.md` 的核心体验你认可。
2. `game-data/` 的资源和建设物清单你认可。
3. `assets/` 的素材流程你认可。
