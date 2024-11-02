using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(CustomVolumeCurve), 556)]
    public class CustomVolumeCurve : SoundBehaviour {
        [SerializeField] AnimationCurve _curve = new();
        public AnimationCurve Curve {
            get => _curve;
            set => _curve = value;
        }

        protected override void OnUpdateIfActive(SoundPlayer player, float deltaTime) {
            SetCustomCurve(player, AudioSourceCurveType.CustomRolloff, _curve);
        }
    }
}