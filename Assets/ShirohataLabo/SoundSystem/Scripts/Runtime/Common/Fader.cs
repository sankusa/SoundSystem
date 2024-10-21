using System;
using UnityEngine;
using UnityEngine.Events;

namespace SoundSystem {
    public class Fader {
        public float Value { get; set; }
        public bool IsFading { get; private set;}

        float _fadeFrom;
        float _fadeTo;
        float _fadeDuration;
        float _fadeTimer;

        event Action _onComplete;
        
        public void Fade(float from, float to, float duration, Action onComplete = null) {
            Clear();
            Value = from;
            IsFading = true;
            _fadeFrom = from;
            _fadeTo = to;
            _fadeDuration = duration;
            _fadeTimer = 0;
            if (onComplete != null) {
                _onComplete += onComplete;
            }
        }

        public void Fade(float to, float duration, Action onComplete = null) {
            Fade(Value, to, duration, onComplete);
        }

        public void Update(float deltaTime) {
            if (IsFading) {
                _fadeTimer += deltaTime;
                if (_fadeDuration == 0) {
                    Value = _fadeTo;
                }
                else {
                    Value = Mathf.Lerp(_fadeFrom, _fadeTo, _fadeTimer / _fadeDuration);
                }
                if(_fadeTimer >= _fadeDuration) {
                    Action onComplete = _onComplete;
                    ClearWithoutValue();
                    onComplete?.Invoke();
                }
            }
        }

        public void Clear() {
            ClearWithoutValue();
            Value = 0;
        }

        void ClearWithoutValue() {
            IsFading = false;
            _fadeFrom = 0;
            _fadeTo = 0;
            _fadeDuration = 0;
            _fadeTimer = 0;
            _onComplete = null;
        }
    }
}