using System;
using System.Collections.Generic;
using UnityEngine;

namespace SoundSystem.ClipResolvers {
    [Serializable]
    public class RandomClip : IClipResolver {
        [SerializeField] List<ClipSlot> _clips;

        public void SetClip(SoundPlayer player) {
            if (_clips.Count == 0) return;
            int index = UnityEngine.Random.Range(0, _clips.Count);
            ClipSlot clip = _clips[index];
            if (clip.HasClip() == false) return;
            switch (clip.Type) {
                case ClipSlot.SlotType.AudioClip:
                    player.SetAudioClip(clip.AudioClip);
                    break;
                case ClipSlot.SlotType.CustomClip:
                    player.SetCustomClip(clip.CustomClip);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Invalid {typeof(ClipSlot.SlotType).Name}");
            }
        }
    }
}