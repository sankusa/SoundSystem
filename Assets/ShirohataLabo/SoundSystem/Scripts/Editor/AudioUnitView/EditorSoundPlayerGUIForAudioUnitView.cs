using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    public class EditorSoundPlayerGUIForAudioUnitView {
        EditorSoundPlayer _editorPlayer;

        bool _playAuto;

        public void OnEnable() {
            _editorPlayer = new();
        }

        public void OnDisable() {
            _editorPlayer.Dispose();
        }

        public void Bind(AudioUnit audioUnit) {
            _editorPlayer.BindAudioUnit(audioUnit);
            if (_playAuto) _editorPlayer.Play();
        }

        public void DrawGUILayout() {
            _editorPlayer.Update();

            using (new EditorGUILayout.VerticalScope(GUIStyles.SimpleBox)) {
                if (_editorPlayer.AudioUnit == null) {
                    EditorGUILayout.LabelField("Not Selected");
                }
                else {
                    string assetPath = AssetDatabase.GetAssetPath(_editorPlayer.AudioUnit);
                    Texture icon = AssetDatabase.GetCachedIcon(assetPath);
                    EditorGUILayout.LabelField(new GUIContent(_editorPlayer.AudioUnit.name, icon));
                }

                _editorPlayer.DrawTimeSlider();
                using (new EditorGUILayout.HorizontalScope()) {
                    _editorPlayer.DrawPlayerGroupSelectPopup();
                    _editorPlayer.DrawPlayButton(GUILayout.Width(19), GUILayout.Height(19));
                    _editorPlayer.DrawPauseButton(GUILayout.Width(19), GUILayout.Height(19));
                    _editorPlayer.DrawLoopButton(GUILayout.Width(19), GUILayout.Height(19));
                    GUILayout.FlexibleSpace();

                    EditorGUI.BeginChangeCheck();
                    using (new LabelWidthScope(32)) {
                        _playAuto = EditorGUILayout.Toggle("Auto", _playAuto);
                    }
                    if (EditorGUI.EndChangeCheck()) {
                        if (_playAuto && _editorPlayer.Player.IsPlayStarted == false) {
                            _editorPlayer.Play();
                        }
                    }
                }
            }
        }
    }
}