using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(RandomVolumeMultiplier), 1)]
    public class RandomVolumeMultiplier : SoundBehaviour {
        [SerializeField, Range(0, 1)] float _min = 0;
        [SerializeField, Range(0, 1)] float _max = 1;

        protected override void OnUpdateIfActive(SoundPlayer player, float deltaTime) {
            float value;
            player.PlayScopeStatusDictionary.TryGetValue(this, out object obj);
            if (obj == null) {
                value = UnityEngine.Random.Range(_min, _max);
                player.PlayScopeStatusDictionary[this] = value;
            }
            else {
                value = (float)player.PlayScopeStatusDictionary[this];
            }
            player.SetVolumeMultiplier(player.VolumeMultiplier * value);
        }
    }
}