using UnityEngine;

namespace SoundSystem {
    public class AudioDistortionFilterAccessor : AudioFilterAccessorBase<AudioDistortionFilter> {
        bool _enableOld;
        public bool Enable { get; set; }

        float _distortionLevelOld;
        public float DistortionLevel { get; set; }

        public AudioDistortionFilterAccessor(GameObject gameObject) : base(gameObject) {}

        protected override void ApplyIfChangedMain() {
            if (Enable != _enableOld) Component.enabled = Enable;
            if (DistortionLevel != _distortionLevelOld) Component.distortionLevel = DistortionLevel;
        }

        protected override void SetDefault() {
            Enable = false;
            DistortionLevel = 0.5f;
        }

        protected override void ApplyMain() {
            Component.enabled = Enable;
            Component.distortionLevel = DistortionLevel;
        }

        protected override void CopyToOld() {
            if (Enable != _enableOld) _enableOld = Enable;
            if (DistortionLevel != _distortionLevelOld) _distortionLevelOld = DistortionLevel;
        }
    }
}