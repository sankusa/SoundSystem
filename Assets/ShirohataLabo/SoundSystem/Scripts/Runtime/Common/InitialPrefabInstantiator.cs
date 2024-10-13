using UnityEngine;
using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SoundSystem {
    [CreateAssetMenu(fileName = nameof(InitialPrefabInstantiator), menuName = nameof(SoundSystem) + "/Develop/" + nameof(InitialPrefabInstantiator))]
    public class InitialPrefabInstantiator : ScriptableObject {
        [Serializable]
        class PrefabInfo {
            [SerializeField] GameObject _prefab;
            public GameObject Prefab => _prefab;

            [SerializeField] bool _instantiate;
            public bool Instantiate => _instantiate;
        }

        [SerializeField] List<PrefabInfo> _prefabInfo;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void InstantiateCoverCanvas() {
            // Resouorcesからロード
            InitialPrefabInstantiator instance = Resources.Load<InitialPrefabInstantiator>(nameof(InitialPrefabInstantiator));
            if (instance == null) {
                Debug.LogError($"\"Resources/{nameof(InitialPrefabInstantiator)}\" not found");
                return;
            }
            // プレハブをInstantiate
            foreach(PrefabInfo prefabInfo in instance._prefabInfo) {
                if(prefabInfo.Instantiate == false) continue;
                if(prefabInfo.Prefab == null) continue;
                GameObject obj = Instantiate(prefabInfo.Prefab);
                DontDestroyOnLoad(obj);
            }
            // アンロード
            Resources.UnloadAsset(instance);
        }

#if UNITY_EDITOR
        // インスペクタから変更した値が再生モード移行時に元に戻る事象への対応
        // 変更があった場合、そのフレームで即時保存する
        [CustomEditor(typeof(InitialPrefabInstantiator))]
        public class CoverCanvasPlacerIsnpector : Editor {
            public override void OnInspectorGUI() {
                base.OnInspectorGUI();
                AssetDatabase.SaveAssetIfDirty(target);
            }
        }
#endif
    }
}