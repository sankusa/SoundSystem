using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace SoundSystem {
    public class AudioUnitTreeViewItem_AudioUnitCategory : TreeViewItem {
        public AudioUnitCategory Category { get; }

        public string Label => Category.name;

        public AudioUnitTreeViewItem_AudioUnitCategory(AudioUnitCategory category) : base(0) {
            Category = category;
            displayName = category.name;
            id = category.GetInstanceID();
        }

        public void OnSingleClick() {
            AssetDatabase.OpenAsset(Category);
        }

        public void OnDoubleClick() {
            EditorGUIUtility.PingObject(Category);
        }
    }
}