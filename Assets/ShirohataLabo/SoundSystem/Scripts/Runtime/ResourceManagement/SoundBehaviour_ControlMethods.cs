using UnityEngine;

namespace SoundSystem {
    public abstract partial class SoundBehaviour {
        protected float GetVolumeMultiplier(SoundPlayer player) {
            return player.VolumeMultiplier;
        }
        protected void SetVolumeMultiplier(SoundPlayer player, float volumeMultiplier) {
            player.SetVolumeMultiplier(volumeMultiplier);
        }

        protected float GetPitchMultiplier(SoundPlayer player) {
            return player.PitchMultiplier;
        }
        protected void SetPitchMultiplier(SoundPlayer player, float pitchMultiplier) {
            player.SetPitchMultiplier(pitchMultiplier);
        }

        protected bool GetBypassEffects(SoundPlayer player) {
            return player.BypassEffects;
        }
        protected void SetBypassEffects(SoundPlayer player, bool bypassEffects) {
            player.SetBypassEffects(bypassEffects);
        }

        protected bool GetBypassListenerEffects(SoundPlayer player) {
            return player.BypassListenerEffects;
        }
        protected void SetBypassListenerEffects(SoundPlayer player, bool bypassListenerEffects) {
            player.SetBypassListenerEffects(bypassListenerEffects);
        }

        protected bool GetBypassReverbZones(SoundPlayer player) {
            return player.BypassReverbZones;
        }
        protected void SetBypassReverbZones(SoundPlayer player, bool bypassReverbZones) {
            player.SetBypassReverbZones(bypassReverbZones);
        }

        protected bool GetIgnoreListenerPause(SoundPlayer player) {
            return player.IgnoreListenerPause;
        }
        protected void SetIgnoreListenerPause(SoundPlayer player, bool ignoreListenerPause) {
            player.SetIgnoreListenerPause(ignoreListenerPause);
        }

        protected bool GetIgnoreListenerVolume(SoundPlayer player) {
            return player.IgnoreListenerVolume;
        }
        protected void SetIgnoreListenerVolume(SoundPlayer player, bool ignoreListenerVolume) {
            player.SetIgnoreListenerVolume(ignoreListenerVolume);
        }

        protected float GetStereoPan(SoundPlayer player) {
            return player.StereoPan;
        }
        protected void SetStereoPan(SoundPlayer player, float stereoPan) {
            player.SetStereoPan(stereoPan);
        }

        protected float GetSpatialBlend(SoundPlayer player) {
            return player.SpatialBlend;
        }
        protected void SetSpatialBlend(SoundPlayer player, float spatialBlend) {
            player.SetSpatialBlend(spatialBlend);
        }

        protected float GetReverbZoneMix(SoundPlayer player) {
            return player.ReverbZoneMix;
        }
        protected void SetReverbZoneMix(SoundPlayer player, float reverbZoneMix) {
            player.SetReverbZoneMix(reverbZoneMix);
        }

        protected float GetDopplerLevel(SoundPlayer player) {
            return player.DopplerLevel;
        }
        protected void SetDopplerLevel(SoundPlayer player, float dopplerLevel) {
            player.SetDopplerLevel(dopplerLevel);
        }

        protected float GetSpread(SoundPlayer player) {
            return player.Spread;
        }
        protected void SetSpread(SoundPlayer player, float spread) {
            player.SetSpread(spread);
        }

        protected AudioRolloffMode GetVolumeRollof(SoundPlayer player) {
            return player.VolumeRollof;
        }
        protected void SetVolumeRollof(SoundPlayer player, AudioRolloffMode rolloffMode) {
            player.SetVolumeRollof(rolloffMode);
        }

        protected float GetMinDistance(SoundPlayer player) {
            return player.MinDistance;
        }
        protected void SetMinDistance(SoundPlayer player, float minDistance) {
            player.SetMinDistance(minDistance);
        }

        protected float GetMaxDistance(SoundPlayer player) {
            return player.MaxDistance;
        }
        protected void SetMaxDistance(SoundPlayer player, float maxDistance) {
            player.SetMaxDistance(maxDistance);
        }

        protected AnimationCurve GetCustomCurve(SoundPlayer player, AudioSourceCurveType type) {
            return player.GetCustomCurve(type);
        }
        protected void SetCustomCurve(SoundPlayer player, AudioSourceCurveType type, AnimationCurve curve) {
            player.SetCustomCurve(type, curve);
        }

        protected UnityEngine.AudioHighPassFilter GetOrCreateAudioHighPassFilter(SoundPlayer player) {
            return player.GetOrCreateAudioHighPassFilter();
        }
        protected UnityEngine.AudioLowPassFilter GetOrCreateAudioLowPassFilter(SoundPlayer player) {
            return player.GetOrCreateAudioLowPassFilter();
        }
        protected UnityEngine.AudioEchoFilter GetOrCreateAudioEchoFilter(SoundPlayer player) {
            return player.GetOrCreateAudioEchoFilter();
        }
        protected UnityEngine.AudioDistortionFilter GetOrCreateAudioDistortionFilter(SoundPlayer player) {
            return player.GetOrCreateAudioDistortionFilter();
        }
        protected UnityEngine.AudioReverbFilter GetOrCreateAudioReverbFilter(SoundPlayer player) {
            return player.GetOrCreateAudioReverbFilter();
        }
        protected UnityEngine.AudioChorusFilter GetOrCreateAudioChorusFilter(SoundPlayer player) {
            return player.GetOrCreateAudioChorusFilter();
        }
        protected UnityEngine.AudioReverbZone GetOrCreateAudioReverbZone(SoundPlayer player) {
            return player.GetOrCreateAudioReverbZone();
        }
    }
}