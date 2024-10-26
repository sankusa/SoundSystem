using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace SoundSystem {
    [CustomPropertyDrawer(typeof(SoundWithKeyDictionary))]
    public class SoundWithKeyDictionaryDrawer : PropertyDrawer {
        ReorderableList _soundList;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            SerializedProperty listProp = SoundWithKeyDictionary.GetListProp(property);
            PrepareList(listProp);
            _soundList.DoList(position);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            SerializedProperty listProp = SoundWithKeyDictionary.GetListProp(property);
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
                onAddCallback = (ReorderableList list) => {
                    listProp.InsertArrayElementAtIndex(listProp.arraySize);
                    SerializedProperty newElementProp = listProp.GetArrayElementAtIndex(listProp.arraySize - 1);
                    SoundWithKey.GetKeyProp(newElementProp).stringValue = "";
                    SerializedProperty soundProp = SoundWithKey.GetSoundProp(newElementProp);
                    Sound.GetCustomClipProp(soundProp).objectReferenceValue = null;
                    Sound.GetBehavioursProp(soundProp).ClearArray();
                }
            };
        }
    }
}