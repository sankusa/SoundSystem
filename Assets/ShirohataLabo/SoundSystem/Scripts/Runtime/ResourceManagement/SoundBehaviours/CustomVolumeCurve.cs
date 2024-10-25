using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    public class CustomVolumeCurve : SoundBehaviour {
        [SerializeField] AnimationCurve _curve = new();
        public AnimationCurve Curve => _curve;

        protected override void ApplyMain(SoundPlayer player) {
            player.SetCustomCurve(AudioSourceCurveType.CustomRolloff, _curve);
        }
    }
}