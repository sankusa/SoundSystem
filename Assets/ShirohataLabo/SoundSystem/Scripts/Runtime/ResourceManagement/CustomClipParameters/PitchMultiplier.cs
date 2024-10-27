using System;
using UnityEngine;

namespace SoundSystem {
    [Serializable]
    public class PitchMultiplier : CustomClipParameter {
        [SerializeField, Min(0)] float _value = 1;
        public float Value => _value;
    }
}