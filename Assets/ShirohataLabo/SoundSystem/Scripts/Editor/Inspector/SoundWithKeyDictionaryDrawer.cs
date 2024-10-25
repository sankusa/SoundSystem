using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace SoundSystem {
    [CustomPropertyDrawer(typeof(SoundWithKeyDictionary))]
    public class SoundWithKeyDictionaryDrawer : PropertyDrawer {
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
            _soundList ??= new ReorderableList(listProp.serializedObject, listProp) {
                drawHeaderCallback = (Rect rect) => {
                    EditorGUI.LabelField(rect, "Sounds");
                },
                drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
                    rect.xMin += 10;
                    rect.yMax -= EditorGUIUtility.standardVerticalSpacing;;
                    EditorGUI.PropertyField(rect, listProp.GetArrayElementAtIndex(index), true);
                },
                elementHeightCallback = (int index) => {
                    return EditorGUI.GetPropertyHeight(listProp.GetArrayElementAtIndex(index), true);
                },
            };
        }
    }
}