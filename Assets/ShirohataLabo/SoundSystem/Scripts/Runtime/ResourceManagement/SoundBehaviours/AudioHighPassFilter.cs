using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(AudioHighPassFilter), 1003)]
    public class AudioHighPassFilter : SoundBehaviour {
        [SerializeField] bool _enable = true;
        public bool Enable => _enable;

        [SerializeField, Range(10, 22000)] float _cutoffFrequency = 5000;
        public float CutoffFrequency => _cutoffFrequency;

        [SerializeField, Range(1, 10)] float _highpassResonanceQ = 1;
        public float HighPassResonanceQ => _highpassResonanceQ;

        protected override void ApplyMain(SoundPlayer player) {
            player.EnableAudioHighPassFilter();
            UnityEngine.AudioHighPassFilter filter = player.AudioHighPassFilter;
            filter.enabled = _enable;
            filter.cutoffFrequency = _cutoffFrequency;
            filter.highpassResonanceQ = _highpassResonanceQ;
        }
    }
}