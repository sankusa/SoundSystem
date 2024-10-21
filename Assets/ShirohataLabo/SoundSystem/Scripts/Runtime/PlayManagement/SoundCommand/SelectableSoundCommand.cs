using System;
using UnityEngine;

namespace SoundSystem {
    [Serializable]
    public class SelectableSoundCommand {
        [SerializeReference, SerializeReferencePopup] ISoundCommand _command = new Play();

        public void BindTransformIfPossible(Transform transform) {
            if (_command is ITransformBinder transformBinder) {
                transformBinder.BindTransform(transform);
            }
        }

        public void Execute() {
            _command?.Execute();
        }
    }
}