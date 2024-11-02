using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(MaxDistance), 554)]
    public class MaxDistance : SoundBehaviour {
        [SerializeField, Min(0)] float _value = 1;
        public float Value {
            get => _value;
            set => _value = value;
        }

        protected override void OnUpdateIfActive(SoundPlayer player, float deltaTime) {
            GetAudioSourceAccessor(player).MaxDistance = _value;
        }
    }
}