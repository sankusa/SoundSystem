using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SoundSystem {
    public class SoundContainerTreeView : TreeView {
        List<SoundContainer> _containers;

        public SoundContainerTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader) : base(state, multiColumnHeader) {
            rowHeight = 16;
            // enableItemHovering = true;
            // showAlternatingRowBackgrounds = true;
        }

        public void Reload(List<SoundContainer> containers) {
            _containers = containers;
            Reload();
        }

        protected override TreeViewItem BuildRoot() {
            TreeViewItem root = new() {
                depth = -1,
                children = new List<TreeViewItem>()
            };
            return root;
        }

        protected override IList<TreeViewItem> BuildRows(TreeViewItem root) {
            var rows = GetRows() ?? new List<TreeViewItem>();
            rows.Clear();

            int id = 0;

            if (_containers != null) {
                foreach (SoundContainer container in _containers) {
                    SerializedObject serializedObject = new(container);
                    var item = new SoundContainerTreeViewItem_SoundContainer(container, serializedObject, container.GetInstanceID());
                    root.AddChild(item);
                    rows.Add(item);
                    SerializedProperty listProp = serializedObject.FindProperty("_soundDic._list");
                    if (listProp.arraySize > 0) {
                        if (IsExpanded(item.id)) {
                            for (int i = 0; i < listProp.arraySize; i++) {
                                var soundItem = new SoundContainerTreeViewItem_Sound(listProp.GetArrayElementAtIndex(i), ++id);
                                item.AddChild(soundItem);
                                rows.Add(soundItem);
                            }
                        }
                        else {
                            for (int i = 0; i < listProp.arraySize; i++) {
                                id++;
                            }
                            item.children = CreateChildListForCollapsedParent();
                        }
                    }
                }
            }

            SetupDepthsFromParentsAndChildren(root);
            return rows;
        }

        protected override void BeforeRowsGUI() {
            foreach (TreeViewItem item in GetRows()) {
                if (item is SoundContainerTreeViewItem_SoundContainer containerItem) {
                    containerItem.SerializedObject.UpdateIfRequiredOrScript();
                }
            }
        }

        protected override void AfterRowsGUI() {
            foreach (TreeViewItem item in GetRows()) {
                if (item is SoundContainerTreeViewItem_SoundContainer containerItem) {
                    containerItem.SerializedObject.ApplyModifiedProperties();
                }
            }
        }

        protected override void RowGUI(RowGUIArgs args) {
            Rect rowRect = new Rect(args.rowRect) {xMin = args.rowRect.xMin + GetContentIndent(args.item)};
            Rect backgroundRect = new(rowRect) {xMin = rowRect.xMin - 16};

            if (args.item is SoundContainerTreeViewItem_SoundContainer containerItem) {
                GUI.Box(backgroundRect, "", GUIStyles.FolderRowBackground);
                for (int i = 0; i < args.GetNumVisibleColumns(); i++) {
                    Rect cellRect = args.GetCellRect(i);
                    int columnIndex = args.GetColumn(i);
                    switch (columnIndex) {
                        case 0:
                            cellRect.xMin += GetContentIndent(args.item);
                            string assetPath = AssetDatabase.GetAssetPath(containerItem.Container);
                            Texture icon = AssetDatabase.GetCachedIcon(assetPath);

                            EditorGUI.LabelField(cellRect, new GUIContent(containerItem.Container.name, icon));
                            break;
                        case 1:
                            bool preload = SoundContainerPreloader.InstanceForEditor.ContainsForEditor(containerItem.Container);
                            bool newPreload = EditorGUI.ToggleLeft(cellRect, GUIContent.none, preload);
                            if (newPreload != preload) {
                                SoundContainerPreloader.InstanceForEditor.UpdatePreloadStateForEditor(containerItem.Container, newPreload);
                            }
                            break;
                    }
                }
            }
            else if (args.item is SoundContainerTreeViewItem_Sound soundItem) {
                GUI.Box(backgroundRect, "", GUIStyles.SoundRowBackground);
                soundItem.RowGUI(rowRect);
            }
        }

        protected override float GetCustomRowHeight(int row, TreeViewItem item) {
            if (item is SoundContainerTreeViewItem_Sound soundItem) {
                return soundItem.GetRowHeight();
            }
            return base.GetCustomRowHeight(row, item);
        }

        protected override void SingleClickedItem(int id) {
            var item = FindItem(id, rootItem);
            if (item is SoundContainerTreeViewItem_SoundContainer containerItem) {
                containerItem.OnSingleClick();
            }
        }

        protected override void DoubleClickedItem(int id) {
            var item = FindItem(id, rootItem);
            if (item is SoundContainerTreeViewItem_SoundContainer containerItem) {
                containerItem.OnDoubleClick();
            }
        }
    }
}