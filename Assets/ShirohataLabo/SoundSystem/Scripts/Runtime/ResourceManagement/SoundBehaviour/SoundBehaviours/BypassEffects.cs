using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(BypassEffects), 500)]
    public class BypassEffects : SoundBehaviour {
        [SerializeField] bool _value = true;
        public bool Value {
            get => _value;
            set => _value = value;
        }

        protected override void OnUpdateIfActive(SoundPlayer player, float deltaTime) {
            GetAudioSourceAccessor(player).BypassEffects = _value;
        }
    }
}