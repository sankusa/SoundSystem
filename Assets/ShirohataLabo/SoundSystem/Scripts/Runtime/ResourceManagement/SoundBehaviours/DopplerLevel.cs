using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(DopplerLevel), 550)]
    public class DopplerLevel : SoundBehaviour {
        [SerializeField, Range(0, 5)] float _value = 1;
        public float Value {
            get => _value;
            set => _value = value;
        }

        protected override void OnUpdateIfActive(SoundPlayer player, float deltaTime) {
            GetAudioSourceAccessor(player).DopplerLevel = _value;
        }
    }
}