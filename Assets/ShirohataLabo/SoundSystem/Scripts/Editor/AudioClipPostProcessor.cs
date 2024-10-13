using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    public class AudioClipPostProcessor : AssetPostprocessor {
        // static readonly List<string> _firstImportedPaths = new();

        void OnPreprocessAudio() {
            // SoundSystemSetting setting = SoundSystemSetting.Instance;
            // if (setting == null) return;

            // // フォルダが登録されているか
            // if (string.IsNullOrEmpty(setting.ManagedFolderRootPath)) return;

            // // 登録されたフォルダの配下か
            // if (assetPath.Contains(setting.ManagedFolderRootPath) == false) return;

            // // 再インポートを弾く
            // AudioImporter importer = assetImporter as AudioImporter;
            // if (importer.importSettingsMissing == false) return;

            // _firstImportedPaths.Add(assetPath);
        }

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
            // SoundSystemSetting状態チェック
            SoundSystemSetting setting = SoundSystemSetting.Instance;
            if (setting == null) return;
            if (string.IsNullOrEmpty(setting.ManagedFolderRootPath)) return;

            foreach (string assetPath in importedAssets) {
                if (AssetDatabase.GetMainAssetTypeAtPath(assetPath) != typeof(AudioClip)) continue;
                if (assetPath.Contains(setting.ManagedFolderRootPath) == false) continue;
                CreateAudioUnitIfNeed(assetPath);
            }

            for (int i = 0; i < movedAssets.Length; i++) {
                string assetPath = movedAssets[i];
                string oldAssetPath = movedFromAssetPaths[i];
                if (AssetDatabase.GetMainAssetTypeAtPath(assetPath) != typeof(AudioClip)) continue;
                // 登録フォルダ外から登録フォルダ内に移動した場合のみ生成
                if (assetPath.Contains(setting.ManagedFolderRootPath) == false) continue;
                if (oldAssetPath.Contains(setting.ManagedFolderRootPath)) continue;
                CreateAudioUnitIfNeed(assetPath);
            }
        }

        static void CreateAudioUnitIfNeed(string assetPath) {
            AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(assetPath);
            string folderPath = EditorUtil.GetFolderPath(assetPath);

            // 同じフォルダにAudioClipを持っているAudioUnitが既にあったらスキップ
            if (AssetDatabase.LoadAllAssetsAtPath(folderPath).OfType<AudioUnit>().Where(x => x.Clip == clip).Any()) {
                return;
            }

            AudioUnitCategory category =
                EditorUtil.FindAssetInNearestAncestorDirectory<AudioUnitCategory>(folderPath);

            EditorApplication.delayCall += () => {
                EditorUtil.CreateAsset<AudioUnit>(
                    Path.GetDirectoryName(assetPath),
                    Path.GetFileNameWithoutExtension(assetPath),
                    data => {
                        data.Clip = clip;
                        data.SetCategoryWithUndo(category, true);
                    }
                );
                AssetDatabase.Refresh();
            };
        }
    }
}