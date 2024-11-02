using UnityEngine;

namespace SoundSystem {
    public class AudioReverbFilterAccessor {
        GameObject GameObject { get; }
        public AudioReverbFilter Filter { get; private set; }

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

        public AudioReverbFilterAccessor(GameObject gameObject) {
            GameObject = gameObject;
        }

        public void CreateFilterIfNull() {
            if (Filter == null) {
                Filter = GameObject.AddComponent<AudioReverbFilter>();
                SetDefault();
                Apply();
            }
        }

        public void Reset() {
            if (Filter == null) return;
            SetDefault();
        }

        public void ApplyIfChanged() {
            if (Filter != null) {
                if (Enable != _enableOld) Filter.enabled = Enable;
                if (ReverbPreset != _reverbPresetOld) Filter.reverbPreset = ReverbPreset;
                if (DryLevel != _dryLevelOld) Filter.dryLevel = DryLevel;
                if (Room != _roomOld) Filter.room = Room;
                if (RoomHF != _roomHFOld) Filter.roomHF = RoomHF;
                if (RoomLF != _roomLFOld) Filter.roomLF = RoomLF;
                if (DecayTime != _decayTimeOld) Filter.decayTime = DecayTime;
                if (DecayHFRatio != _decayHFRatioOld) Filter.decayHFRatio = DecayHFRatio;
                if (ReflectionsLevel != _reflectionsLevelOld) Filter.reflectionsLevel = ReflectionsLevel;
                if (ReflectionsDelay != _reflectionsDelayOld) Filter.reflectionsDelay = ReflectionsDelay;
                if (ReverbLevel != _reverbLevelOld) Filter.reverbLevel = ReverbLevel;
                if (ReverbDelay != _reverbDelayOld) Filter.reverbDelay = ReverbDelay;
                if (HfReference != _hfReferenceOld) Filter.hfReference = HfReference;
                if (LfReference != _lfReferenceOld) Filter.lfReference = LfReference;
                if (Diffusion != _diffusionOld) Filter.diffusion = Diffusion;
                if (Density != _densityOld) Filter.density = Density;
            }
            CopyToOld();
        }

        public void Clear() {
            DestroyAudioDistortionFilter();
            SetDefault();
            CopyToOld();
        }

        void SetDefault() {
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

        void Apply() {
            if (Filter != null) {
                Filter.enabled = Enable;
                Filter.reverbPreset = ReverbPreset;
                Filter.dryLevel = DryLevel;
                Filter.room = Room;
                Filter.roomHF = RoomHF;
                Filter.roomLF = RoomLF;
                Filter.decayTime = DecayTime;
                Filter.decayHFRatio = DecayHFRatio;
                Filter.reflectionsLevel = ReflectionsLevel;
                Filter.reflectionsDelay = ReflectionsDelay;
                Filter.reverbLevel = ReverbLevel;
                Filter.reverbDelay = ReverbDelay;
                Filter.hfReference = HfReference;
                Filter.lfReference = LfReference;
                Filter.diffusion = Diffusion;
                Filter.density = Density;
            }
            CopyToOld();
        }

        void CopyToOld() {
            _enableOld = Enable;
            _reverbPresetOld = ReverbPreset;
            _dryLevelOld = DryLevel;
            _roomOld = Room;
            _roomHFOld = RoomHF;
            _roomLFOld = RoomLF;
            _decayTimeOld = DecayTime;
            _decayHFRatioOld = DecayHFRatio;
            _reflectionsLevelOld = ReflectionsLevel;
            _reflectionsDelayOld = ReflectionsDelay;
            _reverbLevelOld = ReverbLevel;
            _reverbDelayOld = ReverbDelay;
            _hfReferenceOld = HfReference;
            _lfReferenceOld = LfReference;
            _diffusionOld = Diffusion;
            _densityOld = Density;
        }

        void DestroyAudioDistortionFilter() {
            Filter.DestroyFlexible();
        }
    }
}