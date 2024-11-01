using SoundSystem.ClipResolvers;
using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    [CustomPropertyDrawer(typeof(Clip))]
    public class ClipDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            SerializedProperty clipProp = property.FindPropertyRelative("_clip");
            EditorGUI.PropertyField(position, clipProp, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            SerializedProperty clipProp = property.FindPropertyRelative("_clip");
            return EditorGUI.GetPropertyHeight(clipProp, label, true);
        }
    }
}