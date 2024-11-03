using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SoundSystem {
    [Serializable]
    public abstract partial class SoundBehaviour {
        [SerializeField] protected bool _active = true;
        public bool Active {
            get => _active;
            set => _active = value;
        }

        public virtual void OnUpdate(SoundPlayer player, float deltaTime) {
            if (_active == false) return;
            OnUpdateIfActive(player, deltaTime);
        }
        protected virtual void OnUpdateIfActive(SoundPlayer player, float deltaTime) {}

        public virtual void OnReset(SoundPlayer player) {
            if (_active == false) return;
            OnResetIfActive(player);
        }
        protected virtual void OnResetIfActive(SoundPlayer player) {}

        public virtual void OnPause(SoundPlayer player) {
            if (_active == false) return;
            OnPauseIfActive(player);
        }
        protected virtual void OnPauseIfActive(SoundPlayer player) {}

        public virtual void OnResume(SoundPlayer player) {
            if (_active == false) return;
            OnResumeIfActive(player);
        }
        protected virtual void OnResumeIfActive(SoundPlayer player) {}

#if UNITY_EDITOR
        public static SerializedProperty GetActiveProp(SerializedProperty soundBehaviourProp) {
            return soundBehaviourProp.FindPropertyRelative(nameof(_active));
        }
#endif
    }
}