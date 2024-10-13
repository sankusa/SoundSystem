using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    [CustomPropertyDrawer(typeof(Play))]
    [CustomPropertyDrawer(typeof(PlayWithFadeIn))]
    [CustomPropertyDrawer(typeof(Stop))]
    [CustomPropertyDrawer(typeof(StopWithFadeOut))]
    [CustomPropertyDrawer(typeof(Switch))]
    [CustomPropertyDrawer(typeof(CrossFade))]
    public class SoundCommandDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            Rect labelRect = new(position) {height = EditorGUIUtility.singleLineHeight};
            position.yMin += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.LabelField(labelRect, label);

            EditorGUI.indentLevel++;

            int targetDepth = property.depth + 1;
            SerializedProperty copy = property.Copy();
            if(copy.NextVisible(true) && copy.depth == targetDepth) {
                do {
                    float fieldHeight = EditorGUI.GetPropertyHeight(copy);
                    Rect rect = new(position) {height = fieldHeight};
                    position.yMin += fieldHeight + EditorGUIUtility.standardVerticalSpacing;

                    EditorGUI.PropertyField(rect, copy, true);
                } while (copy.NextVisible(false) && copy.depth == targetDepth);
            }

            EditorGUI.indentLevel--;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            float totalHeight = 0;
            totalHeight += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            int targetDepth = property.depth + 1;
            SerializedProperty copy = property.Copy();
            if(copy.NextVisible(true) && copy.depth == targetDepth) {
                do {
                    float fieldHeight = EditorGUI.GetPropertyHeight(copy) + EditorGUIUtility.standardVerticalSpacing;
                    totalHeight += fieldHeight;
                } while (copy.NextVisible(false) && copy.depth == targetDepth);
            }
            return totalHeight;
        }
    }
}