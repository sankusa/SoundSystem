using System;
using UnityEngine;

namespace SoundSystem.AudioUnitEffects {
    [Serializable]
    public class VolumeMultiplier {
        [SerializeField] bool _enable;
        public bool Enable => _enable;

        [SerializeField, Range(0, 1)] float _value = 1;
        public float Value => _value;
    }
}