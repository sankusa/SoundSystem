using System.Collections.Generic;

namespace SoundSystem {
    public class SoundCache {
        static SoundCache _instance;
        public static SoundCache Instance => _instance ??= new SoundCache();

        List<SoundContainer> _soundContainers = new();
        public IReadOnlyList<SoundContainer> SoundContainers => _soundContainers;

        public void AddContainer(SoundContainer container) {
            if (container == null) return;
            _soundContainers.Add(container);
        }
        public void AddContainers(List<SoundContainer> containers) {
            foreach (SoundContainer container in containers) {
                AddContainer(container);
            }
        }

        public void RemoveContainer(SoundContainer container) {
            _soundContainers.Remove(container);
        }
        public void RemoveContainers(List<SoundContainer> containers) {
            foreach (SoundContainer container in containers) {
                RemoveContainer(container);
            }
        }

        public Sound FindSoundByKey(string soundKey) {
            // 他コンテナに重複するキーがある場合、後に追加されたコンテナから取得
            for (int i = _soundContainers.Count - 1; i >= 0; i--) {
                Sound sound = _soundContainers[i].FindSoundByKey(soundKey);
                if (sound != null) {
                    return sound;
                }
            }
            return null;
        }
    }
}