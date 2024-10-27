using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SoundSystem {
    [Serializable]
    public class ClipSlot {
        public enum SlotType {
            AudioClip = 0,
            CustomClip = 1,
        }

        [SerializeField] SlotType _type;
        public SlotType Type => _type;

        [SerializeField] AudioClip _audioClip;
        public AudioClip AudioClip => _audioClip;

        [SerializeField] CustomClip _customClip; 
        public CustomClip CustomClip => _customClip;

        public bool HasClip() {
            return _type switch {
                ClipSlot.SlotType.AudioClip => _audioClip != null,
                ClipSlot.SlotType.CustomClip => _customClip != null,
                _ => throw new ArgumentOutOfRangeException($"Invalid {typeof(SlotType).Name}"),
            };
        }

#if UNITY_EDITOR
        public static SerializedProperty GetTypeProp(SerializedProperty clipSlotProp) {
            return clipSlotProp.FindPropertyRelative(nameof(_type));
        }

        public static SerializedProperty GetAudioClipProp(SerializedProperty clipSlotProp) {
            return clipSlotProp.FindPropertyRelative(nameof(_audioClip));
        }

        public static SerializedProperty GetCustomClipProp(SerializedProperty clipSlotProp) {
            return clipSlotProp.FindPropertyRelative(nameof(_customClip));
        }

        public static void ClearObjectReferences(SerializedProperty clipSlotProp) {
            GetAudioClipProp(clipSlotProp).objectReferenceValue = null;
            GetCustomClipProp(clipSlotProp).objectReferenceValue = null;
        }
#endif
    }
}