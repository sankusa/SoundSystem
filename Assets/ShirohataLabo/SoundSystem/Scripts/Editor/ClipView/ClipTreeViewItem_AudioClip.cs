using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SoundSystem {
    public class ClipTreeViewItem_AudioClip : TreeViewItem {
        public AudioClip Clip { get; }

        bool? _importSettingCheckResult;
        public bool? ImportSettingCheckResult => _importSettingCheckResult;

        public ClipTreeViewItem_AudioClip(AudioClip clip) : base(0, 0, clip.name) {
            Clip = clip;
            id = clip.GetInstanceID();
        }

        public void SetImportSettingCheckResult(bool value) {
            _importSettingCheckResult = value;
        }

        public void OnSingleClick() {
            
        }

        public void OnDoubleClick() {
            Selection.activeObject = Clip;
            EditorGUIUtility.PingObject(Clip);
        }

        public void OnContextClick() {
            GenericMenu menu = new();
            menu.AddItem(
                new GUIContent("Delete"),
                false,
                () => AssetDatabase.MoveAssetToTrash(AssetDatabase.GetAssetPath(Clip))
            );
            menu.AddItem(
                new GUIContent("Create Custom Clip"),
                false,
                () => CustomClipUtil.CreateCustomClip(Clip, EditorUtil.GetFolderPath(AssetDatabase.GetAssetPath(Clip)))
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
            if (standardSettings == null) return null;
            _importSettingCheckResult = standardSettings.Check(Clip);
            return _importSettingCheckResult;
        }

        public bool? CheckImportSettings() {
            string assetPath = AssetDatabase.GetAssetPath(Clip);
            string folderPath = EditorUtil.GetFolderPath(assetPath);
            StandardAudioClipImportSettings setting = EditorUtil.FindAssetInNearestAncestorDirectory<StandardAudioClipImportSettings>(folderPath);
            return CheckImportSettings(setting);
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