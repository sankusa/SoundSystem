using System;
using System.Collections.Generic;
using UnityEngine;

namespace SoundSystem {
    public partial class SoundPlayer {
        readonly GameObject _gameObject;
        readonly Transform _transform;
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

        public bool BypassEffects => _audioSource.bypassEffects;
        public bool BypassListenerEffects => _audioSource.bypassListenerEffects;
        public bool BypassReverbZones => _audioSource.bypassReverbZones;
        public bool IgnoreListenerPause => _audioSource.ignoreListenerPause;
        public bool IgnoreListenerVolume => _audioSource.ignoreListenerVolume;
        public float StereoPan => _audioSource.panStereo;
        public float SpatialBlend => _audioSource.spatialBlend;
        public float ReverbZoneMix => _audioSource.reverbZoneMix;
        public float DopplerLevel => _audioSource.dopplerLevel;
        public float Spread => _audioSource.spread;
        public AudioRolloffMode VolumeRollof => _audioSource.rolloffMode;
        public float MinDistance => _audioSource.minDistance;
        public float MaxDistance => _audioSource.maxDistance;


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

        event Action _onComplete;

        Transform _spawnPoint;

        public UnityEngine.AudioHighPassFilter AudioHighPassFilter { get; private set; }
        public UnityEngine.AudioLowPassFilter AudioLowPassFilter { get; private set; }
        public UnityEngine.AudioEchoFilter AudioEchoFilter { get; private set; }
        public UnityEngine.AudioDistortionFilter AudioDistortionFilter { get; private set; }
        public UnityEngine.AudioReverbFilter AudioReverbFilter { get; private set; }
        public UnityEngine.AudioChorusFilter AudioChorusFilter { get; private set; }
        public UnityEngine.AudioReverbZone AudioReverbZone { get; private set; }

        public SoundPlayer(GameObject parentObject, SoundPlayerGroupSetting groupSetting, List<Volume> volumes) {
            _gameObject = new("Player");
            _transform = _gameObject.transform;
            _transform.SetParent(parentObject.transform);
            AudioSource audioSource = _gameObject.AddComponent<AudioSource>();
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
            duration ??= _groupSetting.DefaultFadeDuration;
            _fadeVolume.Fade(from, to, duration.Value, onComplete);
            return this;
        }

        public SoundPlayer SetFade(float to, float? duration = null, Action onComplete = null) {
            duration ??= _groupSetting.DefaultFadeDuration;
            _fadeVolume.Fade(to, duration.Value, onComplete);
            return this;
        }

        public SoundPlayer SetFadeIn(float? duration = null, Action onComplete = null) {
            duration ??= _groupSetting.DefaultFadeDuration;
            _fadeVolume.Fade(1, duration.Value, onComplete);
            return this;
        }

        public SoundPlayer SetFadeOut(float? duration = null, Action onComplete = null) {
            duration ??= _groupSetting.DefaultFadeDuration;
            _fadeVolume.Fade(0, duration.Value, onComplete);
            return this;
        }

        public SoundPlayer SetTime(float time) {
            _audioSource.timeSamples = _audioSource.clip == null ? 0 : (int)(time * _audioSource.clip.frequency);
            return this;
        }

        public SoundPlayer SetBypassEffects(bool bypassEffects) {
            _audioSource.bypassEffects = bypassEffects;
            return this;
        }

        public SoundPlayer SetBypassListenerEffects(bool bypassListenerEffects) {
            _audioSource.bypassListenerEffects = bypassListenerEffects;
            return this;
        }

        public SoundPlayer SetBypassReverbZones(bool bypassReverbZones) {
            _audioSource.bypassReverbZones = bypassReverbZones;
            return this;
        }

        public SoundPlayer SetIgnoreListenerPause(bool ignoreListenerPause) {
            _audioSource.ignoreListenerPause = ignoreListenerPause;
            return this;
        }

        public SoundPlayer SetIgnoreListenerVolume(bool ignoreListenerVolume) {
            _audioSource.ignoreListenerVolume = ignoreListenerVolume;
            return this;
        }

        public SoundPlayer SetStereoPan(float stereoPan) {
            _audioSource.panStereo = stereoPan;
            return this;
        }

        public SoundPlayer SetSpatialBlend(float spatialBlend) {
            _audioSource.spatialBlend = spatialBlend;
            return this;
        }

        public SoundPlayer SetReverbZoneMix(float reverbZoneMix) {
            _audioSource.reverbZoneMix = reverbZoneMix;
            return this;
        }

        public SoundPlayer SetDopplerLevel(float dopplerLevel) {
            _audioSource.dopplerLevel = dopplerLevel;
            return this;
        }

        public SoundPlayer SetSpread(float spread) {
            _audioSource.spread = spread;
            return this;
        }

        public SoundPlayer SetVolumeRollof(AudioRolloffMode rolloffMode) {
            _audioSource.rolloffMode = rolloffMode;
            return this;
        }

        public SoundPlayer SetMinDistance(float minDistance) {
            _audioSource.minDistance = minDistance;
            return this;
        }

        public SoundPlayer SetMaxDistance(float maxDistance) {
            _audioSource.maxDistance = maxDistance;
            return this;
        }

        public AnimationCurve GetCustomCurve(AudioSourceCurveType type) {
            return _audioSource.GetCustomCurve(type);
        }

        public SoundPlayer SetCustomCurve(AudioSourceCurveType type, AnimationCurve curve) {
            _audioSource.SetCustomCurve(type, curve);
            return this;
        }

        public SoundPlayer EnableAudioHighPassFilter() {
            if (AudioHighPassFilter == null) {
                AudioHighPassFilter = _gameObject.AddComponent<UnityEngine.AudioHighPassFilter>();
            }
            AudioHighPassFilter.enabled = true;
            return this;
        }

        public SoundPlayer EnableAudioLowPassFilter() {
            if (AudioLowPassFilter == null) {
                AudioLowPassFilter = _gameObject.AddComponent<UnityEngine.AudioLowPassFilter>();
            }
            AudioLowPassFilter.enabled = true;
            return this;
        }

        public SoundPlayer EnableAudioEchoFilter() {
            if (AudioEchoFilter == null) {
                AudioEchoFilter = _gameObject.AddComponent<UnityEngine.AudioEchoFilter>();
            }
            AudioEchoFilter.enabled = true;
            return this;
        }

        public SoundPlayer EnableAudioDistortionFilter() {
            if (AudioDistortionFilter == null) {
                AudioDistortionFilter = _gameObject.AddComponent<UnityEngine.AudioDistortionFilter>();
            }
            AudioDistortionFilter.enabled = true;
            return this;
        }

        public SoundPlayer EnableAudioReverbFilter() {
            if (AudioReverbFilter == null) {
                AudioReverbFilter = _gameObject.AddComponent<UnityEngine.AudioReverbFilter>();
            }
            AudioReverbFilter.enabled = true;
            return this;
        }

        public SoundPlayer EnableAudioChorusFilter() {
            if (AudioChorusFilter == null) {
                AudioChorusFilter = _gameObject.AddComponent<UnityEngine.AudioChorusFilter>();
            }
            AudioChorusFilter.enabled = true;
            return this;
        }

        public SoundPlayer EnableAudioReverbZone() {
            if (AudioReverbZone == null) {
                AudioReverbZone = _gameObject.AddComponent<UnityEngine.AudioReverbZone>();
            }
            AudioReverbZone.enabled = true;
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

            UpdatePosition();
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
            Action onComple = _onComplete;
            Stop();
            onComple?.Invoke();
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
            _transform.localPosition = Vector3.zero;
            _spawnPoint = null;
            
            // Reset AudioSource
            _audioSource.Stop();
            _audioSource.clip = null;
            _audioSource.pitch = 1;
            _audioSource.loop = _groupSetting.DefaultLoop;
            _audioSource.timeSamples = 0;

            _audioSource.bypassEffects = false;
            _audioSource.bypassListenerEffects = false;
            _audioSource.bypassReverbZones = false;
            _audioSource.ignoreListenerPause = false;
            _audioSource.ignoreListenerVolume = false;
            _audioSource.panStereo = 0;
            _audioSource.spatialBlend = 0;
            _audioSource.reverbZoneMix = 1;
            _audioSource.dopplerLevel = 1;
            _audioSource.spread = 0;
            _audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
            _audioSource.minDistance = 1;
            _audioSource.maxDistance = 500;

            // Filters
            AudioHighPassFilter.DisableAndReset();
            AudioLowPassFilter.DisableAndReset();
            AudioEchoFilter.DisableAndReset();
            AudioDistortionFilter.DisableAndReset();
            AudioReverbFilter.DisableAndReset();
            AudioChorusFilter.DisableAndReset();
            AudioReverbZone.DisableAndReset();
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
            UpdatePosition();
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

        void UpdatePosition() {
            if (_spawnPoint == null) return;
            _transform.localPosition = _spawnPoint.position;
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
    }
}