using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace SoundSystem {
    [CustomEditor(typeof(SoundContainer))]
    public class SoundContainerInspector : Editor {
        

        public override void OnInspectorGUI() {
            serializedObject.Update();
            
            SoundContainer container = target as SoundContainer;

            SerializedProperty soundDicProp = serializedObject.FindProperty("_soundDic");

            bool preload = SoundContainerPreloader.InstanceForEditor.ContainsForEditor(container);
            bool newPreload = EditorGUILayout.Toggle("Preload", preload);
            if (newPreload != preload) {
                SoundContainerPreloader.InstanceForEditor.UpdatePreloadStateForEditor(container, newPreload);
            }

            EditorGUILayout.Separator();

            // EditorGUILayout.LabelField("Sound List");



            EditorGUILayout.PropertyField(soundDicProp);

            serializedObject.ApplyModifiedProperties();
        }
    }
}