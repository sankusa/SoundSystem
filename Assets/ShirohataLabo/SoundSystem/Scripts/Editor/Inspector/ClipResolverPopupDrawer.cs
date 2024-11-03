using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    [CustomPropertyDrawer(typeof(ClipResolverPopupAttribute))]
    public class ClipResolverDrawer : PropertyDrawer {
        const float indentWidthPerLevel = 14;
        const float popupWidth = 20;
        const float popupHeight = 20;
        const float leftSpace = 4;

        List<Type> _subclasses;
        string[] _subclassFullNames;
        string[] _subclassDisplayNames;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            InitializeIfNeeded(property);

            position.xMin += EditorGUI.indentLevel * indentWidthPerLevel + leftSpace;

            using (new LabelWidthScope(EditorGUIUtility.labelWidth - EditorGUI.indentLevel * indentWidthPerLevel - popupWidth - leftSpace)) {
            using (new IndentLevelScope(0)) {
                Rect popupRect = new(position) {height = popupHeight, width = popupWidth};
                position.xMin += popupRect.width + EditorGUIUtility.standardVerticalSpacing;

                int currentIndex = Array.IndexOf(_subclassFullNames, property.managedReferenceFullTypename);
                int selectedIndex = EditorGUI.Popup(popupRect, currentIndex, _subclassDisplayNames);
                Type selectedType = selectedIndex == -1 ? null : _subclasses[selectedIndex];
                if (selectedIndex != currentIndex && selectedIndex != -1) {
                    property.managedReferenceValue = selectedType == null ? null : Activator.CreateInstance(selectedType);
                    return;
                }

                EditorGUI.PropertyField(position, property, selectedType == null ? GUIContent.none : new GUIContent(selectedType.Name), true);
            }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        void InitializeIfNeeded(SerializedProperty property) {
            if (_subclasses != null) return;

            string[] fieldTypeName = property.managedReferenceFieldTypename.Split(' ');
            Type fieldType = Assembly.Load(fieldTypeName[0]).GetType(fieldTypeName[1]);

            _subclasses = new List<Type>() {};
            _subclasses.AddRange(TypeCache.GetTypesDerivedFrom(fieldType));

            _subclassFullNames = _subclasses
                .Select(type => $"{type.Assembly.FullName.Split(',')[0]} {type.FullName}")
                .ToArray();
            _subclassDisplayNames = _subclasses
                .Select(type => {
                    return type.Name;
                })
                .ToArray();
        }
    }
}