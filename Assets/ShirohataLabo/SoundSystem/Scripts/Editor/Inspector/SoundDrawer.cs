using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace SoundSystem {
    [CustomPropertyDrawer(typeof(Sound))]
    public class SoundDrawer : PropertyDrawer {
        Dictionary<string, EditorSoundPlayerGUIForPropertyDrawer> _soundPlayerGUIDic = new();
        Dictionary<string, ReorderableList> _soundBehaviourDic = new();

        static float _editorPlayerHeight = EditorGUIUtility.singleLineHeight + 8;

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

            EditorSoundPlayerGUIForPropertyDrawer playerGUI = PrepareEditorSoundPlayerGUI(property);
            Rect playerRect = new Rect(position) {height = _editorPlayerHeight, xMin = position.xMin + EditorGUI.indentLevel * 14};
            position.yMin += playerRect.height + EditorGUIUtility.standardVerticalSpacing;
            playerGUI.DrawGUI(playerRect);

            using (var check = new EditorGUI.ChangeCheckScope()) {
                SerializedProperty customClipProp = Sound.GetCustomClipProp(property);
                Rect customClipRect = new(position) {height = EditorGUIUtility.singleLineHeight};
                position.yMin += customClipRect.height + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(customClipRect, customClipProp);

                SerializedProperty behavioursProp = Sound.GetBehavioursProp(property);
                ReorderableList soundBehaviourList = PrepareReorderableList(property, behavioursProp);
                float listHeight = soundBehaviourList.GetHeight();
                soundBehaviourList.DoList(new Rect(position) {width = position.width, height = listHeight, xMin = position.xMin + EditorGUI.indentLevel * 14});

                if (check.changed) {
                    property.serializedObject.ApplyModifiedProperties();
                    playerGUI.ReapplyParameters();
                }
            }

            if (label != GUIContent.none) {
                EditorGUI.indentLevel--;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            float height = 0;
            height += EditorGUIUtility.standardVerticalSpacing * 2;
            height += _editorPlayerHeight;
            if (label != GUIContent.none) {
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            height += EditorGUIUtility.singleLineHeight;

            SerializedProperty behavioursProp = Sound.GetBehavioursProp(property);
            ReorderableList soundBehaviourList = PrepareReorderableList(property, behavioursProp);
            height += soundBehaviourList.GetHeight();
            return height;
        }

        EditorSoundPlayerGUIForPropertyDrawer PrepareEditorSoundPlayerGUI(SerializedProperty property) {
            if (_soundPlayerGUIDic.ContainsKey(property.propertyPath)) return _soundPlayerGUIDic[property.propertyPath];
            EditorSoundPlayerGUIForPropertyDrawer soundPlayerGUI = new();
            soundPlayerGUI.Bind(property.GetObject() as Sound);
            _soundPlayerGUIDic[property.propertyPath] = soundPlayerGUI;
            return soundPlayerGUI;
        }

        ReorderableList PrepareReorderableList(SerializedProperty property, SerializedProperty behavioursProp) {
            if (_soundBehaviourDic.ContainsKey(property.propertyPath)) return _soundBehaviourDic[property.propertyPath];
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
                                    _soundPlayerGUIDic[property.propertyPath].ReapplyParameters(); 
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
            _soundBehaviourDic[property.propertyPath] = soundBehaviourList; 
            return soundBehaviourList;
        }
    }
}