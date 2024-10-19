using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace SoundSystem {
    [CustomPropertyDrawer(typeof(SoundWithKeyDictionary))]
    public class SoundWithKeyDictionaryDrawer : PropertyDrawer {
        static readonly RectUtil.LayoutLength[] _columnWidths = new RectUtil.LayoutLength[] {new(1), new(1)};

        ReorderableList _soundList;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            SerializedProperty listProp = property.FindPropertyRelative("_list");
            PrepareList(listProp);
            _soundList.DoList(position);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            SerializedProperty listProp = property.FindPropertyRelative("_list");
            PrepareList(listProp);
            return _soundList.GetHeight();
        }

        void PrepareList(SerializedProperty listProp) {
            if (_soundList == null) {
                _soundList = new ReorderableList(listProp.serializedObject, listProp) {
                    drawHeaderCallback = (Rect rect) => {
                        rect.xMin += 14;
                        Rect[] rects = RectUtil.DivideRectHorizontal(rect, _columnWidths);
                        EditorGUI.LabelField(rects[0], "Key");
                        EditorGUI.LabelField(rects[1], nameof(Sound));
                    },
                    drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
                        EditorGUI.PropertyField(rect, listProp.GetArrayElementAtIndex(index), true);
                    },
                    elementHeightCallback = (int index) => {
                        return EditorGUI.GetPropertyHeight(listProp.GetArrayElementAtIndex(index), true);
                    },
                };
            }
        }
    }
}