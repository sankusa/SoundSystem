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
            _editorPlayer.Bind(audioUnit);
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

                _editorPlayer.DrawLayoutTimeSlider();
                using (new EditorGUILayout.HorizontalScope()) {
                    _editorPlayer.DrawLayoutPlayerGroupSelectPopup();
                    _editorPlayer.DrawLayoutPlayButton(GUILayout.Width(19), GUILayout.Height(19));
                    _editorPlayer.DrawLayoutPauseButton(GUILayout.Width(19), GUILayout.Height(19));
                    _editorPlayer.DrawLayoutLoopButton(GUILayout.Width(19), GUILayout.Height(19));

                    EditorGUI.BeginChangeCheck();
                    _playAuto = GUILayout.Toggle(_playAuto, "", "Button", GUILayout.Width(19));
                    EditorGUI.LabelField(GUILayoutUtility.GetLastRect(), new GUIContent(Icons.AutoPlayIcon));
                    if (EditorGUI.EndChangeCheck()) {
                        if (_playAuto && _editorPlayer.Player.IsPlayStarted == false) {
                            _editorPlayer.Play();
                        }
                    }

                    GUILayout.FlexibleSpace();
                }
            }
        }
    }
}