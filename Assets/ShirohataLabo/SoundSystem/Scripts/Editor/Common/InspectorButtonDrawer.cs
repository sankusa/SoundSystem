using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    [CustomPropertyDrawer(typeof(InspectorButtonAttribute))]
    public class InspectorButtonDrawer : PropertyDrawer {
        const float inspectorButtonWidth = 18;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if (property.propertyType != SerializedPropertyType.ObjectReference) {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            Rect propertyRect = new Rect(position) {
                xMax = position.xMax - inspectorButtonWidth - EditorGUIUtility.standardVerticalSpacing
            };
            EditorGUI.PropertyField(propertyRect, property, label);

            Rect inspectorButtonRect = new Rect(position) {
                xMin = position.xMax - inspectorButtonWidth,
            };
            if (GUI.Button(inspectorButtonRect, "")) {
                InspectorUtil.OpenAnotherInspector(property.objectReferenceValue);
            }
            GUI.DrawTexture(RectUtil.Margin(inspectorButtonRect, 1, 1, 1, 1), Icons.InspectorIcon);
        }
    }
}