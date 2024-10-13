using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    [System.Serializable]
    public class SoundWithKeyDictionary : Dictionary<string, SoundWithKey>, ISerializationCallbackReceiver {
        [SerializeField] List<SoundWithKey> _list = new();

        public void OnAfterDeserialize() {
            Clear();
            foreach (SoundWithKey item in _list) {
                this[item.Key] = item;
            }
        }

        public void OnBeforeSerialize() {
            // _list = new(Count);
            // foreach (var keyValuePair in this) {
            //     _list.Add(keyValuePair.Value);
            // }
        }
    }
}