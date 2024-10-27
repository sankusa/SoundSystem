using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem("AudioSource/BypassListenerEffects", 101)]
    public class BypassListenerEffects : SoundBehaviour {
        [SerializeField] bool _value = true;
        public bool Value => _value;

        protected override void ApplyMain(SoundPlayer player) {
            player.SetBypassListenerEffects(_value);
        }
    }
}