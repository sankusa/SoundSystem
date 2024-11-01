using SoundSystem.ClipResolvers;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace SoundSystem {
    [CustomPropertyDrawer(typeof(RandomClip))]
    public class RandomClipDrawer : PropertyDrawer {
        ReorderableList _list;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            SerializedProperty clipsProp = property.FindPropertyRelative("_clips");
            PrepareList(clipsProp, label);
            _list.DoList(position);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            if (_list == null) return EditorGUIUtility.singleLineHeight;
            return _list.GetHeight();
        }

        void PrepareList(SerializedProperty clipsProp, GUIContent label) {
            if (_list == null) {
                _list = new ReorderableList(clipsProp.serializedObject, clipsProp);
                _list.drawHeaderCallback = (Rect rect) => {
                    EditorGUI.LabelField(rect, label);
                };
                _list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
                    rect.height -= EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(rect, clipsProp.GetArrayElementAtIndex(index), GUIContent.none);
                };
                _list.elementHeightCallback = (int index) => {
                    return EditorGUI.GetPropertyHeight(clipsProp.GetArrayElementAtIndex(index));
                };
            }
        }
    }
}