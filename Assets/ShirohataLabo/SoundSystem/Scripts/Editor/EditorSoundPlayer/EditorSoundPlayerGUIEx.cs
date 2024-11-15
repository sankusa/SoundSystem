using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    public class EditorSoundPlayerGUIEx {
        EditorSoundPlayer _editorPlayer;

        bool _playAuto;

        public void OnEnable() {
            _editorPlayer = new();
        }

        public void OnDisable() {
            _editorPlayer.Dispose();
        }

        public void Bind(Object target) {
            if (target is AudioClip audioClip) {
                _editorPlayer.Bind(audioClip);
                if (_playAuto) _editorPlayer.Play();
            }
            else if (target is CustomClip customClip) {
                _editorPlayer.Bind(customClip);
                if (_playAuto) _editorPlayer.Play();
            }
        }

        public void DrawGUILayout() {
            _editorPlayer.Update();

            using (new EditorGUILayout.VerticalScope(GUIStyles.SimpleBox)) {
                if (_editorPlayer.AudioClip != null) {
                    string assetPath = AssetDatabase.GetAssetPath(_editorPlayer.AudioClip);
                    Texture icon = AssetDatabase.GetCachedIcon(assetPath);
                    EditorGUILayout.LabelField(new GUIContent(_editorPlayer.AudioClip.name, icon));
                }
                else if (_editorPlayer.CustomClip != null) {
                    string assetPath = AssetDatabase.GetAssetPath(_editorPlayer.CustomClip);
                    Texture icon = AssetDatabase.GetCachedIcon(assetPath);
                    EditorGUILayout.LabelField(new GUIContent(_editorPlayer.CustomClip.name, icon));
                }
                else {
                    EditorGUILayout.LabelField("Not Selected");
                }

                
                using (new EditorGUILayout.HorizontalScope()) {
                    
                    _editorPlayer.DrawLayoutPlayButton(GUILayout.Width(19), GUILayout.Height(19));
                    _editorPlayer.DrawLayoutPauseButton(GUILayout.Width(19), GUILayout.Height(19));
                    _editorPlayer.DrawLayoutLoopButton(GUILayout.Width(19), GUILayout.Height(19));

                    _editorPlayer.DrawLayoutTimeSlider(GUILayout.MinWidth(100));

                    _editorPlayer.DrawLayoutPlayerGroupSelectPopup(GUILayout.Width(100));

                    EditorGUI.BeginChangeCheck();
                    _playAuto = GUILayout.Toggle(_playAuto, "", "Button", GUILayout.Width(19));
                    EditorGUI.LabelField(GUILayoutUtility.GetLastRect(), new GUIContent(Icons.AutoPlayIcon));
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