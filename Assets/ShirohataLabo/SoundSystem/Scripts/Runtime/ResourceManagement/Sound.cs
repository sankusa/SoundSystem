using UnityEngine;
using UnityEngine.Search;

namespace SoundSystem {
    [System.Serializable]
    public class Sound {
        [SerializeField] AudioUnit _audioUnit;
        public AudioUnit AudioUnit => _audioUnit;
    }
}