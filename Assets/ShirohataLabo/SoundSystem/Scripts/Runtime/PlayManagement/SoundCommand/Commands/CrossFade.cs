using System;
using UnityEngine;
using UnityEngine.Events;

namespace SoundSystem {
    public class CrossFade : ISoundCommand {
        [SerializeField, SoundPlayerGroupKey] string _groupKey;
        [SerializeField] SoundSelector _sound;
        [SerializeField] bool _setFadeDuration;
        [SerializeField] float _fadeDuration;
        [SerializeField] UnityEvent _onComplete;
        [SerializeField] UnityEvent _onFadeComplete;

        public void Execute() {
            Action onComplete = _onComplete.GetPersistentEventCount() == 0 ? null : () => _onComplete.Invoke();
            Action onFadeComplete = _onFadeComplete.GetPersistentEventCount() == 0 ? null : () => _onFadeComplete.Invoke();

            SoundManager
                .Instance
                .FindPlayerGroup(_groupKey)
                .CrossFade(
                    _sound.Resolve(),
                    _setFadeDuration ? _fadeDuration : null,
                    onComplete,
                    onFadeComplete
                );
        }
    }
}