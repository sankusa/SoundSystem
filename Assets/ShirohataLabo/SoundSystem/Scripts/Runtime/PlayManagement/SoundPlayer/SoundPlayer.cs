using System;
using System.Collections.Generic;
using SoundSystem.CustomClipParameters;
using UnityEngine;

namespace SoundSystem {
    public partial class SoundPlayer : IAnyClipSettable {
        public GameObject GameObject { get; private set; }
        Transform Transform { get; }
        AudioSource AudioSource { get; }
        public AudioClip AudioClip => AudioSource.clip;
        public bool IsPlayable => AudioSource.clip != null;
        public bool IsPlaying => AudioSource.isPlaying;
        public bool IsStopped => AudioSource.isPlaying == false && IsPaused == false;
        public bool IsPaused { get; private set; }
        public int TimeSamples => AudioSource.timeSamples;
        public float Time => AudioSource.clip == null ? 0 : (float)AudioSource.timeSamples / AudioSource.clip.frequency;
        public bool Loop => AudioSource.loop;

        // ・AudioSource.isPlayingはポーズ時にfalseになってしまう
        // ・AudioSourceは再生位置が終端に達した場合、SoundPlayerが検知する前に自律的に状態をリセットしてしまう場合がある
        // 上記理由からフラグを別途定義
        public bool IsPlayStarted { get; private set; }

        // SoundPlayerGroupが管理する使用状態
        public bool IsUsing { get; private set; }

        public Sound Sound { get; private set; }
        public CustomClip CustomClip { get; private set; }

        readonly Fader _fadeVolume = new();

        readonly SoundPlayerGroupStatus _groupStatus;

        event Action _onComplete;

        Transform _spawnPoint;

        List<SoundBehaviour> SoundBehaviours { get; set; } = new List<SoundBehaviour>();
        public Dictionary<object, object> PlayScopeStatusDictionary { get; private set; } = new Dictionary<object, object>();

        public SoundPlayer(GameObject parentObject, SoundPlayerGroupStatus groupStatus) {
            GameObject = new GameObject("Player");
            Transform = GameObject.transform;
            Transform.SetParent(parentObject.transform);
            AudioSource audioSource = GameObject.AddComponent<AudioSource>();
            AudioSource = audioSource;

            AudioSource.outputAudioMixerGroup = groupStatus.Setting.OutputAudioMixerGroup;

            _groupStatus = groupStatus;

            AudioSourceAccessor = new AudioSourceAccessor(AudioSource);
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

            AudioSource.clip = audioClip;
            return this;
        }

        public SoundPlayer SetCustomClip(CustomClip customClip) {
            if (customClip == null) {
                Debug.LogWarning($"{nameof(CustomClip)} is null");
                return this;
            }

            CustomClip = customClip;
            AudioSource.clip = customClip.AudioClip;
            return this;
        }

        public SoundPlayer SetSound(Sound sound) {
            if (sound == null) {
                Debug.LogWarning($"{nameof(Sound)} is null");
                return this;
            }

            // 元々設定されているSoundに関するデータの内、消さないと残るものを消す
            if (Sound != null) {
                RemoveSoundBehaviour(Sound.SoundBehaviourList);
            }

            Sound = sound;
            Sound.Clip.SetClip(this);
            AddSoundBehaviour(Sound.SoundBehaviourList);
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

        void IAnyClipSettable.SetAudioClip(AudioClip audioClip) {
            SetAudioClip(audioClip);
        }

        void IAnyClipSettable.SetCustomClip(CustomClip customClip) {
            SetCustomClip(customClip);
        }

        public SoundPlayer SetLoop(bool loop) {
            AudioSource.loop = loop;
            return this;
        }

        public SoundPlayer SetPosition(Vector3 position) {
            Transform.localPosition = position;
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
            AudioSource.timeSamples = timeSamples;
            return this;
        }

        public SoundPlayer SetTime(float time) {
            AudioSource.timeSamples = AudioSource.clip == null ? 0 : (int)(time * AudioSource.clip.frequency);
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
            if (CustomClip != null) {
                AudioSourceAccessor.Volume *= CustomClip.GetVolumeMultiplier(AudioSource.time);
            }
            AudioSourceAccessor.Volume *= Volume.MultiplyVolume(_groupStatus.Volumes);
            AudioSourceAccessor.Volume *= _fadeVolume.Value;
            // Update Mute
            AudioSourceAccessor.Mute |= Volume.SumMute(_groupStatus.Volumes);
            // Update Pitch
            if (CustomClip != null) {
                AudioSourceAccessor.Pitch *= CustomClip.GetPitchMultiplier();
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
            if (AudioSource.clip == null) {
                Debug.LogWarning($"{nameof(Play)} failed. {nameof(AudioClip)} is null");
                return this;
            }

            ClampTime();

            if (_fadeVolume.IsFading == false) {
                _fadeVolume.Value = 1;
            }

            UpdatePosition();
            UpdateBehaviours(0);

            AudioSource.Play();
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
            AudioSource.Pause();
            IsPaused = true;
            Behaviours_OnPause();
        }

        public void Resume() {
            AudioSource.UnPause();
            IsPaused = false;
            Behaviours_OnResume();
        }

        public void Reset() {
            Behaviours_OnReset();
            SoundBehaviours.Clear();

            ChorusFilterAccessor?.Clear();
            DistortionFilterAccessor?.Clear();
            EchoFilterAccessor?.Clear();
            HighPassFilterAccessor?.Clear();
            LowPassFilterAccessor?.Clear();
            ReverbFilterAccessor?.Clear();
            ReverbZoneAccessor?.Clear();
            AudioSourceAccessor.Clear();

            PlayScopeStatusDictionary.Clear();

            Sound = null;
            CustomClip = null;
            _fadeVolume.Clear();
            _onComplete = null;
            IsPlayStarted = false;
            IsPaused = false;
            Transform.localPosition = Vector3.zero;
            _spawnPoint = null;
            
            // Reset AudioSource
            AudioSource.Stop();
            AudioSource.clip = null;
            AudioSource.loop = _groupStatus.Setting.DefaultLoop;
            AudioSource.timeSamples = 0;

            IsUsing = false;
        }

        public void Update(float deltaTime) {
            if (IsPlayStarted == false) return;
            // AudioSourceが自律的に停止した場合
            if (IsStopped) {
                RestartOrComplete();
                return;
            }
            if (AudioSource.isPlaying == false) return;
            ClampTime();
            _fadeVolume.Update(deltaTime);
            // フェード完了コールバックでStopが呼ばれた場合、以降は処理しない
            if (IsPlayStarted == false) return;

            UpdatePosition();
            UpdateBehaviours(deltaTime);
            CheckAndHandleEndOfAudio();
        }

        void ClampTime() {
            if (CustomClip != null) {
                PlayRange playRange = CustomClip.PlayRange;
                if (playRange.Enable) {
                    if (AudioSource.timeSamples < playRange.FromSamples) {
                        AudioSource.timeSamples = playRange.FromSamples;
                    }
                }
            }
        }

        void UpdatePosition() {
            if (_spawnPoint == null) return;
            Transform.localPosition = _spawnPoint.position;
        }

        void CheckAndHandleEndOfAudio() {
            if (CustomClip != null && CustomClip.PlayRange.Enable) {
                if (AudioSource.timeSamples >= CustomClip.PlayRange.ToSamples) {
                    RestartOrComplete();
                }
            }
            else {
                if (AudioSource.timeSamples >= AudioSource.clip.samples) {
                    RestartOrComplete();
                }
            }
        }

        void RestartOrComplete() {
            if (Loop) {
                if (CustomClip != null && CustomClip.PlayRange.Enable) {
                    AudioSource.timeSamples = CustomClip.PlayRange.FromSamples;
                }
                else {
                    AudioSource.timeSamples = 0;
                }
            }
            else {
                Complete();
            }
        }
    }
}