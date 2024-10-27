using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(AudioLowPassFilter), 1004)]
    public class AudioLowPassFilter : SoundBehaviour {
        [SerializeField] bool _enable = true;
        public bool Enable => _enable;

        [SerializeField, Range(10, 22000)] float _cutoffFrequency = 5007.7f;
        public float CutoffFrequency => _cutoffFrequency;

        [SerializeField, Range(1, 10)] float _lowpassResonanceQ = 1;
        public float LowPassResonanceQ => _lowpassResonanceQ;

        protected override void ApplyMain(SoundPlayer player) {
            player.EnableAudioLowPassFilter();
            UnityEngine.AudioLowPassFilter filter = player.AudioLowPassFilter;
            filter.enabled = _enable;
            filter.cutoffFrequency = _cutoffFrequency;
            filter.lowpassResonanceQ = _lowpassResonanceQ;
        }
    }
}