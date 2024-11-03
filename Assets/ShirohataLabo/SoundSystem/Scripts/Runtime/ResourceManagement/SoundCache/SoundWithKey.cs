using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SoundSystem {
    [System.Serializable]
    public class SoundWithKey {
        [SerializeField] string _key;
        public string Key => _key;

        [SerializeField] Sound _sound;
        public Sound Sound => _sound;

#if UNITY_EDITOR
        public static SerializedProperty GetKeyProp(SerializedProperty soundWithKeyProp) {
            return soundWithKeyProp.FindPropertyRelative(nameof(_key));
        }

        public static SerializedProperty GetSoundProp(SerializedProperty soundWithKeyProp) {
            return soundWithKeyProp.FindPropertyRelative(nameof(_sound));
        }
#endif
    }
}