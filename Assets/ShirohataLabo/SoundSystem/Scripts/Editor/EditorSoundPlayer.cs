using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SoundSystem {
    public class EditorSoundPlayer : IDisposable {
        GameObject _playerRoot;
        List<SoundPlayer> _players;
        int _currentPlayerIndex;
        public int CurrentPlayerIndex => _currentPlayerIndex;
        public SoundPlayer Player => _players[_currentPlayerIndex];
        SoundManagerConfig _soundManagerConfig;
        public SoundManagerConfig SoundManagerConfig => _soundManagerConfig;

        double _oldTime;

        AudioUnit _audioUnit;
        public AudioUnit AudioUnit => _audioUnit;

        bool _loop;

        public EditorSoundPlayer() {
            CreateAudioSourceObject();
            _oldTime = Time.time;
            SwitchPlayerIndex(0);
        }

        public void Dispose() {
            DestroyAudioSourceObject();
        }

        public void Update() {
            if (Player.AudioUnit != _audioUnit) Player.SetAudioUnit(_audioUnit);

            double deltaTime = (float)(EditorApplication.timeSinceStartup - _oldTime);
            Player.Update((float)deltaTime);
            _oldTime = EditorApplication.timeSinceStartup;

            // エディタを滑らかに動かす
            if (Player.IsPlaying && Event.current.type == EventType.Repaint) {
                HandleUtility.Repaint();
            }
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

        public void BindAudioUnit(AudioUnit audioUnit) {
            _audioUnit = audioUnit;
            Player.Reset();
            Player.SetAudioUnit(_audioUnit);
        }

        public void SwitchPlayerIndex(int index) {
            _currentPlayerIndex = index;
            Player.Stop();
        }

        public void Play() {
            if (_audioUnit == null) return;
            Player.SetAudioUnit(_audioUnit).SetLoop(_loop).Play();
        }

        public void DrawPlayerGroupSelectPopup(params GUILayoutOption[] options) {
            EditorGUI.BeginChangeCheck();
            int currentPlayerIndex = EditorGUILayout.Popup(CurrentPlayerIndex, _soundManagerConfig.PlayerGroupSettings.Select(x => new GUIContent(x.Key, "SoundPlayerGroup")).ToArray(), options);
            if (EditorGUI.EndChangeCheck()) {
                SwitchPlayerIndex(currentPlayerIndex);
            }
        }

        public void DrawPlayButton(params GUILayoutOption[] options) {
            bool playToggleRet = GUILayout.Toggle(Player.IsPlayStarted, "", "Button", options);
            EditorGUI.LabelField(GUILayoutUtility.GetLastRect(), new GUIContent(Icons.PlayIcon));
            if (Player.IsPlayStarted == false && playToggleRet) {
                Play();
            }
            else if (Player.IsPlayStarted && playToggleRet == false) {
                Player.Stop();
            }
        }

        public void DrawPauseButton(params GUILayoutOption[] options) {
            bool pauseToggleRet = GUILayout.Toggle(Player.IsPaused, "", "Button", options);
            EditorGUI.LabelField(GUILayoutUtility.GetLastRect(), new GUIContent(Icons.PauseIcon));
            if (Player.IsPaused == false && pauseToggleRet) {
                Player.Pause();
            }
            else if (Player.IsPaused && pauseToggleRet == false) {
                Player.Resume();
            }
        }

        public void DrawLoopButton(params GUILayoutOption[] options) {
            EditorGUI.BeginChangeCheck();
            _loop = GUILayout.Toggle(_loop, "", "Button", options);
            if (EditorGUI.EndChangeCheck()) {
                Player.SetLoop(_loop);
            }
            EditorGUI.LabelField(GUILayoutUtility.GetLastRect(), new GUIContent(Icons.RepeatIcon));
        }

        public void DrawTimeSlider(params GUILayoutOption[] options) {
            EditorGUI.BeginChangeCheck();
            float clipLength = Player.Clip == null ? 0 : Player.Clip.length;
            float newAudioSourceTime = EditorGUILayout.Slider(Player.Time, 0, clipLength, options);
            if (EditorGUI.EndChangeCheck()) {
                // AudioSource.timeにAudioSource.clip.lengthを設定すると再生位置エラーになる
                if (newAudioSourceTime < clipLength) {
                    Player.SetTime(newAudioSourceTime);
                }
            }
        }
    }
}