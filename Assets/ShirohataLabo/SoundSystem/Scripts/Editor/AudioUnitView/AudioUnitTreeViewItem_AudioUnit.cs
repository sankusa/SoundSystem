using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace SoundSystem {
    public class AudioUnitTreeViewItem_AudioUnit : TreeViewItem {
        public AudioUnit AudioUnit { get; }

        public string Label => AudioUnit.name;

        public AudioUnitTreeViewItem_AudioUnit(int id, AudioUnit audioUnit) : base(id, 0, audioUnit != null ? audioUnit.name : null) {
            AudioUnit = audioUnit;
            displayName = audioUnit.name;
        }

        public void OnSingleClick() {
            AssetDatabase.OpenAsset(AudioUnit);
        }

        public void OnDoubleClick() {
            EditorGUIUtility.PingObject(AudioUnit);
        }
    }
}