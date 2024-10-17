using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    public static class AudioUnitUtil {
        public static void CreateAudioUnit(AudioClip clip, string folderPath) {
            string clipPath = AssetDatabase.GetAssetPath(clip);

            AudioUnitCategory category =
                EditorUtil.FindAssetInNearestAncestorDirectory<AudioUnitCategory>(folderPath);

            EditorApplication.delayCall += () => {
                EditorUtil.CreateAsset<AudioUnit>(
                    folderPath,
                    Path.GetFileNameWithoutExtension(clipPath),
                    data => {
                        data.Clip = clip;
                        data.SetCategoryWithUndo(category, true);
                    }
                );
                AssetDatabase.Refresh();
            };
        }

        public static void CreateAudioUnits(IEnumerable<AudioClip> clips, string folderPath) {
            if (clips.Count() > 0) {
                foreach (AudioClip clip in clips) {
                    CreateAudioUnit(clip, folderPath);
                }
            }
        }
    }
}