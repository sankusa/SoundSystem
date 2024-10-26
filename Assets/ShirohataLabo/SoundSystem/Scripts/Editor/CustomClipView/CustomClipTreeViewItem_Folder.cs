using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace SoundSystem {
    public class CustomClipTreeViewItem_Folder : TreeViewItem {
        string _folderPath;
        public string FolderPath => _folderPath;

        DefaultAsset _folderAsset;
        public DefaultAsset FolderAsset => _folderAsset;

        public CustomClipTreeViewItem_Folder(string folderPath) : base(0, 0, "") {
            _folderPath = folderPath;
            _folderAsset = AssetDatabase.LoadAssetAtPath<DefaultAsset>(folderPath);
            id = _folderAsset.GetInstanceID();
        }

        public void OnSingleClick() {
            
        }

        public void OnDoubleClick() {
            Selection.activeObject = _folderAsset;
            EditorApplication.delayCall += () => {
                AssetDatabase.OpenAsset(_folderAsset);
            };
        }
    }
}