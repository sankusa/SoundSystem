using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(MinDistance), 553)]
    public class MinDistance : SoundBehaviour {
        [SerializeField, Min(0)] float _value = 1;
        public float Value {
            get => _value;
            set => _value = value;
        }

        protected override void OnUpdateIfActive(SoundPlayer player, float deltaTime) {
            SetMinDistance(player, _value);
        }
    }
}