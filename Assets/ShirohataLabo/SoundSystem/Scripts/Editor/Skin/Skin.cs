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

        [SerializeField] Texture2D _playIcon;
        public Texture2D PlayIcon => _playIcon;

        [SerializeField] Texture2D _pauseIcon;
        public Texture2D PauseIcon => _pauseIcon;

        [SerializeField] Texture2D _repeatIcon;
        public Texture2D RepeatIcon => _repeatIcon;
    }
}