using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(IgnoreListenerPause), 601)]
    public class IgnoreListenerPause : SoundBehaviour {
        [SerializeField] bool _value;
        public bool Value {
            get => _value;
            set => _value = value;
        }

        protected override void OnUpdateIfActive(SoundPlayer player, float deltaTime) {
            GetAudioSourceAccessor(player).IgnoreListenerPause = _value;
        }
    }
}