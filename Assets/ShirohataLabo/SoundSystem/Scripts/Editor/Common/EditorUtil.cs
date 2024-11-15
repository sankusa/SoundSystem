using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SoundSystem {
    public class EditorUtil {
        public static T CreateAsset<T>(string folderPath, string assetNameWithoutExtension, Action<T> onBeforeCreateAsset = null) where T : ScriptableObject {
            T asset = ScriptableObject.CreateInstance<T>();
            onBeforeCreateAsset?.Invoke(asset);
            string uncheckedPath = Path.Combine(folderPath, $"{assetNameWithoutExtension}.asset");
            string correctPath = AssetDatabase.GenerateUniqueAssetPath(uncheckedPath);
            AssetDatabase.CreateAsset(asset, correctPath);
            return asset;
        }

        public static int CountAllAsset(Type[] targetTypes, string[] searchInFolders) {
            string filter = string.Join(' ', targetTypes.Select(type => $"t:{type.Name}"));
            return AssetDatabase
                .FindAssets(filter, searchInFolders)
                .Count();
        }

        public static IEnumerable<Object> LoadAllAsset(Type[] targetTypes, string[] searchInFolders) {
            string filter = string.Join(' ', targetTypes.Select(type => $"t:{type.Name}"));
            return AssetDatabase
                .FindAssets(filter, searchInFolders)
                .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .Select(assetPath => AssetDatabase.LoadAssetAtPath<Object>(assetPath));
        }

        public static IEnumerable<T> LoadAllAsset<T>() where T : UnityEngine.Object {
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).FullName}");
            return guids.Select(guid => 
                AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid))
            );
        }

        public static IEnumerable<T> LoadAllAssetFromTargetFolder<T>(string folderPath) where T : UnityEngine.Object {
            return AssetDatabase
                .FindAssets($"t:{typeof(T).Name}", new[]{folderPath})
                .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .Where(assetPath => GetFolderPath(assetPath) == folderPath)
                .Select(assetPath => AssetDatabase.LoadAssetAtPath<T>(assetPath));
        }

        public static bool ExistsTargetTypeAsset<T>(string folderPath) where T : UnityEngine.Object {
            return AssetDatabase
                .FindAssets($"t:{typeof(T).Name}", new[]{folderPath})
                .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .Where(assetPath => GetFolderPath(assetPath) == folderPath)
                .Any();
        }

        public static string GetAssetName(string assetPath) {
            return assetPath.Substring(assetPath.LastIndexOf('/') + 1);
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
                    return LoadAllAssetFromTargetFolder<T>(folderPath).First();
                }
                folderPath = GetParentFolderPath(folderPath);
                if (folderPath == null) break;
            }
            return null;
        }

        public static void RegisterAssetsMoveUndo(string[] paths) {
            typeof(Undo).GetMethod("RegisterAssetsMoveUndo", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] {paths});
        }

        public static void MoveAssetsWithUndo(IEnumerable<Object> assets, string toFolderPath) {
            if (assets.Count() > 0) {
                RegisterAssetsMoveUndo(assets.Select(x => AssetDatabase.GetAssetPath(x)).ToArray());
                foreach (Object asset in assets) {
                    string oldPath = AssetDatabase.GetAssetPath(asset);
                    string newPath = toFolderPath + "/" + GetAssetName(oldPath);
                    AssetDatabase.MoveAsset(oldPath, newPath);
                    AssetDatabase.Refresh();
                }
            }
        }

        public static void DeleteAssetWithDialog(Object asset) {
            string assetPath = AssetDatabase.GetAssetPath(asset);
            bool confirm = EditorUtility.DisplayDialog(
                "Delete selected asset?",
                $"{assetPath}\n\nYou cannot undo the delete assets action.",
                "Delete",
                "Cancel"
            );
            if (confirm) {
                AssetDatabase.DeleteAsset(assetPath);
            }
        }
    }
}