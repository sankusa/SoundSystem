using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace SoundSystem {
    public class SoundContainerTreeViewItem_SoundContainer : TreeViewItem {
        public SoundContainer Container { get; }
        public SerializedObject SerializedObject { get; }
        
        public SoundContainerTreeViewItem_SoundContainer(SoundContainer container, SerializedObject serializedObject, int id) : base(id, 0) {
            Container = container;
            SerializedObject = serializedObject;
        }

        public void OnSingleClick() {
            AssetDatabase.OpenAsset(Container);
        }

        public void OnDoubleClick() {
            EditorGUIUtility.PingObject(Container);
        }
    }
}