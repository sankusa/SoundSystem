using UnityEngine;

namespace SoundSystem {
    public class AudioHighPassFilterAccessor {
        GameObject GameObject { get; }
        public AudioHighPassFilter Filter { get; private set; }

        bool _enableOld;
        public bool Enable { get; set; }

        float _cutoffFrequencyOld;
        public float CutoffFrequency { get; set; }

        float _highpassResonanceQOld;
        public float HighpassResonanceQ { get; set; }

        public AudioHighPassFilterAccessor(GameObject gameObject) {
            GameObject = gameObject;
        }

        public void CreateFilterIfNull() {
            if (Filter == null) {
                Filter = GameObject.AddComponent<AudioHighPassFilter>();
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
                if (HighpassResonanceQ != _highpassResonanceQOld) Filter.highpassResonanceQ = HighpassResonanceQ;
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
            CutoffFrequency = 5000;
            HighpassResonanceQ = 1;
        }

        void Apply() {
            if (Filter != null) {
                Filter.enabled = Enable;
                Filter.cutoffFrequency = CutoffFrequency;
                Filter.highpassResonanceQ = HighpassResonanceQ;
            }
            CopyToOld();
        }

        void CopyToOld() {
            _enableOld = Enable;
            _cutoffFrequencyOld = CutoffFrequency;
            _highpassResonanceQOld = HighpassResonanceQ;
        }

        void DestroyAudioDistortionFilter() {
            Filter.DestroyFlexible();
        }
    }
}