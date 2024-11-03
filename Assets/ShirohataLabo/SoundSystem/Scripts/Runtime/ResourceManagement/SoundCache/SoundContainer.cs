using System.Collections.Generic;
using UnityEngine;

namespace SoundSystem {
    [CreateAssetMenu(fileName = nameof(SoundContainer), menuName = nameof(SoundSystem) + "/" + nameof(SoundContainer))]
    public class SoundContainer : ScriptableObject {
        [SerializeField] SoundWithKeyDictionary _soundDic = new();
        public SoundWithKeyDictionary SoundDic => _soundDic;

        public Sound FindSoundByKey(string key) {
            _soundDic.TryGetValue(key, out SoundWithKey soundWithKey);
            return soundWithKey?.Sound;
        }
    }
}