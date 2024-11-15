using System;
using UnityEngine;
using System.Linq;

namespace SoundSystem {
    [CreateAssetMenu(fileName = nameof(PlayableObjectDatabase), menuName = nameof(SoundSystem) + "/Develop/" + nameof(PlayableObjectDatabase))]
    public class PlayableObjectDatabase : ObjectDatabase<PlayableObjectDatabase> {
        protected override bool CanAccept(Type type) {
            return PlayableObjectTypes.Types.Contains(type);
        }
    }
}