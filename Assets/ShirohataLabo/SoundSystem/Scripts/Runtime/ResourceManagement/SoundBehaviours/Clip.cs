using System;
using UnityEngine;

namespace SoundSystem.SoundBehaviours {
    [Serializable]
    [SoundBehaviourMenuItem(nameof(Clip), -1000)]
    public class Clip : SoundBehaviour {
        [SerializeField] ClipSlot _clip;

        protected override void ApplyMain(SoundPlayer player) {
            if (_clip.HasClip() == false) return;
            switch (_clip.Type) {
                case ClipSlot.SlotType.AudioClip:
                    player.SetAudioClip(_clip.AudioClip);
                    break;
                case ClipSlot.SlotType.CustomClip:
                    player.SetCustomClip(_clip.CustomClip);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Invalid {typeof(ClipSlot.SlotType).Name}");
            }
        }
    }
}