using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SoundSystem {
    [System.Serializable]
    public class ClipView {
        [SerializeField] TreeViewState _treeViewState = new();
        ClipTreeView _treeView;

        SearchField _searchField;

        EditorSoundPlayerGUIEx _editorSoundPlayerGUI = new();

        [SerializeField] bool _showAudioClip = true;
        [SerializeField] bool _showcustomClip = true;

        public void OnEnable(TargetFolders targetFolders) {
            _searchField = new();

            var captionColumn = new MultiColumnHeaderState.Column() {
                headerContent = new GUIContent("Label"),
                canSort = false,
                autoResize = true,
            };
            var importSettingColumn = new MultiColumnHeaderState.Column() {
                headerContent = new GUIContent("Import Setting Check"),
                canSort = false,
                width = 8,
            };
            var headerState = new MultiColumnHeaderState(new MultiColumnHeaderState.Column[] {captionColumn, importSettingColumn});
            var multiColumnHeader = new MultiColumnHeader(headerState);
            multiColumnHeader.ResizeToFit();

            _treeView = new ClipTreeView(_treeViewState, multiColumnHeader, targetFolders);
            _treeView.OnSelectionChanged = selections => {_editorSoundPlayerGUI.Bind(selections.FirstOrDefault());};

            _editorSoundPlayerGUI.OnEnable();
        }

        public void OnDisable() {
            _editorSoundPlayerGUI.OnDisable();
        }

        public void OnGUI() {
            using (new EditorGUILayout.VerticalScope()) {
                using (new EditorGUILayout.HorizontalScope(GUIStyles.DarkToolbar)) {
                    EditorGUILayout.LabelField($"Clip", GUIStyles.CaptionLabel, GUILayout.Width(80));
                    using (var check = new EditorGUI.ChangeCheckScope()) {
                        _showAudioClip = GUILayout.Toggle(_showAudioClip, new GUIContent(Icons.AudioClipIcon), EditorStyles.toolbarButton, GUILayout.Width(32));
                        _showcustomClip = GUILayout.Toggle(_showcustomClip, new GUIContent(Skin.Instance.CustomClipIcon), EditorStyles.toolbarButton, GUILayout.Width(32));
                        if (check.changed) {
                            _treeView.Reload(_showAudioClip, _showcustomClip);
                        }
                    }

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

                _editorSoundPlayerGUI.DrawGUILayout();
            }
        }

        public void Reload() {
            _treeView.Reload(_showAudioClip, _showcustomClip);
        }
    }
}