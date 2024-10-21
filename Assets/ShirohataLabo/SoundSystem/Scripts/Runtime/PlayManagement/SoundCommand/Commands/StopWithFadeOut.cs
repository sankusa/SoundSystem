using System;
using UnityEngine;
using UnityEngine.Events;

namespace SoundSystem {
    public class StopWithFadeOut : ISoundCommand {
        [SerializeField, SoundPlayerGroupKey] string _groupKey;
        [SerializeField] bool _setFadeDuration;
        [SerializeField] float _fadeDuration;
        [SerializeField] UnityEvent _onComplete;

        public void Execute() {
            Action onComplete = _onComplete.GetPersistentEventCount() == 0 ? null : () => _onComplete.Invoke();

            SoundManager
                .Instance
                .FindPlayerGroup(_groupKey)
                .StopWithFadeOut(
                    _setFadeDuration ? _fadeDuration: null,
                    onComplete
                );
        }
    }
}