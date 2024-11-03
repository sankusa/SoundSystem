using System;
using System.Collections.Generic;
using UnityEngine;

namespace SoundSystem {
    public partial class SoundPlayer {
        readonly GameObject _gameObject;
        public GameObject GameObject => _gameObject;
        readonly Transform _transform;
        readonly AudioSource _audioSource;
        internal AudioSourceAccessor AudioSourceAccessor { get; }
        public AudioClip AudioClip => _audioSource.clip;
        public bool IsPlayable => _audioSource.clip != null;
        public bool IsPlaying => _audioSource.isPlaying;
        public bool IsStopped => _audioSource.isPlaying == false && IsPaused == false;
        public bool IsPaused { get; private set; }
        public int TimeSamples => _audioSource.timeSamples;
        public float Time => _audioSource.clip == null ? 0 : (float)_audioSource.timeSamples / _audioSource.clip.frequency;
        public bool Loop => _audioSource.loop;

        // ・AudioSource.isPlayingはポーズ時にfalseになってしまう
        // ・AudioSourceは再生位置が終端に達した場合、SoundPlayerが検知する前に自律的に状態をリセットしてしまう場合がある
        // 上記理由からフラグを別途定義
        public bool IsPlayStarted { get; private set; }

        // SoundPlayerGroupが管理する使用状態
        public bool IsUsing { get; private set; }

        Sound _sound;
        public Sound Sound => _sound;

        CustomClip _customClip;
        public CustomClip CustomClip => _customClip;

        readonly List<Volume> _volumes;
        public IReadOnlyList<Volume> Volumes => _volumes;

        readonly Fader _fadeVolume = new();

        readonly SoundPlayerGroupStatus _groupStatus;

        event Action _onComplete;

        Transform _spawnPoint;

        List<SoundBehaviour> SoundBehaviours { get; set; } = new List<SoundBehaviour>();
        public Dictionary<object, object> PlayScopeStatusDictionary { get; private set; } = new Dictionary<object, object>();

        public SoundPlayer(GameObject parentObject, SoundPlayerGroupStatus groupStatus, List<Volume> volumes) {
            _gameObject = new("Player");
            _transform = _gameObject.transform;
            _transform.SetParent(parentObject.transform);
            AudioSource audioSource = _gameObject.AddComponent<AudioSource>();
            _audioSource = audioSource;

            _audioSource.outputAudioMixerGroup = groupStatus.Setting.OutputAudioMixerGroup;
            _groupStatus = groupStatus;

            _volumes = volumes;

            AudioSourceAccessor = new AudioSourceAccessor(_audioSource);
            AudioSourceAccessor.Reset();
            AudioSourceAccessor.Apply();
        }

        public void StartUsing() {
            IsUsing = true;
        }

        public SoundPlayer SetAudioClip(AudioClip audioClip) {
            if (audioClip == null) {
                Debug.LogWarning($"{nameof(AudioClip)} is null");
                return this;
            }

            _audioSource.clip = audioClip;
            return this;
        }

        public SoundPlayer SetCustomClip(CustomClip customClip) {
            if (customClip == null) {
                Debug.LogWarning($"{nameof(CustomClip)} is null");
                return this;
            }

            _customClip = customClip;
            _audioSource.clip = customClip.AudioClip;
            return this;
        }

        public SoundPlayer SetSound(Sound sound) {
            if (sound == null) {
                Debug.LogWarning($"{nameof(Sound)} is null");
                return this;
            }

            // 元々設定されているSoundに関するデータの内、消さないと残るものを消す
            if (_sound != null) {
                RemoveSoundBehaviour(_sound.SoundBehaviourList);
            }

            _sound = sound;
            _sound.Clip.SetClip(this);
            AddSoundBehaviour(_sound.SoundBehaviourList);
            return this;
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

        public SoundPlayer SetPosition(Vector3 position) {
            _transform.localPosition = position;
            return this;
        }

        public SoundPlayer SetSpawnPoint(Transform spawnPoint) {
            _spawnPoint = spawnPoint;
            return SetPosition(_spawnPoint.position);
        }

        public SoundPlayer AddOnComplete(Action onComplete) {
            if (onComplete != null) {
                _onComplete += onComplete;
            }
            return this;
        }

        public SoundPlayer SetFadeWithFrom(float from, float to, float? duration = null, Action onComplete = null) {
            duration ??= _groupStatus.Setting.DefaultFadeDuration;
            _fadeVolume.Fade(from, to, duration.Value, onComplete);
            return this;
        }

        public SoundPlayer SetFade(float to, float? duration = null, Action onComplete = null) {
            duration ??= _groupStatus.Setting.DefaultFadeDuration;
            _fadeVolume.Fade(to, duration.Value, onComplete);
            return this;
        }

        public SoundPlayer SetFadeIn(float? duration = null, Action onComplete = null) {
            duration ??= _groupStatus.Setting.DefaultFadeDuration;
            _fadeVolume.Fade(1, duration.Value, onComplete);
            return this;
        }

        public SoundPlayer SetFadeOut(float? duration = null, Action onComplete = null) {
            duration ??= _groupStatus.Setting.DefaultFadeDuration;
            _fadeVolume.Fade(0, duration.Value, onComplete);
            return this;
        }

        public SoundPlayer SetTimeSamples(int timeSamples) {
            _audioSource.timeSamples = timeSamples;
            return this;
        }

        public SoundPlayer SetTime(float time) {
            _audioSource.timeSamples = _audioSource.clip == null ? 0 : (int)(time * _audioSource.clip.frequency);
            return this;
        }

        public SoundPlayer AddSoundBehaviour(SoundBehaviour soundBehaviour) {
            SoundBehaviours.Add(soundBehaviour);
            return this;
        }

        public SoundPlayer RemoveSoundBehaviour(SoundBehaviour soundBehaviour) {
            SoundBehaviours.Remove(soundBehaviour);
            return this;
        }

        void UpdateBehaviours(float deltaTime) {
            // Reset
            ChorusFilterAccessor?.Reset();
            DistortionFilterAccessor?.Reset();
            EchoFilterAccessor?.Reset();
            HighPassFilterAccessor?.Reset();
            LowPassFilterAccessor?.Reset();
            ReverbFilterAccessor?.Reset();
            ReverbZoneAccessor?.Reset();
            AudioSourceAccessor.Reset();

            // Update Volume
            if (_customClip != null) {
                AudioSourceAccessor.Volume *= _customClip.GetVolumeMultiplier(_audioSource.time);
            }
            AudioSourceAccessor.Volume *= Volume.MultiplyVolume(_volumes);
            AudioSourceAccessor.Volume *= _fadeVolume.Value;
            // Update Mute
            AudioSourceAccessor.Mute |= Volume.SumMute(_volumes);
            // Update Pitch
            if (_customClip != null) {
                AudioSourceAccessor.Pitch *= _customClip.GetPitchMultiplier();
            }

            // SoundBehaviour.OnUpdate
            foreach (SoundBehaviour behaviour in _groupStatus.BaseSoundBehaviours) {
                behaviour.OnUpdate(this, deltaTime);
            }
            foreach (SoundBehaviour behaviour in SoundBehaviours) {
                behaviour.OnUpdate(this, deltaTime);
            }

            // Apply
            ChorusFilterAccessor?.ApplyIfChanged();
            DistortionFilterAccessor?.ApplyIfChanged();
            EchoFilterAccessor?.ApplyIfChanged();
            HighPassFilterAccessor?.ApplyIfChanged();
            LowPassFilterAccessor?.ApplyIfChanged();
            ReverbFilterAccessor?.ApplyIfChanged();
            ReverbZoneAccessor?.ApplyIfChanged();
            AudioSourceAccessor.ApplyIfChanged();
        }

        void Behaviours_OnReset() {
            foreach (SoundBehaviour behaviour in _groupStatus.BaseSoundBehaviours) {
                behaviour.OnReset(this);
            }
            foreach (SoundBehaviour behaviour in SoundBehaviours) {
                behaviour.OnReset(this);
            }
        }

        void Behaviours_OnPause() {
            foreach (SoundBehaviour behaviour in _groupStatus.BaseSoundBehaviours) {
                behaviour.OnPause(this);
            }
            foreach (SoundBehaviour behaviour in SoundBehaviours) {
                behaviour.OnPause(this);
            }
        }

        void Behaviours_OnResume() {
            foreach (SoundBehaviour behaviour in _groupStatus.BaseSoundBehaviours) {
                behaviour.OnResume(this);
            }
            foreach (SoundBehaviour behaviour in SoundBehaviours) {
                behaviour.OnResume(this);
            }
        }

        public SoundPlayer Play() {
            if (_audioSource.clip == null) {
                Debug.LogWarning($"{nameof(AudioClip)} is null");
                return this;
            }

            ClampTime();

            if (_fadeVolume.IsFading == false) {
                _fadeVolume.Value = 1;
            }

            UpdateBehaviours(0);
            UpdatePosition();

            _audioSource.Play();
            IsPlayStarted = true;
            return this;
        }

        public void Stop() {
            Reset();
        }

        public void Complete() {
            Action onComple = _onComplete;
            Stop();
            onComple?.Invoke();
        }

        public void Pause() {
            _audioSource.Pause();
            IsPaused = true;
            Behaviours_OnPause();
        }

        public void Resume() {
            _audioSource.UnPause();
            IsPaused = false;
            Behaviours_OnResume();
        }

        public void Reset() {
            Behaviours_OnReset();

            ChorusFilterAccessor?.Clear();
            DistortionFilterAccessor?.Clear();
            EchoFilterAccessor?.Clear();
            HighPassFilterAccessor?.Clear();
            LowPassFilterAccessor?.Clear();
            ReverbFilterAccessor?.Clear();
            ReverbZoneAccessor?.Clear();
            AudioSourceAccessor.Clear();

            PlayScopeStatusDictionary.Clear();

            _sound = null;
            _customClip = null;
            _fadeVolume.Clear();
            _onComplete = null;
            IsPlayStarted = false;
            IsPaused = false;
            _transform.localPosition = Vector3.zero;
            _spawnPoint = null;
            
            // Reset AudioSource
            _audioSource.Stop();
            _audioSource.clip = null;
            _audioSource.loop = _groupStatus.Setting.DefaultLoop;
            _audioSource.timeSamples = 0;

            IsUsing = false;
        }

        public void Update(float deltaTime) {
            if (IsPlayStarted == false) return;
            // AudioSourceが自律的に停止した場合
            if (IsStopped) {
                RestartOrComplete();
                return;
            }
            if (_audioSource.isPlaying == false) return;
            ClampTime();
            _fadeVolume.Update(deltaTime);
            // フェード完了コールバックでStopが呼ばれた場合、以降は処理しない
            if (IsPlayStarted == false) return;

            UpdateBehaviours(deltaTime);
            UpdatePosition();
            CheckAndHandleEndOfAudio();
        }

        void ClampTime() {
            if (_customClip != null) {
                PlayRange playRange = _customClip.PlayRange;
                if (playRange.Enable) {
                    if (_audioSource.timeSamples < playRange.FromSamples) {
                        _audioSource.timeSamples = playRange.FromSamples;
                    }
                }
            }
        }

        void UpdatePosition() {
            if (_spawnPoint == null) return;
            _transform.localPosition = _spawnPoint.position;
        }

        void CheckAndHandleEndOfAudio() {
            if (_customClip != null && _customClip.PlayRange.Enable) {
                if (_audioSource.timeSamples >= _customClip.PlayRange.ToSamples) {
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
                if (_customClip != null && _customClip.PlayRange.Enable) {
                    _audioSource.timeSamples = _customClip.PlayRange.FromSamples;
                }
                else {
                    _audioSource.timeSamples = 0;
                }
            }
            else {
                Complete();
            }
        }
    }
}