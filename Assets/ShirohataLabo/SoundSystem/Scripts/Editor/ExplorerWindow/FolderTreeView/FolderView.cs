using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SoundSystem {
    [Serializable]
    public class FolderView {
        [SerializeField] TreeViewState _treeViewState = new();
        FolderTreeView _treeView;

        public event Action<IEnumerable<DefaultAsset>> OnSelectionChanged;

        TargetFolders _targetFolders;

        public void OnEnable(TargetFolders targetFolders) {
            _targetFolders = targetFolders;

            var captionColumn = new MultiColumnHeaderState.Column() {
                headerContent = new GUIContent("Folder"),
                canSort = false,
                autoResize = true,
            };
            var headerState = new MultiColumnHeaderState(new MultiColumnHeaderState.Column[] {captionColumn});
            var multiColumnHeader = new MultiColumnHeader(headerState);
            multiColumnHeader.ResizeToFit();

            _treeView = new FolderTreeView(_treeViewState, targetFolders);
            _treeView.OnSelectionChanged += selections => {
                OnSelectionChanged?.Invoke(selections);
            };
        }

        public void OnGUI() {
            using (new EditorGUILayout.VerticalScope()) {
                _treeView.OnGUI(
                    GUILayoutUtility.GetRect(0, 0, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true))
                );

                Rect addTargetFolderRect = EditorGUILayout.GetControlRect(GUILayout.Height(20));
                GUI.Box(addTargetFolderRect, "Drop here to add folder");
                List<DefaultAsset> folders = DragAndDropUtil.AcceptObjects<DefaultAsset>(addTargetFolderRect);
                if (folders != null) {
                    _targetFolders.Add(folders);
                    Reload();
                }
            }
        }

        public void Reload() {
            _treeView.Reload();
        }

        public void CheckImportSetting() => _treeView.CheckImportSetting();
        public void ApplyImportSetting() => _treeView.ApplyImportSetting();
    }
}