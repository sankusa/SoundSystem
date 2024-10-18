using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    public class EditorSoundPlayer {
        GameObject _playerRoot;
        List<SoundPlayer> _players;
        int _currentPlayerIndex;
        SoundPlayer Player => _players[_currentPlayerIndex];
        SoundManagerConfig _soundManagerConfig;

        AudioUnit _audioUnit;

        bool _loop;

        double _oldTime;

        public void OnEnable() {
            CreateAudioSourceObject();
            _oldTime = Time.time;
        }

        public void OnDisable() {
            DestroyAudioSourceObject();
        }

        void Update() {
            double deltaTime = (float)(EditorApplication.timeSinceStartup - _oldTime);
            Player.Update((float)deltaTime);
            _oldTime = EditorApplication.timeSinceStartup;

            // エディタを滑らかに動かす
            if (Player.IsPlaying && Event.current.type == EventType.Repaint) {
                HandleUtility.Repaint();
            }
        }

        public void Bind(AudioUnit audioUnit) {
            _audioUnit = audioUnit;
        }

        void CreateAudioSourceObject() {
            _soundManagerConfig = EditorUtil.LoadAllAsset<SoundManagerConfig>().First();
            _playerRoot = new(nameof(EditorSoundPlayer)) {
                hideFlags = HideFlags.HideAndDontSave
            };
            _players = new();
            foreach (SoundPlayerGroupSetting setting in _soundManagerConfig.PlayerGroupSettings) {
                _players.Add(new SoundPlayer(_playerRoot, setting, new List<Volume>()));
            }
        }

        void DestroyAudioSourceObject() {
            if (_playerRoot == null) return;
            _players = null;
            Object.DestroyImmediate(_playerRoot);
        }

        public void DrawGUILayout() {
            Update();

            using (new EditorGUILayout.VerticalScope(new GUIStyle("GroupBox"))) {
                using (new EditorGUILayout.HorizontalScope()) {
                    GUILayout.FlexibleSpace();

                    EditorGUI.BeginChangeCheck();
                    _currentPlayerIndex = EditorGUILayout.Popup(_currentPlayerIndex, _soundManagerConfig.PlayerGroupSettings.Select(x => x.Key).ToArray(), GUILayout.Width(80));
                    if (EditorGUI.EndChangeCheck()) {
                        Player.Stop();
                    }

                    bool playToggleRet = GUILayout.Toggle(Player.IsPlaying, "", "Button", GUILayout.Width(19), GUILayout.Height(19));
                    EditorGUI.LabelField(GUILayoutUtility.GetLastRect(), new GUIContent(Skin.Instance.PlayIcon));
                    if (Player.IsPlaying == false && playToggleRet) {
                        Player.SetAudioUnit(_audioUnit).SetLoop(_loop).SetFadeIn().Play();
                    }
                    else if (Player.IsPlaying && playToggleRet == false) {
                        Player.Stop();
                    }

                    bool pauseToggleRet = GUILayout.Toggle(Player.IsPaused, "", "Button", GUILayout.Width(19), GUILayout.Height(19));
                    EditorGUI.LabelField(GUILayoutUtility.GetLastRect(), new GUIContent(Skin.Instance.PauseIcon));
                    if (Player.IsPaused == false && pauseToggleRet) {
                        Player.Pause();
                    }
                    else if (Player.IsPaused && pauseToggleRet == false) {
                        Player.Resume();
                    }

                    EditorGUI.BeginChangeCheck();
                    _loop = GUILayout.Toggle(_loop, "", "Button", GUILayout.Width(19), GUILayout.Height(19));
                    if (EditorGUI.EndChangeCheck()) {
                        Player.SetLoop(_loop);
                    }
                    EditorGUI.LabelField(GUILayoutUtility.GetLastRect(), new GUIContent(Skin.Instance.RepeatIcon));

                    EditorGUI.BeginChangeCheck();
                    float newAudioSourceTime = EditorGUILayout.Slider(Player.Time, 0, Player.ClipLength);
                    if (EditorGUI.EndChangeCheck()) {
                        // AudioSource.timeにAudioSource.clip.lengthを設定すると再生位置エラーになる
                        if (newAudioSourceTime < Player.ClipLength) {
                            Player.Time = newAudioSourceTime;
                        }
                    }
                }
            }
        }
    }
}