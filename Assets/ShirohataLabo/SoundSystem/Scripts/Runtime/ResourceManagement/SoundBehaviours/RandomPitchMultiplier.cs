using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(RandomPitchMultiplier), 11)]
    public class RandomPitchMultiplier : SoundBehaviour {
        [SerializeField] float _min = 1;
        [SerializeField] float _max = 1;

        protected override void ApplyMain(SoundPlayer player) {
            float value = UnityEngine.Random.Range(_min, _max);
            player.SetPitchMultiplier(player.PitchMultiplier * value);
        }
    }
}