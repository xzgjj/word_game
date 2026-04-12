# 星绪森林 Unity 工程骨架

当前目录是 Unity 工程规划骨架，不是完整 Unity 工程。正式实现时请用 Unity Hub 在 `unity/` 目录创建或打开工程，并保持下面结构：

- `Assets/Scenes/WorldHub.unity`：主世界小地图。
- `Assets/Scenes/MiniGame01.unity`：像素小游戏。
- `Assets/Scripts/Core/`：游戏状态、入口和全局配置。
- `Assets/Scripts/Inventory/`：物品栏、格子数公式和 9999 堆叠上限。
- `Assets/Scripts/Player/`：主角移动和情绪能力。
- `Assets/Scripts/World/`：资源点、河水鱼影、木牌、木桥、游戏机入口。
- `Assets/Scripts/Building/`：图纸、摆放网格、木屋周围建设物和自建建筑物。
- `Assets/Scripts/MiniGame/`：贴纸小游戏状态机。
- `Assets/Scripts/Save/`：本地 JSON 存档。
- `Assets/Scripts/UI/`：HUD、提示和建设清单。
- `Assets/ScriptableObjects/`：资源、建设配方、世界节点配置。

实现前先阅读根目录 `PROJECT_ARCHITECTURE_UPDATE.md`、本地 `doc/unity_newbie_guide.md` 和 `doc/interaction_technical_spec.md`。
