using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(AudioDistortionFilter), 1001)]
    public class AudioDistortionFilter : SoundBehaviour {
        [SerializeField] bool _enable = true;
        public bool Enable {
            get => _enable;
            set => _enable = value;
        }

        [SerializeField, Range(0, 1)] float _distortionLevel = 0.5f;
        public float DistortionLevel {
            get => _distortionLevel;
            set => _distortionLevel = value;
        }

        protected override void OnUpdateIfActive(SoundPlayer player, float deltaTime) {
            UnityEngine.AudioDistortionFilter filter = GetOrCreateAudioDistortionFilter(player);
            filter.enabled = _enable;
            filter.distortionLevel = _distortionLevel;
        }
    }
}