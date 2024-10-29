using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(VolumeMultiplier), 0)]
    public class VolumeMultiplier : SoundBehaviour {
        [SerializeField, Range(0, 1)] float _value = 1;
        public float Value => _value;

        protected override void ApplyMain(SoundPlayer player) {
            player.SetVolumeMultiplier(player.VolumeMultiplier * _value);
        }
    }
}