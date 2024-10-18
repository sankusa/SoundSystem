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
                    var item = new SoundContainerTreeViewItem_SoundContainer(container);
                    root.AddChild(item);
                    rows.Add(item);

                    SerializedProperty soundListProp = item.SoundListProp;
                    if (soundListProp.arraySize > 0) {
                        if (IsExpanded(item.id)) {
                            for (int i = 0; i < soundListProp.arraySize; i++) {
                                var soundItem = new SoundContainerTreeViewItem_Sound(soundListProp.GetArrayElementAtIndex(i), ++id);
                                item.AddChild(soundItem);
                                rows.Add(soundItem);
                            }
                        }
                        else {
                            for (int i = 0; i < soundListProp.arraySize; i++) {
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

                            Rect addButtonRect = new Rect(cellRect) {xMin = cellRect.xMax - 20, yMin = cellRect.yMin + 1};
                            if (GUI.Button(addButtonRect, Icons.PlusIcon, GUIStyles.InvisibleButton)) {
                                containerItem.AddElement();
                                containerItem.SerializedObject.ApplyModifiedProperties();
                                Reload();
                            }
                            break;
                        case 1:
                            bool preload = SoundContainerPreloader.InstanceForEditor.ContainsForEditor(containerItem.Container);
                            bool newPreload = EditorGUI.Toggle(cellRect, GUIContent.none, preload);
                            if (newPreload != preload) {
                                SoundContainerPreloader.InstanceForEditor.UpdatePreloadStateForEditor(containerItem.Container, newPreload);
                            }
                            break;
                    }
                }
            }
            else if (args.item is SoundContainerTreeViewItem_Sound soundItem) {
                Rect fieldRect = new Rect(rowRect) {xMax = rowRect.xMax - 20};
                GUI.Box(backgroundRect, "", GUIStyles.SoundRowBackground);
                soundItem.RowGUI(fieldRect);

                Rect removeButtonRect = new Rect(rowRect) {xMin = rowRect.xMax - 20, yMin = rowRect.yMin + 5};
                if (GUI.Button(removeButtonRect, Icons.MinusIcon, GUIStyles.InvisibleButton)) {
                    soundItem.SoundWithKeyProperty.DeleteCommand();
                    soundItem.SoundWithKeyProperty.serializedObject.ApplyModifiedProperties();
                    Reload();
                }
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