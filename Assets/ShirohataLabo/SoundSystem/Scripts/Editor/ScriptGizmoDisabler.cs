using System;
using System.Reflection;
using UnityEditor;

namespace SoundSystem {
    [InitializeOnLoad]
    public static class ScriptGizmoDisabler {
        const int monoBehaviourClassId = 114; // https://docs.unity3d.com/Manual/ClassIDReference.html

        static Type _annotationUtility = Assembly
            .GetAssembly(typeof(Editor))
            .GetType("UnityEditor.AnnotationUtility");

        static MethodInfo _setIconEnabled = _annotationUtility
            .GetMethod(
                "SetIconEnabled",
                BindingFlags.Static | BindingFlags.NonPublic,
                null,
                new Type[] { typeof(int), typeof(string), typeof(int) },
                null
            );

        static MethodInfo _getAnnotations = _annotationUtility.GetMethod("GetAnnotations",
            BindingFlags.Static | BindingFlags.NonPublic,
             null,
             new Type[] { },
             null);

        static ScriptGizmoDisabler() {
            _getAnnotations.Invoke(null, new object[] {});
            SetGizmoIconEnabled(typeof(SoundCommandExecuter), false);
            SetGizmoIconEnabled(typeof(SoundCacheRegistrar), false);
            SetGizmoIconEnabled(typeof(SoundManager), false);
            SetGizmoIconEnabled(typeof(VolumeSlider), false);
            SetGizmoIconEnabled(typeof(BaseSoundRegistrar), false);
        }

        public static void SetGizmoIconEnabled( Type type, bool on ) {
            _setIconEnabled.Invoke( null, new object[] { monoBehaviourClassId, type.Name, on ? 1 : 0 } );
        }
    }
}