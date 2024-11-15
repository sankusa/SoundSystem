using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SoundSystem {
    [System.Serializable]
    public class SoundContainerView {
        List<SoundContainer> _allSoundContainers;

        [SerializeField] TreeViewState _soundContainerTreeViewState = new();
        SoundContainerTreeView _soundContainerTreeView;

        SearchField _searchField;

        public void OnEnable() {
            var containerColumn = new MultiColumnHeaderState.Column() {
                headerContent = new GUIContent("Label"),
                canSort = false,
                autoResize = true,
            };
            var preloadColumn = new MultiColumnHeaderState.Column() {
                headerContent = new GUIContent("Preload"),
                canSort = false,
                autoResize = true,
                width = 52,
                minWidth = 52,
                maxWidth = 52,
            };
            var pathColumn = new MultiColumnHeaderState.Column() {
                headerContent = new GUIContent("Folder"),
                canSort = false,
                autoResize = true,
            };
            var headerState = new MultiColumnHeaderState(new MultiColumnHeaderState.Column[] {containerColumn, preloadColumn, pathColumn});
            var multiColumnHeader = new MultiColumnHeader(headerState);
            multiColumnHeader.ResizeToFit();

            _soundContainerTreeView = new SoundContainerTreeView(_soundContainerTreeViewState, multiColumnHeader);

            Undo.undoRedoPerformed += _soundContainerTreeView.Reload;

            _searchField = new();
        }

        public void OnDisable() {
            Undo.undoRedoPerformed -= _soundContainerTreeView.Reload;
        }
        
        public void OnGUI() {
            using (new EditorGUILayout.VerticalScope()) {
                using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar)) {
                    _soundContainerTreeView.searchString = _searchField.OnToolbarGUI(EditorGUILayout.GetControlRect(GUILayout.ExpandWidth(true)), _soundContainerTreeView.searchString);
                    if (GUILayout.Button(new GUIContent(Icons.RefleshIcon, "Reflesh"), EditorStyles.toolbarButton, GUILayout.Width(26))) {
                        _soundContainerTreeView.Reload();
                    }
                    if (GUILayout.Button(Icons.CommandIcon, EditorStyles.toolbarButton, GUILayout.Width(26))) {
                        GenericMenu menu = new GenericMenu();
                        menu.AddItem(new GUIContent("Generate SoundKey Class"), false, () => ScriptGenerator.GenerateSoundKeyScript());
                        menu.ShowAsContext();
                    }
                }

                _soundContainerTreeView.OnGUI(
                    GUILayoutUtility.GetRect(0, 0, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true))
                );
            }
        }

        public void Reload() {
            _allSoundContainers = EditorUtil.LoadAllAsset<SoundContainer>().ToList();
            _soundContainerTreeView.Reload(_allSoundContainers);
        }
    }
}