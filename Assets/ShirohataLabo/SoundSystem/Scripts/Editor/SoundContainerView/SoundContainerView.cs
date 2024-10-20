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
            var headerState = new MultiColumnHeaderState(new MultiColumnHeaderState.Column[] {containerColumn, preloadColumn});
            var multiColumnHeader = new MultiColumnHeader(headerState);
            multiColumnHeader.ResizeToFit();

            _soundContainerTreeView = new SoundContainerTreeView(_soundContainerTreeViewState, multiColumnHeader);

            Undo.undoRedoPerformed += _soundContainerTreeView.Reload;
        }

        public void OnDisable() {
            Undo.undoRedoPerformed += _soundContainerTreeView.Reload;
        }
        
        public void OnGUI() {
            using (new EditorGUILayout.VerticalScope()) {
                using (new EditorGUILayout.HorizontalScope(GUIStyles.DarkToolbar)) {
                    EditorGUILayout.LabelField($"{nameof(SoundContainer)}", GUIStyles.CaptionLabel, GUILayout.Width(120));
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button(new GUIContent(Icons.RefleshIcon, "Reflesh"), EditorStyles.toolbarButton)) {
                        _soundContainerTreeView.Reload();
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