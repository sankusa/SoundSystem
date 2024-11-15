using UnityEditor.IMGUI.Controls;

namespace SoundSystem {
    public class FolderContentTreeViewItemBase : TreeViewItem {
        public ObjectDatabaseRecord Record { get; }
        public FolderContentTreeViewItemBase(ObjectDatabaseRecord record) : base() {
            Record = record;
            id = record.Asset.GetInstanceID();
        }

        public virtual void OnSingleClick() {}
        public virtual void OnDoubleClick() {}
        public virtual void OnContextClick() {}
    }
}