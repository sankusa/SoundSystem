using UnityEngine;

namespace SoundSystem {
    public class AudioReverbZoneAccessor : ComponentAccessorBase<AudioReverbZone> {
        bool _enableOld;
        public bool Enable { get; set; }

        float _minDistance;
        public float MinDistance { get; set; }

        float _maxDistance;
        public float MaxDistance { get; set; }

        AudioReverbPreset _reverbPresetOld;
        public AudioReverbPreset ReverbPreset { get; set; }

        int _roomOld;
        public int Room { get; set; }

        int _roomHFOld;
        public int RoomHF { get; set; }

        int _roomLFOld;
        public int RoomLF { get; set; }

        float _decayTimeOld;
        public float DecayTime { get; set; }

        float _decayHFRatioOld;
        public float DecayHFRatio { get; set; }

        int _reflectionsOld;
        public int Reflections { get; set; }

        float _reflectionsDelayOld;
        public float ReflectionsDelay { get; set; }

        int _reverbOld;
        public int Reverb { get; set; }

        float _reverbDelayOld;
        public float ReverbDelay { get; set; }

        int _hfReferenceOld;
        public int HFReference { get; set; }

        int _lfReferenceOld;
        public int LFReference { get; set; }

        float _diffusionOld;
        public float Diffusion { get; set; }

        float _densityOld;
        public float Density { get; set; }

        public AudioReverbZoneAccessor(GameObject gameObject) : base(gameObject) {}

        protected override void ApplyIfChangedMain() {
            if (Enable != _enableOld) Component.enabled = Enable;
            if (MinDistance != _minDistance) Component.minDistance = MinDistance;
            if (MaxDistance != _maxDistance) Component.maxDistance = MaxDistance;
            if (ReverbPreset != _reverbPresetOld) Component.reverbPreset = ReverbPreset;
            if (Room != _roomOld) Component.room = Room;
            if (RoomHF != _roomHFOld) Component.roomHF = RoomHF;
            if (RoomLF != _roomLFOld) Component.roomLF = RoomLF;
            if (DecayTime != _decayTimeOld) Component.decayTime = DecayTime;
            if (DecayHFRatio != _decayHFRatioOld) Component.decayHFRatio = DecayHFRatio;
            if (Reflections != _reflectionsOld) Component.reflections = Reflections;
            if (ReflectionsDelay != _reflectionsDelayOld) Component.reflectionsDelay = ReflectionsDelay;
            if (Reverb != _reverbOld) Component.reverb = Reverb;
            if (ReverbDelay != _reverbDelayOld) Component.reverbDelay = ReverbDelay;
            if (HFReference != _hfReferenceOld) Component.HFReference = HFReference;
            if (LFReference != _lfReferenceOld) Component.LFReference = LFReference;
            if (Diffusion != _diffusionOld) Component.diffusion = Diffusion;
            if (Density != _densityOld) Component.density = Density;
        }

        protected override void SetDefault() {
            Enable = false;
            MinDistance = 10;
            MaxDistance = 15;
            ReverbPreset = AudioReverbPreset.User;
            Room = -1000;
            RoomHF = -100;
            RoomLF = 0;
            DecayTime = 1.49f;
            DecayHFRatio = 0.83f;
            Reflections = -2602;
            ReflectionsDelay = 0.007f;
            Reverb = 200;
            ReverbDelay = 0.011f;
            HFReference = 5000;
            LFReference = 250;
            Diffusion = 100;
            Density = 100;
        }

        protected override void ApplyMain() {
            Component.enabled = Enable;
            Component.minDistance = MinDistance;
            Component.maxDistance = MaxDistance;
            Component.reverbPreset = ReverbPreset;
            Component.room = Room;
            Component.roomHF = RoomHF;
            Component.roomLF = RoomLF;
            Component.decayTime = DecayTime;
            Component.decayHFRatio = DecayHFRatio;
            Component.reflections = Reflections;
            Component.reflectionsDelay = ReflectionsDelay;
            Component.reverb = Reverb;
            Component.reverbDelay = ReverbDelay;
            Component.HFReference = HFReference;
            Component.LFReference = LFReference;
            Component.diffusion = Diffusion;
            Component.density = Density;
        }

        protected override void CopyToOld() {
            if (Enable != _enableOld) _enableOld = Enable;
            if (MinDistance != _minDistance) _minDistance = MinDistance;
            if (MaxDistance != _maxDistance) _maxDistance = MaxDistance;
            if (ReverbPreset != _reverbPresetOld) _reverbPresetOld = ReverbPreset;
            if (Room != _roomOld) _roomOld = Room;
            if (RoomHF != _roomHFOld) _roomHFOld = RoomHF;
            if (RoomLF != _roomLFOld) _roomLFOld = RoomLF;
            if (DecayTime != _decayTimeOld) _decayTimeOld = DecayTime;
            if (DecayHFRatio != _decayHFRatioOld) _decayHFRatioOld = DecayHFRatio;
            if (Reflections != _reflectionsOld) _reflectionsOld = Reflections;
            if (ReflectionsDelay != _reflectionsDelayOld) _reflectionsDelayOld = ReflectionsDelay;
            if (Reverb != _reverbOld) _reverbOld = Reverb;
            if (ReverbDelay != _reverbDelayOld) _reverbDelayOld = ReverbDelay;
            if (HFReference != _hfReferenceOld) _hfReferenceOld = HFReference;
            if (LFReference != _lfReferenceOld) _lfReferenceOld = LFReference;
            if (Diffusion != _diffusionOld) _diffusionOld = Diffusion;
            if (Density != _densityOld) _densityOld = Density;
        }
    }
}