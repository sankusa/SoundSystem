using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem("AudioSource/CustomSpreadCurve", 158)]
    public class CustomSpreadCurve : SoundBehaviour {
        [SerializeField] AnimationCurve _curve = new();
        public AnimationCurve Curve => _curve;

        protected override void ApplyMain(SoundPlayer player) {
            player.SetCustomCurve(AudioSourceCurveType.Spread, _curve);
        }
    }
}