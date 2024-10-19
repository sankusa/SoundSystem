using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace SoundSystem {
    public class AudioUnitTreeViewItem_AudioUnit : TreeViewItem {
        public AudioUnit AudioUnit { get; }

        public string Label => AudioUnit.name;

        public AudioUnitTreeViewItem_AudioUnit(AudioUnit audioUnit) : base(0, 0, audioUnit != null ? audioUnit.name : null) {
            AudioUnit = audioUnit;
            displayName = audioUnit.name;
            id = audioUnit.GetInstanceID();
        }

        public void OnSingleClick() {
            
        }

        public void OnDoubleClick() {
            AssetDatabase.OpenAsset(AudioUnit);
            EditorGUIUtility.PingObject(AudioUnit);
        }
    }
}