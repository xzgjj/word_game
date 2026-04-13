using System.IO;
using StarryForest.Core;
using StarryForest.Player;
using StarryForest.Runtime;
using StarryForest.World.Nodes;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StarryForest.EditorTools
{
    public static class WorldHubSceneBuilder
    {
        private const string ScenePath = "Assets/Scenes/WorldHub.unity";
        private const string MiniGameScenePath = "Assets/Scenes/MiniGame01.unity";

        [MenuItem("Starry Forest/Build Visual Playable Scenes")]
        public static void BuildVisualPlayableScenes()
        {
            BuildWorldHubScene();
            BuildMiniGame01Scene();
        }

        [MenuItem("Starry Forest/Build WorldHub Scene")]
        public static void BuildWorldHubScene()
        {
            AssetDatabase.Refresh();
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            GameObject root = new GameObject("WorldHub");
            CreateGround(root.transform);
            CreatePlayer(root.transform);
            CreateHomeArea(root.transform);
            CreateForest(root.transform);
            CreateRiver(root.transform);
            CreateBridgeAndBuildSlots(root.transform);
            CreateArcadeClearing(root.transform);
            CreateDistantLightScreen(root.transform);
            CreateRuntime(root.transform);
            CreateCameraAndLight();

            Directory.CreateDirectory("Assets/Scenes");
            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene(ScenePath, true),
                new EditorBuildSettingsScene(MiniGameScenePath, true)
            };
            AssetDatabase.SaveAssets();
        }

        [MenuItem("Starry Forest/Build MiniGame01 Scene")]
        public static void BuildMiniGame01Scene()
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            GameObject root = new GameObject("MiniGame01");

            CreateCube("PixelSkyPanel", new Vector3(0f, 1.3f, 0.4f), new Vector3(8.8f, 4.8f, 0.12f), new Color(0.08f, 0.09f, 0.16f), root.transform);
            CreateCube("PixelPlatform_Start", new Vector3(-3f, -1.2f, 0f), new Vector3(2f, 0.25f, 0.45f), new Color(0.23f, 0.25f, 0.32f), root.transform);
            CreateCube("PixelPlatform_Middle", new Vector3(0f, -0.35f, 0f), new Vector3(2f, 0.25f, 0.45f), new Color(0.23f, 0.25f, 0.32f), root.transform);
            CreateCube("PixelPlatform_Exit", new Vector3(3f, 0.55f, 0f), new Vector3(2f, 0.25f, 0.45f), new Color(0.23f, 0.25f, 0.32f), root.transform);

            CreatePixelPlayer(root.transform);
            CreateSticker("Sticker_1", new Vector3(-2.5f, -0.75f, -0.05f), root.transform);
            CreateSticker("Sticker_2", new Vector3(0f, 0.12f, -0.05f), root.transform);
            CreateSticker("Sticker_3", new Vector3(2.55f, 1.05f, -0.05f), root.transform);
            CreateCube("ExitDoor", new Vector3(3.75f, 1.1f, -0.05f), new Vector3(0.45f, 1.1f, 0.18f), new Color(0.35f, 0.95f, 0.92f), root.transform, true);
            CreateRuntime(root.transform);
            CreateMiniGameCameraAndLight();

            Directory.CreateDirectory("Assets/Scenes");
            EditorSceneManager.SaveScene(scene, MiniGameScenePath);
            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene(ScenePath, true),
                new EditorBuildSettingsScene(MiniGameScenePath, true)
            };
            AssetDatabase.SaveAssets();
        }

        private static void CreateGround(Transform root)
        {
            CreateCube("IslandCore_Grass", new Vector3(-1.35f, -0.06f, -0.55f), new Vector3(13.2f, 0.12f, 8.8f), new Color(0.43f, 0.68f, 0.43f), root);
            CreateCube("IslandHomeLobe", new Vector3(-6.25f, -0.055f, -2.45f), new Vector3(4.3f, 0.12f, 4.6f), new Color(0.47f, 0.7f, 0.45f), root);
            CreateCube("IslandForestLobe", new Vector3(-5.65f, -0.055f, 2.25f), new Vector3(5.3f, 0.12f, 4.25f), new Color(0.39f, 0.64f, 0.4f), root);
            CreateCube("IslandArcadeLobe", new Vector3(5.7f, -0.055f, -2.4f), new Vector3(4.5f, 0.12f, 3.4f), new Color(0.45f, 0.7f, 0.47f), root);
            CreateCube("NaturalBoundary_NorthWestTrees", new Vector3(-7.6f, 0.08f, 3.9f), new Vector3(0.6f, 0.22f, 1.8f), new Color(0.25f, 0.48f, 0.27f), root);
            CreateCube("NaturalBoundary_SouthBank", new Vector3(1.2f, 0.03f, -5.05f), new Vector3(8.4f, 0.12f, 0.3f), new Color(0.55f, 0.58f, 0.42f), root);
            CreateCube("NaturalBoundary_EastFence", new Vector3(7.55f, 0.28f, -1.05f), new Vector3(0.28f, 0.55f, 3.6f), new Color(0.5f, 0.33f, 0.18f), root);
            CreateCube("HomeYardOpenLawn", new Vector3(-4.85f, 0.015f, -2.75f), new Vector3(4.2f, 0.04f, 1.45f), new Color(0.54f, 0.73f, 0.48f), root);
            CreateCube("HomeYardPath", new Vector3(-4.5f, 0.025f, -2.8f), new Vector3(3.8f, 0.04f, 0.52f), new Color(0.68f, 0.62f, 0.44f), root);
            CreateCube("ForestEntryPath", new Vector3(-4.7f, 0.03f, 0.65f), new Vector3(1.2f, 0.04f, 1.35f), new Color(0.56f, 0.68f, 0.42f), root);
        }

        private static void CreatePlayer(Transform root)
        {
            GameObject player = new GameObject("Player");
            player.transform.position = new Vector3(-5.6f, 0.05f, -1.5f);
            player.AddComponent<PlayerController>();
            player.transform.SetParent(root);

            CreateSphere("PlayerHead_AethelProxy", new Vector3(0f, 1.16f, -0.04f), new Vector3(0.46f, 0.44f, 0.32f), new Color(0.92f, 0.78f, 0.74f), player.transform);
            CreateCube("PlayerBody_AethelProxy", new Vector3(0f, 0.6f, 0f), new Vector3(0.46f, 0.82f, 0.32f), new Color(0.19f, 0.11f, 0.28f), player.transform);
            CreateCube("PlayerDressLayer_UpperPurple", new Vector3(0f, 0.4f, -0.04f), new Vector3(0.68f, 0.36f, 0.28f), new Color(0.31f, 0.16f, 0.42f), player.transform);
            CreateCube("PlayerDressLayer_LowerWave", new Vector3(0f, 0.18f, -0.02f), new Vector3(0.82f, 0.24f, 0.26f), new Color(0.42f, 0.18f, 0.52f), player.transform);
            CreateCube("PlayerLongHair_BackSheet", new Vector3(0f, 0.78f, 0.16f), new Vector3(0.72f, 1.28f, 0.16f), new Color(0.045f, 0.04f, 0.065f), player.transform);
            CreateCube("PlayerHairFringe_Left", new Vector3(-0.16f, 1.28f, -0.18f), new Vector3(0.18f, 0.26f, 0.08f), new Color(0.04f, 0.035f, 0.058f), player.transform);
            CreateCube("PlayerHairFringe_Right", new Vector3(0.12f, 1.28f, -0.18f), new Vector3(0.2f, 0.22f, 0.08f), new Color(0.04f, 0.035f, 0.058f), player.transform);
            CreateCube("PlayerHairTailLeft", new Vector3(-0.42f, 0.45f, 0.08f), new Vector3(0.16f, 0.92f, 0.12f), new Color(0.045f, 0.04f, 0.065f), player.transform);
            CreateCube("PlayerHairTailRight", new Vector3(0.42f, 0.45f, 0.08f), new Vector3(0.16f, 0.92f, 0.12f), new Color(0.045f, 0.04f, 0.065f), player.transform);
            CreateCube("PlayerQuietEye_Left", new Vector3(-0.11f, 1.18f, -0.22f), new Vector3(0.08f, 0.03f, 0.04f), new Color(0.18f, 0.12f, 0.26f), player.transform);
            CreateCube("PlayerQuietEye_Right", new Vector3(0.11f, 1.18f, -0.22f), new Vector3(0.08f, 0.03f, 0.04f), new Color(0.18f, 0.12f, 0.26f), player.transform);
            CreateCube("PlayerCalmMouth", new Vector3(0f, 1.08f, -0.23f), new Vector3(0.12f, 0.025f, 0.04f), new Color(0.36f, 0.16f, 0.2f), player.transform);
            CreateCube("PlayerShellFeather_Tall", new Vector3(0.36f, 1.35f, -0.02f), new Vector3(0.12f, 0.52f, 0.07f), new Color(0.9f, 0.93f, 0.94f), player.transform);
            CreateCube("PlayerShellFeather_Side", new Vector3(0.46f, 1.22f, -0.03f), new Vector3(0.1f, 0.36f, 0.06f), new Color(0.82f, 0.86f, 0.9f), player.transform);
            CreateSphere("PlayerShellPin", new Vector3(0.27f, 1.13f, -0.16f), new Vector3(0.16f, 0.12f, 0.08f), new Color(0.72f, 0.75f, 0.78f), player.transform);
            CreateCube("PlayerCoralAccent_Left", new Vector3(-0.3f, 0.66f, -0.13f), new Vector3(0.1f, 0.42f, 0.08f), new Color(0.82f, 0.28f, 0.33f), player.transform);
            CreateCube("PlayerCoralAccent_Right", new Vector3(0.3f, 0.5f, -0.13f), new Vector3(0.08f, 0.28f, 0.08f), new Color(0.72f, 0.22f, 0.42f), player.transform);
            CreateCube("PlayerLeg_Left", new Vector3(-0.14f, -0.22f, -0.02f), new Vector3(0.16f, 0.46f, 0.14f), new Color(0.08f, 0.07f, 0.12f), player.transform);
            CreateCube("PlayerLeg_Right", new Vector3(0.14f, -0.22f, -0.02f), new Vector3(0.16f, 0.46f, 0.14f), new Color(0.08f, 0.07f, 0.12f), player.transform);
            CreateSphere("SpiritFish_Quiet", new Vector3(0.84f, 0.88f, -0.14f), new Vector3(0.42f, 0.2f, 0.1f), new Color(0.42f, 0.82f, 0.94f), player.transform, true);
            CreateCube("SpiritFish_Tail", new Vector3(0.55f, 0.88f, -0.14f), new Vector3(0.14f, 0.28f, 0.06f), new Color(0.62f, 0.9f, 0.96f), player.transform, true);
            CreateCube("SpiritFish_GlowTrail", new Vector3(0.44f, 0.78f, -0.16f), new Vector3(0.24f, 0.06f, 0.04f), new Color(0.72f, 0.95f, 1f), player.transform, true);
        }

        private static void CreateHomeArea(Transform root)
        {
            GameObject cabin = new GameObject("HomeCabin");
            cabin.transform.SetParent(root);
            cabin.transform.position = new Vector3(-6f, 0f, -3f);
            CreateCube("HomeCabin_WarmWood", new Vector3(0f, 0.75f, 0f), new Vector3(2.2f, 1.5f, 1.8f), new Color(0.58f, 0.39f, 0.24f), cabin.transform);
            CreateCube("HomeCabin_Roof", new Vector3(0f, 1.65f, 0f), new Vector3(2.55f, 0.45f, 2.05f), new Color(0.31f, 0.18f, 0.2f), cabin.transform);
            CreateCube("HomeCabin_Door", new Vector3(0.35f, 0.47f, -0.94f), new Vector3(0.45f, 0.9f, 0.08f), new Color(0.36f, 0.22f, 0.14f), cabin.transform);
            CreateCube("HomeCabin_WindowGlow", new Vector3(-0.55f, 0.9f, -0.94f), new Vector3(0.45f, 0.42f, 0.08f), new Color(0.95f, 0.82f, 0.45f), cabin.transform, true);
            CreateCube("HomeCabin_DoorWreath", new Vector3(0.35f, 0.82f, -1.0f), new Vector3(0.28f, 0.2f, 0.06f), new Color(0.48f, 0.78f, 0.42f), cabin.transform);

            GameObject stickerWall = CreateCube("StickerWall", new Vector3(-4.72f, 0.65f, -3.25f), new Vector3(0.12f, 1.05f, 1.1f), new Color(0.5f, 0.34f, 0.22f), root);
            GameObject[] stickers =
            {
                CreateCube("StickerWall_Sticker_1", new Vector3(0f, 0.28f, -0.32f), new Vector3(0.08f, 0.22f, 0.22f), new Color(1f, 0.55f, 0.72f), stickerWall.transform),
                CreateCube("StickerWall_Sticker_2", new Vector3(0f, 0.02f, 0f), new Vector3(0.08f, 0.22f, 0.22f), new Color(0.58f, 0.92f, 0.75f), stickerWall.transform),
                CreateCube("StickerWall_Sticker_3", new Vector3(0f, -0.25f, 0.32f), new Vector3(0.08f, 0.22f, 0.22f), new Color(0.45f, 0.84f, 1f), stickerWall.transform)
            };
            stickerWall.AddComponent<StickerWallView>().Configure(stickers);

            GameObject signboard = CreateNode("HomeSignboard", "home-signboard", "木屋前木牌", "看看今日可换和图纸", new Vector3(-4.1f, 0.45f, -2.2f), new Vector3(0.7f, 0.9f, 0.15f), new Color(0.72f, 0.51f, 0.28f));
            CreateCube("HomeSignboard_Post", new Vector3(0f, -0.55f, 0f), new Vector3(0.14f, 0.7f, 0.12f), new Color(0.48f, 0.3f, 0.16f), signboard.transform);
            signboard.transform.SetParent(root);
        }

        private static void CreateForest(Transform root)
        {
            CreateTree("ForestTree_AnchorLeft", new Vector3(-7.2f, 0f, 2.2f), 1.2f, root, "tree_pineRoundA");
            CreateTree("ForestTree_BackLeft", new Vector3(-6.1f, 0f, 3.7f), 0.95f, root, "tree_pineRoundB");
            CreateTree("ForestTree_BackCenter", new Vector3(-4.55f, 0f, 3.35f), 0.9f, root, "tree_oak");
            CreateTree("ForestTree_AnchorRight", new Vector3(-3.35f, 0f, 2.35f), 1.1f, root, "tree_pineRoundA");
            CreateTree("ForestTree_FarSoft_1", new Vector3(-7.75f, 0f, 4.55f), 0.7f, root, "tree_small");
            CreateTree("ForestTree_FarSoft_2", new Vector3(-2.75f, 0f, 4.25f), 0.72f, root, "tree_small");

            CreateCube("ForestShadowPatch", new Vector3(-5.4f, 0.02f, 2.6f), new Vector3(4.4f, 0.04f, 2.2f), new Color(0.27f, 0.5f, 0.28f), root);
            CreateModelNode("ForestBranchNode", "forest-branch", "森林树枝", "拾取木材", "log", new Vector3(-6.35f, 0.15f, 1.08f), new Vector3(0.22f, 0.22f, 0.22f), new Color(0.48f, 0.28f, 0.16f), root);
            CreateModelNode("ForestFlowerSeedNode", "forest-flower-seed", "森林花种", "收起花种", "flower_purpleA", new Vector3(-4.55f, 0.12f, 0.75f), new Vector3(0.28f, 0.28f, 0.28f), new Color(0.92f, 0.68f, 0.76f), root);
            CreateKenneyModel("flower_redA", "ForestGarden_RedFlowers", new Vector3(-5.35f, 0.1f, 1.55f), new Vector3(0.22f, 0.22f, 0.22f), root, new Color(0.92f, 0.36f, 0.44f));
            CreateKenneyModel("flower_yellowA", "ForestGarden_YellowFlowers", new Vector3(-4.05f, 0.1f, 1.55f), new Vector3(0.22f, 0.22f, 0.22f), root, new Color(0.95f, 0.78f, 0.34f));
            CreateCube("ForestKoinoboriHint_Pole", new Vector3(-4.0f, 1.0f, 2.1f), new Vector3(0.06f, 1.8f, 0.06f), new Color(0.62f, 0.62f, 0.58f), root);
            CreateCube("ForestKoinoboriHint_Ribbon", new Vector3(-3.65f, 1.55f, 2.1f), new Vector3(0.7f, 0.16f, 0.05f), new Color(0.86f, 0.36f, 0.3f), root);
        }

        private static void CreateRiver(Transform root)
        {
            GameObject river = new GameObject("River");
            river.transform.SetParent(root);
            CreateRiverSegment("River_SouthBend", new Vector3(0.65f, -0.03f, -4.2f), new Vector3(2.45f, 0.08f, 3.1f), river.transform);
            CreateRiverSegment("River_CenterBend", new Vector3(0.95f, -0.035f, -0.95f), new Vector3(2.1f, 0.08f, 3.5f), river.transform);
            CreateRiverSegment("River_NorthBend", new Vector3(0.45f, -0.04f, 2.65f), new Vector3(2.55f, 0.08f, 3.9f), river.transform);
            CreateCube("RiverHighlight_South", new Vector3(0.05f, 0.07f, -3.6f), new Vector3(0.08f, 0.04f, 2.0f), new Color(0.58f, 0.9f, 0.92f), river.transform, true);
            CreateCube("RiverHighlight_North", new Vector3(0.1f, 0.07f, 1.8f), new Vector3(0.08f, 0.04f, 2.5f), new Color(0.58f, 0.9f, 0.92f), river.transform, true);

            CreateModelNode("RiverStoneNode", "river-stone", "河岸石子", "捡起石子", "rock_smallA", new Vector3(-0.8f, 0.15f, -1.75f), new Vector3(0.24f, 0.24f, 0.24f), new Color(0.45f, 0.47f, 0.48f), root);
            CreateNode("RiverShellNode", "river-shell", "河贝", "捡起河贝", new Vector3(1.95f, 0.09f, -2.25f), new Vector3(0.48f, 0.16f, 0.36f), new Color(0.9f, 0.82f, 0.68f)).transform.SetParent(root);
            CreateNode("RiverFishNode", "river-fish", "河水鱼影", "钓一下鱼", new Vector3(0.72f, 0.08f, 1.25f), new Vector3(0.68f, 0.06f, 0.34f), new Color(0.1f, 0.23f, 0.36f)).transform.SetParent(root);
        }

        private static void CreateBridgeAndBuildSlots(Transform root)
        {
            GameObject bridge = CreateNode("DamagedBridge", "river-bridge", "半损坏木桥", "修一下木桥", new Vector3(0.8f, 0.15f, -3.2f), new Vector3(2.6f, 0.12f, 0.8f), new Color(0.52f, 0.34f, 0.19f), 2.6f);
            CreateCube("DamagedBridge_GapShadow", new Vector3(0.12f, 0.16f, 0f), new Vector3(0.56f, 0.08f, 0.92f), new Color(0.15f, 0.11f, 0.09f), bridge.transform);
            CreateCube("DamagedBridge_LeftBoard", new Vector3(-0.72f, 0.22f, 0f), new Vector3(0.72f, 0.12f, 0.78f), new Color(0.42f, 0.25f, 0.14f), bridge.transform);
            CreateCube("DamagedBridge_RightBoard", new Vector3(0.92f, 0.22f, 0f), new Vector3(0.72f, 0.12f, 0.78f), new Color(0.42f, 0.25f, 0.14f), bridge.transform);
            GameObject repairedBridge = CreateKenneyModel("bridge_woodNarrow", "RepairedBridge", new Vector3(0f, 0.08f, 0f), new Vector3(0.42f, 0.42f, 0.42f), bridge.transform, new Color(0.72f, 0.47f, 0.26f));
            bridge.AddComponent<BuildPlacementView>().Configure("river-bridge", BlueprintId.Bridge, 8, 2, repairedBridge);
            bridge.transform.SetParent(root);

            CreateClearingFlowerBedSlot(root, "FlowerBedSlot", "flower-bed-slot", new Vector3(-5f, 0.08f, -4.4f), 3, 1);
            CreateClearingFlowerBedSlot(root, "ForestFlowerBedSlot", "forest-flower-bed-slot", new Vector3(-2.85f, 0.08f, 1.05f), 2, 3);
            CreateBuildSlot(root, "ForestSignSlot", "forest-sign-slot", "林间路牌空位", "立一块路牌", BlueprintId.ForestSign, 2, 4, new Vector3(-3.4f, 0.25f, 0.1f), new Vector3(0.5f, 0.5f, 0.18f), new Color(0.62f, 0.43f, 0.22f), "BuiltForestSign", new Color(0.85f, 0.62f, 0.3f));
            CreateBuildSlot(root, "RiverLampSlot", "river-lamp-slot", "河岸灯空位", "放一盏河岸灯", BlueprintId.RiverLamp, 7, 4, new Vector3(2.3f, 0.35f, 1.8f), new Vector3(0.35f, 0.7f, 0.35f), new Color(0.95f, 0.82f, 0.36f), "BuiltRiverLamp", new Color(0.32f, 0.95f, 0.86f), true);
        }

        private static void CreateArcadeClearing(Transform root)
        {
            CreateCube("ArcadeClearing", new Vector3(5.8f, 0.01f, -2.5f), new Vector3(3.5f, 0.06f, 2.8f), new Color(0.46f, 0.72f, 0.48f), root);

            GameObject arcade = CreateNode("ArcadeMachine", "clearing-arcade", "空地游戏机", "打开游戏机菜单", new Vector3(5.8f, 0.7f, -2.5f), new Vector3(0.9f, 1.4f, 0.7f), new Color(0.16f, 0.17f, 0.22f));
            GameObject screen = CreateCube("ArcadeScreen", new Vector3(0f, 0.22f, -0.37f), new Vector3(0.62f, 0.38f, 0.04f), new Color(0.07f, 0.28f, 0.34f), arcade.transform, true);
            arcade.AddComponent<ArcadeMachineView>().Configure(screen.GetComponent<Renderer>());
            arcade.transform.SetParent(root);

            CreateBuildSlot(root, "ArcadeBaseSlot", "arcade-base-slot", "游戏机底座空位", "搭一个底座", BlueprintId.ArcadeBase, 10, 2, new Vector3(5.8f, 0.08f, -3.2f), new Vector3(1.4f, 0.16f, 1.0f), new Color(0.4f, 0.42f, 0.5f), "BuiltArcadeBase", new Color(0.55f, 0.58f, 0.68f));
        }

        private static void CreateDistantLightScreen(Transform root)
        {
            CreateCube("DistantLightScreen_Unreachable", new Vector3(7.9f, 1.8f, 3.8f), new Vector3(0.12f, 3.2f, 4f), new Color(0.36f, 0.82f, 0.94f), root, true);
        }

        private static void CreateRuntime(Transform root)
        {
            GameObject runtime = new GameObject("GameStateRunner");
            runtime.AddComponent<GameStateRunner>();
            runtime.AddComponent<HudView>();
            runtime.transform.SetParent(root);
        }

        private static void CreateBuildSlot(
            Transform root,
            string slotName,
            string nodeId,
            string label,
            string prompt,
            BlueprintId blueprintId,
            int gridX,
            int gridY,
            Vector3 position,
            Vector3 scale,
            Color slotColor,
            string builtName,
            Color builtColor,
            bool emission = false)
        {
            GameObject slot = CreateNode(slotName, nodeId, label, prompt, position, scale, slotColor);
            GameObject built = CreateCube(builtName, new Vector3(0f, scale.y * 2f + 0.05f, 0f), scale + new Vector3(0.15f, 0.35f, 0.15f), builtColor, slot.transform, emission);
            slot.AddComponent<BuildPlacementView>().Configure(nodeId, blueprintId, gridX, gridY, built);
            slot.transform.SetParent(root);
        }

        private static void CreateClearingFlowerBedSlot(Transform root, string slotName, string nodeId, Vector3 position, int gridX, int gridY)
        {
            GameObject slot = CreateNode(slotName, nodeId, "被杂草占住的地块", "分阶段清理，再建设花圃", position, new Vector3(1.1f, 0.12f, 0.82f), new Color(0.55f, 0.56f, 0.38f));
            GameObject branchLayer = CreateCube($"{slotName}_FallenBranches", new Vector3(-0.18f, 0.16f, -0.1f), new Vector3(0.78f, 0.12f, 0.18f), new Color(0.44f, 0.25f, 0.13f), slot.transform);
            GameObject stoneLayer = CreateCube($"{slotName}_Pebbles", new Vector3(0.22f, 0.2f, 0.2f), new Vector3(0.44f, 0.18f, 0.32f), new Color(0.45f, 0.47f, 0.46f), slot.transform);
            GameObject weedLayer = CreateCube($"{slotName}_Weeds", new Vector3(0f, 0.23f, 0f), new Vector3(0.86f, 0.32f, 0.62f), new Color(0.25f, 0.58f, 0.27f), slot.transform);
            GameObject clearedSlot = CreateCube($"{slotName}_ClearedSoil", new Vector3(0f, 0.08f, 0f), new Vector3(0.94f, 0.08f, 0.66f), new Color(0.66f, 0.5f, 0.38f), slot.transform);
            GameObject built = CreateFlowerBed($"{slotName}_BuiltFlowerBed", slot.transform);
            slot.AddComponent<BuildPlacementView>().Configure(nodeId, BlueprintId.FlowerBed, gridX, gridY, built);
            slot.AddComponent<ClearingPlotView>().Configure(nodeId, BlueprintId.FlowerBed, gridX, gridY, branchLayer, stoneLayer, weedLayer, clearedSlot);
            slot.transform.SetParent(root);
        }

        private static GameObject CreateFlowerBed(string name, Transform parent)
        {
            GameObject bed = new GameObject(name);
            bed.transform.SetParent(parent);
            bed.transform.localPosition = new Vector3(0f, 0.28f, 0f);
            CreateCube($"{name}_Soil", Vector3.zero, new Vector3(1.05f, 0.18f, 0.72f), new Color(0.48f, 0.32f, 0.22f), bed.transform);
            CreateKenneyModel("flower_redA", $"{name}_PinkFlowers", new Vector3(-0.28f, 0.16f, -0.16f), new Vector3(0.2f, 0.2f, 0.2f), bed.transform, new Color(0.92f, 0.48f, 0.68f));
            CreateKenneyModel("flower_yellowA", $"{name}_YellowFlowers", new Vector3(0.18f, 0.16f, 0.08f), new Vector3(0.2f, 0.2f, 0.2f), bed.transform, new Color(0.95f, 0.78f, 0.34f));
            CreateKenneyModel("flower_purpleA", $"{name}_PurpleFlowers", new Vector3(0f, 0.16f, -0.28f), new Vector3(0.16f, 0.16f, 0.16f), bed.transform, new Color(0.7f, 0.56f, 0.9f));
            CreateCube($"{name}_LeafCluster", new Vector3(0.34f, 0.15f, -0.22f), new Vector3(0.24f, 0.22f, 0.2f), new Color(0.35f, 0.66f, 0.36f), bed.transform);
            return bed;
        }

        private static void CreatePixelPlayer(Transform root)
        {
            GameObject player = new GameObject("PixelPlayer");
            player.transform.position = new Vector3(-3.5f, -0.55f, -0.08f);
            player.transform.SetParent(root);
            CreateCube("PixelPlayer_Hair", new Vector3(0f, 0.18f, 0f), new Vector3(0.46f, 0.32f, 0.12f), new Color(0.05f, 0.04f, 0.07f), player.transform);
            CreateCube("PixelPlayer_Dress", new Vector3(0f, -0.16f, 0f), new Vector3(0.36f, 0.42f, 0.12f), new Color(0.24f, 0.13f, 0.32f), player.transform);
            CreateCube("PixelSpiritFish", new Vector3(0.42f, 0.02f, -0.02f), new Vector3(0.24f, 0.1f, 0.08f), new Color(0.45f, 0.88f, 0.95f), player.transform, true);
        }

        private static void CreateSticker(string name, Vector3 position, Transform root)
        {
            GameObject sticker = CreateCube(name, position, new Vector3(0.28f, 0.28f, 0.08f), new Color(1f, 0.52f, 0.68f), root, true);
            CreateCube($"{name}_Shine", new Vector3(0f, 0.03f, -0.05f), new Vector3(0.13f, 0.13f, 0.04f), new Color(1f, 0.95f, 0.45f), sticker.transform, true);
        }

        private static void CreateTree(string name, Vector3 position, float size, Transform root, string modelAssetName = null)
        {
            if (!string.IsNullOrEmpty(modelAssetName)
                && CreateKenneyModel(modelAssetName, name, position, Vector3.one * (0.42f * size), root, new Color(0.2f, 0.52f, 0.28f)) != null)
            {
                return;
            }

            GameObject tree = new GameObject(name);
            tree.transform.position = position;
            tree.transform.SetParent(root);
            CreateCube("TreeTrunk", new Vector3(0f, 0.42f * size, 0f), new Vector3(0.26f, 0.84f, 0.26f) * size, new Color(0.43f, 0.25f, 0.14f), tree.transform);
            CreateCube("TreeCrown_Core", new Vector3(0f, 1.13f * size, 0f), new Vector3(0.98f, 0.84f, 0.98f) * size, new Color(0.2f, 0.52f, 0.28f), tree.transform);
            CreateCube("TreeCrown_Left", new Vector3(-0.32f * size, 1.02f * size, -0.04f * size), new Vector3(0.58f, 0.5f, 0.7f) * size, new Color(0.18f, 0.46f, 0.25f), tree.transform);
            CreateCube("TreeCrown_Highlight", new Vector3(-0.18f * size, 1.42f * size, -0.18f * size), new Vector3(0.38f, 0.22f, 0.38f) * size, new Color(0.34f, 0.67f, 0.36f), tree.transform);
        }

        private static void CreateRiverSegment(string name, Vector3 position, Vector3 waterScale, Transform root)
        {
            CreateCube($"{name}_LeftWetBank", position + new Vector3(-waterScale.x * 0.5f - 0.18f, 0.035f, 0f), new Vector3(0.36f, 0.05f, waterScale.z + 0.18f), new Color(0.5f, 0.46f, 0.34f), root);
            CreateCube($"{name}_RightWetBank", position + new Vector3(waterScale.x * 0.5f + 0.18f, 0.035f, 0f), new Vector3(0.36f, 0.05f, waterScale.z + 0.18f), new Color(0.5f, 0.46f, 0.34f), root);
            CreateCube(name, position, waterScale, new Color(0.2f, 0.56f, 0.68f), root, true);
        }

        private static GameObject CreateNode(string name, string nodeId, string label, string prompt, Vector3 position, Vector3 scale, Color color, float interactionRadius = 1.5f)
        {
            GameObject node = CreateCube(name, position, scale, color);
            node.AddComponent<WorldNodeInteractor>().Configure(nodeId, label, prompt, interactionRadius);
            return node;
        }

        private static GameObject CreateModelNode(
            string name,
            string nodeId,
            string label,
            string prompt,
            string modelAssetName,
            Vector3 position,
            Vector3 modelScale,
            Color fallbackColor,
            Transform root,
            float interactionRadius = 1.5f)
        {
            GameObject node = new GameObject(name);
            node.transform.SetParent(root);
            node.transform.localPosition = position;
            node.AddComponent<WorldNodeInteractor>().Configure(nodeId, label, prompt, interactionRadius);
            GameObject model = CreateKenneyModel(modelAssetName, $"{name}_Model", Vector3.zero, modelScale, node.transform, fallbackColor);
            if (model == null)
            {
                CreateCube($"{name}_Fallback", Vector3.zero, modelScale, fallbackColor, node.transform);
            }

            return node;
        }

        private static GameObject CreateKenneyModel(string assetName, string instanceName, Vector3 position, Vector3 scale, Transform parent, Color fallbackColor)
        {
            string assetPath = $"Assets/External/Kenney/NatureKit/OBJ/{assetName}.obj";
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            if (prefab == null)
            {
                return null;
            }

            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            instance.name = instanceName;
            instance.transform.SetParent(parent);
            instance.transform.localPosition = position;
            instance.transform.localRotation = Quaternion.identity;
            instance.transform.localScale = scale;
            Material material = CreateMaterial($"{instanceName}_Material", fallbackColor);
            foreach (Renderer renderer in instance.GetComponentsInChildren<Renderer>())
            {
                renderer.sharedMaterial = material;
            }

            return instance;
        }

        private static GameObject CreateCube(string name, Vector3 position, Vector3 scale, Color color, Transform parent = null, bool emission = false)
        {
            return CreatePrimitive(PrimitiveType.Cube, name, position, scale, color, parent, emission);
        }

        private static GameObject CreateSphere(string name, Vector3 position, Vector3 scale, Color color, Transform parent = null, bool emission = false)
        {
            return CreatePrimitive(PrimitiveType.Sphere, name, position, scale, color, parent, emission);
        }

        private static GameObject CreatePrimitive(PrimitiveType primitiveType, string name, Vector3 position, Vector3 scale, Color color, Transform parent = null, bool emission = false)
        {
            GameObject gameObject = GameObject.CreatePrimitive(primitiveType);
            gameObject.name = name;
            gameObject.transform.SetParent(parent);
            gameObject.transform.localPosition = parent == null ? position : position;
            gameObject.transform.localScale = scale;
            Renderer renderer = gameObject.GetComponent<Renderer>();
            renderer.sharedMaterial = CreateMaterial($"{name}_Material", color, emission);
            return gameObject;
        }

        private static Material CreateMaterial(string name, Color color, bool emission = false)
        {
            Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"))
            {
                name = name,
                color = color
            };

            if (emission)
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", color * 1.35f);
            }

            return material;
        }

        private static void CreateCameraAndLight()
        {
            GameObject cameraObject = new GameObject("Main Camera");
            Camera camera = cameraObject.AddComponent<Camera>();
            camera.transform.position = new Vector3(0f, 10f, -9.5f);
            camera.transform.rotation = Quaternion.Euler(57f, 0f, 0f);
            camera.orthographic = true;
            camera.orthographicSize = 6.2f;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.62f, 0.78f, 0.86f);

            GameObject lightObject = new GameObject("Sun Light");
            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.15f;
            light.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        }

        private static void CreateMiniGameCameraAndLight()
        {
            GameObject cameraObject = new GameObject("Main Camera");
            Camera camera = cameraObject.AddComponent<Camera>();
            camera.transform.position = new Vector3(0f, 0.3f, -8f);
            camera.transform.rotation = Quaternion.identity;
            camera.orthographic = true;
            camera.orthographicSize = 3.2f;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.07f, 0.08f, 0.14f);

            GameObject lightObject = new GameObject("MiniGame Light");
            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.05f;
            light.transform.rotation = Quaternion.Euler(35f, -20f, 0f);
        }
    }
}
