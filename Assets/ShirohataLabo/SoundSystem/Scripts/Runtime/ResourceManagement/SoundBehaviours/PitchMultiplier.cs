using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(PitchMultiplier), 10)]
    public class PitchMultiplier : SoundBehaviour {
        [SerializeField] float _value = 1;
        public float Value => _value;

        protected override void OnUpdateIfActive(SoundPlayer player, float deltaTime) {
            SetPitchMultiplier(player, GetPitchMultiplier(player) * _value);
        }
    }
}