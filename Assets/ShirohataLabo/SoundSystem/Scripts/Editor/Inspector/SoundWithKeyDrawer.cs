using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    [CustomPropertyDrawer(typeof(SoundWithKey))]
    public class SoundWithKeyDrawer : PropertyDrawer {
        static readonly RectUtil.LayoutLength[] _columnWidths = new RectUtil.LayoutLength[] {new(1), new(2)};

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            SerializedProperty labelProp = property.FindPropertyRelative("_key");
            SerializedProperty soundProp = property.FindPropertyRelative("_sound");

            position.yMin += EditorGUIUtility.standardVerticalSpacing;
            Rect[] rects = RectUtil.DivideRectHorizontal(position, _columnWidths);

            float labelHeight = EditorGUI.GetPropertyHeight(labelProp);
            Rect labelRect = new(RectUtil.Margin(rects[0], rightMargin: 3)) {height = labelHeight};
            
            float soundHeight = EditorGUI.GetPropertyHeight(soundProp);
            Rect soundRect = new Rect(rects[1]) {height = soundHeight};

            EditorGUI.PropertyField(labelRect, labelProp, GUIContent.none);
            EditorGUI.PropertyField(soundRect, soundProp, GUIContent.none);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            SerializedProperty soundProp = property.FindPropertyRelative("_sound");
            
            float height = 0;
            height += EditorGUI.GetPropertyHeight(soundProp, GUIContent.none);
            height += EditorGUIUtility.standardVerticalSpacing;
            return height;
        }
    }
}