using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(AudioReverbZone), 1006)]
    public class AudioReverbZone : SoundBehaviour {
        [SerializeField] bool _enable = true;
        public bool Enable {
            get => _enable;
            set => _enable = value;
        }

        [SerializeField, Min(0)] float _minDistance = 10f;
        public float MixDistance {
            get => _minDistance;
            set => _minDistance = value;
        }

        [SerializeField, Min(0)] float _maxDistance = 15f;
        public float MaxDistance {
            get => _maxDistance;
            set => _maxDistance = value;
        }

        [SerializeField] AudioReverbPreset _reverbPreset = AudioReverbPreset.User;
        public AudioReverbPreset ReverbPreset {
            get => _reverbPreset;
            set => _reverbPreset = value;
        }

        [SerializeField, Range(-10000, 0)] int _room = -1000;
        public int Room {
            get => _room;
            set => _room = value;
        }

        [SerializeField, Range(-10000, 0)] int _roomHF = -100;
        public int RoomHF {
            get => _roomHF;
            set => _roomHF = value;
        }

        [SerializeField, Range(-10000, 0)] int _roomLF;
        public int RoomLF {
            get => _roomLF;
            set => _roomLF = value;
        }

        [SerializeField, Range(0.1f, 20f)] float _decayTime = 1.49f;
        public float DecayTime {
            get => _decayTime;
            set => _decayTime = value;
        }

        [SerializeField, Range(0.1f, 2)] float _decayHFRatio = 0.83f;
        public float DecayHFRatio {
            get => _decayHFRatio;
            set => _decayHFRatio = value;
        }

        [SerializeField, Range(-10000, 1000)] int _reflections = -2602;
        public int Reflections {
            get => _reflections;
            set => _reflections = value;
        }

        [SerializeField, Range(0, 0.3f)] float _reflectionsDelay = 0.007f;
        public float ReflectionsDelay {
            get => _reflectionsDelay;
            set => _reflectionsDelay = value;
        }

        [SerializeField, Range(-10000, 2000)] int _reverb = 200;
        public int Reverb {
            get => _reverb;
            set => _reverb = value;
        }

        [SerializeField, Range(0f, 0.1f)] float _reverbDelay = 0.011f;
        public float ReverbDelay {
            get => _reverbDelay;
            set => _reverbDelay = value;
        }

        [SerializeField, Range(1000, 20000)] int _hfReference = 5000;
        public int HFReference {
            get => _hfReference;
            set => _hfReference = value;
        }

        [SerializeField, Range(20, 1000)] int _lfReference = 250;
        public int LFReference {
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
            AudioReverbZoneAccessor accessor = GetOrCreateAudioReverbZone(player);
            accessor.Enable = _enable;
            accessor.MinDistance = _minDistance;
            accessor.MaxDistance = _maxDistance;
            accessor.ReverbPreset = _reverbPreset;
            accessor.Room = _room;
            accessor.RoomHF = _roomHF;
            accessor.RoomLF = _roomLF;
            accessor.DecayTime = _decayTime;
            accessor.DecayHFRatio = _decayHFRatio;
            accessor.Reflections = _reflections;
            accessor.ReflectionsDelay = _reflectionsDelay;
            accessor.Reverb = _reverb;
            accessor.ReverbDelay = _reverbDelay;
            accessor.HFReference = _hfReference;
            accessor.LFReference = _lfReference;
            accessor.Diffusion = _diffusion;
            accessor.Density = _density;
        }
    }
}