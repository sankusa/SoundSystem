using System.Collections.Generic;
using UnityEngine;

namespace SoundSystem {
    public class Volume {
        public static float MultiplyVolume(List<Volume> volumes) {
            float value = 1;
            foreach (Volume volume in volumes) {
                value *= volume.Value;
            }
            return value;
        }

        public static bool SumMute(List<Volume> volumes) {
            foreach (Volume volume in volumes) {
                if (volume.Mute) return true;
            }
            return false;
        }

        const string SaveKeyPrefix = "SoundSystem_";
        const string SaveKeySuffix_Volume = "_Volume_Value";
        const string SaveKeySuffix_Mute = "_Volume_Mute";

        public string Key { get; }
        bool _save;
        float _value;
        public float Value {
            get => _value;
            set {
                if (_save && _value != value) {
                    SaveValue();
                }
                _value = value;
            }
        }
        bool _mute;
        public bool Mute {
            get => _mute;
            set {
                if (_save && _mute != value) {
                    SaveMute();
                }
                _mute = value;
            }
        }

        string VolumeSaveKey => SaveKeyPrefix + Key + SaveKeySuffix_Volume;
        string MuteSaveKey => SaveKeyPrefix + Key + SaveKeySuffix_Mute;
        
        public Volume(VolumeSetting setting) {
            Key = setting.Key;
            _save = setting.Save;
            _value = setting.DefaultVolume;
            _mute = setting.DefaultMute;

            if (_save) {
                LoadValue();
                LoadMute();
            }
        }

        public void SaveValue() {
            if (_save == false) return;
            SaveUtil.SaveFloat(VolumeSaveKey, _value);
        }

        public void LoadValue() {
            if (_save == false) return;
            if (SaveUtil.HasKey(VolumeSaveKey) == false) return;
            _value = SaveUtil.LoadFloat(VolumeSaveKey);
        }

        public void SaveMute() {
            if (_save == false) return;
            SaveUtil.SaveBool(MuteSaveKey, _mute);
        }

        public void LoadMute() {
            if (_save == false) return;
            if (SaveUtil.HasKey(MuteSaveKey) == false) return;
            _mute = SaveUtil.LoadBool(MuteSaveKey);
        }
    }
}