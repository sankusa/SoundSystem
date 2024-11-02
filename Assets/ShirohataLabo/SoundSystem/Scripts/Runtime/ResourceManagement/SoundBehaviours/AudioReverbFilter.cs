using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(AudioReverbFilter), 1005)]
    public class AudioReverbFilter : SoundBehaviour {
        [SerializeField] bool _enable = true;
        public bool Enable {
            get => _enable;
            set => _enable = value;
        }

        [SerializeField] AudioReverbPreset _reverbPreset =AudioReverbPreset.User;
        public AudioReverbPreset ReverbPreset {
            get => _reverbPreset;
            set => _reverbPreset = value;
        }

        [SerializeField, Range(-10000, 0)] int _dryLevel;
        public int DryLevel {
            get => _dryLevel;
            set => _dryLevel = value;
        }

        [SerializeField, Range(-10000, 0)] int _room;
        public int Room {
            get => _room;
            set => _room = value;
        }

        [SerializeField, Range(-10000, 0)] int _roomHF;
        public int RoomHF {
            get => _roomHF;
            set => _roomHF = value;
        }

        [SerializeField, Range(-10000, 0)] int _roomLF;
        public int RoomLF {
            get => _roomLF;
            set => _roomLF = value;
        }

        [SerializeField, Range(0.1f, 20f)] float _decayTime = 1f;
        public float DecayTime {
            get => _decayTime;
            set => _decayTime = value;
        }

        [SerializeField, Range(0.1f, 2)] float _decayHFRatio = 0.5f;
        public float DecayHFRatio {
            get => _decayHFRatio;
            set => _decayHFRatio = value;
        }

        [SerializeField, Range(-10000, 1000)] int _reflectionsLevel = -10000;
        public int ReflectionsLevel {
            get => _reflectionsLevel;
            set => _reflectionsLevel = value;
        }

        [SerializeField, Range(0, 0.3f)] float _reflectionsDelay = 0;
        public float ReflectionsDelay {
            get => _reflectionsDelay;
            set => _reflectionsDelay = value;
        }

        [SerializeField, Range(-10000, 2000)] int _reverbLevel;
        public int ReverbLevel {
            get => _reverbLevel;
            set => _reverbLevel = value;
        }

        [SerializeField, Range(0f, 0.1f)] float _reverbDelay = 0.04f;
        public float ReverbDelay {
            get => _reverbDelay;
            set => _reverbDelay = value;
        }

        [SerializeField, Range(1000, 20000)] int _hfReference = 5000;
        public int HfReference {
            get => _hfReference;
            set => _hfReference = value;
        }

        [SerializeField, Range(20, 1000)] int _lfReference = 250;
        public int LfReference {
            get => _lfReference;
            set => _lfReference = value;
        }

        [SerializeField, Range(0f, 100f)] float _diffusion = 100f;
        public float Diffusion {
            get => _diffusion;
            set => _diffusion = value;
        }

        [SerializeField, Range(0f, 100f)] float _density = 100f;
        public float Density {
            get => _density;
            set => _density = value;
        }

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