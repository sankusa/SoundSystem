using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(IgnoreListenerPause), 601)]
    public class IgnoreListenerPause : SoundBehaviour {
        [SerializeField] bool _value;
        public bool Value => _value;

        protected override void OnUpdateIfActive(SoundPlayer player, float deltaTime) {
            SetIgnoreListenerPause(player, _value);
        }
    }
}