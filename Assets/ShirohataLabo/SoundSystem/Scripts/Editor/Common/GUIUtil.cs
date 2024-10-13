using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    public static class GUIUtil {
        public static void Separator(bool layoutIsVertical = true) {
            if (layoutIsVertical) {
                GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
            }
            else {
                GUILayout.Box("", GUILayout.Width(2), GUILayout.ExpandHeight(true));
            }
        }

        public static Rect Margin(Rect rect, float margin) {
            rect.xMin += margin;
            rect.xMax -= margin;
            rect.yMin += margin;
            rect.yMax -= margin;
            return rect;
        }

        public static void ChildPropertyFields(Rect position, SerializedProperty property) {
            int targetDepth = property.depth + 1;
            SerializedProperty copy = property.Copy();
            if(copy.NextVisible(true) && copy.depth == targetDepth) {
                do {
                    float fieldHeight = EditorGUI.GetPropertyHeight(copy);
                    Rect rect = new(position) {height = fieldHeight};
                    position.yMin += fieldHeight + EditorGUIUtility.standardVerticalSpacing;

                    EditorGUI.PropertyField(rect, copy, true);
                } while (copy.NextVisible(false) && copy.depth == targetDepth);
            }
        }
        public static float GetChildPropertyHeightTotal(SerializedProperty property) {
            float totalHeight = 0;
            int targetDepth = property.depth + 1;
            SerializedProperty copy = property.Copy();
            if(copy.NextVisible(true) && copy.depth == targetDepth) {
                do {
                    float fieldHeight = EditorGUI.GetPropertyHeight(copy) + EditorGUIUtility.standardVerticalSpacing;
                    totalHeight += fieldHeight;
                } while (copy.NextVisible(false) && copy.depth == targetDepth);
            }
            return totalHeight;
        }

        public static string TextPopup(Rect rect, string text, string[] options) {
            return TextPopup(rect, null, text, options);
        }
        public static string TextPopup(Rect rect, string label, string text, string[] options) {
            string[] displayedOptions = options;
            int index = Array.IndexOf(displayedOptions, text);
            if (label == null) {
                index = EditorGUI.Popup(rect, index, displayedOptions);
            }
            else {
                index = EditorGUI.Popup(rect, label, index, displayedOptions);
            }
            if (index == -1) {
                Rect originalTextLabelRect = new(rect);
                if (label != null) originalTextLabelRect.xMin += EditorGUIUtility.labelWidth + EditorGUIUtility.standardVerticalSpacing + 3;
                using (new IndentLevelScope(0)) {
                    EditorGUI.LabelField(originalTextLabelRect, text);
                }
                EditorGUI.DrawRect(rect, new Color(1, 0, 0, 0.2f));
                return text;
            }
            else {
                return displayedOptions[index];
            }
        }

        public static string LayoutTextPopup(string text, string[] options) {
            return LayoutTextPopup(null, text, options);
        }
        public static string LayoutTextPopup(string label, string text, string[] options) {
            Rect rect = GUILayoutUtility.GetRect(
                0,
                EditorGUIUtility.singleLineHeight,
                EditorStyles.popup,
                GUILayout.ExpandWidth(true)
            );
            return TextPopup(rect, label, text, options);
        }
    }
}