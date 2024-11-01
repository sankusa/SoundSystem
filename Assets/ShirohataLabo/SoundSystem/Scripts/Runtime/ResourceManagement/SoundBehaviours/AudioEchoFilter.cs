using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(AudioEchoFilter), 1002)]
    public class AudioEchoFilter : SoundBehaviour {
        [SerializeField] bool _enable = true;
        public bool Enable => _enable;

        [SerializeField, Range(10, 5000)] int _delay = 500;
        public int Delay => _delay;

        [SerializeField, Range(0, 1)] float _decayRatio = 0.5f; 
        public float DecayRatio => _decayRatio;

        [SerializeField, Range(0, 1)] float _dryMix = 1f;
        public float DryMix => _dryMix;

        [SerializeField, Range(0, 1)] float _wetMix = 1f;
        public float WetMix => _wetMix;

        protected override void OnUpdateIfActive(SoundPlayer player, float deltaTime) {
            UnityEngine.AudioEchoFilter filter = GetOrCreateAudioEchoFilter(player);
            filter.enabled = _enable;
            filter.delay = _delay;
            filter.dryMix = _dryMix;
            filter.wetMix = _wetMix;
        }
    }
}