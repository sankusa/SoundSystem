using UnityEngine;

namespace SoundSystem {
    public partial class SoundPlayer {
        AudioSourceAccessor AudioSourceAccessor { get; }
        AudioChorusFilterAccessor ChorusFilterAccessor { get; set; }
        AudioDistortionFilterAccessor DistortionFilterAccessor { get; set; }
        AudioEchoFilterAccessor EchoFilterAccessor { get; set; }
        AudioHighPassFilterAccessor HighPassFilterAccessor { get; set; }
        AudioLowPassFilterAccessor LowPassFilterAccessor { get; set; }
        AudioReverbFilterAccessor ReverbFilterAccessor { get; set; }
        AudioReverbZoneAccessor ReverbZoneAccessor { get; set; }

        internal AudioSourceAccessor GetAudioSourceAccessor() {
            return AudioSourceAccessor;
        }

        internal AnimationCurve GetCustomCurve(AudioSourceCurveType type) {
            return AudioSource.GetCustomCurve(type);
        }

        internal SoundPlayer SetCustomCurve(AudioSourceCurveType type, AnimationCurve curve) {
            if (curve.length == 0) return this;
            AudioSource.SetCustomCurve(type, curve);
            return this;
        }

        internal AudioChorusFilterAccessor GetOrCreateAudioChorusFilter() {
            ChorusFilterAccessor ??= new AudioChorusFilterAccessor(GameObject);
            ChorusFilterAccessor.CreateFilterIfNull();
            return ChorusFilterAccessor;
        }

        internal AudioDistortionFilterAccessor GetOrCreateAudioDistortionFilter() {
            DistortionFilterAccessor ??= new AudioDistortionFilterAccessor(GameObject);
            DistortionFilterAccessor.CreateFilterIfNull();
            return DistortionFilterAccessor;
        }

        internal AudioEchoFilterAccessor GetOrCreateAudioEchoFilter() {
            EchoFilterAccessor ??= new AudioEchoFilterAccessor(GameObject);
            EchoFilterAccessor.CreateFilterIfNull();
            return EchoFilterAccessor;
        }

        internal AudioHighPassFilterAccessor GetOrCreateAudioHighPassFilter() {
            HighPassFilterAccessor ??= new AudioHighPassFilterAccessor(GameObject);
            HighPassFilterAccessor.CreateFilterIfNull();
            return HighPassFilterAccessor;
        }

        internal AudioLowPassFilterAccessor GetOrCreateAudioLowPassFilter() {
            LowPassFilterAccessor ??= new AudioLowPassFilterAccessor(GameObject);
            LowPassFilterAccessor.CreateFilterIfNull();
            return LowPassFilterAccessor;
        }

        internal AudioReverbFilterAccessor GetOrCreateAudioReverbFilter() {
            ReverbFilterAccessor ??= new AudioReverbFilterAccessor(GameObject);
            ReverbFilterAccessor.CreateFilterIfNull();
            return ReverbFilterAccessor;
        }

        internal AudioReverbZoneAccessor GetOrCreateAudioReverbZone() {
            ReverbZoneAccessor ??= new AudioReverbZoneAccessor(GameObject);
            ReverbZoneAccessor.CreateFilterIfNull();
            return ReverbZoneAccessor;
        }
    }
}