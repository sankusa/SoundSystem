using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(PitchMultiplier), 10)]
    public class PitchMultiplier : SoundBehaviour {
        [SerializeField] float _value = 1;
        public float Value {
            get => _value;
            set => _value = value;
        }

        protected override void OnUpdateIfActive(SoundPlayer player, float deltaTime) {
            GetAudioSourceAccessor(player).Pitch *= _value;
        }
    }
}