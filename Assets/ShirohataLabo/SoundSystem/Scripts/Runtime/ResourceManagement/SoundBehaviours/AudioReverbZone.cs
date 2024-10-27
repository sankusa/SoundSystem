using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(AudioReverbZone), 1006)]
    public class AudioReverbZone : SoundBehaviour {
        [SerializeField] bool _enable = true;
        public bool Enable => _enable;

        [SerializeField, Min(0)] float _minDistance = 10f;
        public float MixDistance => _minDistance;

        [SerializeField, Min(0)] float _maxDistance = 15f;
        public float MaxDistance => _maxDistance;

        [SerializeField] AudioReverbPreset _reverbPreset =AudioReverbPreset.User;
        public AudioReverbPreset ReverbPreset => _reverbPreset;

        [SerializeField, Range(-10000, 0)] int _room = -1000;
        public int Room => _room;

        [SerializeField, Range(-10000, 0)] int _roomHF = -100;
        public int RoomHF => _roomHF;

        [SerializeField, Range(-10000, 0)] int _roomLF;
        public int RoomLF => _roomLF;

        [SerializeField, Range(0.1f, 20f)] float _decayTime = 1.49f;
        public float DecayTime => _decayTime;

        [SerializeField, Range(0.1f, 2)] float _decayHFRatio = 0.83f;
        public float DecayHFRatio => _decayHFRatio;

        [SerializeField, Range(-10000, 1000)] int _reflections = -2602;
        public int Reflections => _reflections;

        [SerializeField, Range(0, 0.3f)] float _reflectionsDelay = 0.007f;
        public float ReflectionsDelay => _reflectionsDelay;

        [SerializeField, Range(-10000, 2000)] int _reverb = 200;
        public int Reverb => _reverb;

        [SerializeField, Range(0f, 0.1f)] float _reverbDelay = 0.011f;
        public float ReverbDelay => _reverbDelay;

        [SerializeField, Range(1000, 20000)] int _hfReference = 5000;
        public int HfReference => _hfReference;

        [SerializeField, Range(20, 1000)] int _lfReference = 250;
        public int LfReference => _lfReference;

        [SerializeField, Range(0f, 100f)] float _diffusion = 100f;
        public float Diffusion => _diffusion;

        [SerializeField, Range(0f, 100f)] float _density = 100f;
        public float Density => _density;

        protected override void ApplyMain(SoundPlayer player) {
            player.EnableAudioReverbZone();
            UnityEngine.AudioReverbZone reverbZone = player.AudioReverbZone;
            reverbZone.enabled = _enable;
            reverbZone.minDistance = _minDistance;
            reverbZone.maxDistance = _maxDistance;
            reverbZone.reverbPreset = _reverbPreset;
            reverbZone.room = _room;
            reverbZone.roomHF = _roomHF;
            reverbZone.roomLF = _roomLF;
            reverbZone.decayTime = _decayTime;
            reverbZone.decayHFRatio = _decayHFRatio;
            reverbZone.reflections = _reflections;
            reverbZone.reflectionsDelay = _reflectionsDelay;
            reverbZone.reverb = _reverb;
            reverbZone.reverbDelay = _reverbDelay;
            reverbZone.HFReference = _hfReference;
            reverbZone.LFReference = _lfReference;
            reverbZone.diffusion = _diffusion;
            reverbZone.density = _density;
        }
    }
}