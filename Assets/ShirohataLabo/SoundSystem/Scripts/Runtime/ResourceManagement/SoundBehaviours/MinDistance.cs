using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(MinDistance), 553)]
    public class MinDistance : SoundBehaviour {
        [SerializeField, Min(0)] float _value = 1;
        public float Value => _value;

        protected override void ApplyMain(SoundPlayer player) {
            player.SetMinDistance(_value);
        }
    }
}