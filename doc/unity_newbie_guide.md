# 星绪森林 Unity 新手协作指南



## 一、适用对象

适用版本：v1 规划版，面向第一次使用 Unity 的个人开发者。

本文件是本地协作指南，默认不上传 git。它的目标是让你知道如何注册、安装、授权、创建项目，并和 Codex 一起把《星绪森林》做成优秀的小体量桌面原生游戏。



## 二、Unity 个人使用资格

根据 Unity 官方当前公开说明，Unity Personal 面向符合收入/融资门槛的个人、小团队和学生使用；Unity 官方页面写明 Unity Personal 适用于最近 12 个月收入和融资低于 200,000 美元的个人和小组织。你需要自行确认自己的资格。官方页面可能更新，最终以 Unity 官网、Unity Hub 和安装器显示的条款为准。

官方核对入口：

- Unity Personal：https://unity.com/products/unity-personal
- Unity license compliance：https://unity.com/en/pages/license-compliance

执行建议：

1. 打开 Unity 官网。
2. 注册 Unity ID。
3. 下载 Unity Hub。
4. 在 Unity Hub 登录账号。
5. 选择 Unity Personal 或符合你身份的计划。
6. 安装一个 LTS 版本编辑器，并勾选 Windows Build Support；如果你有 macOS 设备，再安装 macOS 相关构建支持。

注意：这不是法律意见。只要你没有达到 Unity Personal 的收入/融资上限，通常小个人开发可以先用 Unity Personal 开发学习和原型。



## 三、推荐安装方式

- Unity Hub：统一管理编辑器、项目和许可证。
- Unity LTS：优先选择稳定长期支持版本，避免新功能变动影响小项目。
- 模块：Windows Build Support 必装；macOS Universal 打包通常需要 macOS 设备和对应构建环境。
- 版本记录：创建 `unity/VERSION_NOTES.md`，记录 Unity 编辑器版本和安装模块。



## 四、你和 Codex 的协作方式

你负责：

- 确认玩法方向、画面是否满意、素材是否可接受。
- 提供你喜欢的表情包参考、木屋风格参考、森林色调参考。
- 告诉我你本机 Unity 版本和能否打开工程。

Codex 负责：

- 维护代码结构和实现计划。
- 把玩家行为拆成状态机、组件和场景。
- 生成 Unity 脚本、配置说明、测试步骤和素材清单。
- 每个阶段按 80 分门禁做 review。



## 五、Unity 工程目标结构

```text
unity/
  Assets/
    Scenes/
      WorldHub.unity
      MiniGame01.unity
    Scripts/
      Core/
      Player/
      World/
      Building/
      MiniGame/
      Save/
      UI/
    Art/
      Placeholder/
      Source/
      Processed/
    ScriptableObjects/
      Resources/
      BuildRecipes/
      WorldNodes/
  ProjectSettings/
```



## 六、第一阶段实现范围

第一阶段不要做大地图。只做完整小闭环：

1. 主角在木屋前移动。
2. 森林资源点可拾取木材和表情碎片。
3. 河边木桥可建设。
4. 木屋周围能放置花圃或路牌的占位物。
5. 清晨、午后、夜晚可以切换光照。
6. 空地游戏机可以进入像素小游戏。
7. 像素小游戏可收集 3 个贴纸并返回主世界。
8. 回木屋后贴纸墙更新。



## 七、阶段对齐点

- 行为对齐：确认自由探索没有变成任务跑腿，建设需求能自然解释收集。
- 画面对齐：确认小地图完整、木屋周围有家园感、森林和河流可读、游戏机入口突出。
- 技术对齐：确认状态字段能保存和恢复，Unity 场景之间能传递小游戏结果。
- 素材对齐：确认所有素材来源、许可证和用途都有记录。



## 八、你需要提供或确认的素材

- 主角动态表情包参考。
- 木屋风格参考。
- 森林树木风格参考。
- 河流和木桥参考。
- 游戏机或卡带参考。
- 贴纸墙和贴纸图标参考。

如果没有素材，先用 `assets/source/` 的素材说明和 `assets/prompts/` 的提示词生成占位图，再逐步替换。
