using System;
using UnityEngine;

namespace SoundSystem.AudioUnitEffects {
    [Serializable]
    public class VolumeMultiplierCurve {
        [SerializeField] bool _enable;
        public bool Enable => _enable;

        [SerializeField] AnimationCurve _curve = new();
        public AnimationCurve Curve => _curve;
    }
}