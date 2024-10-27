using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(Spread), 551)]
    public class Spread : SoundBehaviour {
        [SerializeField, Range(0, 360)] float _value;
        public float Value => _value;

        protected override void ApplyMain(SoundPlayer player) {
            player.SetSpread(_value);
        }
    }
}