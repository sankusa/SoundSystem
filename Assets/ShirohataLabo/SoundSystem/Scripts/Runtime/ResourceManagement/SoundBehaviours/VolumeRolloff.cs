using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(VolumeRolloff), 552)]
    public class VolumeRolloff : SoundBehaviour {
        [SerializeField] AudioRolloffMode _mode = AudioRolloffMode.Logarithmic;
        public AudioRolloffMode Mode => _mode;

        protected override void OnUpdateIfActive(SoundPlayer player, float deltaTime) {
            SetVolumeRollof(player, _mode);
        }
    }
}