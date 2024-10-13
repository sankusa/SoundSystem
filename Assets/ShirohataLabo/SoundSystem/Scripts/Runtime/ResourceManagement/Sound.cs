using UnityEngine;
using UnityEngine.Search;

namespace SoundSystem {
    [System.Serializable]
    public class Sound {
        [SerializeField, SearchContext("adb:t:AudioUnit")] AudioUnit _audioUnit;
        public AudioUnit AudioUnit => _audioUnit;
    }
}