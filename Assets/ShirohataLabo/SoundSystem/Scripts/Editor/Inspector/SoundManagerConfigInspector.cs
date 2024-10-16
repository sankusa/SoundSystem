using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    [CustomEditor(typeof(SoundManagerConfig))]
    public class SoundManagerConfigInspector : Editor {
        public override void OnInspectorGUI() {
            if (GUILayout.Button("Generate SoundManager_Helper Script")) {
                ScriptGenerator.GenerateSoundManagerHelperScript(target as SoundManagerConfig);
            }
            if (GUILayout.Button("Generate SoundPlayerGroupKey Enum")) {
                ScriptGenerator.GenerateSoundPlayerGroupKeyScript(target as SoundManagerConfig);
            }
            base.OnInspectorGUI();
        }
    }
}