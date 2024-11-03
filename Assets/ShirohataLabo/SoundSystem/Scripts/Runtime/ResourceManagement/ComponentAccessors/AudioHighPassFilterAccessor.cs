using UnityEngine;

namespace SoundSystem {
    public class AudioHighPassFilterAccessor : ComponentAccessorBase<AudioHighPassFilter> {
        bool _enableOld;
        public bool Enable { get; set; }

        float _cutoffFrequencyOld;
        public float CutoffFrequency { get; set; }

        float _highpassResonanceQOld;
        public float HighpassResonanceQ { get; set; }

        public AudioHighPassFilterAccessor(GameObject gameObject) : base(gameObject) {}

        protected override void ApplyIfChangedMain() {
            if (Enable != _enableOld) Component.enabled = Enable;
            if (CutoffFrequency != _cutoffFrequencyOld) Component.cutoffFrequency = CutoffFrequency;
            if (HighpassResonanceQ != _highpassResonanceQOld) Component.highpassResonanceQ = HighpassResonanceQ;
        }

        protected override void SetDefault() {
            Enable = false;
            CutoffFrequency = 5000;
            HighpassResonanceQ = 1;
        }

        protected override void ApplyMain() {
            Component.enabled = Enable;
            Component.cutoffFrequency = CutoffFrequency;
            Component.highpassResonanceQ = HighpassResonanceQ;
        }

        protected override void CopyToOld() {
            if (Enable != _enableOld) _enableOld = Enable;
            if (CutoffFrequency != _cutoffFrequencyOld) _cutoffFrequencyOld = CutoffFrequency;
            if (HighpassResonanceQ != _highpassResonanceQOld) _highpassResonanceQOld = HighpassResonanceQ;
        }
    }
}