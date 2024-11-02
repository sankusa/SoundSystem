using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(IgnoreListenerVolume), 600)]
    public class IgnoreListenerVolume : SoundBehaviour {
        [SerializeField] bool _value;
        public bool Value {
            get => _value;
            set => _value = value;
        }

        protected override void OnUpdateIfActive(SoundPlayer player, float deltaTime) {
            GetAudioSourceAccessor(player).IgnoreListenerVolume = _value;
        }
    }
}