using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace SoundSystem {
    [CustomPropertyDrawer(typeof(SoundBehaviourList))]
    public class SoundBehavioiurListDrawer : PropertyDrawer {

        Dictionary<string, ReorderableList> _soundBehaviourDic = new();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            SerializedProperty behavioursProp = SoundBehaviourList.GetBehavioursProp(property);
            ReorderableList soundBehaviourList = PrepareReorderableList(property, behavioursProp);
            float listHeight = soundBehaviourList.GetHeight();
            Rect listRect = new Rect(position) {
                width = position.width,
                height = listHeight,
                xMin = position.xMin + EditorGUI.indentLevel * 14
            };
            soundBehaviourList.DoList(listRect);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            SerializedProperty behavioursProp = SoundBehaviourList.GetBehavioursProp(property);
            ReorderableList soundBehaviourList = PrepareReorderableList(property, behavioursProp);
            return soundBehaviourList.GetHeight() - 18;
        }

        ReorderableList PrepareReorderableList(SerializedProperty property, SerializedProperty behavioursProp) {
            if (_soundBehaviourDic.ContainsKey(property.propertyPath)) return _soundBehaviourDic[property.propertyPath];
            ReorderableList soundBehaviourList = new ReorderableList(property.serializedObject, behavioursProp, true, true, false, false) {
                drawHeaderCallback = (Rect rect) => {
                    DrawBehavioursHeader(rect, property, behavioursProp);
                },
                drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
                    // 要素の削除が反映される前に描画処理が呼ばれるため
                    if (index >= behavioursProp.arraySize) return;

                    Rect fieldRect = new Rect(rect) {width = rect.width - 22};
                    EditorGUI.PropertyField(fieldRect, behavioursProp.GetArrayElementAtIndex(index), true);

                    Rect removeButtonRect = new Rect(rect) {xMin = rect.xMax - 20};
                    if (GUI.Button(removeButtonRect, new GUIContent(Icons.MinusIcon))) {
                        behavioursProp.DeleteArrayElementAtIndex(index);
                        property.serializedObject.ApplyModifiedProperties();
                    }
                },
                elementHeightCallback = (int index) => {
                    return EditorGUI.GetPropertyHeight(behavioursProp.GetArrayElementAtIndex(index));
                },
            };
            _soundBehaviourDic[property.propertyPath] = soundBehaviourList; 
            return soundBehaviourList;
        }

        void DrawBehavioursHeader(Rect rect, SerializedProperty soundProp, SerializedProperty behavioursProp) {
            using (new IndentLevelScope(0)) {
                EditorGUI.LabelField(rect, "Sound Behaviours");
            }
                
            Rect itemButtonRect = new Rect(rect) {xMin = rect.xMax - 20};
            if (GUI.Button(itemButtonRect, Icons.PlusIcon)) {
                GenericMenu menu = new();
                int previousPriority = int.MinValue;
                IEnumerable<Type> sortedSoundBehaviourTypes = TypeCache
                    .GetTypesDerivedFrom<SoundBehaviour>()
                    .OrderBy(type => {
                        SoundBehaviourMenuItemAttribute menuItemAttribute = type
                            .GetCustomAttributes(typeof(SoundBehaviourMenuItemAttribute), true)
                            .FirstOrDefault()
                            as SoundBehaviourMenuItemAttribute;
                        if (menuItemAttribute == null) return int.MaxValue;
                        return menuItemAttribute.Priority;

                    });

                foreach (Type type in sortedSoundBehaviourTypes) {
                    SoundBehaviourMenuItemAttribute menuItemAttribute = type
                        .GetCustomAttributes(typeof(SoundBehaviourMenuItemAttribute), true)
                        .FirstOrDefault()
                        as SoundBehaviourMenuItemAttribute;

                    if (menuItemAttribute == null) continue;

                    if (previousPriority != int.MinValue && menuItemAttribute.Priority - previousPriority > 100) menu.AddSeparator("");
                    previousPriority = menuItemAttribute.Priority;

                    string typeFullName = type.Assembly.GetName().Name + " " + type.FullName;
                    bool exists = Enumerable
                        .Range(0, behavioursProp.arraySize)
                        .Any(i => behavioursProp.GetArrayElementAtIndex(i).managedReferenceFullTypename == typeFullName);

                    if (exists) continue;

                    menu.AddItem(
                        new GUIContent(menuItemAttribute.MenuName),
                        false,
                        () => {
                            soundProp.serializedObject.Update();

                            behavioursProp.InsertArrayElementAtIndex(behavioursProp.arraySize);
                            SerializedProperty behaviourProp = behavioursProp.GetArrayElementAtIndex(behavioursProp.arraySize - 1);
                            behaviourProp.managedReferenceValue = Activator.CreateInstance(type);

                            soundProp.serializedObject.ApplyModifiedProperties();
                        }
                    );
                }
                menu.ShowAsContext();
            }
        }
    }
}