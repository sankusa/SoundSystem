using System.Collections.Generic;
using UnityEngine;
using SoundSystem.SoundBehaviours;
using SoundSystem.ClipResolvers;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SoundSystem {
    [System.Serializable]
    public class Sound {
        [SerializeReference, ClipResolverPopup] IClipResolver _clip = new Clip();
        public IClipResolver Clip => _clip;

        [SerializeField] SoundBehaviourList _soundBehaviourList = new();
        public SoundBehaviourList SoundBehaviourList => _soundBehaviourList;

#if UNITY_EDITOR
        public static SerializedProperty GetClipProp(SerializedProperty soundProp) {
            return soundProp.FindPropertyRelative(nameof(_clip));
        }

        public static SerializedProperty GetSoundBehaviourListProp(SerializedProperty soundProp) {
            return soundProp.FindPropertyRelative(nameof(_soundBehaviourList));
        }
#endif
    }
}