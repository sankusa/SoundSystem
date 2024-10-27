using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(StereoPan), 510)]
    public class StereoPan : SoundBehaviour {
        [SerializeField, Range(-1, 1)] float _value;
        public float Value => _value;

        protected override void ApplyMain(SoundPlayer player) {
            player.SetStereoPan(_value);
        }
    }
}