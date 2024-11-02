using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(RandomVolumeMultiplier), 1)]
    public class RandomVolumeMultiplier : SoundBehaviour {
        [SerializeField, Range(0, 1)] float _min = 0;
        public float Min {
            get => _min;
            set => _min = value;
        }

        [SerializeField, Range(0, 1)] float _max = 1;
        public float Max {
            get => _max;
            set => _max = value;
        }

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

            GetAudioSourceAccessor(player).Volume *= value;
        }
    }
}