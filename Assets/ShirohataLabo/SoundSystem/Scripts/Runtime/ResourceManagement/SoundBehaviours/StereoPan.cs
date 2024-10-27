using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem("AudioSource/StereoPan", 110)]
    public class StereoPan : SoundBehaviour {
        [SerializeField, Range(-1, 1)] float _value;
        public float Value => _value;

        protected override void ApplyMain(SoundPlayer player) {
            player.SetStereoPan(_value);
        }
    }
}