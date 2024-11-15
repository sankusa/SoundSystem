using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    [CreateAssetMenu(fileName = nameof(TargetFolders), menuName = nameof(SoundSystem) + "/Develop/" + nameof(TargetFolders))]
    public class TargetFolders : ScriptableObject {
        static TargetFolders _instance;
        public static TargetFolders Instance {
            get {
                if(_instance == null) {
                    _instance = EditorUtil.LoadAllAsset<TargetFolders>().First();
                }
                return _instance;
            }
        }

        [SerializeField] List<DefaultAsset> _folders;

        public IEnumerable<DefaultAsset> SafeGetFolders() {
            return _folders
                .Where(x => x != null)
                .Where(x => AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(x)));
        }

        public IEnumerable<string> SafeGetFolderPaths() {
            return SafeGetFolders().Select(x => AssetDatabase.GetAssetPath(x));
        }

        public void Add(DefaultAsset folder) {
            if (_folders.Contains(folder)) return;
            if (AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(folder))) {
                _folders.Add(folder);
            }
        }

        public void Add(IEnumerable<DefaultAsset> folders) {
            if (folders == null) return;
            foreach (DefaultAsset folder in folders) {
                Add(folder);
            }
        }

        public void Remove(DefaultAsset folder) {
            Undo.RecordObject(this, $"Change {nameof(TargetFolders)}");
            _folders.Remove(folder);
        }
    }
}