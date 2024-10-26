using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    public static class CustomClipUtil {
        public static void CreateCustomClip(AudioClip clip, string folderPath) {
            string clipPath = AssetDatabase.GetAssetPath(clip);

            EditorApplication.delayCall += () => {
                EditorUtil.CreateAsset<CustomClip>(
                    folderPath,
                    Path.GetFileNameWithoutExtension(clipPath),
                    data => {
                        data.AudioClip = clip;
                    }
                );
                AssetDatabase.Refresh();
            };
        }

        public static void CreateCustomClips(IEnumerable<AudioClip> clips, string folderPath) {
            if (clips.Count() > 0) {
                foreach (AudioClip clip in clips) {
                    CreateCustomClip(clip, folderPath);
                }
            }
        }
    }
}