using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(CustomReverbZoneMixCurve), 559)]
    public class CustomReverbZoneMixCurve : SoundBehaviour {
        [SerializeField] AnimationCurve _curve = new();
        public AnimationCurve Curve => _curve;

        protected override void ApplyMain(SoundPlayer player) {
            player.SetCustomCurve(AudioSourceCurveType.ReverbZoneMix, _curve);
        }
    }
}