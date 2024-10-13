using System;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace SoundSystem {
    [CustomPropertyDrawer(typeof(SoundPlayerGroupKeyAttribute))]
    public class SoundPlayerGroupKeyDrawer : PropertyDrawer {
        PopupFromScriptableObject<SoundManagerConfig> _keyPopup = new(x => x.SelectMany(y => y.PlayerGroupSettings.Select(z => z.Key)));

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if (property.propertyType != SerializedPropertyType.String) {
                base.OnGUI(position, property, label);
                return;
            }

            property.stringValue = _keyPopup.Draw(position, label.ToString(), property.stringValue);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}