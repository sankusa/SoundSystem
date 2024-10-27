using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem("AudioSource/VolumeRolloff", 152)]
    public class VolumeRolloff : SoundBehaviour {
        [SerializeField] AudioRolloffMode _mode = AudioRolloffMode.Logarithmic;
        public AudioRolloffMode Mode => _mode;

        protected override void ApplyMain(SoundPlayer player) {
            player.SetVolumeRollof(_mode);
        }
    }
}