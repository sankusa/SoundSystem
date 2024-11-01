using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(ReverbZoneMix), 512)]
    public class ReverbZoneMix : SoundBehaviour {
        [SerializeField, Range(0, 1.1f)] float _value = 1;
        public float Value => _value;

        protected override void OnUpdateIfActive(SoundPlayer player, float deltaTime) {
            SetReverbZoneMix(player, _value);
        }
    }
}