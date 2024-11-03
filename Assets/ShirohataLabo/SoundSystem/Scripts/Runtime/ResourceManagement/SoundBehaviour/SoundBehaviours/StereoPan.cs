using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(StereoPan), 510)]
    public class StereoPan : SoundBehaviour {
        [SerializeField, Range(-1, 1)] float _value;
        public float Value {
            get => _value;
            set => _value = value;
        }

        protected override void OnUpdateIfActive(SoundPlayer player, float deltaTime) {
            GetAudioSourceAccessor(player).StereoPan = _value;
        }
    }
}