using UnityEngine;

namespace SoundSystem {
    public class AudioDistortionFilterAccessor {
        GameObject GameObject { get; }
        public AudioDistortionFilter Filter { get; private set; }

        bool _enableOld;
        public bool Enable { get; set; }

        float _distortionLevelOld;
        public float DistortionLevel { get; set; }

        public AudioDistortionFilterAccessor(GameObject gameObject) {
            GameObject = gameObject;
        }

        public void CreateFilterIfNull() {
            if (Filter == null) {
                Filter = GameObject.AddComponent<AudioDistortionFilter>();
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
                if (DistortionLevel != _distortionLevelOld) Filter.distortionLevel = DistortionLevel;
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
            DistortionLevel = 0.5f;
        }

        void Apply() {
            if (Filter != null) {
                Filter.enabled = Enable;
                Filter.distortionLevel = DistortionLevel;
            }
            CopyToOld();
        }

        void CopyToOld() {
            _enableOld = Enable;
            _distortionLevelOld = DistortionLevel;
        }

        void DestroyAudioDistortionFilter() {
            Filter.DestroyFlexible();
        }
    }
}