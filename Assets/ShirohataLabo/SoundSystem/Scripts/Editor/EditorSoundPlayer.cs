using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    public class EditorSoundPlayer {
        AudioSource _audioSource;
        AudioSource AudioSource {
            get {
                if (_audioSource == null) {
                    _audioSource = CreateAudioSourceObject();
                }
                return _audioSource;
            }
        }

        float ClipLength => AudioSource.clip != null ? AudioSource.clip.length : 0;

        bool IsPaused => AudioSource.isPlaying == false && AudioSource.time != 0 && AudioSource.time != ClipLength;
        // AudioSourceの再生位置が終端に達し、自発的に再生を停止した場合(動作確認した限り、停止時にAudioSource.timeが0とAudioSource.clip.lengthのどちらかになる(条件不明))
        bool IsAudioSourceFinishedPlaying => AudioSource.isPlaying == false && AudioSource.clip != null && (AudioSource.time == 0 || AudioSource.time == ClipLength);

        AudioUnit _audioUnit;

        bool _loop;

        public void OnEnable() {
            CreateAudioSourceObject();
        }

        public void OnDisable() {
            DestroyAudioSourceObject(_audioSource);
        }

        void Update() {
            // AudioSourceが自分で再生を終了していた場合、ループ設定なら再生しなおす
            if (IsAudioSourceFinishedPlaying) {
                if (_loop) {
                    Play();
                }
                else {
                    Stop();
                    return;
                }
            }
            // その他の停止理由ならUpdateは行わない
            else if (AudioSource.isPlaying == false) {
                return;
            }

            // エディタを滑らかに動かす
            HandleUtility.Repaint();

            // AudioUnitが破棄されたら停止
            if (_audioUnit == null) {
                Stop();
                return;
            }

            // クリップが変更されていたら再生しなおす
            if (AudioSource.clip != _audioUnit.Clip) {
                Play();
                return;
            }

            if (AudioSource.time >= _audioUnit.End) {
                if (_loop) {
                    AudioSource.time = _audioUnit.Start;
                }
                else {
                    AudioSource.Stop();
                }
            }

            // 通常Update処理
            AudioSource.time = Mathf.Clamp(AudioSource.time, _audioUnit.Start, _audioUnit.End);
            AudioSource.volume = _audioUnit.GetCurrentVolume(AudioSource.time);
            AudioSource.pitch = _audioUnit.Pitch;
        }

        public void Bind(AudioUnit audioUnit) {
            _audioUnit = audioUnit;
        }

        public void Play() {
            AudioSource.clip = _audioUnit.Clip;
            AudioSource.volume = _audioUnit.GetCurrentVolume(0);
            AudioSource.pitch = _audioUnit.Pitch;
            AudioSource.time = _audioUnit.Start;

            if (_audioUnit.Clip != null) {
                AudioSource.Play();
            }
        }

        public void Stop() {
            AudioSource.Stop();
            AudioSource.clip = null;
            AudioSource.volume = 1;
            AudioSource.pitch = 1;
            AudioSource.time = 0;
        }

        static AudioSource CreateAudioSourceObject() {
            GameObject gameObject = new(nameof(EditorSoundPlayer)) {
                hideFlags = HideFlags.HideAndDontSave
            };
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = false;
            return audioSource;
        }

        static void DestroyAudioSourceObject(AudioSource audioSource) {
            if (audioSource == null) return;
            Object.DestroyImmediate(audioSource.gameObject);
        }

        public void DrawGUILayout() {
            Update();

            using (new EditorGUILayout.VerticalScope(new GUIStyle("GroupBox"))) {
                using (new EditorGUILayout.HorizontalScope()) {
                    // EditorGUILayout.LabelField(nameof(EditorSoundPlayer), EditorStyles.boldLabel, GUILayout.Width(120));

                    GUILayout.FlexibleSpace();

                    bool playToggleRet = GUILayout.Toggle(AudioSource.isPlaying, "", "Button", GUILayout.Width(19), GUILayout.Height(19));
                    EditorGUI.LabelField(GUILayoutUtility.GetLastRect(), new GUIContent(Skin.Instance.PlayIcon));
                    if (AudioSource.isPlaying == false && playToggleRet) {
                        Play();
                    }
                    else if (AudioSource.isPlaying && playToggleRet == false) {
                        Stop();
                    }

                    bool pauseToggleRet = GUILayout.Toggle(IsPaused, "", "Button", GUILayout.Width(19), GUILayout.Height(19));
                    EditorGUI.LabelField(GUILayoutUtility.GetLastRect(), new GUIContent(Skin.Instance.PauseIcon));
                    if (IsPaused == false && pauseToggleRet) {
                        AudioSource.Pause();
                    }
                    else if (IsPaused && pauseToggleRet == false) {
                        AudioSource.UnPause();
                    }

                    _loop = GUILayout.Toggle(_loop, "", "Button", GUILayout.Width(19), GUILayout.Height(19));
                    EditorGUI.LabelField(GUILayoutUtility.GetLastRect(), new GUIContent(Skin.Instance.RepeatIcon));

                    EditorGUI.BeginChangeCheck();
                    float newAudioSourceTime = EditorGUILayout.Slider(AudioSource.time, 0, ClipLength);
                    if (EditorGUI.EndChangeCheck()) {
                        // AudioSource.timeにAudioSource.clip.lengthを設定すると再生位置エラーになる
                        if (newAudioSourceTime < ClipLength) {
                            AudioSource.time = newAudioSourceTime;
                        }
                    }
                }
            }
        }
    }
}