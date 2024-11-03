using System;
using UnityEngine;

namespace SoundSystem.ClipResolvers {
    [Serializable]
    public class Clip : IClipResolver {
        [SerializeField] ClipSlot _clip;

        public void SetClip(IAnyClipSettable obj) {
            _clip.SetClip(obj);
        }
    }
}