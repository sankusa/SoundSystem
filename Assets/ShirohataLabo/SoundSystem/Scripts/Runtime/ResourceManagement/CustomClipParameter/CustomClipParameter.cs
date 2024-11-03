using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SoundSystem {
    [Serializable]
    public abstract class CustomClipParameter {
        [SerializeField] protected bool _enable;
        public bool Enable => _enable;

#if UNITY_EDITOR
        public static SerializedProperty GetEnableProp(SerializedProperty customClipParameterProp) {
            return customClipParameterProp.FindPropertyRelative(nameof(_enable));
        }
#endif
    }
}