using UnityEngine;

namespace SoundSystem {
    public class Stop : ISoundCommand {
        [SerializeField, SoundPlayerGroupKey] string _groupKey;

        public void Execute() {
            SoundManager.Instance.FindPlayerGroup(_groupKey).Stop();
        }
    }
}