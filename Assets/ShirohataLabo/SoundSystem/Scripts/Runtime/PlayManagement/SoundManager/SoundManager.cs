using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SoundSystem {
    [DefaultExecutionOrder(-90)]
    public partial class SoundManager : MonoBehaviour {
        public static SoundManager Instance { get; private set; }

        [SerializeField] SoundManagerConfig _config;

        readonly List<SoundPlayerGroup> _playerGroups = new();
        public IReadOnlyList<SoundPlayerGroup> PlayerGroups => _playerGroups;

        List<Volume> _volumes;
        public IReadOnlyList<Volume> Volumes => _volumes;

        void Awake() {
            if (Instance != null) {
                Debug.LogWarning($"{nameof(SoundManager)} already exists");
                Destroy(this);
                return;
            }

            if (_config == null) {
                Debug.LogError($"{nameof(SoundManagerConfig)} is null");
                return;
            }

            _volumes = _config.VolumeSettings.Select(x => new Volume(x)).ToList();

            foreach (SoundPlayerGroupSetting playerSetting in _config.PlayerGroupSettings) {
                SoundPlayerGroup playerGroup = new SoundPlayerGroup(gameObject, playerSetting, _volumes);
                _playerGroups.Add(playerGroup);
            }

            Instance = this;
        }

        void FixedUpdate() {
            foreach (SoundPlayerGroup playerGroup in _playerGroups) {
                playerGroup.Update(Time.deltaTime);
            }
        }

        public SoundPlayerGroup FindPlayerGroup(string groupKey) {
            foreach (SoundPlayerGroup group in _playerGroups) {
                if (group.GroupKey == groupKey) return group;
            }
            return null;
        }

        public Volume FindVolume(string volumeKey) {
            foreach (Volume volume in _volumes) {
                if (volume.Key == volumeKey) return volume;
            }
            return null;
        }
    }
}