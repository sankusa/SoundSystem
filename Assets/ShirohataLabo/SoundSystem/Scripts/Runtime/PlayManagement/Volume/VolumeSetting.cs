using System;
using UnityEngine;

namespace SoundSystem {
    [Serializable]
    public class VolumeSetting {
        [SerializeField] string _key;
        public string Key => _key;

        [SerializeField] bool _save;
        public bool Save => _save;

        [SerializeField, Range(0, 1)] float _defaultVolume = 0.5f;
        public float DefaultVolume => _defaultVolume;

        [SerializeField] bool _defaultMute;
        public bool DefaultMute => _defaultMute;
    }
}