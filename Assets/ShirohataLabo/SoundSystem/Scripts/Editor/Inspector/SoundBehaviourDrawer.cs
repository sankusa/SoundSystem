using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    [CustomPropertyDrawer(typeof(SoundBehaviour), true)]
    public class SoundBehaviourDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            SoundBehaviour behaviour = property.GetObject() as SoundBehaviour;
            if (behaviour == null) return;

            Rect headerRect = new Rect(position) {height = EditorGUIUtility.singleLineHeight};
            position.yMin += headerRect.height + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.LabelField(headerRect, behaviour.GetType().Name, EditorStyles.boldLabel);

            SerializedProperty activeProp = property.FindPropertyRelative("_active");
            Rect activeRect = new Rect(headerRect) {xMin = headerRect.xMax - 18};
            EditorGUI.PropertyField(activeRect, activeProp, GUIContent.none);

            if (activeProp.boolValue) {
                EditorGUI.indentLevel++;

                SerializedProperty prop = activeProp.Copy();
                int depth = prop.depth;
                while(prop.NextVisible(false)) {
                    if (prop.depth != depth) break;

                    Rect rect = new Rect(position) {height = EditorGUI.GetPropertyHeight(prop)};
                    position.yMin += rect.height + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(rect, prop);
                }

                EditorGUI.indentLevel--;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            SoundBehaviour behaviour = property.GetObject() as SoundBehaviour;
            if (behaviour == null) return EditorGUIUtility.singleLineHeight;

            SerializedProperty activeProp = property.FindPropertyRelative("_active");

            float totalHeight = EditorGUIUtility.singleLineHeight;
            if (activeProp.boolValue) {
                SerializedProperty prop = activeProp.Copy();
                int depth = prop.depth;
                while(prop.NextVisible(false)) {
                    if (prop.depth != depth) break;
                    totalHeight += EditorGUI.GetPropertyHeight(prop) + EditorGUIUtility.standardVerticalSpacing;
                }
            }

            return totalHeight;
        }
    }
}