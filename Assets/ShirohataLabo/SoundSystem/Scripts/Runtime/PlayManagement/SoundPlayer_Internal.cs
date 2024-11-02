using UnityEngine;

namespace SoundSystem {
    public partial class SoundPlayer {
        AudioChorusFilterAccessor ChorusFilterAccessor { get; set; }
        AudioDistortionFilterAccessor DistortionFilterAccessor { get; set; }
        AudioEchoFilterAccessor EchoFilterAccessor { get; set; }
        AudioHighPassFilterAccessor HighPassFilterAccessor { get; set; }
        AudioLowPassFilterAccessor LowPassFilterAccessor { get; set; }
        AudioReverbFilterAccessor ReverbFilterAccessor { get; set; }
        AudioReverbZoneAccessor ReverbZoneAccessor { get; set; }

        internal AnimationCurve GetCustomCurve(AudioSourceCurveType type) {
            return _audioSource.GetCustomCurve(type);
        }

        internal SoundPlayer SetCustomCurve(AudioSourceCurveType type, AnimationCurve curve) {
            if (curve.length == 0) return this;
            _audioSource.SetCustomCurve(type, curve);
            return this;
        }

        internal AudioChorusFilterAccessor GetOrCreateAudioChorusFilter() {
            if (ChorusFilterAccessor == null) {
                ChorusFilterAccessor = new AudioChorusFilterAccessor(_gameObject);
            }
            ChorusFilterAccessor.CreateFilterIfNull();
            return ChorusFilterAccessor;
        }

        internal AudioDistortionFilterAccessor GetOrCreateAudioDistortionFilter() {
            if (DistortionFilterAccessor == null) {
                DistortionFilterAccessor = new AudioDistortionFilterAccessor(_gameObject);
            }
            DistortionFilterAccessor.CreateFilterIfNull();
            return DistortionFilterAccessor;
        }

        internal AudioEchoFilterAccessor GetOrCreateAudioEchoFilter() {
            if (EchoFilterAccessor == null) {
                EchoFilterAccessor = new AudioEchoFilterAccessor(_gameObject);
            }
            EchoFilterAccessor.CreateFilterIfNull();
            return EchoFilterAccessor;
        }

        internal AudioHighPassFilterAccessor GetOrCreateAudioHighPassFilter() {
            if (HighPassFilterAccessor == null) {
                HighPassFilterAccessor = new AudioHighPassFilterAccessor(_gameObject);
            }
            HighPassFilterAccessor.CreateFilterIfNull();
            return HighPassFilterAccessor;
        }

        internal AudioLowPassFilterAccessor GetOrCreateAudioLowPassFilter() {
            if (LowPassFilterAccessor == null) {
                LowPassFilterAccessor = new AudioLowPassFilterAccessor(_gameObject);
            }
            LowPassFilterAccessor.CreateFilterIfNull();
            return LowPassFilterAccessor;
        }

        internal AudioReverbFilterAccessor GetOrCreateAudioReverbFilter() {
            if (ReverbFilterAccessor == null) {
                ReverbFilterAccessor = new AudioReverbFilterAccessor(_gameObject);
            }
            ReverbFilterAccessor.CreateFilterIfNull();
            return ReverbFilterAccessor;
        }

        internal AudioReverbZoneAccessor GetOrCreateAudioReverbZone() {
            if (ReverbZoneAccessor == null) {
                ReverbZoneAccessor = new AudioReverbZoneAccessor(_gameObject);
            }
            ReverbZoneAccessor.CreateFilterIfNull();
            return ReverbZoneAccessor;
        }
    }
}