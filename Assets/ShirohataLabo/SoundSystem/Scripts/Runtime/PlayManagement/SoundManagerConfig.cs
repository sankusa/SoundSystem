using System.Collections.Generic;
using UnityEngine;

namespace SoundSystem {
    [CreateAssetMenu(fileName = nameof(SoundManagerConfig), menuName = nameof(SoundSystem) + "/Develop/" + nameof(SoundManagerConfig))]
    public class SoundManagerConfig : ScriptableObject {
        [SerializeField, ShowOnlyChild] List<SoundPlayerGroupSetting> _playerGroupSettings;
        public IReadOnlyList<SoundPlayerGroupSetting> PlayerGroupSettings => _playerGroupSettings;

        [SerializeField, ShowOnlyChild] List<VolumeSetting> _volumeSettings;
        public IReadOnlyList<VolumeSetting> VolumeSettings => _volumeSettings;
    }
}