using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SoundSystem {
    public static class InspectorUtil {
        public static void OpenAnotherInspector(Object obj) {
            // 描画の途中で呼び出すとserializedObjectに何かしらが起こって、描画エラーが起こる場合がある(そのフレームだけ)
            EditorApplication.delayCall += () => {
                OpenAnotherInspectorInternal(obj);
            };
        }

        static void OpenAnotherInspectorInternal(Object obj) {
            if(obj == null) {
                Debug.Log("Object is null.");
                return;
            }
            
            Object[] originalSelections = Selection.objects;
            Selection.objects = new Object[] {obj};

            Type inspectorWindowType = Assembly.Load("UnityEditor").GetType("UnityEditor.InspectorWindow");
            EditorWindow inspectorWindow = EditorWindow.CreateInstance(inspectorWindowType) as EditorWindow;

            PropertyInfo isLockedPropertyInfo = inspectorWindowType.GetProperty("isLocked", BindingFlags.Public | BindingFlags.Instance);
            isLockedPropertyInfo.SetValue(inspectorWindow, true);

            Selection.objects = originalSelections;

            inspectorWindow.Show(true);
        }
    }
}