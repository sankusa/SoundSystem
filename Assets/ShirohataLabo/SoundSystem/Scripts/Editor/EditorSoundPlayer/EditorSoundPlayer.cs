using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SoundSystem {
    public class EditorSoundPlayer : IDisposable {
        const string volumeSessionStateKey = nameof(EditorSoundPlayer) + nameof(_sharedVolume);
        static Volume _sharedVolume = new(new VolumeSetting());

        // PropertyDrawer等、Disposeを呼べない場合にGameObjectを破棄するための対応
        // ファイナライザからUnityAPIは呼べないらしいので、ファイナライザでStatic領域にGameObjectを渡してUpdateのタイミングで破棄
        static EditorSoundPlayer() {
            EditorApplication.update += () => {
                foreach (GameObject gameObject in _objectsToDestroy) {
                    Object.DestroyImmediate(gameObject);
                }
                _objectsToDestroy.Clear();
            };

            _sharedVolume.Value = SessionState.GetFloat(volumeSessionStateKey, 1);
        }
        static List<GameObject> _objectsToDestroy = new();

        GameObject _playerRoot;
        List<SoundPlayer> _players;
        int _currentPlayerIndex;
        public int CurrentPlayerIndex => _currentPlayerIndex;
        public SoundPlayer Player => _players[_currentPlayerIndex];
        SoundManagerConfig _soundManagerConfig;
        public SoundManagerConfig SoundManagerConfig => _soundManagerConfig;

        double _oldTime;

        AudioClip _audioClip;
        public AudioClip AudioClip => _audioClip;

        CustomClip _customClip;
        public CustomClip CustomClip => _customClip;

        Sound _sound;
        public Sound Sound => _sound;

        bool _loop;

        public EditorSoundPlayer() {
            CreateAudioSourceObject();
            _oldTime = Time.time;
            SwitchPlayerIndex(0);
        }

        ~EditorSoundPlayer() {
            _objectsToDestroy.Add(_playerRoot);
        }

        public void Dispose() {
            DestroyAudioSourceObject();
        }

        public void Bind(AudioClip audioClip) {
            _audioClip = audioClip;
            _customClip = null;
            _sound = null;
            Player.Reset();
        }

        public void Bind(CustomClip customClip) {
            _customClip = customClip;
            _audioClip = null;
            _sound = null;
            Player.Reset();
        }

        public void Bind(Sound sound) {
            _sound = sound;
            _audioClip = null;
            _customClip = null;
            Player.Reset();
        }

        public void Update() {
            double deltaTime = EditorApplication.timeSinceStartup - _oldTime;
            Player.Update((float)deltaTime);
            _oldTime = EditorApplication.timeSinceStartup;

            // エディタを滑らかに動かす
            if ((Player.IsPlaying || Player.IsPaused) && Event.current.type == EventType.Repaint) {
                HandleUtility.Repaint();
            }
        }

        public void Stop() {
            Player.Stop();
        }

        public void Pause() {
            Player.Pause();
        }

        public void Resume() {
            Player.Resume();
        }

        void CreateAudioSourceObject() {
            _soundManagerConfig = EditorUtil.LoadAllAsset<SoundManagerConfig>().First();
            _playerRoot = new(nameof(EditorSoundPlayer)) {
                hideFlags = HideFlags.HideAndDontSave
            };
            _players = new();
            foreach (SoundPlayerGroupSetting setting in _soundManagerConfig.PlayerGroupSettings) {
                _players.Add(new SoundPlayer(_playerRoot, new SoundPlayerGroupStatus(setting, new List<Volume>(){_sharedVolume})));
            }
        }

        void DestroyAudioSourceObject() {
            if (_playerRoot == null) return;
            _players = null;
            Object.DestroyImmediate(_playerRoot);
        }

        public void SwitchPlayerIndex(int index) {
            Player.Stop();
            _currentPlayerIndex = index;
        }

        public void Play() {
            if (EditorApplication.isPlaying) return;
            if (_audioClip != null) {
                Player.SetAudioClip(_audioClip).SetLoop(_loop).Play();
            }
            else if (_customClip != null) {
                Player.SetCustomClip(_customClip).SetLoop(_loop).Play();
            }
            else if (_sound != null) {
                Player.SetSound(_sound).SetLoop(_loop).Play();
            }
        }

        public void DrawPlayerGroupSelectPopup(Rect rect) {
            EditorGUI.BeginChangeCheck();
            int currentPlayerIndex = EditorGUI.Popup(rect, CurrentPlayerIndex, _soundManagerConfig.PlayerGroupSettings.Select(x => new GUIContent(x.Key, "SoundPlayerGroup")).ToArray());
            if (EditorGUI.EndChangeCheck()) {
                SwitchPlayerIndex(currentPlayerIndex);
            }
        }

        public void DrawLayoutPlayerGroupSelectPopup(params GUILayoutOption[] options) {
            DrawPlayerGroupSelectPopup(EditorGUILayout.GetControlRect(options));
        }

        public void DrawPlayButton(Rect rect) {
            bool playToggleRet = GUI.Toggle(rect, Player.IsPlayStarted, "", "Button");
            EditorGUI.LabelField(rect, new GUIContent(Icons.PlayIcon));
            if (Player.IsPlayStarted == false && playToggleRet) {
                Play();
            }
            else if (Player.IsPlayStarted && playToggleRet == false) {
                Player.Stop();
            }
        }

        public void DrawLayoutPlayButton(params GUILayoutOption[] options) {
            DrawPlayButton(EditorGUILayout.GetControlRect(options));
        }

        public void DrawPauseButton(Rect rect) {
            bool pauseToggleRet = GUI.Toggle(rect, Player.IsPaused, "", "Button");
            EditorGUI.LabelField(rect, new GUIContent(Icons.PauseIcon));
            if (Player.IsPaused == false && pauseToggleRet) {
                Player.Pause();
            }
            else if (Player.IsPaused && pauseToggleRet == false) {
                Player.Resume();
            }
        }

        public void DrawLayoutPauseButton(params GUILayoutOption[] options) {
            DrawPauseButton(EditorGUILayout.GetControlRect(options));
        }

        public void DrawLoopButton(Rect rect) {
            EditorGUI.BeginChangeCheck();
            _loop = GUI.Toggle(rect, _loop, "", "Button");
            if (EditorGUI.EndChangeCheck()) {
                Player.SetLoop(_loop);
            }
            EditorGUI.LabelField(rect, new GUIContent(Icons.RepeatIcon));
        }

        public void DrawLayoutLoopButton(params GUILayoutOption[] options) {
            DrawLoopButton(EditorGUILayout.GetControlRect(options));
        }

        public void DrawTimeSlider(Rect rect) {
            EditorGUI.BeginChangeCheck();
            float clipLength = Player.AudioClip == null ? 0 : Player.AudioClip.length;
            float newAudioSourceTime = EditorGUI.Slider(rect, Player.Time, 0, clipLength);
            if (EditorGUI.EndChangeCheck()) {
                // AudioSource.timeにAudioSource.clip.lengthを設定すると再生位置エラーになる
                if (newAudioSourceTime < clipLength) {
                    Player.SetTime(newAudioSourceTime);
                }
            }
        }

        public void DrawLayoutTimeSlider(params GUILayoutOption[] options) {
            DrawTimeSlider(EditorGUILayout.GetControlRect(options));
        }

        public void DrawVolumeField(Rect rect) {
            using (new LabelWidthScope(16)) {
                EditorGUI.BeginChangeCheck();
                _sharedVolume.Value = Mathf.Clamp01(
                    EditorGUI.FloatField(rect, new GUIContent(Icons.VolumeIcon), _sharedVolume.Value)
                );
                if (EditorGUI.EndChangeCheck()) {
                    SessionState.SetFloat(volumeSessionStateKey, _sharedVolume.Value);
                }
            }
        }
        public void DrawLayoutVolumeField(params GUILayoutOption[] options) {
            DrawVolumeField(EditorGUILayout.GetControlRect(options));
        }
    }
}