using UnityEngine;
using UnityEngine.Events;

namespace SoundSystem {
    public class PlayWithFadeIn : ISoundCommand {
        [SerializeField, SoundPlayerGroupKey] string _groupKey;
        [SerializeField] SoundSelector _sound;
        [SerializeField] bool _setFadeDuration;
        [SerializeField] float _fadeDuration;
        [SerializeField] UnityEvent _onComplete;
        [SerializeField] UnityEvent _onFadeComplete;

        public void Execute() {
            SoundManager
                .Instance
                .FindPlayerGroup(_groupKey)
                .PlayWithFadeIn(
                    _sound.Resolve(),
                    _setFadeDuration ? _fadeDuration: null,
                    () => _onComplete?.Invoke(),
                    () => _onFadeComplete?.Invoke()
                );
        }
    }
}