using System;
using UnityEngine;
using UnityEngine.Events;

namespace SoundSystem {
    public class Play : ISoundCommand , ITransformBinder {
        public enum DuplicationHandling {
            None,
            NotPlay,
            Restart,
        }

        [SerializeField, SoundPlayerGroupKey] string _groupKey;
        [SerializeField] SoundSelector _sound;
        [SerializeField] DuplicationHandling _duplicationHandling;
        [SerializeField] bool _fromTransformPosition;
        [SerializeField] UnityEvent _onComplete;

        Transform _transform;

        public void BindTransform(Transform transform) {
            _transform = transform;
        }

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

            if (player != null && _fromTransformPosition) {
                if (_transform == null) {
                    Debug.LogWarning("Transform is null. Please call BindTransform() to set the Transform.");
                }
                else {
                    player.SetPosition(_transform.position);
                }
            }
        }
    }
}