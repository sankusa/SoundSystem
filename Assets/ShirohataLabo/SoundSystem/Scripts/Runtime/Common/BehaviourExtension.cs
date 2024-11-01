using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    public static class BehaviourExtension {
        public static void DestroyFlexible(this Behaviour self) {
#if UNITY_EDITOR
            if (EditorApplication.isPlaying) {
                Object.Destroy(self);
            }
            else {
                Object.DestroyImmediate(self);
            }
#else
            Object.Destroy(self);
#endif
        }
    }
}