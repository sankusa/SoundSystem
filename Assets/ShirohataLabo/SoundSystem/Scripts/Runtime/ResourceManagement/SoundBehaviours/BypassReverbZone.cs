using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(BypassReverbZone), 502)]
    public class BypassReverbZone : SoundBehaviour {
        [SerializeField] bool _value = true;
        public bool Value {
            get => _value;
            set => _value = value;
        }

        protected override void OnUpdateIfActive(SoundPlayer player, float deltaTime) {
            SetBypassReverbZones(player, _value);
        }
    }
}