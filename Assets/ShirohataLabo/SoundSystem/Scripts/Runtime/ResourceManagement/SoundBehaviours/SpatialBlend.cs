using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(SpatialBlend), 511)]
    public class SpatialBlend : SoundBehaviour {
        [SerializeField, Range(0, 1)] float _value;
        public float Value => _value;

        protected override void ApplyMain(SoundPlayer player) {
            player.SetSpatialBlend(_value);
        }
    }
}