using SoundSystem.AudioUnitEffects;
using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    [CustomPropertyDrawer(typeof(VolumeMultiplier))]
    [CustomPropertyDrawer(typeof(VolumeMultiplierCurve))]
    [CustomPropertyDrawer(typeof(PitchMultiplier))]
    public class BasicAudioUnitEffectDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            Rect headerRect = new Rect(position) {height = EditorGUIUtility.singleLineHeight};
            position.yMin += headerRect.height + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.LabelField(headerRect, label, EditorStyles.boldLabel);

            SerializedProperty enableProp = property.FindPropertyRelative("_enable");
            Rect enableRect = new Rect(headerRect) {xMin = headerRect.xMax - 18};
            EditorGUI.PropertyField(enableRect, enableProp, GUIContent.none);

            if (enableProp.boolValue) {
                EditorGUI.indentLevel++;

                SerializedProperty prop = enableProp.Copy();
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
            SerializedProperty enableProp = property.FindPropertyRelative("_enable");

            float totalHeight = EditorGUIUtility.singleLineHeight;
            if (enableProp.boolValue) {
                SerializedProperty prop = enableProp.Copy();
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