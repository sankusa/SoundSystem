#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace SoundSystem {
    public static class BehaviourExtension {
        public static void DestroyFlexible(this Behaviour self) {
            if (self == null) return;
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