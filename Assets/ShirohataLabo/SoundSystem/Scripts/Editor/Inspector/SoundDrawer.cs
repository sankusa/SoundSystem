using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    [CustomPropertyDrawer(typeof(Sound))]
    public class SoundDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if (label != GUIContent.none) {
                Rect labelRect = new(position) {height = EditorGUIUtility.singleLineHeight};
                position.yMin += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.LabelField(labelRect, label);
                EditorGUI.indentLevel++;
            }

            SerializedProperty audioUnitProp = property.FindPropertyRelative("_audioUnit");
            Rect audioUnitRect = new(position) {height = EditorGUIUtility.singleLineHeight};
            EditorGUI.PropertyField(audioUnitRect, audioUnitProp, GUIContent.none);

            if (label != GUIContent.none) {
                EditorGUI.indentLevel--;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            float height = 0;
            if (label != GUIContent.none) {
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            height += EditorGUIUtility.singleLineHeight;
            return height;
        }
    }
}