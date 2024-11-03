using UnityEngine;

namespace SoundSystem {
    [DefaultExecutionOrder(-50)]
    public class BaseSoundBehaviourRegistrar : MonoBehaviour {
        [SerializeField] bool _autoRegister = true;
        [SerializeField, SoundPlayerGroupKey] string _soundPlayerGroup;
        [SerializeField] SoundBehaviourList _soundBehaviourList;

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
                .BaseSoundBehaviours
                .Add(_soundBehaviourList);
        }

        public void Deregister() {
            SoundManager
                .Instance
                .FindPlayerGroup(_soundPlayerGroup)
                .Status
                .BaseSoundBehaviours
                .Remove(_soundBehaviourList);
        }
    }
}