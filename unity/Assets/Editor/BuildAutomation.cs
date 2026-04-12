using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace StarryForest.EditorTools
{
    public static class BuildAutomation
    {
        private const string WindowsBuildPath = "Builds/Windows/StarryForest.exe";
        private static readonly string[] BuildScenes =
        {
            "Assets/Scenes/WorldHub.unity",
            "Assets/Scenes/MiniGame01.unity"
        };

        public static void BuildWindows()
        {
            string buildDirectory = Path.GetDirectoryName(WindowsBuildPath);
            if (!string.IsNullOrEmpty(buildDirectory))
            {
                Directory.CreateDirectory(buildDirectory);
            }

            BuildPlayerOptions options = new BuildPlayerOptions
            {
                scenes = BuildScenes,
                locationPathName = WindowsBuildPath,
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result != BuildResult.Succeeded)
            {
                throw new System.InvalidOperationException($"Windows build failed: {report.summary.result}");
            }

            UnityEngine.Debug.Log($"Windows build succeeded: {WindowsBuildPath}");
        }
    }
}
