using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Search;

namespace SoundSystem {
    [System.Serializable]
    public class Sound {
        [SerializeField] AudioUnit _audioUnit;
        public AudioUnit AudioUnit => _audioUnit;

        [SerializeReference] List<SoundBehaviour> _behavioiurs = new();
        public List<SoundBehaviour> Behaviours => _behavioiurs;
    }
}