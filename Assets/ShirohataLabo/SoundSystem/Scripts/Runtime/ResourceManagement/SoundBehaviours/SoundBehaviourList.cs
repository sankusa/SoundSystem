using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SoundSystem {
    [Serializable]
    public class SoundBehaviourList : SoundBehaviour {
        [SerializeReference] List<SoundBehaviour> _behaviours = new();

        public override void OnUpdate(SoundPlayer player, float deltaTime) {
            foreach (SoundBehaviour behaviour in _behaviours) {
                behaviour.OnUpdate(player, deltaTime);
            }
        }

        public override void OnReset(SoundPlayer player) {
            foreach (SoundBehaviour behaviour in _behaviours) {
                behaviour.OnReset(player);
            }
        }

        public override void OnPause(SoundPlayer player) {
            foreach (SoundBehaviour behaviour in _behaviours) {
                behaviour.OnPause(player);
            }
        }

        public override void OnResume(SoundPlayer player) {
            foreach (SoundBehaviour behaviour in _behaviours) {
                behaviour.OnResume(player);
            }
        }

#if UNITY_EDITOR
        public static SerializedProperty GetBehavioursProp(SerializedProperty soundProp) {
            return soundProp.FindPropertyRelative(nameof(_behaviours));
        }
#endif
    }
}