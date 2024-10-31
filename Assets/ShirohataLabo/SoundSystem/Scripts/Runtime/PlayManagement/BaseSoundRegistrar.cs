using UnityEngine;

namespace SoundSystem {
    [DefaultExecutionOrder(-50)]
    public class BaseSoundRegistrar : MonoBehaviour {
        [SerializeField] bool _autoRegister = true;
        [SerializeField, SoundPlayerGroupKey] string _soundPlayerGroup;
        [SerializeField] SoundSelector _sound;

        void Awake() {
            if (_autoRegister) Register();
        }

        void OnDestroy() {
            if (_autoRegister) Deregister();
        }

        public void Register() {
            SoundManager
                .Instance
                .FindPlayerGroup(_soundPlayerGroup)
                .Status
                .BaseSounds
                .Add(_sound.Resolve());
        }

        public void Deregister() {
            SoundManager
                .Instance
                .FindPlayerGroup(_soundPlayerGroup)
                .Status
                .BaseSounds
                .Remove(_sound.Resolve());
        }
    }
}