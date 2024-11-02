using UnityEngine;

namespace SoundSystem {
    public abstract partial class SoundBehaviour {
        protected AudioSourceAccessor GetAudioSourceAccessor(SoundPlayer player) {
            return player.AudioSourceAccessor;
        }

        protected AnimationCurve GetCustomCurve(SoundPlayer player, AudioSourceCurveType type) {
            return player.GetCustomCurve(type);
        }
        protected void SetCustomCurve(SoundPlayer player, AudioSourceCurveType type, AnimationCurve curve) {
            player.SetCustomCurve(type, curve);
        }

        protected AudioChorusFilterAccessor GetOrCreateAudioChorusFilter(SoundPlayer player) {
            return player.GetOrCreateAudioChorusFilter();
        }
        protected AudioDistortionFilterAccessor GetOrCreateAudioDistortionFilter(SoundPlayer player) {
            return player.GetOrCreateAudioDistortionFilter();
        }
        protected AudioEchoFilterAccessor GetOrCreateAudioEchoFilter(SoundPlayer player) {
            return player.GetOrCreateAudioEchoFilter();
        }
        protected AudioHighPassFilterAccessor GetOrCreateAudioHighPassFilter(SoundPlayer player) {
            return player.GetOrCreateAudioHighPassFilter();
        }
        protected AudioLowPassFilterAccessor GetOrCreateAudioLowPassFilter(SoundPlayer player) {
            return player.GetOrCreateAudioLowPassFilter();
        }
        protected AudioReverbFilterAccessor GetOrCreateAudioReverbFilter(SoundPlayer player) {
            return player.GetOrCreateAudioReverbFilter();
        }
        protected AudioReverbZoneAccessor GetOrCreateAudioReverbZone(SoundPlayer player) {
            return player.GetOrCreateAudioReverbZone();
        }
    }
}