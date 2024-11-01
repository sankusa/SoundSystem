using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(BypassListenerEffects), 501)]
    public class BypassListenerEffects : SoundBehaviour {
        [SerializeField] bool _value = true;
        public bool Value => _value;

        protected override void OnUpdateIfActive(SoundPlayer player, float deltaTime) {
            SetBypassListenerEffects(player, _value);
        }
    }
}