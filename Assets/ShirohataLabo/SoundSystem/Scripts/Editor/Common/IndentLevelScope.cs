using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    public class IndentLevelScope : GUI.Scope {
        int _originalIndentLevel;

        public IndentLevelScope(int indentLevel) {
            _originalIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = indentLevel;
        }

        protected override void CloseScope() {
            EditorGUI.indentLevel = _originalIndentLevel;
        }
    }
}