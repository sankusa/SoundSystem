using System.Linq;
using UnityEngine;

namespace SoundSystem {
    [CreateAssetMenu(fileName = nameof(Skin), menuName = nameof(SoundSystem) + "/Develop/" + nameof(Skin))]
    public class Skin : ScriptableObject {
        static Skin _instance;
        public static Skin Instance {
            get {
                if(_instance == null) {
                    _instance = EditorUtil.LoadAllAsset<Skin>().First();
                }
                return _instance;
            }
        }

        [SerializeField] Texture2D _mainIcon;
        public Texture2D MainIcon => _mainIcon;
    }
}