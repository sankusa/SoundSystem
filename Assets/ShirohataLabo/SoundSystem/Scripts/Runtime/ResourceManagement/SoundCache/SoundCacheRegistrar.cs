using System.Collections.Generic;
using UnityEngine;

namespace SoundSystem {
    [DefaultExecutionOrder(-100)]
    [AddComponentMenu(nameof(SoundSystem) + "/" + nameof(SoundCacheRegistrar))]
    public class SoundCacheRegistrar : MonoBehaviour {
        [SerializeField] List<SoundContainer> _containers;

        void Awake() {
            SoundCache.Instance.AddContainers(_containers);
        }

        void OnDestroy() {
            SoundCache.Instance.RemoveContainers(_containers);
        }
    }
}