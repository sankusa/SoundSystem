using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    public class MaxDistance : SoundBehaviour {
        [SerializeField, Min(0)] float _value = 1;
        public float Value => _value;

        protected override void ApplyMain(SoundPlayer player) {
            player.SetMaxDistance(_value);
        }
    }
}