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

        [SerializeField, InspectorButton] AudioClip _audioClip;
        public AudioClip AudioClip => _audioClip;

        [SerializeField, InspectorButton] CustomClip _customClip; 
        public CustomClip CustomClip => _customClip;

        public void SetClip(IAnyClipSettable obj) {
            switch (Type) {
                case SlotType.AudioClip:
                    obj.SetAudioClip(AudioClip);
                    break;
                case SlotType.CustomClip:
                    obj.SetCustomClip(CustomClip);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Invalid {typeof(SlotType).Name}");
            }
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