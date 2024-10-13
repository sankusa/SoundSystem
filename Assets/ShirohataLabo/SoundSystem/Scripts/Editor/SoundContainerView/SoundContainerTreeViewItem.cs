using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace SoundSystem {
    public class SoundContainerTreeViewItem : TreeViewItem {
        public SoundContainer Container { get; }
        
        public SoundContainerTreeViewItem(int id, SoundContainer container) : base(id, 0) {
            Container = container;
        }

        public void OnSingleClick() {
            AssetDatabase.OpenAsset(Container);
        }

        public void OnDoubleClick() {
            EditorGUIUtility.PingObject(Container);
        }
    }
}