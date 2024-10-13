using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    [CustomPropertyDrawer(typeof(ShowOnlyChildAttribute))]
    public class ShowOnlyChildDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            GUIUtil.ChildPropertyFields(position, property);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return GUIUtil.GetChildPropertyHeightTotal(property);
        }
    }
}