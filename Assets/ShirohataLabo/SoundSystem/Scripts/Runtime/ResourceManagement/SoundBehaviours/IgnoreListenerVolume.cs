using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(IgnoreListenerVolume), 600)]
    public class IgnoreListenerVolume : SoundBehaviour {
        [SerializeField] bool _value;
        public bool Value => _value;

        protected override void ApplyMain(SoundPlayer player) {
            player.SetIgnoreListenerVolume(_value);
        }
    }
}