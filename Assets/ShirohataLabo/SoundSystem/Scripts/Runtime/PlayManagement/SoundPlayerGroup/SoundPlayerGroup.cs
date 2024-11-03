using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SoundSystem {
    public partial class SoundPlayerGroup {
        public string GroupKey { get; }
        readonly List<SoundPlayer> _players = new();
        public IReadOnlyList<SoundPlayer> Players => _players;

        public SoundPlayerGroupStatus Status { get; }

        public SoundPlayerGroup(GameObject parentObject, SoundPlayerGroupSetting groupSetting, List<Volume> allVolumes) {
            List<Volume> volumes = allVolumes.Where(x => groupSetting.VolumeKeys.Contains(x.Key)).ToList();
            Status = new SoundPlayerGroupStatus(groupSetting, volumes);

            GroupKey = Status.Setting.Key;

            GameObject gameObject = new GameObject(groupSetting.Key);
            gameObject.transform.SetParent(parentObject.transform);
            for (int i = 0; i < Status.Setting.PlayerCount; i++) {
                _players.Add(new SoundPlayer(gameObject, Status));
            }
        }

        public void Update(float deltaTime) {
            foreach (SoundPlayer player in _players) {
                player.Update(deltaTime);
            }
        }

        public SoundPlayer GetUnusedPlayer() {
            SoundPlayer target = null;
            foreach(SoundPlayer player in _players) {
                if(player.IsUsing == false) {
                    target = player;
                    break;
                }
            }
            target ??= _players[0];
            target.Reset();
            // 再生したプレイヤーは最後尾にすることで、再生開始が一番古いものが先頭に来る
            _players.Remove(target);
            _players.Add(target);
            target.StartUsing();
            return target;
        }
    }
}