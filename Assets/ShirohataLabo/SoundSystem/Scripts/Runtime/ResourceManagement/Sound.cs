using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SoundSystem {
    [System.Serializable]
    public class Sound {
        [SerializeField] CustomClip _customClip;
        public CustomClip CustomClip => _customClip;

        [SerializeReference] List<SoundBehaviour> _behaviours = new();
        public List<SoundBehaviour> Behaviours => _behaviours;

#if UNITY_EDITOR
        public static SerializedProperty GetCustomClipProp(SerializedProperty soundProp) {
            return soundProp.FindPropertyRelative(nameof(_customClip));
        }

        public static SerializedProperty GetBehavioursProp(SerializedProperty soundProp) {
            return soundProp.FindPropertyRelative(nameof(_behaviours));
        }
#endif
    }
}