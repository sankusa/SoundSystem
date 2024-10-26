using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SoundSystem {
    [System.Serializable]
    public class AudioClipView {
        [SerializeField] TreeViewState _treeViewState = new();
        AudioClipTreeView _treeView;

        SearchField _searchField;

        public void OnEnable() {
            _searchField = new();

            var captionColumn = new MultiColumnHeaderState.Column() {
                headerContent = new GUIContent("Label"),
                canSort = false,
                autoResize = true,
            };
            var descriptionColumn = new MultiColumnHeaderState.Column() {
                headerContent = new GUIContent("Import Setting Check"),
                canSort = false,
                width = 8,
            };
            var headerState = new MultiColumnHeaderState(new MultiColumnHeaderState.Column[] {captionColumn, descriptionColumn});
            var multiColumnHeader = new MultiColumnHeader(headerState);
            multiColumnHeader.ResizeToFit();

            _treeView = new AudioClipTreeView(_treeViewState, multiColumnHeader);
        }

        public void OnGUI() {
            using (new EditorGUILayout.VerticalScope()) {
                using (new EditorGUILayout.HorizontalScope(GUIStyles.DarkToolbar)) {
                    EditorGUILayout.LabelField($"{nameof(AudioClip)}", GUIStyles.CaptionLabel, GUILayout.Width(80));
                    GUILayout.FlexibleSpace();
                    _treeView.searchString = _searchField.OnGUI(
                        EditorGUILayout.GetControlRect(false, GUILayout.ExpandWidth(true), GUILayout.Height(EditorGUIUtility.singleLineHeight)),
                        _treeView.searchString
                    );
                    if (GUILayout.Button(new GUIContent(Icons.RefleshIcon, "Reflesh"), EditorStyles.toolbarButton)) {
                        _treeView.Reload();
                    }
                    if (GUILayout.Button(new GUIContent(Icons.CommandIcon), EditorStyles.toolbarButton)) {
                        GenericMenu menu = new();
                        menu.AddItem(
                            new GUIContent("Check Import Setting"),
                            false,
                            () => _treeView.CheckImportSetting()
                        );
                        menu.AddItem(
                            new GUIContent("Apply Import Setting"),
                            false,
                            () => _treeView.ApplyImportSetting()
                        );
                        menu.ShowAsContext();
                    }
                }

                _treeView.OnGUI(
                    GUILayoutUtility.GetRect(0, 0, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true))
                );
            }
        }

        public void Reload() {
            _treeView.Reload();
        }
    }
}