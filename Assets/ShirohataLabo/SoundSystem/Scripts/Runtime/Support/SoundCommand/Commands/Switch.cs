using System;
using UnityEngine;
using UnityEngine.Events;

namespace SoundSystem.SoundCommands {
    public class Switch : ISoundCommand {
        [SerializeField, SoundPlayerGroupKey] string _groupKey;
        [SerializeField] SoundSelector _sound;
        [SerializeField] UnityEvent _onComplete;

        public void Execute() {
            Action onComplete = _onComplete.GetPersistentEventCount() == 0 ? null : () => _onComplete.Invoke();

            SoundManager
                .Instance
                .FindPlayerGroup(_groupKey)
                .Switch(_sound.Resolve(), onComplete);
        }
    }
}