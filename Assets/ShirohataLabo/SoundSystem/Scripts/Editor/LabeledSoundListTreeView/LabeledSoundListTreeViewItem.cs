using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace SoundSystem {
    public class LabeledSoundListTreeViewItem : TreeViewItem {
        public SerializedProperty LabeledSoundProp { get; }

        public LabeledSoundListTreeViewItem(int id, SerializedProperty labeledSoundProp) : base(id, 0) {
            LabeledSoundProp = labeledSoundProp;
            displayName = labeledSoundProp.FindPropertyRelative("_label").stringValue;
        }
    }
}