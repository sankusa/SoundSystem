using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    public class FolderContentTreeViewItem_CustomClip : FolderContentTreeViewItemBase {
        public CustomClip CustomClip { get; }
        public string Label => CustomClip.name;

        public FolderContentTreeViewItem_CustomClip(ObjectDatabaseRecord record) : base(record) {
            CustomClip = record.Asset as CustomClip;
        }

        public override void OnDoubleClick() {
            AssetDatabase.OpenAsset(CustomClip);
            EditorGUIUtility.PingObject(CustomClip);
        }

        public override void OnContextClick() {
            GenericMenu menu = new();
            menu.AddItem(
                new GUIContent("Delete"),
                false,
                () => AssetDatabase.MoveAssetToTrash(AssetDatabase.GetAssetPath(CustomClip))
            );
            menu.ShowAsContext();
        }
    }
}