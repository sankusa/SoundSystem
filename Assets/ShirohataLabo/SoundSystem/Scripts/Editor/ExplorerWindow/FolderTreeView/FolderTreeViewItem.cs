using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SoundSystem {
    public class FolderTreeViewItem : TreeViewItem {
        public string FolderPath { get; }
        public DefaultAsset FolderAsset { get; }
        public StandardAudioClipImportSettings StandardImportSettings { get; }

        public bool? ImportSettingCheckResult { get; private set; }

        public int PlayableObjectCount { get; }

        public event Action OnReleaseFolder;

        public FolderTreeViewItem(string folderPath) : base(0, 0, "") {
            FolderPath = folderPath;
            FolderAsset = AssetDatabase.LoadAssetAtPath<DefaultAsset>(folderPath);
            StandardImportSettings = EditorUtil.LoadAllAssetFromTargetFolder<StandardAudioClipImportSettings>(FolderPath).FirstOrDefault();
            id = FolderAsset.GetInstanceID();
            displayName = FolderAsset.name;
            PlayableObjectCount = PlayableObjectUtil.CountAllPlayableObjects(new string[] {FolderPath});
        }

        public void OnDoubleClick() {
            Selection.activeObject = FolderAsset;
            EditorApplication.delayCall += () => {
                AssetDatabase.OpenAsset(FolderAsset);
            };
        }

        public void OnContextClick() {
            GenericMenu menu = new();
            if (depth == 0) {
                menu.AddItem(
                    new GUIContent("Release"),
                    false,
                    () => {
                        TargetFolders.Instance.Remove(FolderAsset);
                        OnReleaseFolder?.Invoke();
                    }
                );
            }
            menu.AddItem(
                new GUIContent("Create New Folder"),
                false,
                () => {
                    Directory.CreateDirectory(FolderPath + "/New Folder");
                    AssetDatabase.Refresh();
                }
            );
            menu.AddItem(
                new GUIContent("Delete"),
                false,
                () => {
                    EditorUtil.DeleteAssetWithDialog(FolderAsset);
                }
            );
            menu.AddItem(
                new GUIContent("Check Import Setting"),
                false,
                () => CheckImportSettings()
            );
            menu.AddItem(
                new GUIContent("Apply Import Setting"),
                false,
                () => ApplyImportSettings()
            );
            menu.ShowAsContext();
        }

        public bool? CheckImportSettings(StandardAudioClipImportSettings standardSettings) {
            if (StandardImportSettings != null) standardSettings = StandardImportSettings;

            bool? result = null;
            if (children != null) {
                foreach (TreeViewItem item in children) {
                    if (item is FolderTreeViewItem folderItem) {
                        bool? ret = folderItem.CheckImportSettings(standardSettings);
                        if (ret != null) {
                            result ??= true;
                            result &= ret;
                        }
                    }
                }
            }

            if (standardSettings != null) {
                foreach (AudioClip clip in EditorUtil.LoadAllAssetFromTargetFolder<AudioClip>(FolderPath)) {
                    bool ret = standardSettings.Check(clip);
                    result ??= true;
                    result &= ret;
                }
            }
            ImportSettingCheckResult = result;

            return ImportSettingCheckResult;
        }

        public bool? CheckImportSettings() {
            StandardAudioClipImportSettings setting = EditorUtil.FindAssetInNearestAncestorDirectory<StandardAudioClipImportSettings>(FolderPath);
            return CheckImportSettings(setting);
        }

        public void ApplyImportSettings(StandardAudioClipImportSettings standardSettings) {
            if (StandardImportSettings != null) standardSettings = StandardImportSettings;

            if (children != null) {
                foreach (TreeViewItem item in children) {
                    if (item is FolderTreeViewItem folderItem) {
                        folderItem.ApplyImportSettings(standardSettings);
                    }
                }
            }
            if (standardSettings != null) {
                foreach (AudioClip clip in EditorUtil.LoadAllAssetFromTargetFolder<AudioClip>(FolderPath)) {
                    standardSettings.Apply(clip);
                }
            }
        }

        public void ApplyImportSettings() {
            StandardAudioClipImportSettings setting = EditorUtil.FindAssetInNearestAncestorDirectory<StandardAudioClipImportSettings>(FolderPath);
            ApplyImportSettings(setting);
        }
    }
}