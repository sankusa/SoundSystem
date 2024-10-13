using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    public class EditorUtil {
        public static T CreateAsset<T>(string folderPath, string assetNameWithoutExtension, Action<T> onBeforeCreateAsset = null) where T : ScriptableObject {
            T asset = ScriptableObject.CreateInstance<T>();
            onBeforeCreateAsset?.Invoke(asset);
            AssetDatabase.CreateAsset(asset, Path.Combine(folderPath, $"{assetNameWithoutExtension}.asset"));
            return asset;
        }

        public static IEnumerable<T> LoadAllAsset<T>() where T : UnityEngine.Object {
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).FullName}");
            return guids.Select(guid => 
                AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid))
            );
        }

        public static IEnumerable<T> LoadAllAsset<T>(string folderPath) where T : UnityEngine.Object {
            return AssetDatabase
                .FindAssets($"t:{typeof(T).FullName}", new[]{folderPath})
                .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .Where(assetPath => GetFolderPath(assetPath) == folderPath)
                .Select(assetPath => AssetDatabase.LoadAssetAtPath<T>(assetPath));
        }

        public static bool ExistsTargetTypeAsset<T>(string folderPath) where T : UnityEngine.Object {
            return AssetDatabase
                .FindAssets($"t:{typeof(T).FullName}", new[]{folderPath})
                .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .Where(assetPath => GetFolderPath(assetPath) == folderPath)
                .Any();
        }

        public static string GetFolderPath(string assetPath) {
            return assetPath.Substring(0, assetPath.LastIndexOf('/'));
        }

        public static string GetParentFolderPath(string path) {
            int separatorIndex = path.LastIndexOf('/');
            if (separatorIndex == -1) return null;
            return path.Substring(0, separatorIndex);
        }

        public static T FindAssetInNearestAncestorDirectory<T>(string folderPath) where T : UnityEngine.Object {
            while (true) {
                if (ExistsTargetTypeAsset<T>(folderPath)) {
                    return LoadAllAsset<T>(folderPath).First();
                }
                folderPath = GetParentFolderPath(folderPath);
                if (folderPath == null) break;
            }
            return null;
        }
    }
}