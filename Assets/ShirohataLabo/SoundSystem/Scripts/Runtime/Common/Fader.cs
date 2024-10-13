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

        readonly UnityEvent _onComplete = new();
        
        public void Fade(float from, float to, float duration, UnityAction onComplete = null) {
            Clear();
            Value = from;
            IsFading = true;
            _fadeFrom = from;
            _fadeTo = to;
            _fadeDuration = duration;
            _fadeTimer = 0;
            if (onComplete != null) {
                _onComplete.AddListener(onComplete);
            }
        }

        public void Fade(float to, float duration, UnityAction onComplete = null) {
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
                    _onComplete.Invoke();
                    Clear();
                }
            }
        }

        public void Clear() {
            Value = 0;
            IsFading = false;
            _fadeFrom = 0;
            _fadeTo = 0;
            _fadeDuration = 0;
            _fadeTimer = 0;
            _onComplete.RemoveAllListeners();
        }
    }
}