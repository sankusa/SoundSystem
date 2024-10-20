using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SoundSystem {
    public partial class SoundPlayer {
        readonly AudioSource _audioSource;
        public AudioClip Clip => _audioSource.clip;
        public bool IsUsing => _audioSource.clip != null;
        public bool IsPlaying => _audioSource.isPlaying;
        public bool IsStopped => _audioSource.isPlaying == false && IsPaused == false;
        bool _isPaused;
        public bool IsPaused => _isPaused;
        public int TimeSamples {
            get => _audioSource.timeSamples;
            set => _audioSource.timeSamples = value;
        }
        public float Time => _audioSource.clip == null ? 0 : (float)_audioSource.timeSamples / _audioSource.clip.frequency;
        public bool Loop => _audioSource.loop;

        // ・AudioSource.isPlayingはポーズ時にfalseになってしまう
        // ・AudioSourceは再生位置が終端に達した場合、SoundPlayerが検知する前に自律的に状態をリセットしてしまう場合がある
        // 上記理由からフラグを別途定義
        bool _isPlayStarted;
        public bool IsPlayStarted => _isPlayStarted;

        Sound _sound;
        public Sound Sound => _sound;

        AudioUnit _audioUnit;
        public AudioUnit AudioUnit => _audioUnit;

        readonly List<Volume> _volumes;
        public IReadOnlyList<Volume> Volumes => _volumes;

        readonly Fader _fadeVolume = new();

        SoundPlayerGroupSetting _groupSetting;

        UnityEvent _onComplete;
        public UnityEvent OnComplete => _onComplete;

        public SoundPlayer(GameObject parentObject, SoundPlayerGroupSetting groupSetting, List<Volume> volumes) {
            GameObject gameObject = new("Player");
            gameObject.transform.SetParent(parentObject.transform);
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource = audioSource;

            _audioSource.outputAudioMixerGroup = groupSetting.OutputAudioMixerGroup;
            _groupSetting = groupSetting;

            _volumes = volumes;
        }

        public SoundPlayer SetAudioUnit(AudioUnit audioUnit) {
            if (audioUnit == null) {
                Debug.LogWarning($"{nameof(AudioUnit)} is null");
                return this;
            }

            _audioUnit = audioUnit;
            _audioSource.clip = audioUnit.Clip;
            return this;
        }

        public SoundPlayer SetSound(Sound sound) {
            if (sound == null) {
                Debug.LogWarning($"{nameof(Sound)} is null");
                return this;
            }

            _sound = sound;
            return SetAudioUnit(_sound.AudioUnit);
        }

        public SoundPlayer SetSoundByKey(string soundKey) {
            if (soundKey == null) {
                Debug.LogWarning($"{nameof(soundKey)} is null");
                return this;
            }

            Sound sound = SoundCache.Instance.FindSound(soundKey);
            return SetSound(sound);
        }

        public SoundPlayer SetLoop(bool loop) {
            _audioSource.loop = loop;
            return this;
        }

        public SoundPlayer AddOnComplete(UnityAction onComplete) {
            if (onComplete != null) {
                _onComplete ??= new();
                _onComplete.AddListener(onComplete);
            }
            return this;
        }

        public SoundPlayer SetFadeWithFrom(float from, float to, float? duration = null, UnityAction onComplete = null) {
            duration ??= _groupSetting.DefaultFadeDuration;
            _fadeVolume.Fade(from, to, duration.Value, onComplete);
            return this;
        }

        public SoundPlayer SetFade(float to, float? duration = null, UnityAction onComplete = null) {
            duration ??= _groupSetting.DefaultFadeDuration;
            _fadeVolume.Fade(to, duration.Value, onComplete);
            return this;
        }

        public SoundPlayer SetFadeIn(float? duration = null, UnityAction onComplete = null) {
            duration ??= _groupSetting.DefaultFadeDuration;
            _fadeVolume.Fade(1, duration.Value, onComplete);
            return this;
        }

        public SoundPlayer SetFadeOut(float? duration = null, UnityAction onComplete = null) {
            duration ??= _groupSetting.DefaultFadeDuration;
            _fadeVolume.Fade(0, duration.Value, onComplete);
            return this;
        }

        public void SetTime(float time) {
            _audioSource.timeSamples = _audioSource.clip == null ? 0 : (int)(time * _audioSource.clip.frequency);
        }

        public SoundPlayer Play() {
            if (_audioSource.clip == null) {
                Debug.LogWarning($"{nameof(AudioClip)} is null");
                return this;
            }

            if (_fadeVolume.IsFading == false) {
                _fadeVolume.Value = 1;
            }

            ClampTime();
            UpdateVolume();
            UpdateMute();
            UpdatePitch();

            _audioSource.Play();
            _isPlayStarted = true;
            return this;
        }

        public void Stop() {
            Reset();
        }

        public void Complete() {
            UnityEvent onComplete = _onComplete;
            Stop();
            onComplete?.Invoke();
        }

        public void Pause() {
            _audioSource.Pause();
            _isPaused = true;
        }

        public void Resume() {
            _audioSource.UnPause();
            _isPaused = false;
        }

        public void Reset() {
            _sound = null;
            _audioUnit = null;
            _fadeVolume.Clear();
            _onComplete = null;
            _isPlayStarted = false;
            _isPaused = false;
            
            // Reset AudioSource
            _audioSource.Stop();
            _audioSource.clip = null;
            _audioSource.pitch = 1;
            _audioSource.loop = _groupSetting.DefaultLoop;
            _audioSource.timeSamples = 0;
        }

        public void Update(float deltaTime) {
            if (IsUsing == false) return;
            if (_isPlayStarted == false) return;
            // AudioSourceが自律的に停止した場合
            if (IsStopped) {
                RestartOrComplete();
                return;
            }
            if (_audioSource.isPlaying == false) return;
            _fadeVolume.Update(deltaTime);
            ClampTime();
            UpdateVolume();
            UpdateMute();
            UpdatePitch();
            CheckAndHandleEndOfAudio();
        }

        void ClampTime() {
            PlayRange playRange = _audioUnit.PlayRange;
            if (playRange.Enable) {
                if (_audioSource.timeSamples < playRange.FromSamples) {
                    _audioSource.timeSamples = playRange.FromSamples;
                }
            }
        }

        void UpdateVolume() {
            float volume = 1f;
            if (_audioUnit != null) {
                volume *= _audioUnit.GetVolumeMultiplier(_audioSource.time);
            }
            volume *= Volume.MultiplyVolume(_volumes);
            volume *= _fadeVolume.Value;

            _audioSource.volume = volume;
        }

        void UpdateMute() {
            _audioSource.mute = Volume.SumMute(_volumes);
        }

        void UpdatePitch() {
            _audioSource.pitch = _audioUnit.GetPitchMultiplier();
        }

        void CheckAndHandleEndOfAudio() {
            if (_audioUnit.PlayRange.Enable) {
                if (_audioSource.timeSamples >= _audioUnit.PlayRange.ToSamples) {
                    RestartOrComplete();
                }
            }
            else {
                if (_audioSource.timeSamples >= _audioSource.clip.samples) {
                    RestartOrComplete();
                }
            }
        }

        void RestartOrComplete() {
            if (Loop) {
                if (_audioUnit.PlayRange.Enable) {
                    _audioSource.timeSamples = _audioUnit.PlayRange.FromSamples;
                }
                else {
                    _audioSource.timeSamples = 0;
                }
            }
            else {
                Complete();
            }
        }

        public Volume FindVolume(string volumeKey) {
            return _volumes.Find(x => x.Key == volumeKey);
        }
    }
}