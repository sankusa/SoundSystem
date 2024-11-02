using UnityEngine;

namespace SoundSystem {
    public class AudioLowPassFilterAccessor {
        GameObject GameObject { get; }
        public AudioLowPassFilter Filter { get; private set; }

        bool _enableOld;
        public bool Enable { get; set; }

        float _cutoffFrequencyOld;
        public float CutoffFrequency { get; set; }

        float _lowpassResonanceQOld;
        public float LowpassResonanceQ { get; set; }

        public AudioLowPassFilterAccessor(GameObject gameObject) {
            GameObject = gameObject;
        }

        public void CreateFilterIfNull() {
            if (Filter == null) {
                Filter = GameObject.AddComponent<AudioLowPassFilter>();
                SetDefault();
                Apply();
            }
        }

        public void Reset() {
            if (Filter == null) return;
            SetDefault();
        }

        public void ApplyIfChanged() {
            if (Filter != null) {
                if (Enable != _enableOld) Filter.enabled = Enable;
                if (CutoffFrequency != _cutoffFrequencyOld) Filter.cutoffFrequency = CutoffFrequency;
                if (LowpassResonanceQ != _lowpassResonanceQOld) Filter.lowpassResonanceQ = LowpassResonanceQ;
            }
            CopyToOld();
        }

        public void Clear() {
            DestroyAudioDistortionFilter();
            SetDefault();
            CopyToOld();
        }

        void SetDefault() {
            Enable = false;
            CutoffFrequency = 5007.7f;
            LowpassResonanceQ = 1;
        }

        void Apply() {
            if (Filter != null) {
                Filter.enabled = Enable;
                Filter.cutoffFrequency = CutoffFrequency;
                Filter.lowpassResonanceQ = LowpassResonanceQ;
            }
            CopyToOld();
        }

        void CopyToOld() {
            _enableOld = Enable;
            _cutoffFrequencyOld = CutoffFrequency;
            _lowpassResonanceQOld = LowpassResonanceQ;
        }

        void DestroyAudioDistortionFilter() {
            Filter.DestroyFlexible();
        }
    }
}