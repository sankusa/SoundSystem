using System.Collections.Generic;
using UnityEngine;

namespace SoundSystem {
    [CreateAssetMenu(fileName = nameof(SoundContainer), menuName = nameof(SoundSystem) + "/" + nameof(SoundContainer))]
    public class SoundContainer : ScriptableObject {
        [SerializeField] SoundWithKeyDictionary _soundDic = new();
        public SoundWithKeyDictionary SoundDic => _soundDic;

        public Sound FindSoundByAudioUnitName(string audioUnitName) {
            foreach (SoundWithKey data in _soundDic.Values) {
                if (data.Sound.AudioUnit.name == audioUnitName) return data.Sound;
            }
            return null;
        }

        public Sound FindSoundByKey(string key) {
            _soundDic.TryGetValue(key, out SoundWithKey soundWithKey);
            return soundWithKey?.Sound;
        }
    }
}