using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(DopplerLevel), 550)]
    public class DopplerLevel : SoundBehaviour {
        [SerializeField, Range(0, 5)] float _value = 1;
        public float Value => _value;

        protected override void ApplyMain(SoundPlayer player) {
            player.SetDopplerLevel(_value);
        }
    }
}