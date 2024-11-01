using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(RandomPitchMultiplier), 11)]
    public class RandomPitchMultiplier : SoundBehaviour {
        [SerializeField] float _min = 1;
        [SerializeField] float _max = 1;

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
            player.SetPitchMultiplier(player.PitchMultiplier * value);
        }
    }
}