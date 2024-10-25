using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    public class ReverbZoneMix : SoundBehaviour {
        [SerializeField, Range(0, 1.1f)] float _value = 1;
        public float Value => _value;

        protected override void ApplyMain(SoundPlayer player) {
            player.SetReverbZoneMix(_value);
        }
    }
}