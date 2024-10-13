using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SoundSystem {
    public class SoundContainerTreeView : TreeView {
        List<SoundContainer> _containers;

        public SoundContainerTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader) : base(state, multiColumnHeader) {
            rowHeight = 20;
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

            int id = 0;

            if (_containers != null) {
                foreach (SoundContainer container in _containers) {
                    root.AddChild(new SoundContainerTreeViewItem(++id, container));
                }
            }

            SetupDepthsFromParentsAndChildren(root);
            return root;
        }

        protected override void RowGUI(RowGUIArgs args) {
            GUI.Box(args.rowRect, "", GUIStyles.SoundRowBackground);

            SoundContainerTreeViewItem item = args.item as SoundContainerTreeViewItem;
            for (int i = 0; i < args.GetNumVisibleColumns(); i++) {
                Rect cellRect = args.GetCellRect(i);
                int columnIndex = args.GetColumn(i);
                switch (columnIndex) {
                    case 0:
                        string assetPath = AssetDatabase.GetAssetPath(item.Container);
                        Texture icon = AssetDatabase.GetCachedIcon(assetPath);

                        EditorGUI.LabelField(cellRect, new GUIContent(item.Container.name, icon));
                        break;
                    case 1:
                        bool preload = SoundContainerPreloader.InstanceForEditor.ContainsForEditor(item.Container);
                        bool newPreload = EditorGUI.ToggleLeft(cellRect, GUIContent.none, preload);
                        if (newPreload != preload) {
                            SoundContainerPreloader.InstanceForEditor.UpdatePreloadStateForEditor(item.Container, newPreload);
                        }
                        break;
                }
            }
        }

        protected override void SingleClickedItem(int id) {
            SoundContainerTreeViewItem item = FindItem(id, rootItem) as SoundContainerTreeViewItem;
            item.OnSingleClick();
        }

        protected override void DoubleClickedItem(int id) {
            SoundContainerTreeViewItem item = FindItem(id, rootItem) as SoundContainerTreeViewItem;
            item.OnDoubleClick();
        }
    }
}