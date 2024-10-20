using UnityEngine;
using UnityEngine.Events;

namespace SoundSystem {
    public class Play : ISoundCommand {
        public enum DuplicationHandling {
            None,
            NotPlay,
            Restart,
        }

        [SerializeField, SoundPlayerGroupKey] string _groupKey;
        [SerializeField] SoundSelector _sound;
        [SerializeField] DuplicationHandling _duplicationHandling;
        [SerializeField] UnityEvent _onComplete;

        public void Execute() {
            SoundPlayerGroup playerGroup = SoundManager.Instance.FindPlayerGroup(_groupKey);
            if (_duplicationHandling == DuplicationHandling.None) {
                playerGroup.Play(_sound.Resolve(), () => _onComplete?.Invoke());
            }
            else if (_duplicationHandling == DuplicationHandling.NotPlay) {
                playerGroup.PlayIfNotPlaying(_sound.Resolve(), () => _onComplete?.Invoke());
            }
            else if (_duplicationHandling == DuplicationHandling.Restart) {
                playerGroup.PlayAsRestart(_sound.Resolve(), () => _onComplete?.Invoke());
            }
        }
    }
}