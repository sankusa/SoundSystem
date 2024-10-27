using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(BypassReverbZone), 502)]
    public class BypassReverbZone : SoundBehaviour {
        [SerializeField] bool _value = true;
        public bool Value => _value;

        protected override void ApplyMain(SoundPlayer player) {
            player.SetBypassReverbZones(_value);
        }
    }
}