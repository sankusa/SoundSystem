using System;
using System.Collections.Generic;
using UnityEngine;

namespace SoundSystem.ClipResolvers {
    [Serializable]
    public class RandomClip : IClipResolver {
        [SerializeField] List<ClipSlot> _clips;

        public void SetClip(IAnyClipSettable obj) {
            if (_clips.Count == 0) return;
            int index = UnityEngine.Random.Range(0, _clips.Count);
            ClipSlot clip = _clips[index];
            clip.SetClip(obj);
        }
    }
}