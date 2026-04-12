using System.IO;
using StarryForest.Player;
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

        public static void BuildWorldHubScene()
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            GameObject root = new GameObject("WorldHub");
            CreateGround(root.transform);
            CreatePlayer(root.transform);
            CreateHomeArea(root.transform);
            CreateForest(root.transform);
            CreateRiver(root.transform);
            CreateBridge(root.transform);
            CreateArcadeClearing(root.transform);
            CreateDistantLightScreen(root.transform);
            CreateCameraAndLight();

            Directory.CreateDirectory("Assets/Scenes");
            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
            AssetDatabase.SaveAssets();
        }

        public static void BuildMiniGame01Scene()
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            GameObject root = new GameObject("MiniGame01");

            CreateCube("PixelPlatform_Start", new Vector3(-3f, 0f, 0f), new Vector3(2f, 0.25f, 1f), new Color(0.23f, 0.25f, 0.32f)).transform.SetParent(root.transform);
            CreateCube("PixelPlatform_Middle", new Vector3(0f, 0.8f, 0f), new Vector3(2f, 0.25f, 1f), new Color(0.23f, 0.25f, 0.32f)).transform.SetParent(root.transform);
            CreateCube("PixelPlatform_Exit", new Vector3(3f, 1.4f, 0f), new Vector3(2f, 0.25f, 1f), new Color(0.23f, 0.25f, 0.32f)).transform.SetParent(root.transform);

            CreateCube("PixelPlayer", new Vector3(-3.5f, 0.6f, 0f), new Vector3(0.45f, 0.45f, 0.45f), new Color(1f, 0.86f, 0.22f)).transform.SetParent(root.transform);
            CreateCube("Sticker_1", new Vector3(-2.5f, 0.75f, 0f), new Vector3(0.28f, 0.28f, 0.08f), new Color(1f, 0.52f, 0.68f)).transform.SetParent(root.transform);
            CreateCube("Sticker_2", new Vector3(0f, 1.55f, 0f), new Vector3(0.28f, 0.28f, 0.08f), new Color(1f, 0.52f, 0.68f)).transform.SetParent(root.transform);
            CreateCube("Sticker_3", new Vector3(2.6f, 2.15f, 0f), new Vector3(0.28f, 0.28f, 0.08f), new Color(1f, 0.52f, 0.68f)).transform.SetParent(root.transform);
            CreateCube("ExitDoor", new Vector3(3.8f, 2.05f, 0f), new Vector3(0.45f, 0.9f, 0.2f), new Color(0.35f, 0.85f, 0.95f)).transform.SetParent(root.transform);

            CreateCameraAndLight();

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
            GameObject ground = CreateCube("Ground", new Vector3(0, -0.05f, 0), new Vector3(18f, 0.1f, 12f), new Color(0.38f, 0.68f, 0.43f));
            ground.transform.SetParent(root);
        }

        private static void CreatePlayer(Transform root)
        {
            GameObject player = CreateCube("Player", new Vector3(-5.6f, 0.45f, -1.5f), new Vector3(0.55f, 0.9f, 0.55f), new Color(0.98f, 0.86f, 0.22f));
            player.AddComponent<PlayerController>();
            player.transform.SetParent(root);
        }

        private static void CreateHomeArea(Transform root)
        {
            GameObject house = CreateCube("HomeCabin", new Vector3(-6f, 0.75f, -3f), new Vector3(2.2f, 1.5f, 1.8f), new Color(0.55f, 0.37f, 0.24f));
            house.transform.SetParent(root);

            GameObject signboard = CreateNode("HomeSignboard", "home-signboard", "木屋前木牌", "看看今日可换和图纸", new Vector3(-4.1f, 0.45f, -2.2f), new Vector3(0.7f, 0.9f, 0.15f), new Color(0.72f, 0.51f, 0.28f));
            signboard.transform.SetParent(root);
        }

        private static void CreateForest(Transform root)
        {
            for (int index = 0; index < 8; index++)
            {
                float x = -7f + (index % 4) * 1.2f;
                float z = 1.2f + (index / 4) * 1.4f;
                GameObject tree = CreateCube($"ForestTree_{index + 1}", new Vector3(x, 0.9f, z), new Vector3(0.8f, 1.8f, 0.8f), new Color(0.18f, 0.47f, 0.25f));
                tree.transform.SetParent(root);
            }

            CreateNode("ForestBranchNode", "forest-branch", "森林树枝", "拾取木材", new Vector3(-6.3f, 0.15f, 0.5f), new Vector3(0.5f, 0.3f, 0.35f), new Color(0.48f, 0.28f, 0.16f)).transform.SetParent(root);
            CreateNode("ForestFlowerSeedNode", "forest-flower-seed", "森林花种", "收起花种", new Vector3(-4.7f, 0.12f, 1.1f), new Vector3(0.35f, 0.24f, 0.35f), new Color(0.92f, 0.68f, 0.76f)).transform.SetParent(root);
        }

        private static void CreateRiver(Transform root)
        {
            GameObject river = CreateCube("River", new Vector3(0.8f, 0.02f, 0f), new Vector3(2.2f, 0.08f, 12f), new Color(0.22f, 0.55f, 0.72f));
            river.transform.SetParent(root);

            CreateNode("RiverStoneNode", "river-stone", "河岸石子", "捡起石子", new Vector3(-0.8f, 0.15f, -1.8f), new Vector3(0.4f, 0.3f, 0.4f), new Color(0.45f, 0.47f, 0.48f)).transform.SetParent(root);
            CreateNode("RiverShellNode", "river-shell", "河贝", "捡起河贝", new Vector3(1.9f, 0.12f, -2.3f), new Vector3(0.35f, 0.24f, 0.35f), new Color(0.9f, 0.82f, 0.68f)).transform.SetParent(root);
            CreateNode("RiverFishNode", "river-fish", "河水鱼影", "钓一下鱼", new Vector3(0.8f, 0.15f, 1.2f), new Vector3(0.55f, 0.2f, 0.35f), new Color(0.15f, 0.32f, 0.48f)).transform.SetParent(root);
        }

        private static void CreateBridge(Transform root)
        {
            GameObject bridge = CreateNode("DamagedBridge", "river-bridge", "半损坏木桥", "修一下木桥", new Vector3(0.8f, 0.15f, -3.2f), new Vector3(2.6f, 0.25f, 0.8f), new Color(0.52f, 0.34f, 0.19f));
            bridge.transform.SetParent(root);

            CreateNode("FlowerBedSlot", "flower-bed-slot", "花圃空位", "摆放花圃", new Vector3(-5f, 0.08f, -4.4f), new Vector3(1.1f, 0.16f, 0.8f), new Color(0.75f, 0.58f, 0.68f)).transform.SetParent(root);
            CreateNode("ForestSignSlot", "forest-sign-slot", "林间路牌空位", "立一块路牌", new Vector3(-3.4f, 0.25f, 0.1f), new Vector3(0.5f, 0.5f, 0.18f), new Color(0.62f, 0.43f, 0.22f)).transform.SetParent(root);
            CreateNode("RiverLampSlot", "river-lamp-slot", "河岸灯空位", "放一盏河岸灯", new Vector3(2.3f, 0.35f, 1.8f), new Vector3(0.35f, 0.7f, 0.35f), new Color(0.95f, 0.82f, 0.36f)).transform.SetParent(root);
        }

        private static void CreateArcadeClearing(Transform root)
        {
            GameObject clearing = CreateCube("ArcadeClearing", new Vector3(5.8f, 0.01f, -2.5f), new Vector3(3.5f, 0.06f, 2.8f), new Color(0.46f, 0.72f, 0.48f));
            clearing.transform.SetParent(root);

            CreateNode("ArcadeMachine", "clearing-arcade", "空地游戏机", "打开游戏机菜单", new Vector3(5.8f, 0.7f, -2.5f), new Vector3(0.9f, 1.4f, 0.7f), new Color(0.18f, 0.2f, 0.28f)).transform.SetParent(root);
            CreateNode("ArcadeBaseSlot", "arcade-base-slot", "游戏机底座空位", "搭一个底座", new Vector3(5.8f, 0.08f, -3.2f), new Vector3(1.4f, 0.16f, 1.0f), new Color(0.4f, 0.42f, 0.5f)).transform.SetParent(root);
        }

        private static void CreateDistantLightScreen(Transform root)
        {
            GameObject screen = CreateCube("DistantLightScreen_Unreachable", new Vector3(7.9f, 1.8f, 3.8f), new Vector3(0.12f, 3.2f, 4f), new Color(0.36f, 0.82f, 0.94f));
            screen.transform.SetParent(root);
        }

        private static void CreateCameraAndLight()
        {
            GameObject cameraObject = new GameObject("Main Camera");
            Camera camera = cameraObject.AddComponent<Camera>();
            camera.transform.position = new Vector3(0f, 9f, -9f);
            camera.transform.rotation = Quaternion.Euler(55f, 0f, 0f);
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.62f, 0.78f, 0.86f);

            GameObject lightObject = new GameObject("Sun Light");
            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.1f;
            light.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        }

        private static GameObject CreateNode(string name, string nodeId, string label, string prompt, Vector3 position, Vector3 scale, Color color)
        {
            GameObject node = CreateCube(name, position, scale, color);
            node.AddComponent<WorldNodeInteractor>().Configure(nodeId, label, prompt);
            return node;
        }

        private static GameObject CreateCube(string name, Vector3 position, Vector3 scale, Color color)
        {
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            gameObject.name = name;
            gameObject.transform.position = position;
            gameObject.transform.localScale = scale;
            Renderer renderer = gameObject.GetComponent<Renderer>();
            renderer.sharedMaterial = CreateMaterial($"{name}_Material", color);
            return gameObject;
        }

        private static Material CreateMaterial(string name, Color color)
        {
            Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"))
            {
                name = name,
                color = color
            };

            return material;
        }
    }
}
