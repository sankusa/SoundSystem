using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SoundSystem {
    public class ClipTreeViewItem_Folder : TreeViewItem {
        string _folderPath;
        public string FolderPath => _folderPath;

        DefaultAsset _folderAsset;
        public DefaultAsset FolderAsset => _folderAsset;

        StandardAudioClipImportSettings _standardImportSettings;
        public StandardAudioClipImportSettings StandardImportSetting => _standardImportSettings;

        bool? _importSettingCheckResult;
        public bool? ImportSettingCheckResult => _importSettingCheckResult;

        public ClipTreeViewItem_Folder(string folderPath) : base(0, 0, "") {
            _folderPath = folderPath;
            _folderAsset = AssetDatabase.LoadAssetAtPath<DefaultAsset>(folderPath);
            _standardImportSettings = EditorUtil.LoadAllAssetFromTargetFolder<StandardAudioClipImportSettings>(_folderPath).FirstOrDefault();
            id = _folderAsset.GetInstanceID();
        }

        public void OnSingleClick() {
            
        }

        public void OnDoubleClick() {
            Selection.activeObject = _folderAsset;
            EditorApplication.delayCall += () => {
                AssetDatabase.OpenAsset(_folderAsset);
            };
        }

        public void OnContextClick() {
            GenericMenu menu = new();
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
            if (children == null) return null;

            if (_standardImportSettings != null) standardSettings = _standardImportSettings;

            bool? result = null;
            foreach (TreeViewItem item in children) {
                if (item is ClipTreeViewItem_Folder folderItem) {
                    bool? ret = folderItem.CheckImportSettings(standardSettings);
                    if (ret != null) {
                        result ??= true;
                        result &= ret;
                    }
                }
                else if (item is ClipTreeViewItem_AudioClip clipItem) {
                    bool? ret = clipItem.CheckImportSettings(standardSettings);
                    if (ret != null) {
                        result ??= true;
                        result &= ret;
                    }
                }
            }
            _importSettingCheckResult = result;

            return _importSettingCheckResult;
        }

        public bool? CheckImportSettings() {
            string folderPath = AssetDatabase.GetAssetPath(_folderAsset);
            StandardAudioClipImportSettings setting = EditorUtil.FindAssetInNearestAncestorDirectory<StandardAudioClipImportSettings>(folderPath);
            return CheckImportSettings(setting);
        }

        public void ApplyImportSettings(StandardAudioClipImportSettings standardSettings) {
            if (children == null) return;

            if (_standardImportSettings != null) standardSettings = _standardImportSettings;

            foreach (TreeViewItem item in children) {
                if (item is ClipTreeViewItem_Folder folderItem) {
                    folderItem.ApplyImportSettings(standardSettings);
                }
                else if (item is ClipTreeViewItem_AudioClip clipItem) {
                    clipItem.ApplyImportSettings(standardSettings);
                }
            }
        }

        public void ApplyImportSettings() {
            string folderPath = AssetDatabase.GetAssetPath(_folderAsset);
            StandardAudioClipImportSettings setting = EditorUtil.FindAssetInNearestAncestorDirectory<StandardAudioClipImportSettings>(folderPath);
            ApplyImportSettings(setting);
        }
    }
}