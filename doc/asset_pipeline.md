# 星绪森林素材流程



## 一、素材优先级

1. 用户自有素材：优先使用你自己提供或拥有授权的图片。
2. AI 生成素材：用于角色、木屋、贴纸、游戏机、背景概念图。
3. CC0 或开源素材：用于占位贴图、自然材质、基础图标。
4. 手写占位素材：用于早期验证玩法。



## 二、素材目录

```text
assets/
  source/              原始素材，不覆盖
  reference/           参考图和对照说明
  generated/           AI 生成素材
  open-source/         开源或 CC0 素材
  processed/           已裁切、压缩、改名后的可用素材
  prompts/             生成图片提示词
  licenses/            素材来源和许可证记录
```



## 三、开源素材候选

- Kenney：常见游戏 UI、图标、低多边形和 2D 素材，需逐项确认许可证。
- OpenGameArt：素材来源多，许可证不统一，必须逐项记录。
- Poly Haven：常见 CC0 HDRI、模型、纹理，可用于环境材质参考。
- ambientCG：常见 CC0 PBR 纹理，可用于地面、木材、石材参考。



## 四、AI 图片生成流程

如果没有满意素材，使用 AI 生成占位图：

1. 先确定用途：主角、木屋、森林、河流、游戏机、贴纸墙。
2. 先用 GPT 图片生成能力生成候选图，并上传到 `assets/generated/`。
3. 如果 GPT 生成效果不满意，再使用 Google 图片生成，提示词仍来自 `assets/prompts/google-image-prompts.md`。
4. 每个对象生成 4 张候选图。
5. 按 `doc/visual_alignment_standard.md` 打分，低于 80 分不进入 `processed/`。
6. 只保存通过风格审核的图到 `assets/generated/`，处理后再放入 `assets/processed/`。
7. 在 `assets/licenses/asset_sources.md` 记录生成工具、日期、提示词和用途。



## 四点一、固定自动流程

1. Chrome/Web 搜索官方和可授权参考。
2. 写入 `assets/reference/visual_reference_sources.md`。
3. 生成提示词并写入 `assets/prompts/`。
4. 优先用 GPT 图片生成能力产出候选。
5. 失败或不满意时再用 Google 图片生成。
6. 保存原图到 `assets/generated/`。
7. 按 80 分标准评分。
8. 通过后处理到 `assets/processed/`。
9. 记录来源、工具、提示词和用途到 `assets/licenses/asset_sources.md`。
10. Unity 导入前再做一次分辨率、风格和授权检查。



## 五、素材命名

- `hero_emoji_source_01.png`
- `cabin_exterior_concept_01.png`
- `forest_tree_set_01.png`
- `river_bridge_concept_01.png`
- `arcade_machine_concept_01.png`
- `sticker_wall_icon_set_01.png`



## 六、素材 review

每个素材进入工程前必须回答：

- 是否有来源或提示词记录？
- 是否有明确用途？
- 是否和小地图家园冒险风格一致？
- 是否存在未授权角色、Logo 或明显复制痕迹？
- 是否能在 Unity 中切成可用贴图或 Sprite？
