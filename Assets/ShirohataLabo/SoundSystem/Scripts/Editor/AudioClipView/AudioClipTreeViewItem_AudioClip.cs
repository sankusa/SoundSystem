using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SoundSystem {
    public class AudioClipTreeViewItem_AudioClip : TreeViewItem {
        public AudioClip Clip { get; }

        bool? _importSettingCheckResult;
        public bool? ImportSettingCheckResult => _importSettingCheckResult;

        public AudioClipTreeViewItem_AudioClip(int id, AudioClip clip) : base(id, 0, clip.name) {
            Clip = clip;
        }

        public void SetImportSettingCheckResult(bool value) {
            _importSettingCheckResult = value;
        }

        public void OnSingleClick() {
            Selection.activeObject = Clip;
        }

        public void OnDoubleClick() {
            EditorGUIUtility.PingObject(Clip);
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
            if (standardSettings == null) return;
            _importSettingCheckResult = standardSettings.Check(Clip);
        }

        public void CheckImportSettings() {
            string assetPath = AssetDatabase.GetAssetPath(Clip);
            string folderPath = EditorUtil.GetFolderPath(assetPath);
            StandardAudioClipImportSettings setting = EditorUtil.FindAssetInNearestAncestorDirectory<StandardAudioClipImportSettings>(folderPath);
            CheckImportSettings(setting);
        }

        public void ApplyImportSettings(StandardAudioClipImportSettings standardSettings) {
            if (standardSettings == null) return;
            standardSettings.Apply(Clip);
        }

        public void ApplyImportSettings() {
            string assetPath = AssetDatabase.GetAssetPath(Clip);
            string folderPath = EditorUtil.GetFolderPath(assetPath);
            StandardAudioClipImportSettings setting = EditorUtil.FindAssetInNearestAncestorDirectory<StandardAudioClipImportSettings>(folderPath);
            ApplyImportSettings(setting);
        }
    }
}