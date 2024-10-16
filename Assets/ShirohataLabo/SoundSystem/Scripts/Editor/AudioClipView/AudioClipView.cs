using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SoundSystem {
    [System.Serializable]
    public class AudioClipView {
        [SerializeField] TreeViewState _audioUnitTreeViewState = new();
        AudioClipTreeView _audioClipTreeView;

        SearchField _searchField;

        public void OnEnable() {
            _searchField = new();

            var captionColumn = new MultiColumnHeaderState.Column() {
                headerContent = new GUIContent("Label"),
                canSort = false,
                autoResize = true,
            };
            var descriptionColumn = new MultiColumnHeaderState.Column() {
                headerContent = new GUIContent("Description"),
                canSort = false,
                autoResize = true,
            };
            var headerState = new MultiColumnHeaderState(new MultiColumnHeaderState.Column[] {captionColumn, descriptionColumn});
            var multiColumnHeader = new MultiColumnHeader(headerState);
            multiColumnHeader.ResizeToFit();

            _audioClipTreeView = new AudioClipTreeView(_audioUnitTreeViewState, multiColumnHeader);
        }

        public void OnGUI() {
            using (new EditorGUILayout.VerticalScope()) {
                using (new EditorGUILayout.HorizontalScope(GUIStyles.DarkToolbar)) {
                    EditorGUILayout.LabelField($"{nameof(AudioClip)}", GUIStyles.CaptionLabel, GUILayout.Width(80));
                    GUILayout.FlexibleSpace();
                    _audioClipTreeView.searchString = _searchField.OnGUI(
                        EditorGUILayout.GetControlRect(false, GUILayout.ExpandWidth(true), GUILayout.Height(EditorGUIUtility.singleLineHeight)),
                        _audioClipTreeView.searchString
                    );
                    if (GUILayout.Button("Reflesh", EditorStyles.toolbarButton)) {
                        _audioClipTreeView.Reload();
                    }
                }

                _audioClipTreeView.OnGUI(
                    GUILayoutUtility.GetRect(0, _audioClipTreeView.totalHeight, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true))
                );
            }
        }

        public void Reload() {
            _audioClipTreeView.Reload();
        }
    }
}