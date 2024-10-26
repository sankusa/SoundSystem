using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

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

#if UNITY_EDITOR
        public static SerializedProperty GetListProp(SerializedProperty soundWithKeyDictionaryProp) {
            return soundWithKeyDictionaryProp.FindPropertyRelative(nameof(_list));
        }
#endif
    }
}