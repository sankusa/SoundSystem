using System;
using UnityEngine;

namespace SoundSystem {
    [Serializable]
    public class PitchMultiplier {
        [SerializeField] bool _enable;
        public bool Enable => _enable;

        [SerializeField, Min(0)] float _value = 1;
        public float Value => _value;
    }
}