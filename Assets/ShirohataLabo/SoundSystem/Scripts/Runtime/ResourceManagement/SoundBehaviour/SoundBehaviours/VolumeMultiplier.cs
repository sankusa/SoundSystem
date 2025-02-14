using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(VolumeMultiplier), 0)]
    public class VolumeMultiplier : SoundBehaviour {
        [SerializeField, Range(0, 1)] float _value = 1;
        public float Value {
            get => _value;
            set => _value = value;
        }

        protected override void OnUpdateIfActive(SoundPlayer player, float deltaTime) {
            GetAudioSourceAccessor(player).Volume *= _value;
        }
    }
}