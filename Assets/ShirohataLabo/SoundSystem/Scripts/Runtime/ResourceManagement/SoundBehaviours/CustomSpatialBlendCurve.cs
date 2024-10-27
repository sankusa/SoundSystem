using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem("AudioSource/CustomSpatialBlendCurve", 157)]
    public class CustomSpatialBlendCurve : SoundBehaviour {
        [SerializeField] AnimationCurve _curve = new();
        public AnimationCurve Curve => _curve;

        protected override void ApplyMain(SoundPlayer player) {
            player.SetCustomCurve(AudioSourceCurveType.SpatialBlend, _curve);
        }
    }
}