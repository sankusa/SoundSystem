using System;
using UnityEngine;

namespace SoundSystem.ClipResolvers {
    [Serializable]
    public class Clip : IClipResolver {
        [SerializeField] ClipSlot _clip;

        public void SetClip(SoundPlayer player) {
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