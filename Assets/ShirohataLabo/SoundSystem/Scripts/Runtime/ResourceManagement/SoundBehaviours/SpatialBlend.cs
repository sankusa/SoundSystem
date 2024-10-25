using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    public class SpatialBlend : SoundBehaviour {
        [SerializeField, Range(0, 1)] float _value;
        public float Value => _value;

        protected override void ApplyMain(SoundPlayer player) {
            player.SetSpatialBlend(_value);
        }
    }
}