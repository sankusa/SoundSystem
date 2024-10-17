using UnityEditor;

namespace SoundSystem {
    public class AudioClipImportSettingPreprocessor : AssetPostprocessor {
        void OnPreprocessAudio() {
            // 初回インポート時のみ処理する
            if (assetImporter.importSettingsMissing == false) return;

            AudioImporter importer = assetImporter as AudioImporter;

            string folderPath = EditorUtil.GetFolderPath(assetPath);
            StandardAudioClipImportSettings standardSetting = EditorUtil.FindAssetInNearestAncestorDirectory<StandardAudioClipImportSettings>(folderPath);
            standardSetting?.ApplyToImporter(importer);
        }
    }
}