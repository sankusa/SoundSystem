using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SoundSystem {
    public class SoundContainerTreeView : TreeView {
        List<SoundContainer> _containers;

        const string DragAndDropKey = nameof(SoundContainerTreeView) + "_DragAndDrop";

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

            if (_containers != null) {
                foreach (SoundContainer container in _containers) {
                    var item = new SoundContainerTreeViewItem_SoundContainer(container);
                    root.AddChild(item);
                }
            }

            SetupDepthsFromParentsAndChildren(root);

            return root;
        }

        protected override void BeforeRowsGUI() {
            foreach (TreeViewItem item in GetRows()) {
                if (item is SoundContainerTreeViewItem_SoundContainer containerItem) {
                    containerItem.SerializedObject.UpdateIfRequiredOrScript();
                }
            }
        }

        protected override void AfterRowsGUI() {
            ApplyModifiedPropertiesToAllContainer();
        }

        protected override void RowGUI(RowGUIArgs args) {
            Rect rowRect = new Rect(args.rowRect) {xMin = args.rowRect.xMin + GetContentIndent(args.item)};
            Rect backgroundRect = new(rowRect) {xMin = rowRect.xMin - 16};

            if (args.item is SoundContainerTreeViewItem_SoundContainer containerItem) {
                GUI.Box(backgroundRect, "", GUIStyles.SoundContainerRowBackground);
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
                            bool newPreload = EditorGUI.Toggle(cellRect, GUIContent.none, preload);
                            if (newPreload != preload) {
                                SoundContainerPreloader.InstanceForEditor.UpdatePreloadStateForEditor(containerItem.Container, newPreload);
                            }
                            break;
                        case 2:
                            string folderPath = EditorUtil.GetFolderPath(AssetDatabase.GetAssetPath(containerItem.Container));
                            EditorGUI.LabelField(cellRect, new GUIContent(folderPath));
                            break;
                    }
                }
            }
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

        void ApplyModifiedPropertiesToAllContainer() {
            foreach (TreeViewItem item in GetRows()) {
                if (item is SoundContainerTreeViewItem_SoundContainer containerItem) {
                    containerItem.SerializedObject.ApplyModifiedProperties();
                }
            }
        }

        protected override bool CanRename(TreeViewItem item) {
            return item is SoundContainerTreeViewItem_SoundContainer;
        }

        protected override void RenameEnded(RenameEndedArgs args) {
            if (args.acceptedRename == false) return;
            TreeViewItem item = FindItem(args.itemID, rootItem);
            if (item is SoundContainerTreeViewItem_SoundContainer containerItem) {
                string assetPath = AssetDatabase.GetAssetPath(containerItem.Container);
                AssetDatabase.RenameAsset(assetPath, args.newName);
            }
        }
    }
}