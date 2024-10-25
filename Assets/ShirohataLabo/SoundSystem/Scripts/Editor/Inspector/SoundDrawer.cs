using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace SoundSystem {
    [CustomPropertyDrawer(typeof(Sound))]
    public class SoundDrawer : PropertyDrawer {
        Dictionary<string, ReorderableList> _soundBehaviourLists = new();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if (label != GUIContent.none) {
                Rect labelRect = new(position) {height = EditorGUIUtility.singleLineHeight};
                position.yMin += labelRect.height + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.LabelField(labelRect, label);
                EditorGUI.indentLevel++;
            }

            Rect boxRect = new Rect(position) {xMin = position.xMin + EditorGUI.indentLevel * 14 - 2, xMax = position.xMax + 2};
            GUI.Box(boxRect, "", GUIStyles.SimpleBox);

            position.yMin += EditorGUIUtility.standardVerticalSpacing;

            SerializedProperty audioUnitProp = property.FindPropertyRelative("_audioUnit");
            Rect audioUnitRect = new(position) {height = EditorGUIUtility.singleLineHeight};
            position.yMin += audioUnitRect.height + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(audioUnitRect, audioUnitProp);

            SerializedProperty behavioursProp = property.FindPropertyRelative("_behavioiurs");
            ReorderableList soundBehaviourList = PrepareReorderableList(property, behavioursProp);
            float listHeight = soundBehaviourList.GetHeight();
            soundBehaviourList.DoList(new Rect(position) {width = position.width, height = listHeight, xMin = position.xMin + EditorGUI.indentLevel * 14});

            if (label != GUIContent.none) {
                EditorGUI.indentLevel--;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            float height = 0;
            height += EditorGUIUtility.standardVerticalSpacing * 2;
            if (label != GUIContent.none) {
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            height += EditorGUIUtility.singleLineHeight;

            SerializedProperty behavioursProp = property.FindPropertyRelative("_behavioiurs");
            ReorderableList soundBehaviourList = PrepareReorderableList(property, behavioursProp);
            height += soundBehaviourList.GetHeight();
            return height;
        }

        ReorderableList PrepareReorderableList(SerializedProperty property, SerializedProperty behavioursProp) {
            if (_soundBehaviourLists.ContainsKey(property.propertyPath)) return _soundBehaviourLists[property.propertyPath];
            ReorderableList soundBehaviourList = new ReorderableList(property.serializedObject, behavioursProp, true, true, false, false) {
                drawHeaderCallback = (Rect rect) => {
                    EditorGUI.LabelField(rect, "Sound Behaviours");
                    Rect itemButtonRect = new Rect(rect) {xMin = rect.xMax - 80};
                    if (GUI.Button(itemButtonRect, "Behaviours")) {
                        GenericMenu menu = new GenericMenu();
                        foreach (Type type in TypeCache.GetTypesDerivedFrom<SoundBehaviour>()) {
                            bool exists = false;
                            int i = 0;
                            string typeFullName = type.Assembly.GetName().Name + " " + type.FullName;
                            for (; i < behavioursProp.arraySize; i++) {
                                if (behavioursProp.GetArrayElementAtIndex(i).managedReferenceFullTypename == typeFullName) {
                                    exists = true;
                                    break;
                                }
                            }
                            menu.AddItem(
                                new GUIContent(type.Name),
                                exists,
                                () => {
                                    property.serializedObject.Update();
                                    if (exists) {
                                        behavioursProp.DeleteArrayElementAtIndex(i);
                                    }
                                    else {
                                        behavioursProp.InsertArrayElementAtIndex(behavioursProp.arraySize);
                                        SerializedProperty behaviourProp = behavioursProp.GetArrayElementAtIndex(behavioursProp.arraySize - 1);
                                        behaviourProp.managedReferenceValue = Activator.CreateInstance(type);
                                    }
                                    property.serializedObject.ApplyModifiedProperties();
                                }
                            );
                        }
                        menu.ShowAsContext();
                    }
                },
                drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
                    EditorGUI.PropertyField(rect, behavioursProp.GetArrayElementAtIndex(index), true);
                },
                elementHeightCallback = (int index) => {
                    return EditorGUI.GetPropertyHeight(behavioursProp.GetArrayElementAtIndex(index));
                },
                onAddDropdownCallback = (Rect buttonRect, ReorderableList list) => {

                }
            };
            _soundBehaviourLists[property.propertyPath] = soundBehaviourList; 
            return soundBehaviourList;
        }
    }
}