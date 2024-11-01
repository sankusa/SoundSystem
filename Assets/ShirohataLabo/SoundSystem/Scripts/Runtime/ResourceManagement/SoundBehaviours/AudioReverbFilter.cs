using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(AudioReverbFilter), 1005)]
    public class AudioReverbFilter : SoundBehaviour {
        [SerializeField] bool _enable = true;
        public bool Enable => _enable;

        [SerializeField] AudioReverbPreset _reverbPreset =AudioReverbPreset.User;
        public AudioReverbPreset ReverbPreset => _reverbPreset;

        [SerializeField, Range(-10000, 0)] int _dryLevel;
        public int DryLevel => _dryLevel;

        [SerializeField, Range(-10000, 0)] int _room;
        public int Room => _room;

        [SerializeField, Range(-10000, 0)] int _roomHF;
        public int RoomHF => _roomHF;

        [SerializeField, Range(-10000, 0)] int _roomLF;
        public int RoomLF => _roomLF;

        [SerializeField, Range(0.1f, 20f)] float _decayTime = 1f;
        public float DecayTime => _decayTime;

        [SerializeField, Range(0.1f, 2)] float _decayHFRatio = 0.5f;
        public float DecayHFRatio => _decayHFRatio;

        [SerializeField, Range(-10000, 1000)] int _reflectionsLevel = -10000;
        public int ReflectionsLevel => _reflectionsLevel;

        [SerializeField, Range(0, 0.3f)] float _reflectionsDelay = 0;
        public float ReflectionsDelay => _reflectionsDelay;

        [SerializeField, Range(-10000, 2000)] int _reverbLevel;
        public int ReverbLevel => _reverbLevel;

        [SerializeField, Range(0f, 0.1f)] float _reverbDelay = 0.04f;
        public float ReverbDelay => _reverbDelay;

        [SerializeField, Range(1000, 20000)] int _hfReference = 5000;
        public int HfReference => _hfReference;

        [SerializeField, Range(20, 1000)] int _lfReference = 250;
        public int LfReference => _lfReference;

        [SerializeField, Range(0f, 100f)] float _diffusion = 100f;
        public float Diffusion => _diffusion;

        [SerializeField, Range(0f, 100f)] float _density = 100f;
        public float Density => _density;

        protected override void OnUpdateIfActive(SoundPlayer player, float deltaTime) {
            UnityEngine.AudioReverbFilter filter = GetOrCreateAudioReverbFilter(player);
            filter.enabled = _enable;
            filter.reverbPreset = _reverbPreset;
            filter.dryLevel = _dryLevel;
            filter.room = _room;
            filter.roomHF = _roomHF;
            filter.roomLF = _roomLF;
            filter.decayTime = _decayTime;
            filter.decayHFRatio = _decayHFRatio;
            filter.reflectionsLevel = _reflectionsLevel;
            filter.reflectionsDelay = _reflectionsDelay;
            filter.reverbLevel = _reverbLevel;
            filter.reverbDelay = _reverbDelay;
            filter.hfReference = _hfReference;
            filter.lfReference = _lfReference;
            filter.diffusion = _diffusion;
            filter.density = _density;
        }
    }
}