using System.Collections.Generic;
using System.Linq;
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
            useScrollView = false;
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
                                SerializedProperty soundWithKeyProp = soundListProp.GetArrayElementAtIndex(i);
                                var soundItem = new SoundContainerTreeViewItem_Sound(soundWithKeyProp, ++id);
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
            ApplyModifiedPropertiesToAllContainer();
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

        protected override bool CanBeParent(TreeViewItem item) {
            return true;
        }

        protected override bool CanStartDrag(CanStartDragArgs args) {
            return args.draggedItem is SoundContainerTreeViewItem_Sound;
        }

        protected override void SetupDragAndDrop(SetupDragAndDropArgs args) {
            SoundContainerTreeViewItem_Sound[] draggedSoundItem = GetRows()
                .Where(item => args.draggedItemIDs.Contains(item.id))
                .OfType<SoundContainerTreeViewItem_Sound>()
                .ToArray();
            DragAndDropUtil.SendGenericObject(DragAndDropKey, draggedSoundItem);
        }

        protected override DragAndDropVisualMode HandleDragAndDrop(DragAndDropArgs args) {
            if (args.performDrop) {
                SoundContainerTreeViewItem_Sound[] draggedSoundItems = DragAndDrop.GetGenericData(DragAndDropKey) as SoundContainerTreeViewItem_Sound[];
                if (draggedSoundItems == null || draggedSoundItems.Length == 0) {
                    return DragAndDropVisualMode.None;
                }

                switch (args.dragAndDropPosition) {
                    case DragAndDropPosition.UponItem:
                    case DragAndDropPosition.BetweenItems:
                        if (args.parentItem is SoundContainerTreeViewItem_SoundContainer containerItem) {
                            int insertIndex = args.insertAtIndex;
                            if (insertIndex == -1) insertIndex = containerItem.SoundListProp.arraySize; // UponItemの場合
                            foreach (SoundContainerTreeViewItem_Sound draggedSoundItem in draggedSoundItems) {
                                // 挿入前のインデックスを保存
                                int sourceIndex = draggedSoundItem.SoundWithKeyProperty.GetArrayElementIndex();
                                var parentContainerItem = draggedSoundItem.parent as SoundContainerTreeViewItem_SoundContainer;

                                // 挿入→削除の順番のため、同じコンテナ内で元より小さいインデックスに挿入する場合、削除するインデックスがずれる
                                if (containerItem.Container == parentContainerItem.Container && sourceIndex > insertIndex) sourceIndex++;

                                containerItem.SoundListProp.InsertArrayElementAtIndex(insertIndex);

                                // 最新のインデックスで再取得
                                // (TreeViewItemが持つSerializedPropertyをそのまま使うとCopyFrom内で元のインデックスを参照して処理が行われてしまう)
                                SerializedProperty draggedDoundProp = parentContainerItem.SoundListProp.GetArrayElementAtIndex(sourceIndex);
                                containerItem.SoundListProp.GetArrayElementAtIndex(insertIndex).CopyFrom(draggedDoundProp);
                                insertIndex++;
                                parentContainerItem.SoundListProp.DeleteArrayElementAtIndex(sourceIndex);
                            }
                            ApplyModifiedPropertiesToAllContainer();
                        }
                        Reload();
                        break;
                    default:
                        return DragAndDropVisualMode.None;
                }
            }

            if (DragAndDrop.GetGenericData(DragAndDropKey) == null) {
                return DragAndDropVisualMode.None;
            }

            if (args.parentItem is SoundContainerTreeViewItem_SoundContainer &&
                (args.dragAndDropPosition == DragAndDropPosition.UponItem ||
                 args.dragAndDropPosition == DragAndDropPosition.BetweenItems)
            ) {
                return DragAndDropVisualMode.Move;
            }

            return DragAndDropVisualMode.None;
        }

        void ApplyModifiedPropertiesToAllContainer() {
            foreach (TreeViewItem item in GetRows()) {
                if (item is SoundContainerTreeViewItem_SoundContainer containerItem) {
                    containerItem.SerializedObject.ApplyModifiedProperties();
                }
            }
        }
    }
}