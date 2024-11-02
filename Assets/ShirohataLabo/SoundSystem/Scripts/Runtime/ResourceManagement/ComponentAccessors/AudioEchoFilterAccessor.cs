using UnityEngine;

namespace SoundSystem {
    public class AudioEchoFilterAccessor {
        GameObject GameObject { get; }
        public AudioEchoFilter Filter { get; private set; }

        bool _enableOld;
        public bool Enable { get; set; }

        float _delayOld;
        public float Delay { get; set; }

        float _decayRatioOld;
        public float DecayRatio { get; set; }

        float _dryMixOld;
        public float DryMix { get; set; }

        float _wetMixOld;
        public float WetMix { get; set; }

        public AudioEchoFilterAccessor(GameObject gameObject) {
            GameObject = gameObject;
        }

        public void CreateFilterIfNull() {
            if (Filter == null) {
                Filter = GameObject.AddComponent<AudioEchoFilter>();
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
                if (Delay != _delayOld) Filter.delay = Delay;
                if (DecayRatio != _decayRatioOld) Filter.decayRatio = DecayRatio;
                if (DryMix != _dryMixOld) Filter.dryMix = DryMix;
                if (WetMix != _wetMixOld) Filter.wetMix = WetMix;
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
            Delay = 500;
            DecayRatio = 0.5f;
            DryMix = 1;
            WetMix = 1;
        }

        void Apply() {
            if (Filter != null) {
                Filter.enabled = Enable;
                Filter.delay = Delay;
                Filter.decayRatio = DecayRatio;
                Filter.dryMix = DryMix;
                Filter.wetMix = WetMix;
            }
            CopyToOld();
        }

        void CopyToOld() {
            _enableOld = Enable;
            _delayOld = Delay;
            _decayRatioOld = DecayRatio;
            _dryMixOld = DryMix;
            _wetMixOld = WetMix;
        }

        void DestroyAudioDistortionFilter() {
            Filter.DestroyFlexible();
        }
    }
}