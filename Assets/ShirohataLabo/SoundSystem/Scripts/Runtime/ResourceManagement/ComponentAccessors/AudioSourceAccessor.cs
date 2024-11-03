using UnityEngine;

namespace SoundSystem {
    public class AudioSourceAccessor {
        AudioSource AudioSource { get; }

        bool _muteOld;
        public bool Mute { get; set; }

        bool _bypassEffectsOld;
        public bool  BypassEffects { get; set; }

        bool _bypassListenerEffectsOld;
        public bool  BypassListenerEffects { get; set; }

        bool _bypassReverbZonesOld;
        public bool  BypassReverbZones { get; set; }

        float _volumeOld;
        public float Volume { get; set; }

        float _pitchOld;
        public float Pitch { get; set; }

        float _stereoPanOld;
        public float StereoPan { get; set; }

        float _spatialBlendOld;
        public float SpatialBlend { get; set; }

        float _reverbZoneMixOld;
        public float ReverbZoneMix { get; set; }

        float _dopplerLevelOld;
        public float DopplerLevel { get; set; }

        float _spreadOld;
        public float Spread { get; set; }

        AudioRolloffMode _volumeRolloffOld;
        public AudioRolloffMode VolumeRolloff { get; set; }

        float _minDistanceOld;
        public float MinDistance { get; set; }

        float _maxDistanceOld;
        public float MaxDistance { get; set; }

        bool _ignoreListenerPauseOld;
        public bool IgnoreListenerPause { get; set; }

        bool _ignoreListenerVolumeOld;
        public bool IgnoreListenerVolume { get; set; }

        public AudioSourceAccessor(AudioSource audioSource) {
            AudioSource = audioSource;
        }

        public void Reset() {
            SetDefault();
        }

        public void ApplyIfChanged() {
            if (Mute != _muteOld) AudioSource.mute = Mute;
            if (BypassEffects != _bypassEffectsOld) AudioSource.bypassEffects = BypassEffects;
            if (BypassListenerEffects != _bypassListenerEffectsOld) AudioSource.bypassListenerEffects = BypassListenerEffects;
            if (BypassReverbZones != _bypassReverbZonesOld) AudioSource.bypassReverbZones = BypassReverbZones;
            if (Volume != _volumeOld) AudioSource.volume = Volume;
            if (Pitch != _pitchOld) AudioSource.pitch = Pitch;
            if (StereoPan != _stereoPanOld) AudioSource.panStereo = StereoPan;
            if (SpatialBlend != _spatialBlendOld) AudioSource.spatialBlend = SpatialBlend;
            if (ReverbZoneMix != _reverbZoneMixOld) AudioSource.reverbZoneMix = ReverbZoneMix;
            if (DopplerLevel != _dopplerLevelOld) AudioSource.dopplerLevel = DopplerLevel;
            if (Spread != _spreadOld) AudioSource.spread = Spread;
            if (VolumeRolloff != _volumeRolloffOld) AudioSource.rolloffMode = VolumeRolloff;
            if (MinDistance != _minDistanceOld) AudioSource.minDistance = MinDistance;
            if (MaxDistance != _maxDistanceOld) AudioSource.maxDistance = MaxDistance;
            if (IgnoreListenerPause != _ignoreListenerPauseOld) AudioSource.ignoreListenerPause = IgnoreListenerPause;
            if (IgnoreListenerVolume != _ignoreListenerVolumeOld) AudioSource.ignoreListenerVolume = IgnoreListenerVolume;
            CopyToOld();
        }

        public void Clear() {
            SetDefault();
            ApplyIfChanged();
        }

        void SetDefault() {
            Mute = false;
            BypassEffects = false;
            BypassListenerEffects = false;
            BypassReverbZones = false;
            Volume = 1;
            Pitch = 1;
            StereoPan = 0;
            SpatialBlend = 0;
            ReverbZoneMix = 1;
            DopplerLevel = 1;
            Spread = 0;
            VolumeRolloff = AudioRolloffMode.Logarithmic;
            MinDistance = 1;
            MaxDistance = 500;
            IgnoreListenerPause = false;
            IgnoreListenerVolume = false;
        }

        public void Apply() {
            AudioSource.mute = Mute;
            AudioSource.bypassEffects = BypassEffects;
            AudioSource.bypassListenerEffects = BypassListenerEffects;
            AudioSource.bypassReverbZones = BypassReverbZones;
            AudioSource.volume = Volume;
            AudioSource.pitch = Pitch;
            AudioSource.panStereo = StereoPan;
            AudioSource.spatialBlend = SpatialBlend;
            AudioSource.reverbZoneMix = ReverbZoneMix;
            AudioSource.dopplerLevel = DopplerLevel;
            AudioSource.spread = Spread;
            AudioSource.rolloffMode = VolumeRolloff;
            AudioSource.minDistance = MinDistance;
            AudioSource.maxDistance = MaxDistance;
            AudioSource.ignoreListenerPause = IgnoreListenerPause;
            AudioSource.ignoreListenerVolume = IgnoreListenerVolume;
            CopyToOld();
        }

        void CopyToOld() {
            if (Mute != _muteOld) _muteOld = Mute;
            if (BypassEffects != _bypassEffectsOld) _bypassEffectsOld = BypassEffects;
            if (BypassListenerEffects != _bypassListenerEffectsOld) _bypassListenerEffectsOld = BypassListenerEffects;
            if (BypassReverbZones != _bypassReverbZonesOld) _bypassReverbZonesOld = BypassReverbZones;
            if (Volume != _volumeOld) _volumeOld = Volume;
            if (Pitch != _pitchOld) _pitchOld = Pitch;
            if (StereoPan != _stereoPanOld) _stereoPanOld = StereoPan;
            if (SpatialBlend != _spatialBlendOld) _spatialBlendOld = SpatialBlend;
            if (ReverbZoneMix != _reverbZoneMixOld) _reverbZoneMixOld = ReverbZoneMix;
            if (DopplerLevel != _dopplerLevelOld) _dopplerLevelOld = DopplerLevel;
            if (Spread != _spreadOld) _spreadOld = Spread;
            if (VolumeRolloff != _volumeRolloffOld) _volumeRolloffOld = VolumeRolloff;
            if (MinDistance != _minDistanceOld) _minDistanceOld = MinDistance;
            if (MaxDistance != _maxDistanceOld) _maxDistanceOld = MaxDistance;
            if (IgnoreListenerPause != _ignoreListenerPauseOld) _ignoreListenerPauseOld = IgnoreListenerPause;
            if (IgnoreListenerVolume != _ignoreListenerVolumeOld) _ignoreListenerVolumeOld = IgnoreListenerVolume;
        }
    }
}