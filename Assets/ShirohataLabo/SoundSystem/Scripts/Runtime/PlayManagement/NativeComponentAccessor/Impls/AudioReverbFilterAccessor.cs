using UnityEngine;

namespace SoundSystem {
    public class AudioReverbFilterAccessor : AudioFilterAccessorBase<AudioReverbFilter> {
        bool _enableOld;
        public bool Enable { get; set; }

        AudioReverbPreset _reverbPresetOld;
        public AudioReverbPreset ReverbPreset { get; set; }

        int _dryLevelOld;
        public int DryLevel { get; set; }

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

        int _reflectionsLevelOld;
        public int ReflectionsLevel { get; set; }

        float _reflectionsDelayOld;
        public float ReflectionsDelay { get; set; }

        int _reverbLevelOld;
        public int ReverbLevel { get; set; }

        float _reverbDelayOld;
        public float ReverbDelay { get; set; }

        int _hfReferenceOld;
        public int HfReference { get; set; }

        int _lfReferenceOld;
        public int LfReference { get; set; }

        float _diffusionOld;
        public float Diffusion { get; set; }

        float _densityOld;
        public float Density { get; set; }

        public AudioReverbFilterAccessor(GameObject gameObject) : base(gameObject) {}

        protected override void ApplyIfChangedMain() {
            if (Enable != _enableOld) Component.enabled = Enable;
            if (ReverbPreset != _reverbPresetOld) Component.reverbPreset = ReverbPreset;
            if (DryLevel != _dryLevelOld) Component.dryLevel = DryLevel;
            if (Room != _roomOld) Component.room = Room;
            if (RoomHF != _roomHFOld) Component.roomHF = RoomHF;
            if (RoomLF != _roomLFOld) Component.roomLF = RoomLF;
            if (DecayTime != _decayTimeOld) Component.decayTime = DecayTime;
            if (DecayHFRatio != _decayHFRatioOld) Component.decayHFRatio = DecayHFRatio;
            if (ReflectionsLevel != _reflectionsLevelOld) Component.reflectionsLevel = ReflectionsLevel;
            if (ReflectionsDelay != _reflectionsDelayOld) Component.reflectionsDelay = ReflectionsDelay;
            if (ReverbLevel != _reverbLevelOld) Component.reverbLevel = ReverbLevel;
            if (ReverbDelay != _reverbDelayOld) Component.reverbDelay = ReverbDelay;
            if (HfReference != _hfReferenceOld) Component.hfReference = HfReference;
            if (LfReference != _lfReferenceOld) Component.lfReference = LfReference;
            if (Diffusion != _diffusionOld) Component.diffusion = Diffusion;
            if (Density != _densityOld) Component.density = Density;
        }

        protected override void SetDefault() {
            Enable = false;
            ReverbPreset = AudioReverbPreset.User;
            DryLevel = 0;
            Room = 0;
            RoomHF = 0;
            RoomLF = 0;
            DecayTime = 1;
            DecayHFRatio = 0.5f;
            ReflectionsLevel = -10000;
            ReflectionsDelay = 0;
            ReverbLevel = 0;
            ReverbDelay = 0.04f;
            HfReference = 5000;
            LfReference = 250;
            Diffusion = 100;
            Density = 100;
        }

        protected override void ApplyMain() {
            Component.enabled = Enable;
            Component.reverbPreset = ReverbPreset;
            Component.dryLevel = DryLevel;
            Component.room = Room;
            Component.roomHF = RoomHF;
            Component.roomLF = RoomLF;
            Component.decayTime = DecayTime;
            Component.decayHFRatio = DecayHFRatio;
            Component.reflectionsLevel = ReflectionsLevel;
            Component.reflectionsDelay = ReflectionsDelay;
            Component.reverbLevel = ReverbLevel;
            Component.reverbDelay = ReverbDelay;
            Component.hfReference = HfReference;
            Component.lfReference = LfReference;
            Component.diffusion = Diffusion;
            Component.density = Density;
        }

        protected override void CopyToOld() {
            if (Enable != _enableOld) _enableOld = Enable;
            if (ReverbPreset != _reverbPresetOld) _reverbPresetOld = ReverbPreset;
            if (DryLevel != _dryLevelOld) _dryLevelOld = DryLevel;
            if (Room != _roomOld) _roomOld = Room;
            if (RoomHF != _roomHFOld) _roomHFOld = RoomHF;
            if (RoomLF != _roomLFOld) _roomLFOld = RoomLF;
            if (DecayTime != _decayTimeOld) _decayTimeOld = DecayTime;
            if (DecayHFRatio != _decayHFRatioOld) _decayHFRatioOld = DecayHFRatio;
            if (ReflectionsLevel != _reflectionsLevelOld) _reflectionsLevelOld = ReflectionsLevel;
            if (ReflectionsDelay != _reflectionsDelayOld) _reflectionsDelayOld = ReflectionsDelay;
            if (ReverbLevel != _reverbLevelOld) _reverbLevelOld = ReverbLevel;
            if (ReverbDelay != _reverbDelayOld) _reverbDelayOld = ReverbDelay;
            if (HfReference != _hfReferenceOld) _hfReferenceOld = HfReference;
            if (LfReference != _lfReferenceOld) _lfReferenceOld = LfReference;
            if (Diffusion != _diffusionOld) _diffusionOld = Diffusion;
            if (Density != _densityOld) _densityOld = Density;
        }
    }
}