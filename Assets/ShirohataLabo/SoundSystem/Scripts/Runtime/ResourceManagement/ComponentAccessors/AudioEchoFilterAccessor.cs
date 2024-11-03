using UnityEngine;

namespace SoundSystem {
    public class AudioEchoFilterAccessor : ComponentAccessorBase<AudioEchoFilter> {
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

        public AudioEchoFilterAccessor(GameObject gameObject) : base(gameObject) {}

        protected override void ApplyIfChangedMain() {
            if (Enable != _enableOld) Component.enabled = Enable;
            if (Delay != _delayOld) Component.delay = Delay;
            if (DecayRatio != _decayRatioOld) Component.decayRatio = DecayRatio;
            if (DryMix != _dryMixOld) Component.dryMix = DryMix;
            if (WetMix != _wetMixOld) Component.wetMix = WetMix;
        }

        protected override void SetDefault() {
            Enable = false;
            Delay = 500;
            DecayRatio = 0.5f;
            DryMix = 1;
            WetMix = 1;
        }

        protected override void ApplyMain() {
            Component.enabled = Enable;
            Component.delay = Delay;
            Component.decayRatio = DecayRatio;
            Component.dryMix = DryMix;
            Component.wetMix = WetMix;
        }

        protected override void CopyToOld() {
            if (Enable != _enableOld) _enableOld = Enable;
            if (Delay != _delayOld) _delayOld = Delay;
            if (DecayRatio != _decayRatioOld) _decayRatioOld = DecayRatio;
            if (DryMix != _dryMixOld) _dryMixOld = DryMix;
            if (WetMix != _wetMixOld) _wetMixOld = WetMix;
        }
    }
}