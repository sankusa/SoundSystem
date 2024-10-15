using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SoundSystem {
    [System.Serializable]
    public class AudioUnitView {
        [SerializeField] TreeViewState _audioUnitTreeViewState = new();
        AudioUnitTreeView _audioUnitTreeView;

        SearchField _searchField;

        List<AudioUnit> _allAudioUnits;
        List<AudioUnitCategory> _allcategories;

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

            _audioUnitTreeView = new AudioUnitTreeView(_audioUnitTreeViewState, multiColumnHeader);
        }

        public void OnGUI() {
            using (new EditorGUILayout.VerticalScope()) {
                using (new EditorGUILayout.HorizontalScope(GUIStyles.DarkToolbar)) {
                    EditorGUILayout.LabelField($"{nameof(AudioUnit)} List", GUIStyles.CaptionLabel, GUILayout.Width(80));
                    GUILayout.FlexibleSpace();
                    _audioUnitTreeView.searchString = _searchField.OnGUI(
                        EditorGUILayout.GetControlRect(false, GUILayout.ExpandWidth(true), GUILayout.Height(EditorGUIUtility.singleLineHeight)),
                        _audioUnitTreeView.searchString
                    );
                    if (GUILayout.Button("Reflesh", EditorStyles.toolbarButton)) {
                        _audioUnitTreeView.Reload();
                    }
                }

                _audioUnitTreeView.OnGUI(
                    GUILayoutUtility.GetRect(0, _audioUnitTreeView.totalHeight, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true))
                );
            }
        }

        public void Reload() {
            _allAudioUnits = EditorUtil.LoadAllAsset<AudioUnit>().ToList();

            _audioUnitTreeView.Reload(_allAudioUnits);
        }
    }
}