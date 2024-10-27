using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace SoundSystem {
    [CustomPropertyDrawer(typeof(Sound))]
    public class SoundDrawer : PropertyDrawer {
        Dictionary<string, EditorSoundPlayerGUIForPropertyDrawer> _soundPlayerGUIDic = new();
        Dictionary<string, ReorderableList> _soundBehaviourDic = new();

        static float _editorPlayerHeight = EditorGUIUtility.singleLineHeight + 8;
        static readonly float _sideSpace = 4;
        static readonly float _afterSpace = 8;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if (label != GUIContent.none) {
                Rect labelRect = new(position) {height = EditorGUIUtility.singleLineHeight};
                position.yMin += labelRect.height + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.LabelField(labelRect, label);
                EditorGUI.indentLevel++;
            }

            Rect boxRect = new Rect(position) {
                xMin = position.xMin + EditorGUI.indentLevel * 14 - _sideSpace,
                xMax = position.xMax + _sideSpace,
                yMax = position.yMax - _afterSpace,
            };
            GUI.Box(boxRect, "", GUIStyles.SimpleBox);

            position.yMin += EditorGUIUtility.standardVerticalSpacing;

            EditorSoundPlayerGUIForPropertyDrawer playerGUI = PrepareEditorSoundPlayerGUI(property);
            Rect playerRect = new Rect(position) {
                y = position.y - 2,
                height = _editorPlayerHeight,
                xMin = position.xMin + EditorGUI.indentLevel * 14 - _sideSpace,
                xMax = position.xMax + _sideSpace
            };
            position.yMin += playerRect.height + EditorGUIUtility.standardVerticalSpacing;
            playerGUI.DrawGUI(playerRect);

            using (var check = new EditorGUI.ChangeCheckScope()) {
                SerializedProperty clipProp = Sound.GetClipProp(property);
                Rect clipRect = new(position) {height = EditorGUIUtility.singleLineHeight};
                position.yMin += clipRect.height + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(clipRect, clipProp);

                SerializedProperty behavioursProp = Sound.GetBehavioursProp(property);
                Rect behavioursHeaderRect = new Rect(position) {height = EditorGUIUtility.singleLineHeight};
                position.yMin += behavioursHeaderRect.height + EditorGUIUtility.standardVerticalSpacing;
                
                EditorGUI.LabelField(behavioursHeaderRect, "Sound Behaviours");
                
                Rect itemButtonRect = new Rect(behavioursHeaderRect) {xMin = behavioursHeaderRect.xMin + EditorGUIUtility.labelWidth + EditorGUIUtility.standardVerticalSpacing, width = 20};
                if (GUI.Button(itemButtonRect, Icons.PlusIcon)) {
                    GenericMenu menu = new GenericMenu();
                    int previousPriority = int.MinValue;
                    foreach (Type type in TypeCache
                        .GetTypesDerivedFrom<SoundBehaviour>()
                        .OrderBy(x => {
                            SoundBehaviourMenuItemAttribute menuItemAttribute = x.GetCustomAttributes(typeof(SoundBehaviourMenuItemAttribute), true).FirstOrDefault() as SoundBehaviourMenuItemAttribute;
                            if (menuItemAttribute == null) return int.MaxValue;
                            return menuItemAttribute.Priority;

                        })
                    ) {
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
                                property.serializedObject.Update();

                                behavioursProp.InsertArrayElementAtIndex(behavioursProp.arraySize);
                                SerializedProperty behaviourProp = behavioursProp.GetArrayElementAtIndex(behavioursProp.arraySize - 1);
                                behaviourProp.managedReferenceValue = Activator.CreateInstance(type);

                                property.serializedObject.ApplyModifiedProperties();
                                _soundPlayerGUIDic[property.propertyPath].ReapplyParameters(); 
                            }
                        );
                    }
                    menu.ShowAsContext();
                }

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
            height += _editorPlayerHeight + EditorGUIUtility.standardVerticalSpacing;
            if (label != GUIContent.none) {
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            height += (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 2;

            SerializedProperty behavioursProp = Sound.GetBehavioursProp(property);
            ReorderableList soundBehaviourList = PrepareReorderableList(property, behavioursProp);
            height += soundBehaviourList.GetHeight() - 14;
            height += _afterSpace;
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
            ReorderableList soundBehaviourList = new ReorderableList(property.serializedObject, behavioursProp, true, false, false, false) {
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
    }
}