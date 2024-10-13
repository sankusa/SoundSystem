using UnityEngine;
using UnityEngine.Events;

namespace SoundSystem {
    public class Switch : ISoundCommand {
        [SerializeField, SoundPlayerGroupKey] string _groupKey;
        [SerializeField] SoundSelector _sound;
        [SerializeField] UnityEvent _onComplete;

        public void Execute() {
            SoundManager.Instance.FindPlayerGroup(_groupKey).Switch(_sound.Resolve(), () => _onComplete?.Invoke());
        }
    }
}