using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(PitchMultiplier), 0)]
    public class PitchMultiplier : SoundBehaviour {
        [SerializeField] float _value = 1;
        public float Value => _value;

        protected override void ApplyMain(SoundPlayer player) {
            player.SetPitchMultiplier(_value);
        }
    }
}