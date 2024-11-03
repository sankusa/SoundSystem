using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SoundSystem {
    [CustomPropertyDrawer(typeof(ClipSlot))]
    public class ClipSlotDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            Rect labelRect = new Rect(position) {width = label == GUIContent.none ? 0 : EditorGUIUtility.labelWidth};
            EditorGUI.LabelField(labelRect, label);

            SerializedProperty typeProp = ClipSlot.GetTypeProp(property);
            SerializedProperty audioClipProp = ClipSlot.GetAudioClipProp(property);
            SerializedProperty customClipProp = ClipSlot.GetCustomClipProp(property);

            using (new IndentLevelScope(0)) {
                Rect typePopupRect = new Rect(position) {
                    xMin = labelRect.xMax + EditorGUIUtility.standardVerticalSpacing,
                    width = 36,
                };
                GUIContent buttonLabel = null;

                buttonLabel = (ClipSlot.SlotType)typeProp.enumValueIndex switch {
                    ClipSlot.SlotType.AudioClip => new GUIContent(Icons.AudioClipIcon),
                    ClipSlot.SlotType.CustomClip => new GUIContent(Skin.Instance.CustomClipIcon),
                    _ => throw new ArgumentOutOfRangeException($"Invalid {typeof(ClipSlot.SlotType).Name}"),
                };
                if (GUI.Button(typePopupRect, buttonLabel, EditorStyles.popup)) {
                    GenericMenu menu = new();
                    menu.AddItem(
                        new GUIContent($"{nameof(AudioClip)}"),
                        false,
                        () => {
                            property.serializedObject.Update();
                            typeProp.enumValueIndex = (int)ClipSlot.SlotType.AudioClip;
                            customClipProp.objectReferenceValue = null;
                            property.serializedObject.ApplyModifiedProperties();
                        }
                    );
                    menu.AddItem(
                        new GUIContent($"{nameof(CustomClip)}"),
                        false,
                        () => {
                            property.serializedObject.Update();
                            typeProp.enumValueIndex = (int)ClipSlot.SlotType.CustomClip;
                            audioClipProp.objectReferenceValue = null;
                            property.serializedObject.ApplyModifiedProperties();
                        }
                    );
                    menu.ShowAsContext();
                }

                SerializedProperty clipProp = (ClipSlot.SlotType)typeProp.enumValueIndex switch {
                    ClipSlot.SlotType.AudioClip => audioClipProp,
                    ClipSlot.SlotType.CustomClip => customClipProp,
                    _ => throw new ArgumentOutOfRangeException($"Invalid {typeof(ClipSlot.SlotType).Name}"),
                };

                Rect objectRect = new Rect(position) {
                    xMin = typePopupRect.xMax + EditorGUIUtility.standardVerticalSpacing,
                };
                EditorGUI.PropertyField(objectRect, clipProp, GUIContent.none);
            }

            // ドラッグ&ドロップ
            Object droppedObject = DragAndDropUtil.AcceptObject<Object>(position);
            if (droppedObject != null) {
                if (droppedObject is AudioClip) {
                    typeProp.enumValueIndex = (int)ClipSlot.SlotType.AudioClip;
                    audioClipProp.objectReferenceValue = droppedObject;
                }
                if (droppedObject is CustomClip) {
                    typeProp.enumValueIndex = (int)ClipSlot.SlotType.CustomClip;
                    customClipProp.objectReferenceValue = droppedObject;
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}