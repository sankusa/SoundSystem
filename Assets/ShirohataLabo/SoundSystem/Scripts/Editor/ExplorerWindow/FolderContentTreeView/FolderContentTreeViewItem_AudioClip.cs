using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    public class FolderContentTreeViewItem_AudioClip : FolderContentTreeViewItemBase {
        public AudioClip AudioClip { get; }

        bool? _importSettingCheckResult;
        public bool? ImportSettingCheckResult => _importSettingCheckResult;

        public FolderContentTreeViewItem_AudioClip(ObjectDatabaseRecord record) : base(record) {
            AudioClip = Record.Asset as AudioClip;
        }

        public void SetImportSettingCheckResult(bool value) {
            _importSettingCheckResult = value;
        }

        public override void OnDoubleClick() {
            Selection.activeObject = AudioClip;
            EditorGUIUtility.PingObject(AudioClip);
        }

        public override void OnContextClick() {
            GenericMenu menu = new();
            menu.AddItem(
                new GUIContent("Delete"),
                false,
                () => AssetDatabase.MoveAssetToTrash(AssetDatabase.GetAssetPath(AudioClip))
            );
            menu.AddItem(
                new GUIContent("Create Custom Clip"),
                false,
                () => CustomClipUtil.CreateCustomClip(AudioClip, EditorUtil.GetFolderPath(AssetDatabase.GetAssetPath(AudioClip)))
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
            _importSettingCheckResult = standardSettings.Check(AudioClip);
            return _importSettingCheckResult;
        }

        public bool? CheckImportSettings() {
            string assetPath = AssetDatabase.GetAssetPath(AudioClip);
            string folderPath = EditorUtil.GetFolderPath(assetPath);
            StandardAudioClipImportSettings setting = EditorUtil.FindAssetInNearestAncestorDirectory<StandardAudioClipImportSettings>(folderPath);
            return CheckImportSettings(setting);
        }

        public void ApplyImportSettings(StandardAudioClipImportSettings standardSettings) {
            if (standardSettings == null) return;
            standardSettings.Apply(AudioClip);
        }

        public void ApplyImportSettings() {
            string assetPath = AssetDatabase.GetAssetPath(AudioClip);
            string folderPath = EditorUtil.GetFolderPath(assetPath);
            StandardAudioClipImportSettings setting = EditorUtil.FindAssetInNearestAncestorDirectory<StandardAudioClipImportSettings>(folderPath);
            ApplyImportSettings(setting);
        }
    }
}