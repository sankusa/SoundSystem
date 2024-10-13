using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace SoundSystem {
    public class AudioUnitTreeViewItem_AudioUnitCategory : TreeViewItem {
        public AudioUnitCategory Category { get; }

        public string Label => Category.name;

        public AudioUnitTreeViewItem_AudioUnitCategory(int id, AudioUnitCategory category) : base(id) {
            Category = category;
            displayName = category.name;
        }

        public void OnSingleClick() {
            AssetDatabase.OpenAsset(Category);
        }

        public void OnDoubleClick() {
            EditorGUIUtility.PingObject(Category);
        }
    }
}