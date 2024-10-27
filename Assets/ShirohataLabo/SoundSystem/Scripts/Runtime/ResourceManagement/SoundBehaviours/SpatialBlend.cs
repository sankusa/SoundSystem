using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem("AudioSource/SpatialBlend", 111)]
    public class SpatialBlend : SoundBehaviour {
        [SerializeField, Range(0, 1)] float _value;
        public float Value => _value;

        protected override void ApplyMain(SoundPlayer player) {
            player.SetSpatialBlend(_value);
        }
    }
}