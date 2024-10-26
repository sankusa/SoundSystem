using System;
using UnityEngine;

namespace SoundSystem.CustomClipParameters {
    [Serializable]
    public class VolumeMultiplierCurve : CustomClipParameter {
        [SerializeField] AnimationCurve _curve = new();
        public AnimationCurve Curve => _curve;
    }
}