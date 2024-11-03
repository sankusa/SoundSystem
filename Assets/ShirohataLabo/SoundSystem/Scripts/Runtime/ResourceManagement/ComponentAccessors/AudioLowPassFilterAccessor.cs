using UnityEngine;

namespace SoundSystem {
    public class AudioLowPassFilterAccessor : ComponentAccessorBase<AudioLowPassFilter> {
        bool _enableOld;
        public bool Enable { get; set; }

        float _cutoffFrequencyOld;
        public float CutoffFrequency { get; set; }

        float _lowpassResonanceQOld;
        public float LowpassResonanceQ { get; set; }

        public AudioLowPassFilterAccessor(GameObject gameObject) : base(gameObject) {}

        protected override void ApplyIfChangedMain() {
            if (Enable != _enableOld) Component.enabled = Enable;
            if (CutoffFrequency != _cutoffFrequencyOld) Component.cutoffFrequency = CutoffFrequency;
            if (LowpassResonanceQ != _lowpassResonanceQOld) Component.lowpassResonanceQ = LowpassResonanceQ;
        }

        protected override void SetDefault() {
            Enable = false;
            CutoffFrequency = 5007.7f;
            LowpassResonanceQ = 1;
        }

        protected override void ApplyMain() {
            Component.enabled = Enable;
            Component.cutoffFrequency = CutoffFrequency;
            Component.lowpassResonanceQ = LowpassResonanceQ;
        }

        protected override void CopyToOld() {
            if (Enable != _enableOld) _enableOld = Enable;
            if (CutoffFrequency != _cutoffFrequencyOld) _cutoffFrequencyOld = CutoffFrequency;
            if (LowpassResonanceQ != _lowpassResonanceQOld) _lowpassResonanceQOld = LowpassResonanceQ;
        }
    }
}