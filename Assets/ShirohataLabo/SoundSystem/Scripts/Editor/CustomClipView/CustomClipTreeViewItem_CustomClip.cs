using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace SoundSystem {
    public class CustomClipTreeViewItem_CustomClip : TreeViewItem {
        public CustomClip CustomClip { get; }

        public string Label => CustomClip.name;

        public CustomClipTreeViewItem_CustomClip(CustomClip customClip) : base(0, 0, customClip != null ? customClip.name : null) {
            CustomClip = customClip;
            displayName = customClip.name;
            id = customClip.GetInstanceID();
        }

        public void OnSingleClick() {
            
        }

        public void OnDoubleClick() {
            AssetDatabase.OpenAsset(CustomClip);
            EditorGUIUtility.PingObject(CustomClip);
        }
    }
}