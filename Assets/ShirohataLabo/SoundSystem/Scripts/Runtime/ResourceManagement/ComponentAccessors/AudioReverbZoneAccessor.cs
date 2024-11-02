using UnityEngine;

namespace SoundSystem {
    public class AudioReverbZoneAccessor {
        GameObject GameObject { get; }
        public AudioReverbZone Filter { get; private set; }

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

        public AudioReverbZoneAccessor(GameObject gameObject) {
            GameObject = gameObject;
        }

        public void CreateFilterIfNull() {
            if (Filter == null) {
                Filter = GameObject.AddComponent<AudioReverbZone>();
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
                if (MinDistance != _minDistance) Filter.minDistance = MinDistance;
                if (MaxDistance != _maxDistance) Filter.maxDistance = MaxDistance;
                if (ReverbPreset != _reverbPresetOld) Filter.reverbPreset = ReverbPreset;
                if (Room != _roomOld) Filter.room = Room;
                if (RoomHF != _roomHFOld) Filter.roomHF = RoomHF;
                if (RoomLF != _roomLFOld) Filter.roomLF = RoomLF;
                if (DecayTime != _decayTimeOld) Filter.decayTime = DecayTime;
                if (DecayHFRatio != _decayHFRatioOld) Filter.decayHFRatio = DecayHFRatio;
                if (Reflections != _reflectionsOld) Filter.reflections = Reflections;
                if (ReflectionsDelay != _reflectionsDelayOld) Filter.reflectionsDelay = ReflectionsDelay;
                if (Reverb != _reverbOld) Filter.reverb = Reverb;
                if (ReverbDelay != _reverbDelayOld) Filter.reverbDelay = ReverbDelay;
                if (HFReference != _hfReferenceOld) Filter.HFReference = HFReference;
                if (LFReference != _lfReferenceOld) Filter.LFReference = LFReference;
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

        void Apply() {
            if (Filter != null) {
                Filter.enabled = Enable;
                Filter.minDistance = MinDistance;
                Filter.maxDistance = MaxDistance;
                Filter.reverbPreset = ReverbPreset;
                Filter.room = Room;
                Filter.roomHF = RoomHF;
                Filter.roomLF = RoomLF;
                Filter.decayTime = DecayTime;
                Filter.decayHFRatio = DecayHFRatio;
                Filter.reflections = Reflections;
                Filter.reflectionsDelay = ReflectionsDelay;
                Filter.reverb = Reverb;
                Filter.reverbDelay = ReverbDelay;
                Filter.HFReference = HFReference;
                Filter.LFReference = LFReference;
                Filter.diffusion = Diffusion;
                Filter.density = Density;
            }
            CopyToOld();
        }

        void CopyToOld() {
            _enableOld = Enable;
            _minDistance = MinDistance;
            _maxDistance = MaxDistance;
            _reverbPresetOld = ReverbPreset;
            _roomOld = Room;
            _roomHFOld = RoomHF;
            _roomLFOld = RoomLF;
            _decayTimeOld = DecayTime;
            _decayHFRatioOld = DecayHFRatio;
            _reflectionsOld = Reflections;
            _reflectionsDelayOld = ReflectionsDelay;
            _reverbOld = Reverb;
            _reverbDelayOld = ReverbDelay;
            _hfReferenceOld = HFReference;
            _lfReferenceOld = LFReference;
            _diffusionOld = Diffusion;
            _densityOld = Density;
        }

        void DestroyAudioDistortionFilter() {
            Filter.DestroyFlexible();
        }
    }
}