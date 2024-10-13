using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    public class LayoutSplitter {
        readonly bool _horizontalSplit;
        Vector2 _scrollPos1;
        Vector2 _scrollPos2;
        float _splitRate = 0.5f;
        bool _resizing;

        Rect _availableArea;

        public float Length {
            get {
                if (_horizontalSplit) {
                    return _availableArea.width * _splitRate;
                }
                else {
                    return _availableArea.height * _splitRate;
                }
            }
        }

        readonly float _minSize1;
        readonly float _minSize2;
        readonly string _sessionStateKey;
        Color _handleColor = Color.white;

        const float _handleWidth = 2;

        public LayoutSplitter(bool horizontalSplit = false, float defaultSplitRate = 0.5f, float minSize1 = 100, float minSize2 = 100, string sessionStateKey = "", Color handleColor = default) {
            _horizontalSplit = horizontalSplit;
            _splitRate = defaultSplitRate;
            _minSize1 = minSize1;
            _minSize2 = minSize2;
            _sessionStateKey = sessionStateKey;
            if (handleColor == default) {
                _handleColor = Color.white;
            }
            else {
                _handleColor = handleColor;
            }
            // 比率を復元
            if (!string.IsNullOrEmpty(_sessionStateKey)) {
                _splitRate = SessionState.GetFloat(_sessionStateKey, _splitRate);
            }
        }

        public void Begin() {
            Rect uncheckedAvailableArea;
            if (_horizontalSplit) {
                uncheckedAvailableArea = EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
                if (uncheckedAvailableArea.width > 0) {
                    _availableArea = uncheckedAvailableArea;
                }
                _scrollPos1 = GUILayout.BeginScrollView(_scrollPos1, GUILayout.Width(_availableArea.width * _splitRate));
            }
            else {
                uncheckedAvailableArea = EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));
                if (uncheckedAvailableArea.height > 0) {
                    _availableArea = uncheckedAvailableArea;
                }
                _scrollPos1 = GUILayout.BeginScrollView(_scrollPos1, GUILayout.Height(_availableArea.height * _splitRate));
            }
        }

        public void Split() {
            GUILayout.EndScrollView();
            if (_horizontalSplit) {
                GUILayoutUtility.GetRect(0, 0, GUILayout.Width(_handleWidth));
            }
            else {
                GUILayoutUtility.GetRect(0, 0, GUILayout.Height(_handleWidth));
            }
            DrawResizeHandle();
            if (_horizontalSplit) {
                _scrollPos2 = GUILayout.BeginScrollView(_scrollPos2, GUILayout.ExpandWidth(true));
            }
            else {
                _scrollPos2 = GUILayout.BeginScrollView(_scrollPos2, GUILayout.ExpandHeight(true));
            }
            
        }

        public void End() {
            GUILayout.EndScrollView();
            if (_horizontalSplit) {
                EditorGUILayout.EndHorizontal();
            }
            else {
                EditorGUILayout.EndVertical();
            }
            if(_resizing) {
                HandleUtility.Repaint();
            }
        }

        void DrawResizeHandle() {
            // ハンドルの矩形
            Rect resizeHandleRect;
            if (_horizontalSplit) {
                resizeHandleRect = new Rect(_availableArea.x + _availableArea.width * _splitRate, _availableArea.y, _handleWidth, _availableArea.height);
            }
            else {
                resizeHandleRect = new Rect(_availableArea.x, _availableArea.y + _availableArea.height * _splitRate, _availableArea.width, _handleWidth);
            }

            Rect mouseAcceptRect = new Rect(resizeHandleRect.x - 1, resizeHandleRect.y - 1, resizeHandleRect.width + 2, resizeHandleRect.height + 2);

            // カーソル変更
            if (_horizontalSplit) {
                EditorGUIUtility.AddCursorRect(mouseAcceptRect, MouseCursor.ResizeHorizontal);
            }
            else {
                EditorGUIUtility.AddCursorRect(mouseAcceptRect, MouseCursor.ResizeVertical);
            }

            // ハンドル描画
            Color originalColor = GUI.color;
            GUI.color = _handleColor;
            GUI.DrawTexture(resizeHandleRect, EditorGUIUtility.whiteTexture);
            GUI.color = originalColor;

            // ハンドルの矩形内のマウス押下でサイズ変更開始
            if (Event.current.type == EventType.MouseDown && mouseAcceptRect.Contains(Event.current.mousePosition)) {
                _resizing = true;
            }

            // サイズ変形終了
            if (Event.current.type == EventType.MouseUp || Event.current.type == EventType.Ignore) {
                _resizing  = false;
            }

            // サイズ変形
            if (_resizing) {
                if (_horizontalSplit) {
                    if (_availableArea.width < _minSize1 + _minSize2) {
                        _splitRate = 0.5f;
                    }
                    else {
                        _splitRate = Mathf.Clamp(Event.current.mousePosition.x - _availableArea.x, _minSize1, _availableArea.width - _minSize2) / _availableArea.width;
                    }
                }
                else {
                    if (_availableArea.height < _minSize1 + _minSize2) {
                        _splitRate = 0.5f;
                    }
                    else {
                        _splitRate = Mathf.Clamp(Event.current.mousePosition.y - _availableArea.y, _minSize1, _availableArea.height - _minSize2) / _availableArea.height;
                    }
                }
                // 比率を保存
                if (string.IsNullOrEmpty(_sessionStateKey) == false) {
                    SessionState.SetFloat(_sessionStateKey, _splitRate);
                }
            }
        }
    }
}