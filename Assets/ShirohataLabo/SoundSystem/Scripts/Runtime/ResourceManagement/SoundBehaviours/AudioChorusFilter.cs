using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(AudioChorusFilter), 1000)]
    public class AudioChorusFilter : SoundBehaviour {
        [SerializeField] bool _enable = true;
        public bool Enable => _enable;

        [SerializeField, Range(0f, 1f)] float _dryMix = 0.5f;
        public float DryMix => _dryMix;

        [SerializeField, Range(0f, 1f)] float _wetMix1 = 0.5f;
        public float WetMix1 => _wetMix1;

        [SerializeField, Range(0f, 1f)] float _wetMix2 = 0.5f;
        public float WetMix2 => _wetMix2;

        [SerializeField, Range(0f, 1f)] float _wetMix3 = 0.5f;
        public float WetMix3 => _wetMix3;

        [SerializeField, Range(0.1f, 100f)] float _delay = 40f;
        public float Delay => _delay;

        [SerializeField, Range(0f, 20f)] float _rate = 0.8f;
        public float Rate => _rate;

        [SerializeField, Range(0f, 1f)] float _depth = 0.03f;
        public float Depth => _depth;

        protected override void OnUpdateIfActive(SoundPlayer player, float deltaTime) {
            UnityEngine.AudioChorusFilter filter = GetOrCreateAudioChorusFilter(player);
            filter.enabled = _enable;
            filter.dryMix = _dryMix;
            filter.wetMix1 = _wetMix1;
            filter.wetMix2 = _wetMix2;
            filter.wetMix3 = _wetMix3;
            filter.delay = _delay;
            filter.rate = _rate;
            filter.depth = _depth;
        }
    }
}