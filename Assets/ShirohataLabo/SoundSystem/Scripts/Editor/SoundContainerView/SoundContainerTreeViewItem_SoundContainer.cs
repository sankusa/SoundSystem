using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace SoundSystem {
    public class SoundContainerTreeViewItem_SoundContainer : TreeViewItem {
        public SoundContainer Container { get; }
        public SerializedObject SerializedObject { get; }
        public SerializedProperty SoundListProp { get; }
        
        public SoundContainerTreeViewItem_SoundContainer(SoundContainer container) : base(0, 0) {
            Container = container;
            SerializedObject = new SerializedObject(Container);
            SoundListProp = SerializedObject.FindProperty("_soundDic._list");
            id = Container.GetInstanceID();
            displayName = container.name;
        }

        public void AddElement() {
            SoundListProp.InsertArrayElementAtIndex(SoundListProp.arraySize);
        }

        public void OnSingleClick() {
            
        }

        public void OnDoubleClick() {
            AssetDatabase.OpenAsset(Container);
            EditorGUIUtility.PingObject(Container);
        }
    }
}