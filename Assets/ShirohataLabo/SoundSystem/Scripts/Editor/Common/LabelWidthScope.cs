using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    public class LabelWidthScope : GUI.Scope {
        float _originalLabelWidth;

        public LabelWidthScope(float labelWidth) {
            _originalLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = labelWidth;
        }

        protected override void CloseScope() {
            EditorGUIUtility.labelWidth = _originalLabelWidth;
        }
    }
}