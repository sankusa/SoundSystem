using System;
using UnityEngine;

namespace SoundSystem {
    [Serializable]
    public abstract class SoundBehaviour {
        [SerializeField] protected bool _active = true;
        public bool Active {
            get => _active;
            set => _active = value;
        }

        public void ApplyTo(SoundPlayer player) {
            if (_active == false) return;
            ApplyMain(player);
        }

        protected abstract void ApplyMain(SoundPlayer player);
    }
}