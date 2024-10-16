using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace SoundSystem {
    public class AudioClipTreeViewItem_Folder : TreeViewItem {
        string _folderPath;
        public string FolderPath => _folderPath;

        DefaultAsset _folderAsset;
        public DefaultAsset FolderAsset => _folderAsset;

        public AudioClipTreeViewItem_Folder(int id, string folderPath) : base(id, 0, "") {
            _folderPath = folderPath;
            _folderAsset = AssetDatabase.LoadAssetAtPath<DefaultAsset>(folderPath);
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