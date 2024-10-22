using UnityEngine;

namespace SoundSystem {
    public class SoundCommandExecuter : MonoBehaviour {
        [SerializeField] bool _executeOnAwake = true;
        [SerializeField] SelectableSoundCommand _soundCommand;

        void Awake() {
            if (_executeOnAwake) {
                Execute();
            }
        }
        
        public void Execute() {
            _soundCommand.Execute();
        }
    }
}