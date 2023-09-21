using System;
using System.Collections.Generic;
using System.IO;
using Unity.Scenes.Editor;
using UnityEngine;
using UnityEditor;
using Unity.Entities.Build;
using Unity.Entities.Content;
namespace Editor
{
    static class BuildUtilities
    {
        private static string PathCombine(string path1, string path2)
        {
            if (!Path.IsPathRooted(path2)) return Path.Combine(path1, path2);
 
            path2 = path2.TrimStart(Path.DirectorySeparatorChar);
            path2 = path2.TrimStart(Path.AltDirectorySeparatorChar);
 
            return Path.Combine(path1, path2);
        }

        //不构建Player，只构建Content
        [MenuItem("Build/Content Update")]
        static void CreateContentUpdate()
        {
            var buildFolder = EditorUtility.OpenFolderPanel("Select Build To Publish", Path.GetDirectoryName(Application.dataPath), "Builds");
            if (!string.IsNullOrEmpty(buildFolder))
            {
                var buildTarget = EditorUserBuildSettings.activeBuildTarget;
                var tmpBuildFolder = PathCombine(Path.GetDirectoryName(Application.dataPath), $"/Library/ContentUpdateBuildDir/{PlayerSettings.productName}");

                var instance = DotsGlobalSettings.Instance;
                var playerGuid = instance.GetPlayerType() == DotsGlobalSettings.PlayerType.Client ? instance.GetClientGUID() : instance.GetServerGUID();
                if (!playerGuid.IsValid)
                    throw new Exception("Invalid Player GUID");

                var subSceneGuids = new HashSet<Unity.Entities.Hash128>();
                for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
                {
                    var ssGuids = EditorEntityScenes.GetSubScenes(EditorBuildSettings.scenes[i].guid);
                    foreach (var ss in ssGuids)
                        subSceneGuids.Add(ss);
                }
                RemoteContentCatalogBuildUtility.BuildContent(subSceneGuids, playerGuid, buildTarget, tmpBuildFolder);

                var publishFolder = Path.Combine(Path.GetDirectoryName(Application.dataPath), "Builds", $"{buildFolder}-RemoteContent");
                RemoteContentCatalogBuildUtility.PublishContent(tmpBuildFolder, publishFolder, f => new string[] { "all" });
            }
        }
        
        //准备发布内容文件。在此过程中可以通过更改PublishContent最后一个参数删除或保留原始文件。
        [MenuItem("Build/Existing Build")]
        static void PublishExistingBuild()
        {
            var buildFolder = EditorUtility.OpenFolderPanel("Select Build To Publish", Path.GetDirectoryName(Application.dataPath), "Builds");
            if (!string.IsNullOrEmpty(buildFolder))
            {
                var streamingAssetsPath = $"{buildFolder}/{PlayerSettings.productName}_Data/StreamingAssets";
                RemoteContentCatalogBuildUtility.PublishContent(streamingAssetsPath, $"{buildFolder}-RemoteContent", f => new string[] { "all" }, true);
            }
        }
    }
}

