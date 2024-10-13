using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SoundSystem {
    public partial class SoundPlayer {
        readonly AudioSource _audioSource;
        
        Sound _sound;
        public Sound Sound => _sound;

        AudioUnit _audioUnit;
        public AudioUnit AudioUnit => _audioUnit;

        readonly List<Volume> _volumes;
        public IReadOnlyList<Volume> Volumes => _volumes;

        readonly Fader _fadeVolume = new();

        public bool IsUsing => _audioSource.clip != null;

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

            Sound sound = SoundCache.Instance.FindSoundByKey(soundKey);
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

        public SoundPlayer Play() {
            if (_audioSource.clip == null) {
                Debug.LogWarning($"{nameof(AudioClip)} is null");
                return this;
            }

            if (_fadeVolume.IsFading == false) {
                _fadeVolume.Value = 1;
            }

            _audioSource.time = _audioUnit.Start;

            UpdateVolume();
            UpdateMute();
            UpdatePitch();

            _audioSource.Play();
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
        }

        public void Resume() {
            _audioSource.UnPause();
        }

        public void Reset() {
            _sound = null;
            _audioUnit = null;
            _fadeVolume.Clear();
            _onComplete = null;
            
            // Reset AudioSource
            _audioSource.Stop();
            _audioSource.clip = null;
            _audioSource.pitch = 1;
            _audioSource.loop = _groupSetting.DefaultLoop;
            _audioSource.time = 0;
        }

        public void Update(float deltaTime) {
            if (IsUsing == false) return;
            if (_audioSource.isPlaying == false) return;
            _fadeVolume.Update(deltaTime);
            ClampTime();
            UpdateVolume();
            UpdateMute();
            UpdatePitch();
            CheckAndHandleEndOfAudio();
        }

        void ClampTime() {
            if (_audioSource.time < _audioUnit.Start) {
                _audioSource.time = _audioUnit.Start;
            }
            // else if (_audioSource.time > _audioUnit.End) {
            //     _audioSource.time = _audioUnit.End;Debug.Log(_audioSource.time + "*" + _audioUnit.End);
            // }
        }

        void UpdateVolume() {
            float volume = 1f;
            if (_audioUnit != null) {
                volume *= _audioUnit.GetCurrentVolume(_audioSource.time);
            }
            volume *= Volume.MultiplyVolume(_volumes);
            volume *= _fadeVolume.Value;

            _audioSource.volume = volume;
        }

        void UpdateMute() {
            _audioSource.mute = Volume.SumMute(_volumes);
        }

        void UpdatePitch() {
            _audioSource.pitch = _audioUnit.Pitch;
        }

        void CheckAndHandleEndOfAudio() {
            if (_audioSource.time >= _audioUnit.End) {
                if (_audioSource.loop) {
                    _audioSource.time = _audioUnit.Start;
                }
                else {
                    Complete();
                }
            }
        }

        public Volume FindVolumeByKey(string volumeKey) {
            return _volumes.Find(x => x.Key == volumeKey);
        }
    }
}