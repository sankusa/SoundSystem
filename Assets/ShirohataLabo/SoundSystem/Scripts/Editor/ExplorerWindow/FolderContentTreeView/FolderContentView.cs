using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;


namespace SoundSystem {
    public class FolderContentView {
        [SerializeField] TreeViewState _treeViewState = new();
        FolderContentTreeView _treeView;

        public string SearchString {
            get => _treeView.searchString;
            set => _treeView.searchString = value;
        }

        EditorSoundPlayerGUIEx _editorSoundPlayerGUI = new();

        public void OnEnable(PlayableObjectDatabase objectDatabase, TargetFolders targetFolders) {
            List<MultiColumnHeaderState.Column> columns = new List<MultiColumnHeaderState.Column>();
            var labelColumn = new MultiColumnHeaderState.Column() {
                headerContent = new GUIContent("Label"),
                canSort = false,
                autoResize = true,
            };
            columns.Add(labelColumn);

            foreach (ObjectDatabaseColumnDefinitionElement columnDefine in objectDatabase.ColumnDefinition.Columns) {
                var column = new MultiColumnHeaderState.Column() {
                    headerContent = new GUIContent(columnDefine.ColumnName),
                    autoResize = true,
                };
                columns.Add(column);
            }

            var headerState = new MultiColumnHeaderState(columns.ToArray());
            var multiColumnHeader = new MultiColumnHeader(headerState);
            multiColumnHeader.ResizeToFit();

            _treeView = new FolderContentTreeView(_treeViewState, multiColumnHeader, objectDatabase, targetFolders);

            Undo.undoRedoPerformed += _treeView.Reload;

            _editorSoundPlayerGUI.OnEnable();

            _treeView.OnSelectionChanged += selections => {
                _editorSoundPlayerGUI.Bind(selections.FirstOrDefault());
            };
        }

        public void OnDisable() {
            Undo.undoRedoPerformed -= _treeView.Reload;

            _editorSoundPlayerGUI.OnDisable();
        }

        public void OnGUI() {
            using (new EditorGUILayout.VerticalScope()) {
                _treeView.OnGUI(
                    GUILayoutUtility.GetRect(0, 0, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true))
                );
                _editorSoundPlayerGUI.DrawGUILayout();
            }
        }

        public void Reload(DefaultAsset folder) => _treeView.Reload(folder);
        public void Reload() => _treeView.Reload();

        public void CheckImportSetting() => _treeView.CheckImportSetting();
        public void ApplyImportSetting() => _treeView.ApplyImportSetting();
    }
}