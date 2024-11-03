using System;
using SoundSystem.SoundCommands;
using UnityEngine;

namespace SoundSystem {
    [Serializable]
    public class SelectableSoundCommand {
        [SerializeReference, SerializeReferencePopup] ISoundCommand _command = new Play();

        public void Execute() {
            _command?.Execute();
        }
    }
}