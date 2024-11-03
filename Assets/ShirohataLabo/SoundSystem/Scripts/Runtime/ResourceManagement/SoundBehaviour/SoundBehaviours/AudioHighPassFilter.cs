using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(AudioHighPassFilter), 1003)]
    public class AudioHighPassFilter : SoundBehaviour {
        [SerializeField] bool _enable = true;
        public bool Enable {
            get => _enable;
            set => _enable = value;
        }

        [SerializeField, Range(10, 22000)] float _cutoffFrequency = 5000;
        public float CutoffFrequency {
            get => _cutoffFrequency;
            set => _cutoffFrequency = value;
        }

        [SerializeField, Range(1, 10)] float _highpassResonanceQ = 1;
        public float HighpassResonanceQ {
            get => _highpassResonanceQ;
            set => _highpassResonanceQ = value;
        }

        protected override void OnUpdateIfActive(SoundPlayer player, float deltaTime) {
            AudioHighPassFilterAccessor accessor = GetOrCreateAudioHighPassFilter(player);
            accessor.Enable = _enable;
            accessor.CutoffFrequency = _cutoffFrequency;
            accessor.HighpassResonanceQ = _highpassResonanceQ;
        }
    }
}