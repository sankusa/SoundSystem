using UnityEngine;
using UnityEditor.IMGUI.Controls;
using UnityEditor;
using System.Linq;

namespace SoundSystem {
    public class LabeledSoundListView {
        [SerializeField] TreeViewState _soundTreeViewState = new();
        LabeledSoundListTreeView _labeledSoundListTreeView;

        SerializedProperty _labeledSoundListProp;

        public LabeledSoundListView(SerializedProperty labeledSoundListProp) {
            _labeledSoundListProp = labeledSoundListProp;

            var labelColumn = new MultiColumnHeaderState.Column() {
                headerContent = new GUIContent("Label"),
                canSort = false,
                autoResize = true,
            };
            var soundColumn = new MultiColumnHeaderState.Column() {
                headerContent = new GUIContent("Sound"),
                canSort = false,
                autoResize = true,
            };
            var headerState = new MultiColumnHeaderState(new MultiColumnHeaderState.Column[] {labelColumn, soundColumn});
            var multiColumnHeader = new MultiColumnHeader(headerState);
            multiColumnHeader.ResizeToFit();

            _labeledSoundListTreeView = new LabeledSoundListTreeView(_soundTreeViewState, multiColumnHeader);
            _labeledSoundListTreeView.Reload(_labeledSoundListProp);
        }

        public void OnGUI() {
            using (new EditorGUILayout.VerticalScope()) {
                using (new EditorGUILayout.HorizontalScope(GUIStyles.DarkToolbar)) {
                    EditorGUILayout.LabelField("Sound List"/*, GUIStyles.CaptionLabel*/);
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("+")) {
                        _labeledSoundListProp.InsertArrayElementAtIndex(_labeledSoundListProp.arraySize);
                        _labeledSoundListTreeView.Reload();
                    }
                    if (GUILayout.Button("-")) {
                        
                        int removedIndex = -1;
                        foreach (int i in _labeledSoundListTreeView.GetSelection().OrderByDescending(x => x)) {
                            if (i - 1 >= _labeledSoundListProp.arraySize) continue;
                            _labeledSoundListProp.DeleteArrayElementAtIndex(i- 1);
                            removedIndex = i - 1;
                        }
                        if (removedIndex != -1) {
                            _labeledSoundListTreeView.Reload();
                            _labeledSoundListTreeView.SetSelection(new int[] {removedIndex});
                        }
                    }
                }
                _labeledSoundListTreeView.OnGUI(
                    GUILayoutUtility.GetRect(0, _labeledSoundListTreeView.totalHeight, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true))
                );
            }
        }
    }
}