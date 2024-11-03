using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    public class EditorSoundPlayerGUI {
        RectUtil.LayoutLength[] _contentWidths = new RectUtil.LayoutLength[] {
            new(19, RectUtil.LayoutType.Fixed),
            new(19, RectUtil.LayoutType.Fixed),
            new(19, RectUtil.LayoutType.Fixed),
            new(1, RectUtil.LayoutType.Expand),
            new(80, RectUtil.LayoutType.Fixed),
        };

        EditorSoundPlayer _player;
        public EditorSoundPlayer Player => _player;

        public void OnEnable() {
            _player = new();
        }

        public void OnDisable() {
            _player.Dispose();
        }

        public void Bind(CustomClip customClip) {
            _player.Bind(customClip);
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

        public void DrawGUILayout() {
            _player.Update();

            using (new EditorGUILayout.VerticalScope(GUIStyles.DarkSimpleBox)) {
                using (new EditorGUILayout.HorizontalScope()) {
                    _player.DrawLayoutPlayButton(GUILayout.Width(19), GUILayout.Height(19));
                    _player.DrawLayoutPauseButton(GUILayout.Width(19), GUILayout.Height(19));
                    _player.DrawLayoutLoopButton(GUILayout.Width(19), GUILayout.Height(19));
                    _player.DrawLayoutTimeSlider();
                    _player.DrawLayoutPlayerGroupSelectPopup(GUILayout.Width(80));
                }
            }
        }

        public void DrawGUI(Rect rect) {
            _player.Update();

            GUI.Box(rect, "", GUIStyles.SimpleBox);
            GUI.Box(new RectOffset(1, 1, 1, 1).Remove(rect), "", GUIStyles.DarkSimpleBox);
            Rect[] rects = RectUtil.DivideRectHorizontal(RectUtil.Margin(rect, 4, 4, 4, 4), _contentWidths);

            using (new IndentLevelScope(0)) {
                _player.DrawPlayButton(rects[0]);
                _player.DrawPauseButton(rects[1]);
                _player.DrawLoopButton(rects[2]);
                _player.DrawTimeSlider(RectUtil.Margin(rects[3], 4, 4));
                _player.DrawPlayerGroupSelectPopup(rects[4]);
            }
        }
    }
}