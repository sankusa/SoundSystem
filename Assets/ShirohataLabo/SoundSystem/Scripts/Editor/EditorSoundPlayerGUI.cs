using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    public class EditorSoundPlayerGUI {
        EditorSoundPlayer _editorPlayer;

        public void OnEnable() {
            _editorPlayer = new();
        }

        public void OnDisable() {
            _editorPlayer.Dispose();
        }

        public void Bind(AudioUnit audioUnit) {
            _editorPlayer.BindAudioUnit(audioUnit);
        }

        public void DrawGUILayout() {
            _editorPlayer.Update();

            using (new EditorGUILayout.VerticalScope(GUIStyles.SimpleBox)) {
                using (new EditorGUILayout.HorizontalScope()) {
                    _editorPlayer.DrawPlayerGroupSelectPopup(GUILayout.Width(80));
                    _editorPlayer.DrawPlayButton(GUILayout.Width(19), GUILayout.Height(19));
                    _editorPlayer.DrawPauseButton(GUILayout.Width(19), GUILayout.Height(19));
                    _editorPlayer.DrawLoopButton(GUILayout.Width(19), GUILayout.Height(19));
                    _editorPlayer.DrawTimeSlider();
                }
            }
        }
    }
}