using System;
using UnityEngine;

namespace SoundSystem {
    [Serializable]
    public class SoundSelector {
        public enum SelectType {
            Sound,
            SoundKey,
        }

        [SerializeField] SelectType _selectType;
        [SerializeField] Sound _sound;
        [SerializeField, SoundKey] string _soundKey;

        public Sound Resolve() {
            if (_selectType == SelectType.Sound) {
                return _sound;
            }
            else if (_selectType == SelectType.SoundKey) {
                return SoundCache.Instance.FindSound(_soundKey);
            }
            throw new InvalidOperationException("Invalid SelectType");
        }
    }
}