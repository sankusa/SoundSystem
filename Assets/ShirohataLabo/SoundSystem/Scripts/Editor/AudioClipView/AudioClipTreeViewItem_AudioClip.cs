using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SoundSystem {
    public class AudioClipTreeViewItem_AudioClip : TreeViewItem {
        public AudioClip Clip { get; }

        public AudioClipTreeViewItem_AudioClip(int id, AudioClip clip) : base(id, 0, clip.name) {
            Clip = clip;
        }

        public void OnSingleClick() {
            Selection.activeObject = Clip;
        }

        public void OnDoubleClick() {
            EditorGUIUtility.PingObject(Clip);
        }
    }
}