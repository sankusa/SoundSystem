using UnityEngine;

namespace SoundSystem {
    public class SoundCommandExecuter : MonoBehaviour {
        [SerializeField] bool _executeOnAwake = true;
        [SerializeField] SelectableSoundCommand _soundCommand;

        void Awake() {
            _soundCommand.BindTransformIfPossible(transform);
            if (_executeOnAwake) {
                Execute();
            }
        }
        
        public void Execute() {
            _soundCommand.Execute();
        }
    }
}