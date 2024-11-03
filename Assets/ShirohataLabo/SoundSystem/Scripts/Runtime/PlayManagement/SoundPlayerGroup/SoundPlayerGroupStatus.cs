
using System.Collections.Generic;

namespace SoundSystem {
    public class SoundPlayerGroupStatus {
        public SoundPlayerGroupSetting Setting { get; }
        public IReadOnlyList<Volume> Volumes { get; }

        public List<SoundBehaviour> BaseSoundBehaviours = new();

        public SoundPlayerGroupStatus(SoundPlayerGroupSetting setting, List<Volume> volumes) {
            Setting = setting;
            Volumes = volumes;
        }
    }
}