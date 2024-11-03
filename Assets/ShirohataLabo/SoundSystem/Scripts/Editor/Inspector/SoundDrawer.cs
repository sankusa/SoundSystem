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

        static float _editorPlayerHeight = EditorGUIUtility.singleLineHeight + 8;
        static readonly float _sideSpace = 0;
        static readonly float _topSpace = 4;
        static readonly float _spaceAfterPlayer = 4;
        static readonly float _bottomSpace = 16;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            position.yMin += _topSpace;

            if (label != GUIContent.none) {
                Rect labelRect = new(position) {height = EditorGUIUtility.singleLineHeight};
                position.yMin += labelRect.height + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.LabelField(labelRect, label);
                EditorGUI.indentLevel++;
            }

            position.yMin += EditorGUIUtility.standardVerticalSpacing;

            Rect boxRect = new Rect(position) {
                xMin = position.xMin + EditorGUI.indentLevel * 14 - _sideSpace,
                xMax = position.xMax + _sideSpace,
                yMax = position.yMax - _bottomSpace,
            };
            GUI.Box(boxRect, "", GUIStyles.SimpleBox);

            EditorSoundPlayerGUIForPropertyDrawer playerGUI = PrepareEditorSoundPlayerGUI(property);
            Rect playerRect = new Rect(position) {
                y = position.y - 2,
                height = _editorPlayerHeight,
                xMin = position.xMin + EditorGUI.indentLevel * 14 - _sideSpace,
                xMax = position.xMax + _sideSpace
            };
            position.yMin += playerRect.height + _spaceAfterPlayer; 
            playerGUI.DrawGUI(playerRect);

            position.yMin += EditorGUIUtility.standardVerticalSpacing; 

            SerializedProperty clipProp = Sound.GetClipProp(property);
            Rect clipRect = new(position) {height = EditorGUI.GetPropertyHeight(clipProp), xMax = position.xMax - 4};
            position.yMin += clipRect.height + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(clipRect, clipProp, true);

            SerializedProperty soundBehaviourListProp = Sound.GetSoundBehaviourListProp(property);
            Rect behaviourListRect = new(position) {height = EditorGUI.GetPropertyHeight(soundBehaviourListProp)};
            position.yMin += behaviourListRect.height + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(behaviourListRect, soundBehaviourListProp, true);

            if (label != GUIContent.none) {
                EditorGUI.indentLevel--;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            float height = 0;
            height += _topSpace;
            height += _editorPlayerHeight + _spaceAfterPlayer;
            if (label != GUIContent.none) {
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }

            height += EditorGUI.GetPropertyHeight(Sound.GetClipProp(property));
            height += EditorGUI.GetPropertyHeight(Sound.GetSoundBehaviourListProp(property));
            height += _bottomSpace;
            return height;
        }

        EditorSoundPlayerGUIForPropertyDrawer PrepareEditorSoundPlayerGUI(SerializedProperty property) {
            if (_soundPlayerGUIDic.ContainsKey(property.propertyPath)) return _soundPlayerGUIDic[property.propertyPath];
            EditorSoundPlayerGUIForPropertyDrawer soundPlayerGUI = new();
            soundPlayerGUI.Bind(property.GetObject() as Sound);
            _soundPlayerGUIDic[property.propertyPath] = soundPlayerGUI;
            return soundPlayerGUI;
        }
    }
}