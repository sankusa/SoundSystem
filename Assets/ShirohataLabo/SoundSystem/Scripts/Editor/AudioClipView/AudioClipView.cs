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
                headerContent = new GUIContent("Import Setting Check"),
                canSort = false,
                width = 8,
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
                    if (GUILayout.Button(new GUIContent(Icons.RefleshIcon, "Reflesh"), EditorStyles.toolbarButton)) {
                        _audioClipTreeView.Reload();
                    }
                    if (GUILayout.Button(new GUIContent(Icons.CommandIcon), EditorStyles.toolbarButton)) {
                        GenericMenu menu = new();
                        menu.AddItem(
                            new GUIContent("Check Import Setting"),
                            false,
                            () => _audioClipTreeView.CheckImportSetting()
                        );
                        menu.AddItem(
                            new GUIContent("Apply Import Setting"),
                            false,
                            () => _audioClipTreeView.ApplyImportSetting()
                        );
                        menu.ShowAsContext();
                    }
                }

                _audioClipTreeView.OnGUI(
                    GUILayoutUtility.GetRect(0, 0, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true))
                );
            }
        }

        public void Reload() {
            _audioClipTreeView.Reload();
        }
    }
}