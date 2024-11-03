using UnityEngine;

namespace SoundSystem {
    public class AudioChorusFilterAccessor : ComponentAccessorBase<AudioChorusFilter> {
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

        public AudioChorusFilterAccessor(GameObject gameObject) : base(gameObject) {}

        protected override void ApplyIfChangedMain() {
            if (Enable != _enableOld) Component.enabled = Enable;
            if (DryMix != _dryMixOld) Component.dryMix = DryMix;
            if (WetMix1 != _wetMix1Old) Component.wetMix1 = WetMix1;
            if (WetMix2 != _wetMix2Old) Component.wetMix2 = WetMix2;
            if (WetMix3 != _wetMix3Old) Component.wetMix3 = WetMix3;
            if (Delay != _delayOld) Component.delay = Delay;
            if (Rate != _rateOld) Component.rate = Rate;
            if (Depth != _depthOld) Component.depth = Depth;
        }

        protected override void SetDefault() {
            Enable = false;
            DryMix = 0.5f;
            WetMix1 = 0.5f;
            WetMix2 = 0.5f;
            WetMix3 = 0.5f;
            Delay = 40;
            Rate = 0.8f;
            Depth = 0.03f;
        }

        protected override void ApplyMain() {
            Component.enabled = Enable;
            Component.dryMix = DryMix;
            Component.wetMix1 = WetMix1;
            Component.wetMix2 = WetMix2;
            Component.wetMix3 = WetMix3;
            Component.delay = Delay;
            Component.rate = Rate;
            Component.depth = Depth;
        }

        protected override void CopyToOld() {
            if (Enable != _enableOld) _enableOld = Enable;
            if (DryMix != _dryMixOld) _dryMixOld = DryMix;
            if (WetMix1 != _wetMix1Old) _wetMix1Old = WetMix1;
            if (WetMix2 != _wetMix2Old) _wetMix2Old = WetMix2;
            if (WetMix3 != _wetMix3Old) _wetMix3Old = WetMix3;
            if (Delay != _delayOld) _delayOld = Delay;
            if (Rate != _rateOld) _rateOld = Rate;
            if (Depth != _depthOld) _depthOld = Depth;
        }
    }
}