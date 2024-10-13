using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Linq;

namespace SoundSystem {
    [CustomPropertyDrawer(typeof(SerializeReferencePopupAttribute))]
    public class SerializeReferencePopupDrawer : PropertyDrawer {
        Type[] _subclasses;
        string[] _subclassFullNames;
        string[] _subclassDisplayNames;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            InitializeIfNeed(property);

            Rect popupRect = new(position);
            popupRect.height = EditorGUIUtility.singleLineHeight;
            popupRect.xMin += EditorGUIUtility.labelWidth + EditorGUIUtility.standardVerticalSpacing;

            int currentIndex = Array.IndexOf(_subclassFullNames, property.managedReferenceFullTypename);
            int selectedIndex = EditorGUI.Popup(popupRect, currentIndex, _subclassDisplayNames);
            if (selectedIndex != currentIndex) {
                Type selectedType = _subclasses[selectedIndex];
                property.managedReferenceValue = selectedType == null ? null : Activator.CreateInstance(selectedType);
            }
            
            EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        void InitializeIfNeed(SerializedProperty property) {
            if (_subclasses != null) return;

            string[] fieldTypeName = property.managedReferenceFieldTypename.Split(' ');
            Assembly assembly = Assembly.Load(fieldTypeName[0]);
            Type baseType = assembly.GetType(fieldTypeName[1]);
            _subclasses = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => baseType.IsAssignableFrom(type) && type.IsClass && type.IsAbstract == false)
                .Prepend(null)
                .ToArray();
            _subclassFullNames = _subclasses
                .Select(type => type == null ? "" : $"{type.Assembly.FullName.Split(',')[0]} {type.FullName}")
                .ToArray();
            _subclassDisplayNames = _subclasses
                .Select(type => {
                    if(type == null) return "<null>";
                    return type.Name;
                })
                .ToArray();
        }
    }
}