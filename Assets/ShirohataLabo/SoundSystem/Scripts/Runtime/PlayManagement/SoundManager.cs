using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SoundSystem {
    [DefaultExecutionOrder(-90)]
    public partial class SoundManager : MonoBehaviour {
        static SoundManager _instance;
        public static SoundManager Instance => _instance;

        [SerializeField] SoundManagerConfig _config;

        readonly List<SoundPlayerGroup> _playerGroups = new();
        public IReadOnlyList<SoundPlayerGroup> PlayerGroups => _playerGroups;

        List<Volume> _volumes;
        public IReadOnlyList<Volume> Volumes => _volumes;

        void Awake() {
            if (_instance != null) {
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

            _instance = this;
        }

        void FixedUpdate() {
            foreach (SoundPlayerGroup playerGroup in _playerGroups) {
                playerGroup.Update(Time.deltaTime);
            }
        }

        public SoundPlayerGroup FindPlayerGroup(string groupKey) {
            return _playerGroups.Find(x => x.GroupKey == groupKey);
        }

        public Volume FindVolume(string volumeKey) {
            return _volumes.Find(x => x.Key == volumeKey);
        }
    }
}