using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SoundSystem {
    public class AudioClipTreeViewItem_Folder : TreeViewItem {
        string _folderPath;
        public string FolderPath => _folderPath;

        DefaultAsset _folderAsset;
        public DefaultAsset FolderAsset => _folderAsset;

        StandardAudioClipImportSettings _standardImportSettings;
        public StandardAudioClipImportSettings StandardImportSetting => _standardImportSettings;

        public AudioClipTreeViewItem_Folder(int id, string folderPath) : base(id, 0, "") {
            _folderPath = folderPath;
            _folderAsset = AssetDatabase.LoadAssetAtPath<DefaultAsset>(folderPath);
            _standardImportSettings = EditorUtil.LoadAllAsset<StandardAudioClipImportSettings>(_folderPath).FirstOrDefault();
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

        public void CheckImportSettings(StandardAudioClipImportSettings standardSettings) {
            if (children == null) return;

            if (_standardImportSettings != null) standardSettings = _standardImportSettings;

            foreach (TreeViewItem item in children) {
                if (item is AudioClipTreeViewItem_Folder folderItem) {
                    folderItem.CheckImportSettings(standardSettings);
                }
                else if (item is AudioClipTreeViewItem_AudioClip clipItem) {
                    clipItem.CheckImportSettings(standardSettings);
                }
            }
        }

        public void CheckImportSettings() {
            string folderPath = AssetDatabase.GetAssetPath(_folderAsset);
            StandardAudioClipImportSettings setting = EditorUtil.FindAssetInNearestAncestorDirectory<StandardAudioClipImportSettings>(folderPath);
            CheckImportSettings(setting);
        }

        public void ApplyImportSettings(StandardAudioClipImportSettings standardSettings) {
            if (children == null) return;

            if (_standardImportSettings != null) standardSettings = _standardImportSettings;

            foreach (TreeViewItem item in children) {
                if (item is AudioClipTreeViewItem_Folder folderItem) {
                    folderItem.ApplyImportSettings(standardSettings);
                }
                else if (item is AudioClipTreeViewItem_AudioClip clipItem) {
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