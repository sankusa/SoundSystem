using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    public class AudioDistortionFilter : SoundBehaviour {
        [SerializeField] bool _enable = true;
        public bool Enable => _enable;

        [SerializeField, Range(0, 1)] float _distortionLevel = 0.5f;
        public float DistortionLevel => _distortionLevel;

        protected override void ApplyMain(SoundPlayer player) {
            player.EnableAudioDistortionFilter();
            UnityEngine.AudioDistortionFilter filter = player.AudioDistortionFilter;
            filter.enabled = _enable;
            filter.distortionLevel = _distortionLevel;
        }
    }
}