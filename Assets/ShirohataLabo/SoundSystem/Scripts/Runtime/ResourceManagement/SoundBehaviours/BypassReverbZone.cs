using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem("AudioSource/BypassReverbZone", 102)]
    public class BypassReverbZone : SoundBehaviour {
        [SerializeField] bool _value = true;
        public bool Value => _value;

        protected override void ApplyMain(SoundPlayer player) {
            player.SetBypassReverbZones(_value);
        }
    }
}