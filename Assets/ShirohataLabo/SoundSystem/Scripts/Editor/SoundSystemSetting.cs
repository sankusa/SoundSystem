using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    [CreateAssetMenu(fileName = nameof(SoundSystemSetting), menuName = nameof(SoundSystem) + "/Develop/" + nameof(SoundSystemSetting))]
    public class SoundSystemSetting : ScriptableObject {
        static SoundSystemSetting _instance;
        public static SoundSystemSetting Instance {
            get {
                if(_instance == null) {
                    _instance = EditorUtil.LoadAllAsset<SoundSystemSetting>().First();
                }
                return _instance;
            }
        }

        [SerializeField] DefaultAsset _managedFolderRoot;
        public string ManagedFolderRootPath => AssetDatabase.GetAssetPath(_managedFolderRoot);
    }
}