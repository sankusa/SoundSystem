using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(SpatialBlend), 511)]
    public class SpatialBlend : SoundBehaviour {
        [SerializeField, Range(0, 1)] float _value;
        public float Value {
            get => _value;
            set => _value = value;
        }

        protected override void OnUpdateIfActive(SoundPlayer player, float deltaTime) {
            SetSpatialBlend(player, _value);
        }
    }
}