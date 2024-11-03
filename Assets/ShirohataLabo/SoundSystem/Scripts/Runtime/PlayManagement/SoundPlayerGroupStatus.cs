
using System.Collections.Generic;

namespace SoundSystem {
    public class SoundPlayerGroupStatus {
        public SoundPlayerGroupSetting Setting { get; }

        public List<SoundBehaviour> BaseSoundBehaviours = new();

        public SoundPlayerGroupStatus(SoundPlayerGroupSetting setting) {
            Setting = setting;
        }
    }
}