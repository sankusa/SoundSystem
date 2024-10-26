using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    [CustomPropertyDrawer(typeof(SoundWithKey))]
    public class SoundWithKeyDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            SerializedProperty keyProp = SoundWithKey.GetKeyProp(property);
            SerializedProperty soundProp = SoundWithKey.GetSoundProp(property);

            Rect headerRect = new Rect(position) {height = EditorGUIUtility.singleLineHeight};
            position.yMin += headerRect.height + EditorGUIUtility.standardVerticalSpacing;

            property.isExpanded = EditorGUI.Foldout(headerRect, property.isExpanded, "");

            Rect keyRect = new Rect(headerRect) {xMin = headerRect.xMin};
            EditorGUI.PropertyField(keyRect, keyProp);

            if (property.isExpanded) {
                EditorGUI.PropertyField(position, soundProp, GUIContent.none, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            SerializedProperty soundProp = SoundWithKey.GetSoundProp(property);
            
            float height = 0;
            height += EditorGUIUtility.singleLineHeight;
            if (property.isExpanded) {
                height += EditorGUI.GetPropertyHeight(soundProp, GUIContent.none, true) + EditorGUIUtility.standardVerticalSpacing;
            }
            return height;
        }
    }
}