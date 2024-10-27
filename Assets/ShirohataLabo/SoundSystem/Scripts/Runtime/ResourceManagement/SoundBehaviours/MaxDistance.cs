using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem("AudioSource/MaxDistance", 154)]
    public class MaxDistance : SoundBehaviour {
        [SerializeField, Min(0)] float _value = 1;
        public float Value => _value;

        protected override void ApplyMain(SoundPlayer player) {
            player.SetMaxDistance(_value);
        }
    }
}