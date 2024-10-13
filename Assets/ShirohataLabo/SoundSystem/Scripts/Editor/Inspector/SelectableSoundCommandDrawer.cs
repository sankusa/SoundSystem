using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    [CustomPropertyDrawer(typeof(SelectableSoundCommand))]
    public class SelectableSoundCommandDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            SerializedProperty commandProp = property.FindPropertyRelative("_command");
            EditorGUI.PropertyField(position, commandProp, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            SerializedProperty commandProp = property.FindPropertyRelative("_command");
            return EditorGUI.GetPropertyHeight(commandProp, label, true);
        }
    }
}