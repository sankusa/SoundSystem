using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(MaxDistance), 554)]
    public class MaxDistance : SoundBehaviour {
        [SerializeField, Min(0)] float _value = 1;
        public float Value => _value;

        protected override void OnUpdateIfActive(SoundPlayer player, float deltaTime) {
            SetMaxDistance(player, _value);
        }
    }
}