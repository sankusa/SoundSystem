using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SoundSystem {
    public class SoundContainerTreeViewItem_Sound : TreeViewItem {
        public SerializedProperty SoundWithKeyProperty { get; }

        public SoundContainerTreeViewItem_Sound(SerializedProperty soundWithKeyProperty, int id) : base(id, 0) {
            SoundWithKeyProperty = soundWithKeyProperty;
        }

        public void RowGUI(Rect rect) {
            rect.yMin += 2;
            EditorGUI.PropertyField(rect, SoundWithKeyProperty);
        }

        public float GetRowHeight() {
            return EditorGUI.GetPropertyHeight(SoundWithKeyProperty) + 6;
        }
    }
}