using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(BypassEffects), 500)]
    public class BypassEffects : SoundBehaviour {
        [SerializeField] bool _value = true;
        public bool Value => _value;

        protected override void ApplyMain(SoundPlayer player) {
            player.SetBypassEffects(_value);
        }
    }
}