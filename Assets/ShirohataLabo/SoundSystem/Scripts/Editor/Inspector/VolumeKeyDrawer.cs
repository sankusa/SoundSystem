using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    [CustomPropertyDrawer(typeof(VolumeKeyAttribute))]
    public class VolumeKeyDrawer : PropertyDrawer {
        PopupFromScriptableObject<SoundManagerConfig> _keyPopup = new(x => x.SelectMany(y => y.VolumeSettings.Select(z => z.Key)));

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if (property.propertyType != SerializedPropertyType.String) {
                base.OnGUI(position, property, label);
                return;
            }

            property.stringValue = _keyPopup.Draw(position, label.text, property.stringValue);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return base.GetPropertyHeight(property, label);
        }
    }
}