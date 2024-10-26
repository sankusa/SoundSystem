using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SoundSystem {
    [System.Serializable]
    public class CustomClipView {
        [SerializeField] TreeViewState _treeViewState = new();
        CustomClipTreeView _treeView;

        SearchField _searchField;

        EditorSoundPlayerGUIForCustomClipView _editorSoundPlayerGUI = new();

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

            _treeView = new CustomClipTreeView(_treeViewState, multiColumnHeader);
            _treeView._onSelectedCustomClipChanged = customClips => {_editorSoundPlayerGUI.Bind(customClips.FirstOrDefault());};

            _editorSoundPlayerGUI.OnEnable();
        }

        public void OnDisable() {
            _editorSoundPlayerGUI.OnDisable();
        }

        public void OnGUI() {
            using (new EditorGUILayout.VerticalScope()) {
                using (new EditorGUILayout.HorizontalScope(GUIStyles.DarkToolbar)) {
                    EditorGUILayout.LabelField($"{nameof(CustomClip)}", GUIStyles.CaptionLabel, GUILayout.Width(80));
                    GUILayout.FlexibleSpace();
                    _treeView.searchString = _searchField.OnGUI(
                        EditorGUILayout.GetControlRect(false, GUILayout.ExpandWidth(true), GUILayout.Height(EditorGUIUtility.singleLineHeight)),
                        _treeView.searchString
                    );
                    if (GUILayout.Button(new GUIContent(Icons.RefleshIcon, "Reflesh"), EditorStyles.toolbarButton)) {
                        _treeView.Reload();
                    }
                }

                _treeView.OnGUI(
                    GUILayoutUtility.GetRect(0, 0, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true))
                );

                _editorSoundPlayerGUI.DrawGUILayout();
            }
        }

        public void Reload() {
            _treeView.Reload();
        }
    }
}