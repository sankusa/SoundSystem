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
        [SerializeReference, SerializeReferencePopup] IClipResolver _clip = new Clip();
        public IClipResolver Clip => _clip;

        [SerializeReference] List<SoundBehaviour> _behaviours = new();
        public List<SoundBehaviour> Behaviours => _behaviours;

#if UNITY_EDITOR
        public static SerializedProperty GetClipProp(SerializedProperty soundProp) {
            return soundProp.FindPropertyRelative(nameof(_clip));
        }

        public static SerializedProperty GetBehavioursProp(SerializedProperty soundProp) {
            return soundProp.FindPropertyRelative(nameof(_behaviours));
        }
#endif
    }
}