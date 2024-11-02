using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(Spread), 551)]
    public class Spread : SoundBehaviour {
        [SerializeField, Range(0, 360)] float _value;
        public float Value {
            get => _value;
            set => _value = value;
        }

        protected override void OnUpdateIfActive(SoundPlayer player, float deltaTime) {
            SetSpread(player, _value);
        }
    }
}