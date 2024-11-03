using UnityEngine;

namespace SoundSystem.SoundCommands {
    public class Stop : ISoundCommand {
        [SerializeField, SoundPlayerGroupKey] string _groupKey;

        public void Execute() {
            SoundManager.Instance.FindPlayerGroup(_groupKey).Stop();
        }
    }
}