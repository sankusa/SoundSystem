using System.Collections.Generic;
using UnityEngine;

namespace SoundSystem {
    [System.Serializable]
    public class Sound {
        [SerializeField] AudioUnit _audioUnit;
        public AudioUnit AudioUnit => _audioUnit;

        [SerializeReference] List<SoundBehaviour> _behaviours = new();
        public List<SoundBehaviour> Behaviours => _behaviours;
    }
}