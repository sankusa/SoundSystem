using SoundSystem.SoundCommands;
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
            position.yMin += labelRect.height + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.LabelField(labelRect, label);

            EditorGUI.indentLevel++;

            GUIUtil.ChildPropertyFields(position, property);

            if (EditorApplication.isPlaying) {
                Rect rect = new(position) {height = EditorGUIUtility.singleLineHeight};
                if (GUI.Button(rect, "Invoke")) {
                    (property.GetObject() as ISoundCommand).Execute();
                }
            }

            EditorGUI.indentLevel--;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            float totalHeight = 0;
            totalHeight += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            totalHeight += GUIUtil.GetChildPropertyHeightTotal(property);

            if (EditorApplication.isPlaying) {
                totalHeight += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            return totalHeight;
        }
    }
}