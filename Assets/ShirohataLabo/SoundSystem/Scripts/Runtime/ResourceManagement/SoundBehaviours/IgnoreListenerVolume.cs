using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem("AudioSource/IgnoreListenerVolume", 200)]
    public class IgnoreListenerVolume : SoundBehaviour {
        [SerializeField] bool _value;
        public bool Value => _value;

        protected override void ApplyMain(SoundPlayer player) {
            player.SetIgnoreListenerVolume(_value);
        }
    }
}