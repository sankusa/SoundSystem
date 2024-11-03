using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundSystem {
    [Serializable]
    public class SoundPlayerGroupSetting {
        [SerializeField] string _key;
        public string Key => _key;

        [SerializeField, Min(1)] int _playerCount = 1;
        public int PlayerCount => _playerCount;

        [SerializeField] AudioMixerGroup _outputAudioMixerGroup;
        public AudioMixerGroup OutputAudioMixerGroup => _outputAudioMixerGroup;

        [SerializeField] bool _defaultLoop;
        public bool DefaultLoop => _defaultLoop;

        [SerializeField, Min(0)] float _defaultFadeDuration = 0.5f;
        public float DefaultFadeDuration => _defaultFadeDuration;

        [SerializeField, VolumeKey] List<string> _volumeKeys;
        public IReadOnlyList<string> VolumeKeys => _volumeKeys;
    }
}