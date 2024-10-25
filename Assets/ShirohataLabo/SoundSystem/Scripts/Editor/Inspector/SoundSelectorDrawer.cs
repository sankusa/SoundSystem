using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    [CustomPropertyDrawer(typeof(SoundSelector))]
    public class SoundSelectorDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            SerializedProperty selectTypeProp = property.FindPropertyRelative("_selectType");
            SerializedProperty soundProp = property.FindPropertyRelative("_sound");
            SerializedProperty soundKeyProp = property.FindPropertyRelative("_soundKey");

            Rect selectTypeRect = new(position) {height = EditorGUI.GetPropertyHeight(selectTypeProp, true)};
            position.yMin += selectTypeRect.height + EditorGUIUtility.standardVerticalSpacing;

            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(selectTypeRect, selectTypeProp, label);
            if (EditorGUI.EndChangeCheck()) {
                if (selectTypeProp.enumValueIndex != (int)SoundSelector.SelectType.Sound) {
                    soundProp.FindPropertyRelative("_audioUnit").objectReferenceValue = null;
                }
            }

            EditorGUI.indentLevel++;

            if (selectTypeProp.enumValueIndex == (int)SoundSelector.SelectType.Sound) {
                Rect soundRect = new(position) {height = EditorGUI.GetPropertyHeight(soundProp, GUIContent.none, true)};
                EditorGUI.PropertyField(soundRect, soundProp, GUIContent.none, true);
            }
            else if (selectTypeProp.enumValueIndex == (int)SoundSelector.SelectType.SoundKey) {
                Rect soundKeyRect = new(position) {height = EditorGUI.GetPropertyHeight(soundKeyProp, true)};
                EditorGUI.PropertyField(soundKeyRect, soundKeyProp, true);
            }

            EditorGUI.indentLevel--;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            SerializedProperty selectTypeProp = property.FindPropertyRelative("_selectType");
            SerializedProperty soundProp = property.FindPropertyRelative("_sound");
            SerializedProperty soundKeyProp = property.FindPropertyRelative("_soundKey");

            float height = 0;
            height += EditorGUI.GetPropertyHeight(selectTypeProp, true) + EditorGUIUtility.standardVerticalSpacing;
            if (selectTypeProp.enumValueIndex == (int)SoundSelector.SelectType.Sound) {
                height += EditorGUI.GetPropertyHeight(soundProp, GUIContent.none, true);
            }
            else if (selectTypeProp.enumValueIndex == (int)SoundSelector.SelectType.SoundKey) {
                height += EditorGUI.GetPropertyHeight(soundKeyProp, true);
            }
            return height;
        }
    }
}