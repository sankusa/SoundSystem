using UnityEngine;

namespace SoundSystem {
    [System.Serializable]
    public class SoundWithKey {
        [SerializeField] string _key;
        public string Key => _key;

        [SerializeField] Sound _sound;
        public Sound Sound => _sound;
    }
}