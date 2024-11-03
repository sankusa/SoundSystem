using System;
using UnityEngine;
using UnityEngine.Events;

namespace SoundSystem.SoundCommands {
    public class Play : ISoundCommand {
        public enum DuplicationHandling {
            None,
            NotPlay,
            Restart,
        }

        [SerializeField, SoundPlayerGroupKey] string _groupKey;
        [SerializeField] SoundSelector _sound;
        [SerializeField] DuplicationHandling _duplicationHandling;
        [SerializeField] Transform _spawnPoint;
        [SerializeField] bool _followSpawnPoint;
        [SerializeField] UnityEvent _onComplete;

        public void Execute() {
            SoundPlayerGroup playerGroup = SoundManager.Instance.FindPlayerGroup(_groupKey);
            SoundPlayer player = null;
            
            Action onComplete = _onComplete.GetPersistentEventCount() == 0 ? null : () => _onComplete.Invoke();

            if (_duplicationHandling == DuplicationHandling.None) {
                player = playerGroup.Play(_sound.Resolve(), onComplete);
            }
            else if (_duplicationHandling == DuplicationHandling.NotPlay) {
                player = playerGroup.PlayIfNotPlaying(_sound.Resolve(), onComplete);
            }
            else if (_duplicationHandling == DuplicationHandling.Restart) {
                player = playerGroup.PlayAsRestart(_sound.Resolve(), onComplete);
            }

            if (player != null && _spawnPoint != null) {
                if (_followSpawnPoint) {
                    player.SetSpawnPoint(_spawnPoint);
                }
                else {
                    player.SetPosition(_spawnPoint.position);
                }
            }
        }
    }
}