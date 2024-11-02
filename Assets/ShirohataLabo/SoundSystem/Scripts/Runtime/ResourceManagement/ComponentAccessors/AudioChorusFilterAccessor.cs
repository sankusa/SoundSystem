using UnityEngine;

namespace SoundSystem {
    public class AudioChorusFilterAccessor {
        GameObject GameObject { get; }
        public AudioChorusFilter Filter { get; private set; }

        bool _enableOld;
        public bool Enable { get; set; }

        float _dryMixOld;
        public float DryMix { get; set; }

        float _wetMix1Old;
        public float WetMix1 { get; set; }

        float _wetMix2Old;
        public float WetMix2 { get; set; }

        float _wetMix3Old;
        public float WetMix3 { get; set; }

        float _delayOld;
        public float Delay { get; set; }

        float _rateOld;
        public float Rate { get; set; }

        float _depthOld;
        public float Depth { get; set; }

        public AudioChorusFilterAccessor(GameObject gameObject) {
            GameObject = gameObject;
        }

        public void CreateFilterIfNull() {
            if (Filter == null) {
                Filter = GameObject.AddComponent<AudioChorusFilter>();
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
                if (DryMix != _dryMixOld) Filter.dryMix = DryMix;
                if (WetMix1 != _wetMix1Old) Filter.wetMix1 = WetMix1;
                if (WetMix2 != _wetMix2Old) Filter.wetMix2 = WetMix2;
                if (WetMix3 != _wetMix3Old) Filter.wetMix3 = WetMix3;
                if (Delay != _delayOld) Filter.delay = Delay;
                if (Rate != _rateOld) Filter.rate = Rate;
                if (Depth != _depthOld) Filter.depth = Depth;
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
            DryMix = 0.5f;
            WetMix1 = 0.5f;
            WetMix2 = 0.5f;
            WetMix3 = 0.5f;
            Delay = 40;
            Rate = 0.8f;
            Depth = 0.03f;
        }

        void Apply() {
            if (Filter != null) {
                Filter.enabled = Enable;
                Filter.dryMix = DryMix;
                Filter.wetMix1 = WetMix1;
                Filter.wetMix2 = WetMix2;
                Filter.wetMix3 = WetMix3;
                Filter.delay = Delay;
                Filter.rate = Rate;
                Filter.depth = Depth;
            }
            CopyToOld();
        }

        void CopyToOld() {
            _enableOld = Enable;
            _dryMixOld = DryMix;
            _wetMix1Old = WetMix1;
            _wetMix2Old = WetMix2;
            _wetMix3Old = WetMix3;
            _delayOld = Delay;
            _rateOld = Rate;
            _depthOld = Depth;
        }

        void DestroyAudioDistortionFilter() {
            Filter.DestroyFlexible();
        }
    }
}