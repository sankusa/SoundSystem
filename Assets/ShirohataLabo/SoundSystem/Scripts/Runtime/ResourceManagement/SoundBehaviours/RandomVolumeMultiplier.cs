using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(RandomVolumeMultiplier), 1)]
    public class RandomVolumeMultiplier : SoundBehaviour {
        [SerializeField, Range(0, 1)] float _min = 0;
        [SerializeField, Range(0, 1)] float _max = 1;

        protected override void ApplyMain(SoundPlayer player) {
            float value = UnityEngine.Random.Range(_min, _max);
            player.SetVolumeMultiplier(player.VolumeMultiplier * value);
        }
    }
}