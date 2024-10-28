using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SoundSystem {
    public class ClipTreeViewItem_CustomClip : TreeViewItem {
        public CustomClip CustomClip { get; }

        public string Label => CustomClip.name;

        public ClipTreeViewItem_CustomClip(CustomClip customClip) : base(0, 0) {
            CustomClip = customClip;
            displayName = customClip.name;
            id = customClip.GetInstanceID();
            displayName = customClip != null ? customClip.name + " " + customClip.Description : null;
        }

        public void OnSingleClick() {
            
        }

        public void OnDoubleClick() {
            AssetDatabase.OpenAsset(CustomClip);
            EditorGUIUtility.PingObject(CustomClip);
        }

        public void OnContextClick() {
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