using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(AudioLowPassFilter), 1004)]
    public class AudioLowPassFilter : SoundBehaviour {
        [SerializeField] bool _enable = true;
        public bool Enable {
            get => _enable;
            set => _enable = value;
        }

        [SerializeField, Range(10, 22000)] float _cutoffFrequency = 5007.7f;
        public float CutoffFrequency {
            get => _cutoffFrequency;
            set => _cutoffFrequency = value;
        }

        [SerializeField, Range(1, 10)] float _lowpassResonanceQ = 1;
        public float LowPassResonanceQ {
            get => _lowpassResonanceQ;
            set => _lowpassResonanceQ = value;
        }

        protected override void OnUpdateIfActive(SoundPlayer player, float deltaTime) {
            UnityEngine.AudioLowPassFilter filter = GetOrCreateAudioLowPassFilter(player);
            filter.enabled = _enable;
            filter.cutoffFrequency = _cutoffFrequency;
            filter.lowpassResonanceQ = _lowpassResonanceQ;
        }
    }
}