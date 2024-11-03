using System;
using UnityEngine;

namespace SoundSystem.CustomClipParameters {
    [Serializable]
    public class VolumeMultiplier : CustomClipParameter {
        [SerializeField, Range(0, 1)] float _value = 1;
        public float Value => _value;
    }
}