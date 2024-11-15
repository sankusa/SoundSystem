using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    public static class PlayableObjectUtil {
        public static IEnumerable<Object> LoadAllPlayableObjects(string[] searchInFolders) {
            return EditorUtil.LoadAllAsset(
                PlayableObjectTypes.Types,
                searchInFolders
            );
        }


        public static int CountAllPlayableObjects(string[] searchInFolders) {
            return EditorUtil.CountAllAsset(
                PlayableObjectTypes.Types,
                searchInFolders
            );
        }
    }
}