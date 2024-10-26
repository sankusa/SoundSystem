using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    public class EditorSoundPlayerGUI {
        RectUtil.LayoutLength[] _contentWidths = new RectUtil.LayoutLength[] {
            new(80, RectUtil.LayoutType.Fixed),
            new(19, RectUtil.LayoutType.Fixed),
            new(19, RectUtil.LayoutType.Fixed),
            new(19, RectUtil.LayoutType.Fixed),
            new(1, RectUtil.LayoutType.Expand)
        };

        EditorSoundPlayer _player;
        public EditorSoundPlayer Player => _player;

        public void OnEnable() {
            _player = new();
        }

        public void OnDisable() {
            _player.Dispose();
        }

        public void Bind(AudioUnit audioUnit) {
            _player.Bind(audioUnit);
        }

        public void Bind(Sound sound) {
            _player.Bind(sound);
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

        public void ReapplyParameters() {
            _player.ReapplySoundBehaviours();
        }

        public void DrawGUILayout() {
            _player.Update();

            using (new EditorGUILayout.VerticalScope(GUIStyles.SimpleBox)) {
                using (new EditorGUILayout.HorizontalScope()) {
                    _player.DrawLayoutPlayerGroupSelectPopup(GUILayout.Width(80));
                    _player.DrawLayoutPlayButton(GUILayout.Width(19), GUILayout.Height(19));
                    _player.DrawLayoutPauseButton(GUILayout.Width(19), GUILayout.Height(19));
                    _player.DrawLayoutLoopButton(GUILayout.Width(19), GUILayout.Height(19));
                    _player.DrawLayoutTimeSlider();
                }
            }
        }

        public void DrawGUI(Rect rect) {
            _player.Update();

            GUI.Box(rect, "", GUIStyles.SimpleBox);
            Rect[] rects = RectUtil.DivideRectHorizontal(RectUtil.Margin(rect, 4, 4, 4, 4), _contentWidths);

            using (new IndentLevelScope(0)) {
                _player.DrawPlayerGroupSelectPopup(rects[0]);
                _player.DrawPlayButton(rects[1]);
                _player.DrawPauseButton(rects[2]);
                _player.DrawLoopButton(rects[3]);
                _player.DrawTimeSlider(RectUtil.Margin(rects[4], 4));
            }
        }
    }
}