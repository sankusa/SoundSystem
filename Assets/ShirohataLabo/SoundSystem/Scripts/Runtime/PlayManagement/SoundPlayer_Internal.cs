using UnityEngine;

namespace SoundSystem {
    public partial class SoundPlayer {
        internal float VolumeMultiplier { get; private set; } = 1;
        internal float PitchMultiplier { get; private set; } = 1;
        internal UnityEngine.AudioHighPassFilter AudioHighPassFilter { get; private set; }
        internal UnityEngine.AudioLowPassFilter AudioLowPassFilter { get; private set; }
        internal UnityEngine.AudioEchoFilter AudioEchoFilter { get; private set; }
        internal UnityEngine.AudioDistortionFilter AudioDistortionFilter { get; private set; }
        internal UnityEngine.AudioReverbFilter AudioReverbFilter { get; private set; }
        internal UnityEngine.AudioChorusFilter AudioChorusFilter { get; private set; }
        internal UnityEngine.AudioReverbZone AudioReverbZone { get; private set; }

        internal SoundPlayer SetVolumeMultiplier(float volumeMultiplier) {
            VolumeMultiplier = volumeMultiplier;
            return this;
        }

        internal SoundPlayer SetPitchMultiplier(float pitchMultiplier) {
            PitchMultiplier = pitchMultiplier;
            return this;
        }

        internal SoundPlayer SetBypassEffects(bool bypassEffects) {
            _audioSource.bypassEffects = bypassEffects;
            return this;
        }

        internal SoundPlayer SetBypassListenerEffects(bool bypassListenerEffects) {
            _audioSource.bypassListenerEffects = bypassListenerEffects;
            return this;
        }

        internal SoundPlayer SetBypassReverbZones(bool bypassReverbZones) {
            _audioSource.bypassReverbZones = bypassReverbZones;
            return this;
        }

        internal SoundPlayer SetIgnoreListenerPause(bool ignoreListenerPause) {
            _audioSource.ignoreListenerPause = ignoreListenerPause;
            return this;
        }

        internal SoundPlayer SetIgnoreListenerVolume(bool ignoreListenerVolume) {
            _audioSource.ignoreListenerVolume = ignoreListenerVolume;
            return this;
        }

        internal SoundPlayer SetStereoPan(float stereoPan) {
            _audioSource.panStereo = stereoPan;
            return this;
        }

        internal SoundPlayer SetSpatialBlend(float spatialBlend) {
            _audioSource.spatialBlend = spatialBlend;
            return this;
        }

        internal SoundPlayer SetReverbZoneMix(float reverbZoneMix) {
            _audioSource.reverbZoneMix = reverbZoneMix;
            return this;
        }

        internal SoundPlayer SetDopplerLevel(float dopplerLevel) {
            _audioSource.dopplerLevel = dopplerLevel;
            return this;
        }

        internal SoundPlayer SetSpread(float spread) {
            _audioSource.spread = spread;
            return this;
        }

        internal SoundPlayer SetVolumeRollof(AudioRolloffMode rolloffMode) {
            _audioSource.rolloffMode = rolloffMode;
            return this;
        }

        internal SoundPlayer SetMinDistance(float minDistance) {
            _audioSource.minDistance = minDistance;
            return this;
        }

        internal SoundPlayer SetMaxDistance(float maxDistance) {
            _audioSource.maxDistance = maxDistance;
            return this;
        }

        internal AnimationCurve GetCustomCurve(AudioSourceCurveType type) {
            return _audioSource.GetCustomCurve(type);
        }

        internal SoundPlayer SetCustomCurve(AudioSourceCurveType type, AnimationCurve curve) {
            if (curve.keys.Length == 0) return this;
            _audioSource.SetCustomCurve(type, curve);
            return this;
        }

        internal UnityEngine.AudioHighPassFilter GetOrCreateAudioHighPassFilter() {
            if (AudioHighPassFilter == null) {
                AudioHighPassFilter = _gameObject.AddComponent<UnityEngine.AudioHighPassFilter>();
            }
            return AudioHighPassFilter;
        }

        internal UnityEngine.AudioLowPassFilter GetOrCreateAudioLowPassFilter() {
            if (AudioLowPassFilter == null) {
                AudioLowPassFilter = _gameObject.AddComponent<UnityEngine.AudioLowPassFilter>();
                AudioLowPassFilter.DisableAndReset();
            }
            return AudioLowPassFilter;
        }

        internal UnityEngine.AudioEchoFilter GetOrCreateAudioEchoFilter() {
            if (AudioEchoFilter == null) {
                AudioEchoFilter = _gameObject.AddComponent<UnityEngine.AudioEchoFilter>();
                AudioEchoFilter.DisableAndReset();
            }
            return AudioEchoFilter;
        }

        internal UnityEngine.AudioDistortionFilter GetOrCreateAudioDistortionFilter() {
            if (AudioDistortionFilter == null) {
                AudioDistortionFilter = _gameObject.AddComponent<UnityEngine.AudioDistortionFilter>();
                AudioDistortionFilter.DisableAndReset();
            }
            return AudioDistortionFilter;
        }

        internal UnityEngine.AudioReverbFilter GetOrCreateAudioReverbFilter() {
            if (AudioReverbFilter == null) {
                AudioReverbFilter = _gameObject.AddComponent<UnityEngine.AudioReverbFilter>();
                AudioReverbFilter.DisableAndReset();
            }
            return AudioReverbFilter;
        }

        internal UnityEngine.AudioChorusFilter GetOrCreateAudioChorusFilter() {
            if (AudioChorusFilter == null) {
                AudioChorusFilter = _gameObject.AddComponent<UnityEngine.AudioChorusFilter>();
                AudioChorusFilter.DisableAndReset();
            }
            return AudioChorusFilter;
        }

        internal UnityEngine.AudioReverbZone GetOrCreateAudioReverbZone() {
            if (AudioReverbZone == null) {
                AudioReverbZone = _gameObject.AddComponent<UnityEngine.AudioReverbZone>();
                AudioReverbZone.DisableAndReset();
            }
            return AudioReverbZone;
        }

        void ResetSoundBehaviours() {
            VolumeMultiplier = 1;
            PitchMultiplier = 1;
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
    }
}