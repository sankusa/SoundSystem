using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

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

#if UNITY_EDITOR
        public static SerializedProperty GetActiveProp(SerializedProperty soundBehaviourProp) {
            return soundBehaviourProp.FindPropertyRelative(nameof(_active));
        }
#endif
    }
}