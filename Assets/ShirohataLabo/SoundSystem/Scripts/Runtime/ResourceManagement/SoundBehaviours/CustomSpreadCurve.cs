using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(CustomSpreadCurve), 558)]
    public class CustomSpreadCurve : SoundBehaviour {
        [SerializeField] AnimationCurve _curve = new();
        public AnimationCurve Curve => _curve;

        protected override void OnUpdateIfActive(SoundPlayer player, float deltaTime) {
            SetCustomCurve(player, AudioSourceCurveType.Spread, _curve);
        }
    }
}